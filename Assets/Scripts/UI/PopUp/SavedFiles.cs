using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class SavedFiles : UI_PopUp
{

    enum Buttons
    {
        save0,
        save1, 
        save2, 
        save3, 
        save4,
        ESC,
    }

    enum Texts
    {
        saveTxt0,
        saveTxt1,
        saveTxt2,
        saveTxt3,
        saveTxt4,
    
    }
    const int PageSize = 5;
    public int StartIDX { get; set; } = 0;
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        
        ButtonBind();
        SetTxt();

    }
    private void OnEnable()
    {
        SetTxt();
    }
    #region Btn
    void ButtonBind()
    {
        for (int i = 0; i < PageSize; i++)
        {
            BindEvent(GetButton(i).gameObject, PlayRecordedFile);
        }
        BindEvent(GetButton((int)Buttons.ESC).gameObject, ESC);

    }

    void PlayRecordedFile(PointerEventData evt)
    {
        Buttons button;
        if (System.Enum.TryParse(evt.pointerEnter.gameObject.name, out button))
        {
            if (RecordController.Instance.RecordedList.recorddatas.Count > StartIDX + (int)button)
            {

                if (RecordController.Instance.RecordedList.recorddatas[StartIDX + (int)button].startA)
                {
                    SyncController.JobCollector_Start_A_OneTime += () => RecordController.Instance.PlayRecordedMusic(RecordController.Instance.RecordedList.recorddatas[StartIDX + (int)button]);
                }
                else
                {
                    SyncController.JobCollector_Start_B_OneTime += () => RecordController.Instance.PlayRecordedMusic(RecordController.Instance.RecordedList.recorddatas[StartIDX + (int)button]);

                }
            }
        }
        else
        {
            Debug.Log($"{evt.pointerEnter.gameObject.name} != save0 ");
        }
    }

    void ESC(PointerEventData evt)
    {
        GameManager.UI.ClosePopUpUI();
    }



    #endregion

    #region Txt

    public void SetTxt()
    {
        Debug.Log("RecordController.Instance.RecordedList.DataList.Count "+RecordController.Instance.RecordedList.recorddatas.Count);
        for (int i = 0; i < PageSize; i++)
        {
            if (RecordController.Instance.RecordedList.recorddatas.Count > StartIDX + i)
            {
                GetText(i).text = RecordController.Instance.RecordedList.recorddatas[StartIDX + i].name;

                Debug.Log("Have "+(StartIDX + i));
            }
            else
            {
                Debug.Log("None "+ (StartIDX + i));
                GetText(i).text = "No Data";
            }
        }
    }

    #endregion Txt

    public override void ReOpen()
    {
    }
}
