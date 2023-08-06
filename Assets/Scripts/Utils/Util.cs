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
        string json = jsonTxt.text;
        return JsonUtility.FromJson<Handler>($"{{\"{handle}\" : {json} }}");
    }
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
}