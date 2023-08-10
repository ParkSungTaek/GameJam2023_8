using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Temp : UI_Scene
{
    enum Buttons 
    { 
        button,
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.button).gameObject, DeBug, Define.UIEvent.Exit);
        
    }

    private void DeBug(PointerEventData data)
    {
        string _clickButtonName;
        _clickButtonName = data.pointerEnter.gameObject.name;
        Debug.Log(_clickButtonName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
