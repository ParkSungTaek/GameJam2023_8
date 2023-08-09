using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3State : State
{
    
    public override void StartPlayingAnim(PlayerController go, ButtonData buttonData = null)
    {
        if (buttonData != null)
        {
            go.GetComponent<SpriteRenderer>().sprite = buttonData.sprite;
            go.GetComponent<SpriteRenderer>().color = buttonData.color;
        }
    }
    
}
