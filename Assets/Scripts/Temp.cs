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
        int num = GetNum("Button4");
        Debug.Log(num);
        num = GetNum("Button42");
        Debug.Log(num);
        num = GetNum("Button14");
        Debug.Log(num);
        num = GetNum("Button54");
        Debug.Log(num);

    }

    private void DeBug(PointerEventData data)
    {
        string _clickButtonName;
        _clickButtonName = data.pointerEnter.gameObject.name;
        Debug.Log(_clickButtonName);
    }

    public static int GetNum(string buttonName)
    {
        if (!buttonName.StartsWith("Button"))
        {
            throw new ArgumentException("Invalid button name. It should start with 'Button'");
        }

        string numberPart = buttonName.Substring(6); // Remove "Button" from the start

        if (int.TryParse(numberPart, out int number))
        {
            return number;
        }

        throw new ArgumentException("Invalid button name. It should contain a valid integer after 'Button'");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
