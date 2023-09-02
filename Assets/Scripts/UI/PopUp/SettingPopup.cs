using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingPopup : UI_PopUp
{
    enum UIs
    {
        UI,
        BG,
        SoundVolumeSlider,
    }
    enum Buttons
    {
        Arrow,
        Auto,
    }
    Vector3 StartUIPosition;

    Vector3 MoveUIVec = new Vector3(-860, 0, 0);
    bool active = false;
    const float MoveUITime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        Bind<GameObject>(typeof(UIs));
        Bind<Button>(typeof(Buttons));
        Get<GameObject>((int)UIs.SoundVolumeSlider).GetComponent<Slider>().onValueChanged.AddListener(delegate { VolumeChange(); });
        Get<GameObject>((int)UIs.SoundVolumeSlider).GetComponent<Slider>().value = GameManager.Sound.Volume;
        BindEvent(GetButton((int)Buttons.Arrow).gameObject, ClickArrow);

        BindEvent(GetButton((int)Buttons.Auto).gameObject, Auto);

        StartUIPosition = Get<GameObject>((int)UIs.UI).transform.position;
        Get<GameObject>((int)UIs.BG).SetActive(false);
        AutoIdx = random.Next(0, GameManager.InGameData.ActiveAchievementsIndex.Count);

    }
    void VolumeChange()
    {
        float volume = Get<GameObject>((int)UIs.SoundVolumeSlider).GetComponent<Slider>().value;
        GameManager.Sound.Volume = volume;
        for(int i = 0; i < 4; i++)
        {
            
            if (GameUI.Instance.NowAudioPlaying[i])
            {
                GameManager.Sound.SetVolume((Define.Sound)i, volume);
            }
            else
            {

                GameManager.Sound.SetVolume((Define.Sound)i, 0);
            }

        }
    }
    int AutoIdx = 0;
    int sameAudio = 0;
    System.Random random = new System.Random();
    bool RandomPresent (int persent)
    {
        if (random.Next(0, 100) < persent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Auto(PointerEventData evt)
    {

        if (!InGameDataManager.IsAutoPlay)
        {
            InGameDataManager.IsAutoPlay = true;
            SyncController.JobCollector_Start_A -= Auto;
            SyncController.JobCollector_Start_A += Auto;
            SyncController.Flush();
            GetButton((int)Buttons.Auto).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);

        }
        else
        {
            InGameDataManager.IsAutoPlay = false;
            SyncController.JobCollector_Start_A -= Auto;
            GetButton((int)Buttons.Auto).GetComponent<Image>().color = new Color(1f, 1f, 1f);

        }

    }
    
    public void Auto()
    {
        if (InGameDataManager.IsAutoPlay) { 
            int randomIndex;
            if (GameManager.InGameData.ActiveAchievementsIndex.Count == 1)
            {
                AchievementPopup.AchievementPlay(GameManager.InGameData.ActiveAchievementsIndex[0]);
            }
            else if (GameManager.InGameData.ActiveAchievementsIndex.Count > 1)
            {

                switch (sameAudio)
                {
                    case 0:
                        if (RandomPresent(40))
                        {
                            Debug.Log("HelloWorld");
                            randomIndex = random.Next(0, GameManager.InGameData.ActiveAchievementsIndex.Count - 1);
                            if (randomIndex >= AutoIdx)
                            {
                                randomIndex++;
                            }
                            AutoIdx = randomIndex;
                            Debug.Log($"HelloWorld2  {GameManager.InGameData.ActiveAchievementsIndex[AutoIdx]}");
                            AchievementPopup.AchievementPlay(GameManager.InGameData.ActiveAchievementsIndex[AutoIdx]);
                            sameAudio = 0;
                        }
                        else
                        {
                            AchievementPopup.AchievementPlay(GameManager.InGameData.ActiveAchievementsIndex[AutoIdx]);
                            sameAudio = 1;
                        }
                        break;
                    case 1:
                        if (RandomPresent(80))
                        {
                            randomIndex = random.Next(0, GameManager.InGameData.ActiveAchievementsIndex.Count - 1);
                            if (randomIndex >= AutoIdx)
                            {
                                randomIndex++;
                            }
                            AutoIdx = randomIndex;
                            AchievementPopup.AchievementPlay(GameManager.InGameData.ActiveAchievementsIndex[AutoIdx]);
                            sameAudio = 0;
                        }
                        else
                        {
                            AchievementPopup.AchievementPlay(GameManager.InGameData.ActiveAchievementsIndex[AutoIdx]);
                            sameAudio = 2;
                        }
                        break;
                    default:
                        randomIndex = random.Next(0, GameManager.InGameData.ActiveAchievementsIndex.Count - 1);
                        if (randomIndex >= AutoIdx)
                        {
                            randomIndex++;
                        }
                        AutoIdx = randomIndex;
                        AchievementPopup.AchievementPlay(GameManager.InGameData.ActiveAchievementsIndex[AutoIdx]);
                        sameAudio = 0;
                        break;
                }
            }
           
        }
    }

    Coroutine Coroutine;
    void ClickArrow(PointerEventData evt)
    {
        if (!active)
        {
            active = true;
            Get<GameObject>((int)UIs.BG).SetActive(true);
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }
            StartCoroutine(Util.SmoothMoveUI(Get<GameObject>((int)UIs.UI).GetComponent<RectTransform>(), StartUIPosition, Get<GameObject>((int)UIs.UI).transform.position + MoveUIVec, MoveUITime));

        }
        else
        {
            active = false;
            StartCoroutine(Util.SmoothMoveUI(Get<GameObject>((int)UIs.UI).GetComponent<RectTransform>(), Get<GameObject>((int)UIs.UI).transform.position, StartUIPosition, MoveUITime));
            Coroutine = StartCoroutine(InActiveUI());

        }
    }
    IEnumerator InActiveUI()
    {
        yield return new WaitForSeconds(MoveUITime);
        Get<GameObject>((int)UIs.BG).SetActive(false);
    }
    public override void ReOpen()
    {
        //throw new System.NotImplementedException();
    }
}
