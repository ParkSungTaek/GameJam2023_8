using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordController : MonoBehaviour
{

    static RecordController _instance;
    public static RecordController Instance { get { return _instance; } private set { _instance = value; } }
    RecordController() { }

    public bool IsREC { get; set; } = false;


    public class SaveDataNameHandler
    {
        public List<SaveDataName> NameList = new List<SaveDataName>();

    }

    [Serializable]
    public class SaveDataName
    {
        public string name;
    }





    public class SaveDataHandler
    {
        public List<SaveData> DataList = new List<SaveData>();

    }

    [Serializable]
    public class SaveData
    {
        public float DeltaTime;
        public Action DoAction;
    }

    public static void StartREC(Define.RecordMethod recordMethod,int buttonName)
    {
        Instance.startREC();
    }
    public static void EndREC()
    {
        Instance.endREC();
    }



    void startREC()
    {
        IsREC = true;
    }


    void endREC()
    {
        IsREC = false;
    }

    public SaveDataHandler now = new SaveDataHandler();


    private void Start()
    {
        StartCoroutine(after());

    }

    IEnumerator after()
    {
        yield return new WaitForSeconds(5f);
        string tmp = JsonUtility.ToJson(now);
        Debug.Log(tmp);
        
    }
}
