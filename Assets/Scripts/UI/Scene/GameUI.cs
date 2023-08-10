using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System;
using static Define;
using System.Reflection;


public class GameUI : UI_Scene
{
    static GameUI _instance;
    public static GameUI Instance { get { return _instance; } }
    GameUI() { }

    #region Data
    const int TYPENUM = InGameDataManager.TYPENUM;
    string _clickButtonName { get; set; }
    float[] time { get; set; } = new float[TYPENUM];
    bool[] Click { get; set; } = { false, false, false, false };
    //int _clickIndex;
    Buttons[] buttonsDown { get; set; } = new Buttons[TYPENUM];
    Buttons[] buttons = new Buttons[TYPENUM];
    bool[] _isPlaying = new bool[TYPENUM];
    public bool[] IsPlaying { get { return _isPlaying; } }
    const float deltaTime = 0.5f;
    bool[] _isWaiting = new bool[TYPENUM];
    float[] _isWaitingTime = new float[TYPENUM];
    GameObject[] SliderBackground { get; set; } = new GameObject[TYPENUM];

    #endregion Data



    enum GameObjects
    {
        Road,
    }
    enum Buttons
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

        ButtonBind();

        for(int i = 0; i <= (int)Buttons.Button23; i++)
        {
            _activeButtons[i] = false;
        }
        for(int i = 0; i < TYPENUM; i++)
        {
            buttons[i] = Buttons.None;
            _isPlaying[i] = true;
        }
        for (int i = 0; i < TYPENUM; i++)
        {
            Get<Slider>(i).value = 0;
            _isWaiting[i] = false;
            SliderBackground[i] = Get<Slider>(i).transform.Find("Background").gameObject;
            SliderBackground[i].SetActive(false);

        }


        SyncController.JobCollector_Start_A += () => { Get<Slider>((int)Sliders.Slider).value = 0; };
        SyncController.JobCollector_Start_B += () => { Get<Slider>((int)Sliders.Slider).value = 1; };

