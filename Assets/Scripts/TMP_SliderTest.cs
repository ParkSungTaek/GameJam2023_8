using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMP_SliderTest: MonoBehaviour
{
    bool start = false;
    Slider Slider;

    // Start is called before the first frame update
    void Start()
    {
        Slider = GetComponent<Slider>();
        SyncController.Flush();
        //SyncController.JobCollector_Start += StartSlider;
        SyncController.JobCollector_End += ResetSlider;


    }
    
    public void StartSlider()
    {
        SyncController.JobCollector_Start -= () => { start = true; };
        SyncController.JobCollector_Start += () => { start = true; };

    }

    public void EndSlider()
    {
        SyncController.JobCollector_Start -= () => { start = false; };
        SyncController.JobCollector_Start += () => { start = false; };
    }
    
    void ResetSlider()
    {
        Slider.value = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (start) 
        {
            Slider.value += Time.deltaTime * (1/ SyncController.SyncTime);
        }
    }
}
