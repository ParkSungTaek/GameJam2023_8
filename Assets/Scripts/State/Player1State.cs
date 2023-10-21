using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1State : State
{
    
    public override void StartPlayingAnim(PlayerController go, int buttonData) 
    {
        base.StartPlayingAnim(go, buttonData);

    }
    /*
    public override void StartPlayingAnim(PlayerController go, ButtonData buttonData = null)
    {
        base.StartPlayingAnim(go);
        for (int i = 0; i < BGnum; i++)
        {
            childObjects[i].GetComponent<SpriteRenderer>().color = new Color((193f / 255f), (209f / 255f), 0);
        }
        if (!isPlaying)
        {

            isPlaying = true;
            for (int i = 0; i < BGnum; i++)
            {
                Coroutine[i] = StartCoroutine(SmoothMove(childObjects[i].transform, i, i - 1));

                //Coroutine[i] = StartCoroutine(SmoothMove(childObjects[i].transform, i, i-1));
            }
        }

    }
    */
}
