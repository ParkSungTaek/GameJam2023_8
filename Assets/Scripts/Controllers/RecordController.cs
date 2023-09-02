using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordController : MonoBehaviour
{

    static RecordController _instance;
    public static RecordController Instance { get { Init(); return _instance; } private set { _instance = value; } }
    RecordController() { }

    public bool IsREC { get; set; } = false;

    SaveData nowREC;
    float RECStartTime;

    public class SaveDataHandler
    {
        public List<SaveData> DataList = new List<SaveData>();

    }
    [Serializable]
    public class SaveData
    {
        public string name;
        public bool startA;
        public List<SaveDataPacket> SaveDataPackets = new List<SaveDataPacket>();

    }
    [SerializeField]
    public class SaveDataPacket
    {
        float Time;
        int Protocol;
        int Input;
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject gm = GameObject.Find("RecordController");
            if (gm == null)
            {
                gm = new GameObject { name = "RecordController" };
                gm.AddComponent<RecordController>();
            }
            DontDestroyOnLoad(gm);
            _instance = gm.GetComponent<RecordController>();
        }
        
    }




    public static void StartREC(Define.RecordMethod recordMethod, int buttonName)
    {
        Instance.nowREC = new SaveData();
    }
    public static void REC(Define.RecordMethod recordMethod,int buttonName)
    {
        Instance.rec(recordMethod, buttonName);
    }
    public static void EndREC()
    {
        Instance.endREC();
    }



    void rec(Define.RecordMethod recordMethod, int buttonName)
    {
        IsREC = true;
    }


    void endREC()
    {
        IsREC = false;
    }

    
}
