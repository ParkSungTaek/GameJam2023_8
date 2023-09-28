using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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

    RecordDataHandler.RecordData nowREC;
    float RECStartTime;
    string Name;

    Coroutine[] CommendList;
    
    public RecordDataHandler RecordedList { get; set; }
    
    static int idx = 0;

    static string path = Application.dataPath + "/Resources/Jsons/RecordedDatas.json";
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

            string json = System.IO.File.ReadAllText(path);
            Instance.RecordedList = JsonUtility.FromJson<RecordDataHandler>(json);

            


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
        Instance.nowREC = new RecordDataHandler.RecordData();
        Instance.RECStartTime = Time.time;
        Instance.nowREC.startA = SyncController.APart;

        for(int i = 0; i < 4; i++)
        {
            Instance.nowREC.NowPlaying[i] = GameUI.Instance.NowAudioPlaying[i];
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
        RecordDataHandler.RecordDataPacket recordDataPacket = new RecordDataHandler.RecordDataPacket();
        recordDataPacket.Time = Time.time - Instance.RECStartTime;
        recordDataPacket.Protocol = (int)recordMethod;
        recordDataPacket.Input = buttonName;
        Instance.nowREC.SaveDataPackets.Add(recordDataPacket);

        Debug.Log(recordDataPacket);
    
    
    }


    void endREC()
    {
        IsREC = false;
        Instance.nowREC.name = $"HelloWorld{RecordedList.recorddatas.Count}";
        Instance.RecordedList.recorddatas.Add(Instance.nowREC);
        
        string RecordedData = JsonUtility.ToJson(RecordedList);
        System.IO.File.WriteAllText(path, RecordedData);

        if(GameManager.UI.GetRecentPopup<SavedFiles>() != null)
        {
            GameManager.UI.GetRecentPopup<SavedFiles>()?.SetTxt();

        }
        else
        {
            Debug.Log("WTF??");
        }

        Debug.Log(Instance.nowREC.SaveDataPackets.Count);
        Instance.nowREC = null;
        
    }

    
    IEnumerator PlayPacket(RecordDataHandler.RecordDataPacket recordDataPacket)
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

  
    public void PlayRecordedMusic(RecordDataHandler.RecordData recordData)
    {
        Debug.Log("RecordedMusicPlay!!");
        for (int i = 0; i < 4; i++)
        {
            if (recordData.NowPlaying[i])
            {
                GameUI.Instance.Volume_Re(i);
            }
            else
            {
                GameUI.Instance.Volume_Zero(i);

            }

            //GameUI Buttons.None == 24
            if (recordData.StartSet[i] == (int)GameUI.Buttons.None)
            {
                GameUI.Instance.ButtonAction((int)GameUI.Instance.buttons[i]);
            }
            GameUI.Instance.Add(recordData.StartSet[i]);

        }
        CommendList = new Coroutine[recordData.SaveDataPackets.Count];
        for (int i = 0; i < recordData.SaveDataPackets.Count; i++)
        {
            CommendList[i] = StartCoroutine(PlayPacket(recordData.SaveDataPackets[i]));
        }

    }
    public void EndRecordedMusic()
    {
        Debug.Log("RecordedMusicEnd!!");
        for (int i = 0; i < CommendList.Length; i++)
        {
            if (CommendList[i] != null)
            {
                StopCoroutine(CommendList[i]);
            }
        }
    }
}
