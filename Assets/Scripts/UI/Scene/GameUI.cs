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
    bool[] _activeButtons = new bool[Enum.GetValues(typeof(Buttons)).Length];

    public void SetRoadColor(Color color)
    {
        Get<GameObject>((int)GameObjects.Road).GetComponent<Image>().color = color;
    }
    Buttons button;
    Buttons[] buttons = new Buttons[4];
    bool[] playToggles = new bool[4];
    public bool[] PlayToggles { get { return playToggles; } }
    const float deltaTime = 0.5f;

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

        ButtonBind();

        for(int i = 0; i <= (int)Buttons.Button23; i++)
        {
            _activeButtons[i] = false;
        }
        for(int i = 0; i < 4; i++)
        {
            buttons[i] = Buttons.None;
            playToggles[i] = true;
        }


    }

    GameObject UiDragImage;

    
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


    }
    void PlayPause(PointerEventData evt)
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            GetButton((int)Buttons.PlayPause).GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>("Sprites/UI/midBtn_pause");
            for (int i = 0; i < 4; i++)
            {
                GameManager.Sound.AudioSources[i].UnPause();
            }

        }
        else if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            GetButton((int)Buttons.PlayPause).GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>("Sprites/UI/midBtn_play");
            
            for (int i = 0; i < 4; i++)
            {
                GameManager.Sound.AudioSources[i].Pause();
            }

        }
    }

    /// <summary>
    /// SoundPlayer[0]ø°º≠ Play
    /// </summary>
    /// <param name="evt"></param>
    /*
    
    */
    string _clickButtonName;
    float time;
    bool Click = false;
    int _clickIndex;
    void Down(PointerEventData evt)
    {
        Click = true;
        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;
        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            _clickIndex = ((int)button)/6;
        }
        time = Time.time;
        //Down(tmp);
    }
    void Up(PointerEventData evt)
    {
        Click = false;
        // ¬™∞‘ ≈¨∏Ø
        if (time + deltaTime > Time.time)
        {
            Down(_clickButtonName);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (PlayToggles[i])
                {
                    GameManager.Sound.SetVolume((Define.Sound)i, GameManager.Sound.Volume);
                }
            }
        }
    }

    void RemoteButtonDown(PointerEventData evt)
    {
        
        _clickButtonName = evt.pointerCurrentRaycast.gameObject.name;

        if (System.Enum.TryParse(_clickButtonName, out button))
        {
            //_clickIndex = ((int)button) / 6;
            _clickIndex = button - Buttons.RemoteButton0;
        }

        _clickButtonName = Enum.GetName(typeof(Buttons), buttons[_clickIndex]);
        if(buttons[_clickIndex] == Buttons.None)
        {
            return;
        }
        Click = true;

        time = Time.time;
        //Down(tmp);
    }
    void RemoteButtonUp(PointerEventData evt)
    {
        if (buttons[_clickIndex] == Buttons.None)
        {
            return;
        }
        Click = false;
        // ¬™∞‘ ≈¨∏Ø
        if (time + deltaTime > Time.time)
        {
            Down(_clickButtonName);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (PlayToggles[i])
                {
                    GameManager.Sound.SetVolume((Define.Sound)i, GameManager.Sound.Volume);
                }
            }
        }
    }
    private void Update()
    {
        if(Click && time + deltaTime <Time.time)
        {
            for(int i = 0; i < 4; i++)
            {
                if(i != _clickIndex)
                {
                    GameManager.Sound.SetVolume((Define.Sound)i, 0);

                }
            }
        }
    }
    public void Down(string buttonName,bool None = false, bool PreSet = false)
    {
        string tmp = buttonName;

        if (System.Enum.TryParse(tmp, out button))
        {
            int index = (int)button;
            int beforePlaylist = (int)buttons[index / 6];

            if (!None )
            {
                // ¥Ÿ∏• ¿Ωæ«¿∏∑Œ πŸ≤‹ ∂ß
                if (index != beforePlaylist)
                {
                    ResetType(index / 6);
                    buttons[index / 6] = button;
                    playToggles[index / 6] = true;
                    GameManager.Sound.SetVolume((Sound)(index / 6), GameManager.Sound.Volume);

                    Color color = GetButton(index).GetComponent<Image>().color;
                    color.a = 0.5f;
                    GetButton(index).GetComponent<Image>().color = color;
                }
                // ∞∞¿∫ ¿Ωæ« ≈‰±€«“ ∂ß
                else
                {
                    if (PreSet)
                    {
                        ResetType(index / 6);
                        buttons[index / 6] = button;
                        playToggles[index / 6] = true;
                        GameManager.Sound.SetVolume((Sound)(index / 6), GameManager.Sound.Volume);

                        Color color = GetButton(index).GetComponent<Image>().color;
                        color.a = 0.5f;
                        GetButton(index).GetComponent<Image>().color = color;

                    }
                    else
                    {
                        if (playToggles[index / 6])
                        {
                            playToggles[index / 6] = false;
                            GameManager.Sound.SetVolume((Sound)(index / 6), 0);
                            Color color = GetButton(index).GetComponent<Image>().color;
                            color.a = 1f;
                            GetButton(index).GetComponent<Image>().color = color;
                        }
                        else
                        {
                            playToggles[index / 6] = true;
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


            TMP_SliderTest.Init();

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
            Debug.Log("∫Ø»Ø Ω«∆– At GameUI: " + tmp);
        }

    }

    public void ResetType(int type)
    {

        switch (type)
        {
            case 0:
                playToggles[0] = true;
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
                playToggles[1] = true;
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
                playToggles[2] = true;
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
                playToggles[3] = true;
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
    //0 : ∂•     1: Ω√∆º   2: «ÿ    4: «œ¥√
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
