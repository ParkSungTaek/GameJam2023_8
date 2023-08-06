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
        Button1,
        Button2,
        Button3,
        Button4,
        Button5,
        Button6,
        Button7,
        Button8,

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

    GameObject UiDragImage;

    #region Buttons
    void ButtonBind()
    {
        BindEvent(GetButton((int)Buttons.Button1).gameObject, Down1, Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.Button1).gameObject, Drag1, Define.UIEvent.Drag);
        BindEvent(GetButton((int)Buttons.Button1).gameObject, DragEnd1, Define.UIEvent.DragEnd);

        BindEvent(GetButton((int)Buttons.Button2).gameObject, Down2, Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.Button2).gameObject, Drag2, Define.UIEvent.Drag);
        BindEvent(GetButton((int)Buttons.Button2).gameObject, DragEnd2, Define.UIEvent.DragEnd);

        BindEvent(GetButton((int)Buttons.Button3).gameObject, Down3,Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.Button3).gameObject, Drag3, Define.UIEvent.Drag);
        BindEvent(GetButton((int)Buttons.Button3).gameObject, DragEnd3, Define.UIEvent.DragEnd);

        BindEvent(GetButton((int)Buttons.Button4).gameObject, Down4, Define.UIEvent.Down);
        BindEvent(GetButton((int)Buttons.Button4).gameObject, Drag4, Define.UIEvent.Drag);
        BindEvent(GetButton((int)Buttons.Button4).gameObject, DragEnd4, Define.UIEvent.DragEnd);

    }

    void Down1(PointerEventData evt)
    {
        UiDragImage = GameManager.Resource.Instantiate("UI/Elements/Circle1",this.transform);
        Debug.Log(UiDragImage.name);
        UiDragImage.transform.position = evt.position;

    }
    void Drag1(PointerEventData evt)
    {
        if (UiDragImage != null) 
        {
            UiDragImage.transform.position = evt.position;
            Debug.Log($"Drag: {UiDragImage.name}");

        }
        else 
        {
            Debug.Log($"Fail: {UiDragImage.name}");
        }
        
    }
    void DragEnd1(PointerEventData evt)
    {
        Debug.Log("DragEnd");

        GameObject tile = Util.GetObjRaycast2D();
        if (tile != null)

            tile.GetComponent<SpriteRenderer>().color = new Color((67f/255f),0,0);

        GameManager.Resource.Destroy(UiDragImage);


    }


    void Down2(PointerEventData evt)
    {
        Debug.Log("Click2");
        UiDragImage = GameManager.Resource.Instantiate("UI/Elements/Circle2",this.transform);
        UiDragImage.transform.position = evt.position;
    }
    void Drag2(PointerEventData evt)
    {
        Debug.Log("Drag");
        if(UiDragImage != null)
            UiDragImage.transform.position = evt.position;

    }
    void DragEnd2(PointerEventData evt)
    {
        Debug.Log("DragEnd");
        GameObject tile = Util.GetObjRaycast2D();
        if (tile != null)

            tile.GetComponent<SpriteRenderer>().color = new Color((106f / 255f), 0, (54f / 255f));

        GameManager.Resource.Destroy(UiDragImage);
    }


    void Down3(PointerEventData evt)
    {
        Debug.Log("Click");
        UiDragImage = GameManager.Resource.Instantiate("UI/Elements/Circle3", this.transform);
        UiDragImage.transform.position = evt.position;
    }
    void Drag3(PointerEventData evt)
    {
        Debug.Log("Drag");
        if (UiDragImage != null)

            UiDragImage.transform.position = evt.position;

    }
    void DragEnd3(PointerEventData evt)
    {
        Debug.Log("DragEnd");
        GameObject tile = Util.GetObjRaycast2D();
        if (tile != null)

            tile.GetComponent<SpriteRenderer>().color = new Color((58f / 255f), 0, (99f / 255f));
        GameManager.Resource.Destroy(UiDragImage);

    }

    void Down4(PointerEventData evt)
    {
        Debug.Log("Click");
    }
    void Drag4(PointerEventData evt)
    {
        Debug.Log("Drag");
    }
    void DragEnd4(PointerEventData evt)
    {
        Debug.Log("DragEnd");
    }




    #endregion Buttons


}
