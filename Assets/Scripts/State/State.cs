using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour 
{
    public virtual void StartPlayingAnim(PlayerController go, int buttonData) 
    {
        TimeSlider.Init();
    }
    public virtual void EndPlayingAnim(PlayerController go) { }

}
