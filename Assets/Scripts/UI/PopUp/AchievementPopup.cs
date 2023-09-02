using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementPopUp : UI_PopUp
{
    enum UIs
    {
        BG,
        UI,
    }
    enum Buttons
    {
        Achievement0,
        Achievement1, 
        Achievement2, 
        Achievement3, 
        Achievement4, 
        Achievement5,
        Achievement6,
        Achievement7,

        Arrow,
    }

    enum Link_Idx
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
    Vector3 StartUIPosition;

    Vector3 MoveUIVec = new Vector3(900, 0, 0);
    const float MoveUITime = 0.5f;

    int AchievementMAX = (int)Buttons.Achievement7;
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        //Object 바인드
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(UIs));

        ButtonBind();
        ColorRefresh();
        StartUIPosition = Get<GameObject>((int)UIs.UI).transform.position;
        Get<GameObject>((int)UIs.BG).SetActive(false);
    }
    bool active = false;
    #region Buttons
    public void ColorRefresh()
    {
        for (int i = 0; i <= AchievementMAX; i++)
        {
            if (!GameManager.InGameData.GetActiveAchievements(i))
            {
                GetButton(i).gameObject.GetComponent<Image>().color = Color.black;
            }
            else
            {
                GetButton(i).gameObject.GetComponent<Image>().color = Color.clear;
            }

        }

    }
    void ButtonBind()
    {
        for (int i = 0; i <= AchievementMAX; i++)
        {
            BindEvent(GetButton(i).gameObject, Achievement);
        }

        BindEvent(GetButton((int)Buttons.Arrow).gameObject, ClickArrow);

    }
    public void OpenAchiveIDX(int i)
    {
        GetButton(i).gameObject.GetComponent<Image>().color = Color.white;
    }

    public static void AchievementPlay(int idx)
    {
        InGameDataManager.Achievement achievement = GameManager.InGameData.Achievements[idx];

        Link_Idx button;
        if (System.Enum.TryParse(achievement.Track0, out button))
        {
            GameUI.Instance.Add((int)button);
        }
        if (System.Enum.TryParse(achievement.Track1, out button))
        {
            GameUI.Instance.Add((int)button);
        }
        if (System.Enum.TryParse(achievement.Track2, out button))
        {
            GameUI.Instance.Add((int)button);
        }
        if (System.Enum.TryParse(achievement.Track3, out button))
        {
            GameUI.Instance.Add((int)button);
        }
    }
    void Achievement(PointerEventData evt) 
    {
        string tmp = evt.pointerCurrentRaycast.gameObject.name;
        Buttons button;
        if (System.Enum.TryParse(tmp, out button))
        {
            if (GameManager.InGameData.GetActiveAchievements((int)button))
            {
                AchievementPlay((int)button);
            }
        }
        else
        {
            Debug.Log("변환 실패: " + tmp);
        }
    }


    Coroutine Coroutine;
    void ClickArrow(PointerEventData evt)
    {
        ColorRefresh();
        if (!active)
        {
            active = true;
            Get<GameObject>((int)UIs.BG).SetActive(true);
            if(Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }
            StartCoroutine(Util.SmoothMoveUI(Get<GameObject>((int)UIs.UI).GetComponent<RectTransform>(), StartUIPosition, Get<GameObject>((int)UIs.UI).transform.position + MoveUIVec, MoveUITime ) );
        
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
    #endregion Buttons
    public override void ReOpen()
    {
        //throw new System.NotImplementedException();
    }
}
