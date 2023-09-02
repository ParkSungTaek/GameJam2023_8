
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;
using static Define;

public class InGameDataManager
{
    #region 현재 플레이중인 음악 
    public const int TYPENUM = 4;
    public Define.SoundtrackType0 SoundtrackType0 { get; set; }
    public Define.SoundtrackType1 SoundtrackType1 { get; set; }
    public Define.SoundtrackType2 SoundtrackType2 { get; set; }
    public Define.SoundtrackType3 SoundtrackType3 { get; set; }
    #endregion 현재 플레이중인 음악 

    #region Player
    public PlayerController[] SoundPlayer { get; set; }
    const int PlayerNum = 4;
    #endregion Player

    public static bool IsAutoPlay { get; set; } = false;

    #region 업적 시스템
    /// <summary>
    /// 반드시 4개를 전부 적을것 {"None","Button6","None","Button18"} 같이;
    /// </summary>
    public class Achievement
    {
        public string Track0;
        public string Track1;
        public string Track2;
        public string Track3;
    }

    public ButtonData[] ButtonDatas { get; set; }

    const int Achievementnum = 8;

    public Achievement[] Achievements { get; private set; } = new Achievement[Achievementnum]
    {
        new Achievement { Track0 = "Button0", Track1 = "Button6",  Track2 = "Button12", Track3 = "Button18" },
        new Achievement { Track0 = "Button1",  Track1 = "Button7", Track2 = "Button13",    Track3 = "Button19" },
        new Achievement { Track0 = "Button2",  Track1 = "Button8",    Track2 = "Button14",    Track3 = "Button20" },
        new Achievement { Track0 = "Button3", Track1 = "Button9",  Track2 = "Button15",    Track3 = "Button21" },
        new Achievement { Track0 = "Button0",  Track1 = "Button7",    Track2 = "Button15",    Track3 = "Button21" },
        new Achievement { Track0 = "Button1",  Track1 = "Button9",    Track2 = "Button14",    Track3 = "Button19" },
        new Achievement { Track0 = "Button2",  Track1 = "Button8",    Track2 = "Button12",    Track3 = "Button19" },
        new Achievement { Track0 = "Button3",  Track1 = "Button7",    Track2 = "Button14",    Track3 = "Button20" },
    };
    int[] openedAchievements = new int[Achievementnum];
    public bool GetActiveAchievements(int idx)
    {
        //return PlayerPrefs.GetInt($"GetActiveAchievements{idx}", 0) == 1;
        return openedAchievements[idx] == 1;
    }
    public void SetActiveAchievements(int idx,bool input = true)
    {
        if(input)
        {
            PlayerPrefs.SetInt($"GetActiveAchievements{idx}", 1);
            ActiveAchievementsIndex.Add(idx);
            openedAchievements[idx] = 1;
        }

        else
        {
            PlayerPrefs.SetInt($"GetActiveAchievements{idx}", 0);
            openedAchievements[idx] = 0;
        }
            
    }
    public List<int> ActiveAchievementsIndex { get; set; } = new List<int>();
    #endregion


    /// <summary> InGameData 게임 시작시 초기화</summary>
    public void Init()
    {
        SoundPlayer = new PlayerController[PlayerNum];

        for(int i = 0; i < PlayerNum; i++)
        {
            SoundPlayer[i] = GameObject.Find($"Player{i}")?.GetComponent<PlayerController>();
            
        }
        for(int i=0;i< Achievementnum; i++)
        {
            openedAchievements[i] = PlayerPrefs.GetInt($"GetActiveAchievements{i}", 0);
        }

        SoundtrackType0 = Define.SoundtrackType0.MaxCount;
        SoundtrackType1 = Define.SoundtrackType1.MaxCount;
        SoundtrackType2 = Define.SoundtrackType2.MaxCount;
        SoundtrackType3 = Define.SoundtrackType3.MaxCount;

        ButtonDatas = new ButtonData[24];




        ButtonDatas[6] = new ButtonData();
        ButtonDatas[6].sprite = GameManager.Resource.Load<Sprite>("Sprites/Objects/city_00_01");
        ButtonDatas[6].color = Color.white;
        ButtonDatas[7] = new ButtonData();
        ButtonDatas[7].sprite = GameManager.Resource.Load<Sprite>("Sprites/Objects/city_00_02");
        ButtonDatas[7].color = Color.white;


        ButtonDatas[12] = new ButtonData();
        ButtonDatas[12].sprite = GameManager.Resource.Load<Sprite>("Sprites/Objects/sun_00_2");
        ButtonDatas[12].color = Color.white; 
        ButtonDatas[13] = new ButtonData();
        ButtonDatas[13].sprite = GameManager.Resource.Load<Sprite>("Sprites/Objects/sun_01");
        ButtonDatas[13].color = Color.white;

        SetActiveAchievements(0, true);
        SetActiveAchievements(1, true);
        SetActiveAchievements(2, true);


        for (int i=0;i< Achievementnum; i++)
        {
            if (GetActiveAchievements(i))
            {
                ActiveAchievementsIndex.Add(i);
            }
        }
    }
    

    /// <summary> 게임 플레이 정보 초기화 </summary>
    public void Clear()
    {

    }
}


