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
    /// json을 Wirte하지 않고 읽기만 하는것을 전제로 빠르게 사용 가능한 Json Parse
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
            Debug.Log("????");
            return default(Handler);
        }
        finally
        {
            Resources.UnloadAsset(jsonTxt);
        }
    }







    ///////////////////////////////////////////
    /// <summary>
    /// Json 정보를 Write하는 방법 
    /// UNITY_EDITOR 와 UNITY_ANDROID를 기준으로 작성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_data"></param>
    /// <param name="_name"></param>
    public static void SaveJson<T>(T _data, string _name)
    {
        string _jsonText;

        //안드로이드에서의 저장 위치를 다르게 해주어야 한다
        //안드로이드의 경우에는 데이터조작을 막기위해 2진데이터로 변환을 해야한다
        Debug.Log("??");
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
            Debug.Log("No Directory");
            Directory.CreateDirectory(_builder.ToString());

        }
        _builder.Append(_nameString);

        _jsonText = JsonUtility.ToJson(_data, true);

        using (FileStream _fileStream = new FileStream(_builder.ToString(), FileMode.Create))
        {
            byte[] _bytes = Encoding.UTF8.GetBytes(_jsonText);
            _fileStream.Write(_bytes, 0, _bytes.Length);
            _fileStream.Close();
        }

    }

    /// <summary>
    /// Json 정보를 Read하는 방법 
    /// UNITY_EDITOR 와 UNITY_ANDROID를 기준으로 작성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static T LoadSaveData<T>(string _name)
    {
        
        T _gameData;
        string _loadPath;
        string _directory = "/userData";
        string _appender = "/";
        string _dotJson = ".json";

#if UNITY_EDITOR
        _loadPath = Application.dataPath;
#elif UNITY_ANDROID
        _loadPath = Application.persistentDataPath;
#endif  

        StringBuilder _builder = new StringBuilder(_loadPath);
        _builder.Append(_directory);

        string _builderToString = _builder.ToString();
        if (!Directory.Exists(_builderToString))
        {
            Directory.CreateDirectory(_builderToString);

        }
        _builder.Append(_appender);
        _builder.Append(_name);
        _builder.Append(_dotJson);

        if (File.Exists(_builder.ToString()))
        {
            //세이브 파일이 있는경우

            using (FileStream _stream = new FileStream(_builder.ToString(), FileMode.Open))
            {
                byte[] _bytes = new byte[_stream.Length];
                _stream.Read(_bytes, 0, _bytes.Length);
                _stream.Close();
                string _jsonData = Encoding.UTF8.GetString(_bytes);
                //텍스트를 string으로 바꾼다음에 FromJson에 넣어주면은 우리가 쓸 수 있는 객체로 바꿀 수 있다
                _gameData = JsonUtility.FromJson<T>(_jsonData);
            }

        }
        else
        {
            //세이브파일이 없는경우
            _gameData = default(T);
        }
        return _gameData;
    }

    //////////////////////////////////////////
    /// <summary>
    /// 
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