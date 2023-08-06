using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncController : MonoBehaviour
{
    #region Singleton
    static SyncController _sync;
    static SyncController sync { get { Init();  return _sync; } set { _sync = value; } } 
    SyncController() { }
    #endregion Singleton


    #region Data
    public const float SyncTime = 3f;

    #endregion Data

    #region Initiator
    static void Init()
    {
        if(_sync == null)
        {
            GameObject gm = GameObject.Find("SyncController");
            if(gm == null)
            {
                gm = new GameObject { name = "SyncController" };
                gm.AddComponent<SyncController>();
            }
            _sync = gm.GetComponent<SyncController>();

        }
    }
    #endregion Initiator

    #region SyncSystem
    public static Action JobCollector_Start { get; set; }
    public static Action JobCollector_End { get; set; }

    Coroutine coroutine;
    
    public static void Flush()
    {
        sync.coroutine = sync.StartCoroutine(SyncSystem());
    }
    public static void EndFlush()
    {
        sync.StopCoroutine(sync.coroutine);
    }

    WaitForSeconds seconds = new WaitForSeconds(SyncTime);
    static IEnumerator SyncSystem()
    {
        while (true)
        {
            JobCollector_Start?.Invoke();
            yield return sync.seconds;
            JobCollector_End?.Invoke();
        }

    }
    #endregion SyncSystem
}
