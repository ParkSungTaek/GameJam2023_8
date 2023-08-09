using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour 
{
    public virtual void StartPlayingAnim(PlayerController go, ButtonData buttonData = null) 
    {
        TMP_SliderTest.Init();
    }
    public virtual void EndPlayingAnim(PlayerController go) { }

}