        //SyncController.JobCollector_End_A = null;
        //SyncController.JobCollector_End_B = null;
        SyncController.JobCollector_Start_A += () => {
            for (int i = (int)Sliders.Slider0; i <= (int)Sliders.Slider3; i++)
            {
                _isWaiting[i] = false;
                SliderBackground[i].SetActive(false);
                Get<Slider>(i).value = 0;
            }

        };
        SyncController.JobCollector_Start_B += () => {
            for (int i = (int)Sliders.Slider0; i <= (int)Sliders.Slider3; i++)
            {
                _isWaiting[i] = false;
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
            BindEvent(GetButton(i).gameObject, Up, Define.UIEvent.Up);

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
    /// 중앙 Play or Pause 버튼 GameUI가 통제하는것이 합리적
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
    /// SoundPlayer[0]에서 Play
    /// </summary>
    /// <param name="evt"></param>
    /*
    
    */
    
    /// <summary>
    /// 음악 버튼 눌렀을때 처리
    /// </summary>
    /// <param name="evt"></param>
    void Down(PointerEventData evt)
    {
        
        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;

        Buttons button;
        int _clickIndex;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            
            _clickIndex = ((int)button)/6;
            buttonsDown[_clickIndex] = button;
            ButtonAction(_clickButtonName);

        }


    }
    void Up(PointerEventData evt)
    {
        /*

        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;

        Buttons button;
        int _clickIndex;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            _clickIndex = ((int)button) / 6;

            //Click[_clickIndex] = false;
            // 짧게 클릭
            ButtonAction(_clickButtonName);

            //if (time[_clickIndex] + deltaTime > Time.time)
            //{
            //    ButtonAction(_clickButtonName);
            //}
            ////버튼 길게 클릭 후 떼는 시점
            //else
            //{
            //    for (int i = 0; i < TYPENUM; i++)
            //    {
            //        if (IsPlaying[i])
            //        {
            //            GameManager.Sound.SetVolume((Define.Sound)i, GameManager.Sound.Volume);
            //        }
            //    }
            //}
        }
        */

        
    }

    void RemoteButtonDown(PointerEventData evt)
    {
        
        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;
        Buttons button;
        int _clickIndex;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            _clickIndex = button - Buttons.RemoteButton0;
            if (buttons[_clickIndex] == Buttons.None)
            {
                return;
            }
            _clickButtonName = Enum.GetName(typeof(Buttons), buttons[_clickIndex]);
            
            Click[_clickIndex] = true;

            time[_clickIndex] = Time.time;
            //Down(tmp);
        }


    }
    void RemoteButtonUp(PointerEventData evt)
    {
        _clickButtonName = evt.pointerEnter.gameObject.name;
        Buttons button;
        int _clickIndex;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            
            _clickIndex = button - Buttons.RemoteButton0;
            if (Click[_clickIndex])
            {
                if (buttons[_clickIndex] == Buttons.None)
                {
                    return;
                }

                Click[_clickIndex] = false;

                _clickButtonName = Enum.GetName(typeof(Buttons), buttons[_clickIndex]);
                // 짧게 클릭
                if (time[_clickIndex] + deltaTime > Time.time)
                {
                    ButtonAction(_clickButtonName);
                }
                else
                {
                    for (int i = 0; i < TYPENUM; i++)
                    {
                        if (IsPlaying[i])
                        {
                            GameManager.Sound.SetVolume((Define.Sound)i, GameManager.Sound.Volume);
                        }
                    }
                }
            }
        }

        
    }
    private void Update()
    {
        bool isStillDown = false;
        for(int _clickIndex = 0; _clickIndex < TYPENUM; _clickIndex++)
        {
            if (Click[_clickIndex] && time[_clickIndex] + deltaTime < Time.time)
            {
                isStillDown = true;
            }
        }
        if(isStillDown) 
        {
            for (int _clickIndex = 0; _clickIndex < TYPENUM; _clickIndex++)
            {
                if (Click[_clickIndex] && time[_clickIndex] + deltaTime < Time.time)
                {
                    isStillDown = true;
                }

                GameManager.Sound.SetVolume((Define.Sound)_clickIndex, (Click[_clickIndex] && time[_clickIndex] + deltaTime < Time.time) ? GameManager.Sound.Volume : 0);
            }
        }
        for (int _clickIndex = 0; _clickIndex < TYPENUM; _clickIndex++)
        {
            if (_isWaiting[_clickIndex])
            {
                Get<Slider>(_clickIndex).value += Time.deltaTime * (1 / _isWaitingTime[_clickIndex]);

            }
        }
        


    }
    public void ButtonAction(string buttonName,bool None = false, bool PreSet = false)
    {
        string tmp = buttonName;
        Buttons button;
        if (System.Enum.TryParse(tmp, out button))
        {
            int index = (int)button;
            buttonsDown[index/6] = button;
            int beforePlaylist = (int)buttons[index / 6];

            if (!None )
            {
                // 다른 음악으로 바꿀 때
                if (index != beforePlaylist)
                {
                    ResetType(index / 6);
                    buttons[index / 6] = button;
                    _isPlaying[index / 6] = true;
                    GameManager.Sound.SetVolume((Sound)(index / 6), GameManager.Sound.Volume);

                    Color color = GetButton(index).GetComponent<Image>().color;
                    color.a = 0.5f;
                    GetButton(index).GetComponent<Image>().color = color;

                    _isWaiting[index / 6] = true;
                    _isWaitingTime[index / 6] = SyncController.NextTick-Time.time;
                    SliderBackground[index / 6].SetActive(true);

                }
                // 같은 음악 토글할 때
                else
                {
                    if (PreSet)
                    {
                        ResetType(index / 6);
                        buttons[index / 6] = button;
                        _isPlaying[index / 6] = true;
                        GameManager.Sound.SetVolume((Sound)(index / 6), GameManager.Sound.Volume);

                        Color color = GetButton(index).GetComponent<Image>().color;
                        color.a = 0.5f;
                        GetButton(index).GetComponent<Image>().color = color;

                    }
                    else
                    {
                        if (_isPlaying[index / 6])
                        {
                            _isPlaying[index / 6] = false;
                            GameManager.Sound.SetVolume((Sound)(index / 6), 0);
                            Color color = GetButton(index).GetComponent<Image>().color;
                            color.a = 1f;
                            GetButton(index).GetComponent<Image>().color = color;
                        }
                        else
                        {
                            _isPlaying[index / 6] = true;
                            GameManager.Sound.SetVolume((Sound)(index / 6), GameManager.Sound.Volume);
                            Color color = GetButton(index).GetComponent<Image>().color;
                            color.a = 0.5f;
                            GetButton(index).GetComponent<Image>().color = color;

                        }
                    }

                }
                
            }
            
            else
            {
                ResetType(index / 6);
                return;
            }


            TimeSlider.Init();

            

            if (index >= 0 && index < 6)
            {
                SyncController.JobCollector_Start_Player0_A = null;
                SyncController.JobCollector_Start_Player0_A += Play0_A;

                SyncController.JobCollector_Start_Player0_B = null;
                SyncController.JobCollector_Start_Player0_B += Play0_B;
                return;
            }
            if (index >= 6 && index < 12)
            {

                SyncController.JobCollector_Start_Player1_A = null;
                SyncController.JobCollector_Start_Player1_A += Play1_A;

                SyncController.JobCollector_Start_Player1_B = null;
                SyncController.JobCollector_Start_Player1_B += Play1_B;
                return;

            }
            if (index >= 12 && index < 18)
            {

                SyncController.JobCollector_Start_Player2_A = null;
                SyncController.JobCollector_Start_Player2_A += Play2_A;

                SyncController.JobCollector_Start_Player2_B = null;
                SyncController.JobCollector_Start_Player2_B += Play2_B;
                return;

            }
            if (index >= 18 && index < 24)
            {

                SyncController.JobCollector_Start_Player3_A = null;
                SyncController.JobCollector_Start_Player3_A += Play3_A;

                SyncController.JobCollector_Start_Player3_B = null;
                SyncController.JobCollector_Start_Player3_B += Play3_B;
                return;

            }

        }
        else
        {
            Debug.Log("변환 실패 At GameUI: " + tmp);
        }

    }

    public void ResetType(int type)
    {

        switch (type)
        {
            case 0:
                _isPlaying[0] = true;
                buttons[0] = Buttons.None;
                SyncController.JobCollector_Start_Player0_A = null;
                SyncController.JobCollector_Start_Player0_B = null;
                for (int i = 0; i < 6; i++)
                {

                    Color color = GetButton(i).GetComponent<Image>().color;
                    color.a = 1f;
                    GetButton(i).GetComponent<Image>().color = color;
                }

                break;
            case 1:
                _isPlaying[1] = true;
                buttons[1] = Buttons.None;
                SyncController.JobCollector_Start_Player1_A = null;
                SyncController.JobCollector_Start_Player1_B = null;

                for (int i = 6; i < 12; i++)
                {
                    
                    Color color = GetButton(i).GetComponent<Image>().color;
                    color.a = 1f;
                    GetButton(i).GetComponent<Image>().color = color;
                }
                break;
            case 2:
                _isPlaying[2] = true;
                buttons[2] = Buttons.None;
                SyncController.JobCollector_Start_Player2_A = null;
                SyncController.JobCollector_Start_Player2_B = null;

                for (int i = 12; i < 18; i++)
                {

                    Color color = GetButton(i).GetComponent<Image>().color;
                    color.a = 1f;
                    GetButton(i).GetComponent<Image>().color = color;
                }
                break;
            case 3:
                _isPlaying[3] = true;
                buttons[3] = Buttons.None;
                SyncController.JobCollector_Start_Player3_A = null;
                SyncController.JobCollector_Start_Player3_B = null;
                for (int i = 18; i < 24; i++)
                {

                    Color color = GetButton(i).GetComponent<Image>().color;
                    color.a = 1f;
                    GetButton(i).GetComponent<Image>().color = color;
                }
                break;
            default: break;
        }
        
    }
    //0 : 땅     1: 시티   2: 해    4: 하늘
    void Play0_A()
    {

        int index = (int)buttons[0];

        string tmp = $"S{index / 6}_{index % 6}_a";
        SoundtrackType0 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound,GameManager.Sound.GetVolume(Sound.Play0));
            GameManager.InGameData.SoundPlayer[0].StartPlayingAnim();
        }
    }
    void Play0_B()
    {
        int index = (int)buttons[0];
        string tmp = $"S{index / 6}_{index % 6}_b";
        SoundtrackType0 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play0));
            GameManager.InGameData.SoundPlayer[0].StartPlayingAnim();
        }
    }



    void Play1_A()
    {

        int index = (int)buttons[1];
        string tmp = $"S{index / 6}_{index % 6}_a";
        SoundtrackType1 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play1));
            GameManager.InGameData.SoundPlayer[1].StartPlayingAnim(GameManager.InGameData.ButtonDatas[index]);
        }
    }
    void Play1_B()
    {
        int index = (int)buttons[1];
        string tmp = $"S{index / 6}_{index % 6}_b";
        SoundtrackType1 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play1));
            GameManager.InGameData.SoundPlayer[1].StartPlayingAnim(GameManager.InGameData.ButtonDatas[index]);
        }
    }

    void Play2_A()
    {

        int index = (int)buttons[2];
        string tmp = $"S{index / 6}_{index % 6}_a";
        SoundtrackType2 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play2));
            GameManager.InGameData.SoundPlayer[2].StartPlayingAnim(GameManager.InGameData.ButtonDatas[index]);
        }
    }
    void Play2_B()
    {
        int index = (int)buttons[2];
        string tmp = $"S{index / 6}_{index % 6}_b";
        SoundtrackType2 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play2));
            GameManager.InGameData.SoundPlayer[2].StartPlayingAnim(GameManager.InGameData.ButtonDatas[index]);
        }
    }

    void Play3_A()
    {

        int index = (int)buttons[3];
        string tmp = $"S{index / 6}_{index % 6}_a";
        SoundtrackType3 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play3));
            GameManager.InGameData.SoundPlayer[3].StartPlayingAnim();
        }
    }
    void Play3_B()
    {
        int index = (int)buttons[3];
        string tmp = $"S{index / 6}_{index % 6}_b";
        SoundtrackType3 sound;
        if (System.Enum.TryParse(tmp, out sound))
        {
            GameManager.Sound.Play(sound, GameManager.Sound.GetVolume(Sound.Play3));
            GameManager.InGameData.SoundPlayer[3].StartPlayingAnim();
        }
    }
    
    #endregion Buttons

}
