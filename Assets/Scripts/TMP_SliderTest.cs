using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMP_SliderTest: MonoBehaviour
{

    static TMP_SliderTest _instance; 

    public static TMP_SliderTest Instance { get { Init(); return _instance; }}

    bool start = false;
    Slider Slider;
    bool A = true;

    // Start is called before the first frame update
    public static void Init()
    {
        _instance = GameObject.Find("Slider").GetComponent<TMP_SliderTest>();
        if (!_instance.start)
        {
            _instance.Slider = _instance.GetComponent<Slider>();
            SyncController.Flush();
            _instance.start = true;
            //SyncController.JobCollector_Start += StartSlider;
            SyncController.JobCollector_End_A += _instance.ResetSliderA;
            SyncController.JobCollector_End_B += _instance.ResetSliderB;


        }


    }
    


    public void StartSlider()
    {
        SyncController.JobCollector_Start_Player0_A -= () => { start = true; };
        SyncController.JobCollector_Start_Player0_A += () => { start = true; };

    }

    public void EndSlider()
    {
        SyncController.JobCollector_Start_Player0_A -= () => { start = false; };
        SyncController.JobCollector_Start_Player0_A += () => { start = false; };
    }
    
    void ResetSliderA()
    {
        A = false;
    }
    void ResetSliderB()
    {
        A = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (start) 
        {
            if (A)
                Slider.value += Time.deltaTime * (1 / SyncController.SyncTime);
            else 
                Slider.value -= Time.deltaTime * (1 / SyncController.SyncTime);
        }
    }
}
