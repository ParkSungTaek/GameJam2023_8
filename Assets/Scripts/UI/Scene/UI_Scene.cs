using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UI_Scene : UI_Base
{
    public override void Init()
    {
        GameManager.UI.SetCanvas(gameObject, false);
        SetResolution();
    }
}
