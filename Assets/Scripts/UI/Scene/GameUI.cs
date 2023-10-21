using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System;
using static Define;
using System.Reflection;
using System.Collections;

public class GameUI : UI_Scene
{
    #region Singleton 
    static GameUI _instance;
    public static GameUI Instance { get { return _instance; } }
    GameUI() { }
    #endregion Singleton 


    #region Data
    const int TYPENUM = InGameDataManager.TYPENUM;
    string _clickButtonName { get; set; }
    bool[] RemoteButtonHold { get; set; } = { false, false, false, false };
    bool[] RemoteButtonClick { get; set; } = { false, false, false, false };

    /// <summary>
    /// 무슨 버튼을 눌렀는지 기억
    /// </summary>
    public Buttons[] buttons { get; set; } = new Buttons[TYPENUM];
    /// <summary>
    /// 현재 idx 플레이어에서 어떤 노래를 플레이중인가?
    /// </summary>
    /// <param name="idx">플레이어 idx </param>
    /// <returns></returns>
    public int PlayButton(int idx) { return (int)buttons[idx]; }
    /// <summary>
    /// 현재 해당 오디오가 Play중인가?
    /// </summary>
    public bool[] NowAudioPlaying { get; set; } = new bool[TYPENUM] { true, true, true, true };
    const float deltaTime = 0.5f;
    GameObject[] SliderBackground { get; set; } = new GameObject[TYPENUM];

    Color ActiveColor = new Color(1, 1, 1, 0.5f);
    #endregion Data

    enum Images
    {
        SKY,
        SUN,
        CITY,
    }
    enum GameObjects
    {
        Road,
    }
    public enum Buttons
    {
        Button0,
        Button1,
        Button2,
        Button3,
        Button4,
        Button5,
        
        Button6,
        Button7,
        Button8,
        Button9,
        Button10,
        Button11,

        Button12,
        Button13,
        Button14,
        Button15,
        Button16,
        Button17,

        Button18,
        Button19,
        Button20,
        Button21,
        Button22,
        Button23,

        None,
        PlayPause,
        RemoteButton0,
        RemoteButton1,
        RemoteButton2,
        RemoteButton3,
    }



    enum Texts
    {
    }
    enum Sliders
    {
        Slider0, 
        Slider1, 
        Slider2, 
        Slider3,
        Slider,
    }

    bool[] _activeButtons = new bool[Enum.GetValues(typeof(Buttons)).Length];

    public void SetRoadColor(Color color)
    {
        Get<GameObject>((int)GameObjects.Road).GetComponent<Image>().color = color;
    }
    

    private void Awake()
    {
        Init();
        
    }
    public override void Init()
    {
        base.Init();
        
        _instance = this;
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
        Bind<Image>(typeof(Images));

        ButtonBind();

        for(int i = 0; i <= (int)Buttons.Button23; i++)
        {
            _activeButtons[i] = false;
        }
        for(int i = 0; i < TYPENUM; i++)
        {
            buttons[i] = Buttons.None;
        }
        for (int i = 0; i < TYPENUM; i++)
        {
            Get<Slider>(i).value = 0;
            SliderBackground[i] = Get<Slider>(i).transform.Find("Background").gameObject;
            SliderBackground[i].SetActive(false);

        }


        SyncController.JobCollector_Start_A += () => { Get<Slider>((int)Sliders.Slider).value = 0; };
        SyncController.JobCollector_Start_B += () => { Get<Slider>((int)Sliders.Slider).value = 1; };

        SyncController.JobCollector_End_A += () => {
            for (int i = (int)Sliders.Slider0; i <= (int)Sliders.Slider3; i++)
            {
                SliderBackground[i].SetActive(false);
                Get<Slider>(i).value = 0;
            }

        };
        SyncController.JobCollector_End_B += () => {
            for (int i = (int)Sliders.Slider0; i <= (int)Sliders.Slider3; i++)
            {
                SliderBackground[i].SetActive(false);
                Get<Slider>(i).value = 0;
            }
        };
    }



