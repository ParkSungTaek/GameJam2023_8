using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingPopup : UI_PopUp
{
 
    enum UIs
    {
        UI,
        BG,
        SoundVolumeSlider,
    }
    enum Buttons
    {
        Arrow,
    }
    Vector3 StartUIPosition;

    Vector3 MoveUIVec = new Vector3(-860, 0, 0);
    bool active = false;
    const float MoveUITime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        Bind<GameObject>(typeof(UIs));
        Bind<Button>(typeof(Buttons));
        Get<GameObject>((int)UIs.SoundVolumeSlider).GetComponent<Slider>().onValueChanged.AddListener(delegate { VolumeChange(); });
        Get<GameObject>((int)UIs.SoundVolumeSlider).GetComponent<Slider>().value = GameManager.Sound.Volume;
        BindEvent(GetButton((int)Buttons.Arrow).gameObject, ClickArrow);
        StartUIPosition = Get<GameObject>((int)UIs.UI).transform.position;
        Get<GameObject>((int)UIs.BG).SetActive(false);

    }
    void VolumeChange()
    {
        float volume = Get<GameObject>((int)UIs.SoundVolumeSlider).GetComponent<Slider>().value;
        GameManager.Sound.Volume = volume;
        for(int i = 0; i < 4; i++)
        {
            
            if (GameUI.Instance.IsPlaying[i])
            {
                GameManager.Sound.SetVolume((Define.Sound)i, volume);
            }
            else
            {

                GameManager.Sound.SetVolume((Define.Sound)i, 0);
            }

        }
    }

    Coroutine Coroutine;
    void ClickArrow(PointerEventData evt)
    {
        if (!active)
        {
            active = true;
            Get<GameObject>((int)UIs.BG).SetActive(true);
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }
            StartCoroutine(Util.SmoothMoveUI(Get<GameObject>((int)UIs.UI).GetComponent<RectTransform>(), StartUIPosition, Get<GameObject>((int)UIs.UI).transform.position + MoveUIVec, MoveUITime));

        }
        else
        {
            active = false;
            StartCoroutine(Util.SmoothMoveUI(Get<GameObject>((int)UIs.UI).GetComponent<RectTransform>(), Get<GameObject>((int)UIs.UI).transform.position, StartUIPosition, MoveUITime));
            Coroutine = StartCoroutine(InActiveUI());

        }
    }
    IEnumerator InActiveUI()
    {
        yield return new WaitForSeconds(MoveUITime);
        Get<GameObject>((int)UIs.BG).SetActive(false);
    }
    public override void ReOpen()
    {
        //throw new System.NotImplementedException();
    }
}
