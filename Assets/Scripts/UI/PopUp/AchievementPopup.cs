using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class AchievementPopup : UI_PopUp
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
    Vector3 StartUIPosition;

    Vector3 MoveUIVec = new Vector3(900, 0, 0);
    const float MoveUITime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        //Object 바인드
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(UIs));

        ButtonBind();
        StartUIPosition = Get<GameObject>((int)UIs.UI).transform.position;
        Debug.Log($" X : {Get<GameObject>((int)UIs.BG).transform.position.x}   Y : {Get<GameObject>((int)UIs.UI).transform.position.y}  Z : {Get<GameObject>((int)UIs.UI).transform.position.z}");
        Get<GameObject>((int)UIs.BG).SetActive(false);
    }
    bool active = false;
    #region Buttons
    void ButtonBind()
    {
        for(int i = 0;i <= (int)Buttons.Achievement7;i++)
        {
            BindEvent(GetButton(i).gameObject, Achievement);

        }
        BindEvent(GetButton((int)Buttons.Arrow).gameObject, ClickArrow);


    }

    void Achievement(PointerEventData evt) 
    {
        string tmp = evt.pointerCurrentRaycast.gameObject.name;
        Buttons button;
        if (System.Enum.TryParse(tmp, out button))
        {
            int index = (int)button;
        }
        else
        {
            Debug.Log("변환 실패: " + tmp);
        }


        InGameDataManager.Achievement achievement = GameManager.InGameData.Achievements[(int)button];
        if (achievement.Track0 != "None")
        {
            GameUI.Instance.Down(achievement.Track0, false, true);
        }
        else
        {
            GameUI.Instance.Down("Button0",true);
        }

        if (achievement.Track1 != "None")
        {
            GameUI.Instance.Down(achievement.Track1, false, true);
        }
        else
        {
            GameUI.Instance.Down("Button6", true);

        }

        if (achievement.Track2 != "None")
        {
            GameUI.Instance.Down(achievement.Track2, false, true);
        }
        else
        {
            GameUI.Instance.Down("Button12", true);

        }
        if (achievement.Track3 != "None")
        {
            GameUI.Instance.Down(achievement.Track3, false, true);
        }
        else
        {
            GameUI.Instance.Down("Button18", true);

        }

    }
    Coroutine Coroutine;
    void ClickArrow(PointerEventData evt)
    {
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