    #region Buttons
    void ButtonBind()
    {
        for(int i = 0; i < 24; i++)
        {
            BindEvent(GetButton(i).gameObject, Down, Define.UIEvent.Down);
            //BindEvent(GetButton(i).gameObject, Up, Define.UIEvent.Up);

        }
        
        BindEvent(GetButton((int)Buttons.PlayPause).gameObject, PlayPause, Define.UIEvent.Down);

        BindEvent(GetButton((int)Buttons.RemoteButton0).gameObject, RemoteButtonDown, Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.RemoteButton1).gameObject, RemoteButtonDown, Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.RemoteButton2).gameObject, RemoteButtonDown, Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.RemoteButton3).gameObject, RemoteButtonDown, Define.UIEvent.Down);

        BindEvent(GetButton((int)Buttons.RemoteButton0).gameObject, RemoteButtonUp, Define.UIEvent.Up);
        BindEvent(GetButton((int)Buttons.RemoteButton1).gameObject, RemoteButtonUp, Define.UIEvent.Up);
        BindEvent(GetButton((int)Buttons.RemoteButton2).gameObject, RemoteButtonUp, Define.UIEvent.Up);
        BindEvent(GetButton((int)Buttons.RemoteButton3).gameObject, RemoteButtonUp, Define.UIEvent.Up);

        BindEvent(GetButton((int)Buttons.RemoteButton0).gameObject, RemoteButtonUp, Define.UIEvent.Exit);
        BindEvent(GetButton((int)Buttons.RemoteButton1).gameObject, RemoteButtonUp, Define.UIEvent.Exit);
        BindEvent(GetButton((int)Buttons.RemoteButton2).gameObject, RemoteButtonUp, Define.UIEvent.Exit);
        BindEvent(GetButton((int)Buttons.RemoteButton3).gameObject, RemoteButtonUp, Define.UIEvent.Exit);


    }
    /// <summary>
    /// 중앙 Play or Pause 버튼 
    /// </summary>
    /// <param name="evt"></param>
    void PlayPause(PointerEventData evt)
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            GetButton((int)Buttons.PlayPause).GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>("Sprites/UI/midBtn_pause");
            for (int i = 0; i < TYPENUM; i++)
            {
                GameManager.Sound.AudioSources[i].UnPause();
            }

        }
        else if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            GetButton((int)Buttons.PlayPause).GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>("Sprites/UI/midBtn_play");
            
            for (int i = 0; i < TYPENUM; i++)
            {
                GameManager.Sound.AudioSources[i].Pause();
            }

        }
    }

    
    /// <summary>
    /// 음악 버튼 눌렀을때 처리
    /// </summary>
    /// <param name="evt"></param>
    void Down(PointerEventData evt)
    {
        
        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;

        Buttons button;
        int audioIdx;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            ButtonAction((int)button);

        }


    }

    Coroutine[] remoteButtonsCoroutine = new Coroutine[TYPENUM];
    void RemoteButtonDown(PointerEventData evt)
    {
        
        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;
        Buttons button;
        // 어디 오디오야?
        int audioIdx;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            audioIdx = button - Buttons.RemoteButton0;
            //노래가 안틀어져있으면 할 일이 없어
            RemoteButtonDown(audioIdx);
        }
    }
    public void RemoteButtonDown(int audioIdx)
    {
        //노래가 안틀어져있으면 할 일이 없어
        RemoteButtonClick[audioIdx] = true;
        if (buttons[audioIdx] == Buttons.None)
        {
            return;
        }
        remoteButtonsCoroutine[audioIdx] = StartCoroutine(RemoteButtonsCoroutine(audioIdx));

    }
    IEnumerator RemoteButtonsCoroutine(int audioIdx)
    {
        yield return new WaitForSeconds(deltaTime);
        //버튼 장시간 사용
        RemoteButtonHold[audioIdx] = true;
        for (int i = 0; i < TYPENUM; i++)
        {
            if (!RemoteButtonHold[i])
            {
                Volume_Zero(i);
            }
            
        }


    }
    void RemoteButtonUp(PointerEventData evt)
    {
        _clickButtonName = evt.pointerEnter.gameObject.name;
        Buttons button;
        int audioIdx;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {

            audioIdx = button - Buttons.RemoteButton0;

            if (RemoteButtonClick[audioIdx])
            {
                Debug.Log(audioIdx+ " Out");
                RemoteButtonClick[audioIdx] = false;
                
                if (buttons[audioIdx] == Buttons.None)
                {
                    return;
                }
                if (remoteButtonsCoroutine[audioIdx] != null)
                {
                    StopCoroutine(remoteButtonsCoroutine[audioIdx]);
                }
                //길게 잡았다가 떼는거라면
                if (RemoteButtonHold[audioIdx])
                {
                    RemoteButtonHold[audioIdx] = false;

                    if (NowUsingRemote())
                    {
                        Volume_Zero(audioIdx);

                    }
                    else
                    {
                        for (int i = 0; i < TYPENUM; i++)
                        {
                            if (NowAudioPlaying[i])
                            {
                                Volume_Re(i);

                            }
                        }
                    }

                }
                // 짧게 클릭
                else
                {
                    
                    //지금 노래가 나오는 중이야?
                    if (GetButton((int)Buttons.RemoteButton0 + audioIdx).GetComponent<Image>().color == Color.white)
                    {
                        //꺼
                        Volume_Zero(audioIdx);
                    }
                    else
                    {
                        //켜
                        Volume_Re(audioIdx);
                    }

                }
            }
        }
        
    }
    
    public void ButtonAction(int buttonIdx)
    {
        if (buttonIdx == (int)Buttons.None) { Debug.Log("None"); return; }
        // 내가 누른거랑 달라Buttons?
        if ((buttons[AudioIdx(buttonIdx)] != (Buttons)buttonIdx))
        {
            //노래 추가
            Add(buttonIdx);
        }
        //내가 누른거랑 같아?
        else
        {
            
            Delete(buttonIdx);
        }

    }
    



    /// <summary>
    /// 이 아이디어의 장점 : 깔끔함, 재사용성, 중복코드 감소
    /// </summary>
    /// <param name="audioIdx"></param>
    /// <param name="buttonIdx"></param>
    void Play_Action(int audioIdx,int buttonIdx,bool a)
    {
        string tmp;
        if (a)
        {
            tmp = $"S{audioIdx}_{buttonIdx % 6}_a";

        }
        else
        {
            tmp = $"S{audioIdx}_{buttonIdx % 6}_b";
        }
        SoundtrackType sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
           
            if (GetButton((int)Buttons.RemoteButton0 + audioIdx).GetComponent<Image>().color == ActiveColor)
            {
                GameManager.Sound.Play(audioIdx, sound, 0);
            }
            else
            {
                GameManager.Sound.Play(audioIdx, sound, GameManager.Sound.Volume);
            }
            //New
            TypesImage(audioIdx, buttonIdx % 6);
            
            NowAudioPlaying[audioIdx] = true;
        }
        FindAcivement();
    }


    #endregion Buttons

    #region Action
    /// <summary>
    /// 곡 넣기 & 바꾸기
    /// </summary>
    /// <param name="buttonIdx">어떤 버튼을 눌렀는가?</param>
    public void Add(int buttonIdx )//int audioIdx = 0, bool None = false)
    {
        if (buttonIdx == (int)Buttons.None) { return; } 
        //레코딩 처리 
        RecordController.REC(Define.RecordProtocol.Add, buttonIdx);

        //타임슬라이더 처리
        TimeSlider.Init();

        int audioIdx = AudioIdx(buttonIdx);


        // 지금 내 슬라이더가 안흘러가는 중이면 켜
        if (SliderBackground[audioIdx].activeSelf == false && (buttons[AudioIdx(buttonIdx)] != (Buttons)buttonIdx))
        {
            SliderBackground[audioIdx].gameObject.SetActive(true);
            FillSliderOverTime(Get<Slider>(audioIdx),audioIdx);
        }

        //전에 눌린게 있었다면 그 색은 원래 색으로 바꿔주고
        if (buttons[audioIdx] != Buttons.None) 
        { 
            GetButton((int)buttons[audioIdx]).GetComponent<Image>().color = Color.white;
        }

        //내가 누른거 활성화 색으로 바꿔줘
        GetButton(buttonIdx).GetComponent<Image>().color = ActiveColor;

        //나는 이제 이거 틀꺼야!
        buttons[audioIdx] = (Buttons)buttonIdx;

        // 예약
        switch (audioIdx)
        {
            case 0:
                SyncController.JobCollector_Start_Player0_A = null;
                SyncController.JobCollector_Start_Player0_A += () => Play_Action(audioIdx,buttonIdx,true);

                SyncController.JobCollector_Start_Player0_B = null;
                SyncController.JobCollector_Start_Player0_B += () => Play_Action(audioIdx, buttonIdx, false);
                break;
            case 1:
                SyncController.JobCollector_Start_Player1_A = null;
                SyncController.JobCollector_Start_Player1_A += () => Play_Action(audioIdx, buttonIdx, true);

                SyncController.JobCollector_Start_Player1_B = null;
                SyncController.JobCollector_Start_Player1_B += () => Play_Action(audioIdx, buttonIdx, false);
                break;
            case 2:
                SyncController.JobCollector_Start_Player2_A = null;
                SyncController.JobCollector_Start_Player2_A += () => Play_Action(audioIdx, buttonIdx, true);

                SyncController.JobCollector_Start_Player2_B = null;
                SyncController.JobCollector_Start_Player2_B += () => Play_Action(audioIdx, buttonIdx, false);
                break;
            case 3:
                SyncController.JobCollector_Start_Player3_A = null;
                SyncController.JobCollector_Start_Player3_A += () => Play_Action(audioIdx, buttonIdx, true);

                SyncController.JobCollector_Start_Player3_B = null;
                SyncController.JobCollector_Start_Player3_B += () => Play_Action(audioIdx, buttonIdx, false);
                break;

        }

    }
    /// <summary>
    /// 곡 제거
    /// </summary>
    /// <param name="buttonIdx"></param>
    public void Delete(int buttonIdx)
    {
        //레코딩 처리
        RecordController.REC(Define.RecordProtocol.Volume_Zero, buttonIdx);

        int audioIdx = AudioIdx(buttonIdx);
        
        //버튼 이미지 비 활성화
        GetButton(buttonIdx).GetComponent<Image>().color = Color.white;
        //소리끄기
        GameManager.Sound.SetVolume((Define.Sound)audioIdx, 0);
        NowAudioPlaying[audioIdx] = false;

        //나는 이제 이거 끌꺼야!
        buttons[audioIdx] = Buttons.None;

        switch (audioIdx)
        {
            case 0:
                SyncController.JobCollector_Start_Player0_A = null;
                SyncController.JobCollector_Start_Player0_B = null;
                break;
            case 1:
                SyncController.JobCollector_Start_Player1_A = null;
                SyncController.JobCollector_Start_Player1_B = null;
                break;
            case 2:
                SyncController.JobCollector_Start_Player2_A = null;
                SyncController.JobCollector_Start_Player2_B = null;
                break;
            case 3:
                SyncController.JobCollector_Start_Player3_A = null;
                SyncController.JobCollector_Start_Player3_B = null;
                break;

        }

    }
    public void Volume_Zero(int audioIdx)
    {
        RecordController.REC(Define.RecordProtocol.Volume_Zero, audioIdx);
        
        GetButton((int)Buttons.RemoteButton0 + audioIdx).GetComponent<Image>().color = ActiveColor;

        GameManager.Sound.SetVolume((Define.Sound)audioIdx, 0);
        
    }

    public void Volume_Re(int audioIdx)
    {
        RecordController.REC(Define.RecordProtocol.Volume_Re, audioIdx);

        GetButton((int)Buttons.RemoteButton0 + audioIdx).GetComponent<Image>().color = Color.white;
        
        GameManager.Sound.SetVolume((Define.Sound)audioIdx, GameManager.Sound.Volume);
        
    }

    public void PauseAll()
    {
        Time.timeScale = 0;
    }
    public void UnPauseAll()
    {
        Time.timeScale = 1.0f;

    }

    #endregion

    #region Image

    /// <summary>
    /// 버튼에 따른 이미지 변경
    /// </summary>
    /// <param name="audioIDX">오디오 타입 0~3</param>
    /// <param name="buttonIDX">버튼 타입 0~5</param>
    public void TypesImage(int audioIDX, int buttonIDX)
    {
        switch (audioIDX)
        {
            case (int)Define.Types.Land:
                GameManager.InGameData.PlayerState[(int)Define.Types.Land].StartPlayingAnim(buttonIDX);
                break;
            case (int)Define.Types.City:
                GetImage((int)Images.CITY).sprite = GameManager.Resource.Load<Sprite>($"Sprites/City/{buttonIDX}");

                break;
            case (int)Define.Types.Sun:
                GetImage((int)Images.SUN).sprite = GameManager.Resource.Load<Sprite>($"Sprites/Sun/{buttonIDX}");
                break;
            case (int)Define.Types.Sky:
                GetImage((int)Images.SKY).sprite = GameManager.Resource.Load<Sprite>($"Sprites/Sky/{buttonIDX}");

                break;
            default: break;
        }
    }

    #endregion

    int AudioIdx(int buttonIdx)
    {
        return buttonIdx / 6;
    }

    /// <summary>
    /// 개별 슬라이더 조작
    /// </summary>
    /// <param name="slider"></param>
    public void FillSliderOverTime(Slider slider, int audioIdx)
    {
        StartCoroutine(FillCoroutine(slider, audioIdx));
    }

    private IEnumerator FillCoroutine(Slider slider, int audioIdx)
    {
        float startTime = Time.time;

        float duration = SyncController.NextTick - startTime;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            slider.value = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;  // 다음 프레임까지 대기
        }

        slider.value = 1;  // 마지막 값 보장
        SliderBackground[audioIdx].SetActive(false);
        
    }

    bool NowUsingRemote()
    {
        for (int audioIdx = 0; audioIdx < TYPENUM; audioIdx++)
        {
            if (RemoteButtonHold[audioIdx])
            {
                return true;
            }
                
        }

        return false;
    }

    void FindAcivement()
    {

        for (int i=0;i < GameManager.InGameData.Achievements.Length;i++)
        {
            if (!GameManager.InGameData.GetActiveAchievements(i) && FindAcivementIDX(i))
            {
                GameManager.InGameData.SetActiveAchievements(i);
            }
        }

    }
    bool FindAcivementIDX(int idx)
    {
        if (Parse(GameManager.InGameData.Achievements[idx].Track0) != buttons[0])
        {
            return false;
        }
        if (Parse(GameManager.InGameData.Achievements[idx].Track1) != buttons[1])
        {
            return false;
        }
        if (Parse(GameManager.InGameData.Achievements[idx].Track2) != buttons[2])
        {
            return false;
        }
        if (Parse(GameManager.InGameData.Achievements[idx].Track3) != buttons[3])
        {
            return false;
        }
        return true;

    }
    Buttons Parse(string str)
    {
        Buttons button;
        if (System.Enum.TryParse(str, out button))
        {
            return button;
        }
        else
        {
            Debug.Log($"Can't Parse {str}");
            return Buttons.None;
        }
        
    }


}
