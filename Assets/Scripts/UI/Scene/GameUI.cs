using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class GameUI : UI_Scene
{
    enum GameObjects
    {
    }
    enum Buttons
    {
        Button,
    }
    enum Texts
    {
    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        ButtonBind();



    }



    #region Buttons
    void ButtonBind()
    {
        BindEvent(GetButton((int)Buttons.Button).gameObject, Click , Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button).gameObject, Drag, Define.UIEvent.Drag);
        BindEvent(GetButton((int)Buttons.Button).gameObject, DragEnd, Define.UIEvent.DragEnd);

    }

    void Click(PointerEventData evt)
    {
        Debug.Log("Click");
    }
    void Drag(PointerEventData evt)
    {
        Debug.Log("Drag");
    }
    void DragEnd(PointerEventData evt) 
    {
        Debug.Log("DragEnd");    
    }
    #endregion Buttons


}
