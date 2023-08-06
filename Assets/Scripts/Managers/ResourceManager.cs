using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ResourceManager
{
    /// <summary> 로드한 적 있는 object cache </summary>
    Dictionary<string, UnityEngine.Object> _cache = new Dictionary<string, UnityEngine.Object>();
    //Dictionary<Define.PoolingType, Dictionary<string,GameObject>> _pooling{ get; } = new Dictionary<Define.PoolingType, Dictionary<string, GameObject>>();
    Dictionary<Define.PoolingType, Stack<GameObject>> _poolingStack { get; } = new Dictionary<Define.PoolingType, Stack<GameObject>>();  

    GameObject _rootPool;
    GameObject RootPool { get { Init();return _rootPool; } }

    public void Init()
    {
        if (_rootPool == null)
        {
            _rootPool = GameObject.Find("RootPool");
            if (_rootPool == null)
            {
                _rootPool = new GameObject { name = "RootPool" };
            }
            UnityEngine.Object.DontDestroyOnLoad(_rootPool);
        }
    }

    /// <summary> 
    /// Resources.Load로 불러오기
    /// </summary>
    public T Load<T>(string path) where T : UnityEngine.Object
    {

        if (!_cache.ContainsKey(path))
        {
            _cache.Add(path, Resources.Load<T>(path));
        }

        return _cache[path] as T;
    }

    public GameObject Instantiate(string path, Transform parent = null) => Instantiate<GameObject>(path, parent);
 
    public T Instantiate<T>(string path, Transform parent = null, Define.PoolingType poolingType = Define.PoolingType.DontPool) where T : UnityEngine.Object
    {
        T prefab = Load<T>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }
        string name = prefab.name;

        if (poolingType == Define.PoolingType.DontPool)
        {
            T instance = UnityEngine.Object.Instantiate<T>(prefab, parent);
            instance.name = name;

            return instance;
        }
        else
        {
            if(_poolingStack[poolingType] == null)
            {
                _poolingStack[poolingType] = new Stack<GameObject>();
            }

            if (_poolingStack[poolingType].Count != 0)
            {
                GameObject instance = _poolingStack[poolingType].Pop();
                instance.SetActive(true);
                instance.name = name;

                instance.transform.parent = parent;
                return instance as T;
            }
            else
            {
                T instance = UnityEngine.Object.Instantiate<T>(prefab, parent);
                instance.name = name;

                return instance;
            }
        }

    }

    public void Destroy(GameObject go, Define.PoolingType poolingType = Define.PoolingType.DontPool)    
    {
        if (go == null)
            return;
        if (poolingType == Define.PoolingType.DontPool)
        {
            UnityEngine.Object.Destroy(go);
        }
        else
        {
            go.transform.parent = RootPool.transform;
            go.gameObject.SetActive(false);
            _poolingStack[poolingType].Push(go);

        }


    }
    /// <summary>
    /// Cache 초기화 (맵 이동, 메모리 초과 상황)
    /// </summary>
    public void Clear()
    {
        _cache.Clear();


        int childCount = RootPool.transform.childCount;
        GameObject[] childObjects = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childObjects[i] = RootPool.transform.GetChild(i).gameObject;
        }
        foreach(GameObject go in childObjects)
        {
            UnityEngine.Object.Destroy(go);
        }

        _poolingStack.Clear();

    }
}