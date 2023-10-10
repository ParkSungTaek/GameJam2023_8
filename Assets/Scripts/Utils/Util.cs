using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Util
{
    /// <summary>
    /// Game Object에서 해당 Component 얻거나 없으면 추가
    /// </summary>
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        return go.GetComponent<T>() ?? go.AddComponent<T>();
    }

    /// <summary>
    /// 해당 Game Object의 자식 중 T 컴포넌트를 가진 자식 얻기
    /// </summary>
    /// <param name="name">자식의 이름</param>
    /// <param name="recursive">재귀적 탐색 여부</param>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform child = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T child in go.GetComponentsInChildren<T>())
                if (string.IsNullOrEmpty(name) || child.name == name)
                    return child;
        }

        return null;
    }

    /// <summary>
    /// Game Object 전용 FindChild
    /// </summary>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) return null;
        return transform.gameObject;
    }


    /// <summary>
    /// json 파일 파싱하여 오브젝트로 반환<br/>
    /// * 필요 클래스<br/>
    /// {0} - 실제 데이터 가지고 있는 클래스<br/>
    /// {0}Handler - member로 이름이 ({0}s).ToLower() 인 List&lt;{0}&gt; 필요<br/>
    ///<br/>
    /// * json 파일<br/>
    /// Assets/Resources/Jsons/{0}s.json<br/>
    /// <br/>
    /// ex)<br/>
    /// {0} = MonsterStat인 경우,<br/>
    /// <br/>
    /// class MonsterStat - 데이터 가지고 있는 클래스<br/>
    /// class MonsterStatHandler { public List&lt;MonsterStat&gt; monsterstats; }<br/>
    /// Assets/Resources/Jsons/MonsterStats.json
    /// </summary>
    /// <typeparam name="Handler">{0}Handler</typeparam>
    public static Handler ParseJson<Handler>(string path = null, string handle = null)
    {
        if (string.IsNullOrEmpty(path))
        {
            string name = typeof(Handler).Name;
            int idx = name.IndexOf("Handler");

            path = string.Concat(name.Substring(0, idx), 's');
            handle = path.ToLower();
            Debug.Log(handle);
        }
        else if (string.IsNullOrEmpty(handle))
        {
            string name = typeof(Handler).Name;
            int idx = name.IndexOf("Handler");
            handle = string.Concat(name.Substring(0, idx), 's').ToLower();
        }

        TextAsset jsonTxt = Resources.Load<TextAsset>($"Jsons/{path}");
        if (jsonTxt == null)
        {
            Debug.LogError($"Can't load json : {path}");
            return default(Handler);
        }

        
        Resources.UnloadAsset(jsonTxt);
        try{
            return JsonUtility.FromJson<Handler>($"{{\"{handle}\" : {jsonTxt.text} }}");
        }
        catch(Exception e)
        {
            Debug.Log("똑띠하쇼!");
            return default(Handler);
        }
        finally
        {
            Resources.UnloadAsset(jsonTxt);
        }
    }



    ///////////////////////////////////////////

    public static void SaveJson<T>(T _data, string _name)
    {
        string _jsonText;


        //안드로이드에서의 저장 위치를 다르게 해주어야 한다
        //Application.dataPath를 이용하면 어디로 가는지는 구글링 해보길 바란다
        //안드로이드의 경우에는 데이터조작을 막기위해 2진데이터로 변환을 해야한다
        
        string _savePath;
        string _appender = "/userData/";
        string _nameString = _name + ".json";

#if UNITY_EDITOR
        _savePath = Application.dataPath;
#elif UNITY_ANDROID
        _savePath = Application.persistentDataPath;
#endif  


        StringBuilder _builder = new StringBuilder(_savePath);
        _builder.Append(_appender);
        if (!Directory.Exists(_builder.ToString()))
        {
            //디렉토리가 없는경우 만들어준다
            Debug.Log("뭐야");
            Directory.CreateDirectory(_builder.ToString());

        }
        _builder.Append(_nameString);


        

        //stringBuilder는 최적화에 좋대서 쓰고있다. string+string은 메모리낭비가 심하다
        // 사실 이정도 한두번 쓰는건 상관없긴한데 그냥 써주자. 우리의 컴은 좋으니까..
        _jsonText = JsonUtility.ToJson(_data, true);

        //이러면은 일단 데이터가 텍스트로 변환이 된다
        //jsonUtility를 이용하여 data인 WholeGameData를 json형식의 text로 바꾸어준다
        //파일스트림을 이렇게 지정해주고 저장해주면된당 끗
        FileStream _fileStream = new FileStream(_builder.ToString(), FileMode.Create);


        byte[] _bytes = Encoding.UTF8.GetBytes(_jsonText);
        _fileStream.Write(_bytes, 0, _bytes.Length);
        _fileStream.Close();
    }

    //    public SaveDataTimeWrapper LoadSaveDataTime()
    //    {
    //        //이제 우리가 이전에 저장했던 데이터를 꺼내야한다
    //        //만약 저장한 데이터가 없다면? 이걸 실행 안하고 튜토리얼을 실행하면 그만이다. 그 작업은 씬로더에서 해준다
    //        SaveDataTimeWrapper gameData;
    //        string loadPath = Application.dataPath;
    //        string directory = "/userData";
    //        string appender = "/SaveDataTimeWrapper.json";
    //#if UNITY_EDITOR_WIN

    //#endif

    //#if UNITY_ANDROID
    //        //loadPath = Application.persistentDataPath;


    //#endif
    //        StringBuilder builder = new StringBuilder(loadPath);
    //        builder.Append(directory);
    //        //위까지는 세이브랑 똑같다
    //        //파일스트림을 만들어준다. 파일모드를 open으로 해서 열어준다. 다 구글링이다
    //        string builderToString = builder.ToString();
    //        if (!Directory.Exists(builderToString))
    //        {
    //            //디렉토리가 없는경우 만들어준다
    //            Directory.CreateDirectory(builderToString);

    //        }
    //        builder.Append(appender);

    //        if (File.Exists(builder.ToString()))
    //        {
    //            //세이브 파일이 있는경우

    //            FileStream stream = new FileStream(builder.ToString(), FileMode.Open);

    //            byte[] bytes = new byte[stream.Length];
    //            stream.Read(bytes, 0, bytes.Length);
    //            stream.Close();
    //            string jsonData = Encoding.UTF8.GetString(bytes);

    //            //텍스트를 string으로 바꾼다음에 FromJson에 넣어주면은 우리가 쓸 수 있는 객체로 바꿀 수 있다
    //            gameData = JsonUtility.FromJson<SaveDataTimeWrapper>(jsonData);
    //        }
    //        else
    //        {
    //            //세이브파일이 없는경우
    //            gameData = new SaveDataTimeWrapper();
    //        }
    //        return gameData;
    //        //이 정보를 게임매니저나, 로딩으로 넘겨주는 것이당
    //    }


    //로딩, 게임매니저에서 호출
    public static T LoadSaveData<T>(string _name)
    {
        //이제 우리가 이전에 저장했던 데이터를 꺼내야한다
        

        T _gameData;
        string _loadPath = Application.dataPath;
        string _directory = "/userData";
        string _appender = "/";
        string _dotJson = ".json";



#if UNITY_ANDROID
        _loadPath = Application.persistentDataPath;
#endif
        StringBuilder _builder = new StringBuilder(_loadPath);
        _builder.Append(_directory);
        //위까지는 세이브랑 똑같다
        //파일스트림을 만들어준다. 파일모드를 open으로 해서 열어준다. 다 구글링이다
        string _builderToString = _builder.ToString();
        if (!Directory.Exists(_builderToString))
        {
            //디렉토리가 없는경우 만들어준다
            Directory.CreateDirectory(_builderToString);

        }
        _builder.Append(_appender);
        _builder.Append(_name);
        _builder.Append(_dotJson);

        if (File.Exists(_builder.ToString()))
        {
            //세이브 파일이 있는경우

            FileStream _stream = new FileStream(_builder.ToString(), FileMode.Open);

            byte[] _bytes = new byte[_stream.Length];
            _stream.Read(_bytes, 0, _bytes.Length);
            _stream.Close();
            string _jsonData = Encoding.UTF8.GetString(_bytes);

            //텍스트를 string으로 바꾼다음에 FromJson에 넣어주면은 우리가 쓸 수 있는 객체로 바꿀 수 있다
            _gameData = JsonUtility.FromJson<T>(_jsonData);
        }
        else
        {
            //세이브파일이 없는경우
            _gameData = default(T);
        }
        return _gameData;
        //이 정보를 게임매니저나, 로딩으로 넘겨주는 것이당
    }
    //////////////////////////////////////////
    /// <summary>
    /// /
    /// </summary>
    /// <param name="LayerMask"></param>
    /// <returns></returns>


    public static GameObject GetObjRaycast2D(int LayerMask = (1<<0))
    {
        GameObject target = null;


        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit;
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask);

        if (hit) //마우스 근처에 오브젝트가 있는지 확인
        {
            //있으면 오브젝트를 저장한다.
            target = hit.collider.gameObject;
        }
        return target;
    }
    public static GameObject GetObjRaycast3D(int LayerMask = (1<<0))
    {
        GameObject target = null;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask))
        {
            target = hit.transform.gameObject;
            // Do something with the object that was hit by the raycast.
        }

        return target;
    }

    
    public static IEnumerator SmoothMove(Transform transform, Vector3 start, Vector3 end, float moveTime = 1.0f)
    {

        float startTime = Time.time;
        float endTime = startTime + moveTime;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / moveTime;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }
    public static IEnumerator SmoothMoveUI(RectTransform transform, Vector3 start, Vector3 end, float moveTime = 1.0f)
    {

        float startTime = Time.time;
        float endTime = startTime + moveTime;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / moveTime;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }

}