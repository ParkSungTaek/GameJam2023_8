using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;
using static RecordController;

public class RecordController : MonoBehaviour
{

    static RecordController _instance;
    public static RecordController Instance { get { Init(); return _instance; } private set { _instance = value; } }
    RecordController() { }

    public static bool IsREC { get; set; } = false;

    RecordData nowREC;
    float RECStartTime;
    string Name;

    Coroutine[] CommendList;
    [Serializable]
    public class SaveDataHandler
    {
        public List<RecordData> DataList = new List<RecordData>();

    }

    SaveDataHandler RecordedList { get; set; }
    [Serializable]
    public class RecordData
    {
        public string name;
        public bool startA;
        public int[] StartSet = new int[4];

        public List<RecordDataPacket> SaveDataPackets = new List<RecordDataPacket>();

    }
    [Serializable]
    public class RecordDataPacket
    {
        public float Time;
        public int Protocol;
        public int Input;
    }
    static int idx = 0;
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

            Instance.Name = PlayerPrefs.GetString("Record", $"{idx}");
            //Instance.SavedData = Util.ParseJson<SaveDataHandler>();
            Instance.RecordedList = new SaveDataHandler();
        }
        
    }


    public static void RecordToggle()
    {
        if (!IsREC)
        {
            IsREC = true;
            if (SyncController.APart)
            {
                SyncController.JobCollector_Start_B += StartREC;
            }
            else
            {
                SyncController.JobCollector_Start_A += StartREC;
            }
        }
        else
        {
            IsREC = false;
            if (SyncController.APart)
            {
                SyncController.JobCollector_Start_B += EndREC;
            }
            else
            {
                SyncController.JobCollector_Start_A += EndREC;
            }
        }
    }

    public static void StartREC()
    {
        Debug.Log("RECSTART!");
        IsREC = true;
        Instance.nowREC = new RecordData();
        Instance.RECStartTime = Time.time;
        Instance.nowREC.startA = SyncController.APart;
        for(int i = 0; i < 4; i++)
        {
            Instance.nowREC.StartSet[i] = GameUI.Instance.PlayButton(i);
        }
        SyncController.JobCollector_Start_A -= StartREC;
        SyncController.JobCollector_Start_B -= StartREC;

    }
    public static void REC(Define.RecordProtocol recordMethod,int buttonName)
    {

        if (IsREC && Instance.nowREC != null)
        {

            Instance.rec(recordMethod, buttonName);
        }
            
    }
    public static void EndREC()
    {
        IsREC = false;
        Instance.endREC();
        SyncController.JobCollector_Start_A -= EndREC;
        SyncController.JobCollector_Start_B -= EndREC;

    }



    void rec(Define.RecordProtocol recordMethod, int buttonName = -1)
    {
        RecordDataPacket recordDataPacket = new RecordDataPacket();
        recordDataPacket.Time = Time.time - Instance.RECStartTime;
        recordDataPacket.Protocol = (int)recordMethod;
        recordDataPacket.Input = buttonName;
        Instance.nowREC.SaveDataPackets.Add(recordDataPacket);

        Debug.Log(recordDataPacket);
    
    
    }


    void endREC()
    {
        IsREC = false;
        Debug.Log(Instance.nowREC.SaveDataPackets);
        Instance.RecordedList.DataList.Add(Instance.nowREC);
        Instance.nowREC = null;
        
    }

    
    IEnumerator PlayPacket(RecordDataPacket recordDataPacket)
    {
        //GameUI.Instance.
        yield return new WaitForSeconds(recordDataPacket.Time);

        switch (recordDataPacket.Protocol) 
        {
            case (int)Define.RecordProtocol.Add:
                GameUI.Instance.Add(recordDataPacket.Input);
                break;

            case (int)Define.RecordProtocol.Delete:
                GameUI.Instance.Delete(recordDataPacket.Input);
                break;

            case (int)Define.RecordProtocol.Volume_Zero:
                GameUI.Instance.Volume_Zero(recordDataPacket.Input);
                break;
            
            case (int)Define.RecordProtocol.Volume_Re:
                GameUI.Instance.Volume_Re(recordDataPacket.Input);
                break;
        }
    }
    public void PlayRecordedMusic()
    {
        RecordData recordData = RecordedList.DataList[0];
        for(int i = 0; i < 4; i++)
        {

            GameUI.Instance.Add(recordData.StartSet[i]);
        }
        CommendList = new Coroutine[recordData.SaveDataPackets.Count];
        for (int i = 0;i< recordData.SaveDataPackets.Count;i++)
        {
            CommendList[i] = StartCoroutine(PlayPacket(recordData.SaveDataPackets[i]));
        }   
    }



}
