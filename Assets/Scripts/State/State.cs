using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour 
{
    public virtual void StartPlayingAnim(int buttonData = 0) 
    {
        TimeSlider.Init();
    }
    public virtual void EndPlayingAnim(int buttonData) { }

}
