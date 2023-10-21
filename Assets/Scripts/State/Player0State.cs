using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player0State : State
{
    const float MOVETIME =22f;
    Vector3[] position = new Vector3[3];

    [SerializeField]
    GameObject[] childObjects;
    const int BGnum = 3;
    const int BGTypes = 6;


    enum BGTiles
    {
        BG0_0,
        BG0_1,
        BG0_2,
        BG1_0,
        BG1_1,
        BG1_2,
        BG2_0,
        BG2_1,
        BG2_2,
        BG3_0,
        BG3_1,
        BG3_2,
        BG4_0,
        BG4_1,
        BG4_2,
        BG5_0,
        BG5_1,
        BG5_2,
        Maxnum
    }



    Coroutine[] Coroutine;
    bool isPlaying = false;
    private void Start()
    {
        childObjects = new GameObject[(int)BGTiles.Maxnum];
        for (int i = 0;i < (int)BGTiles.Maxnum; i++)
        {
            childObjects[i] = transform.Find(Enum.GetName(typeof(BGTiles), i)).gameObject;
        }

        Coroutine = new Coroutine[BGnum];
        
        for (int i = 0; i < BGnum; i++)
        {
            position[i] = childObjects[i].transform.localPosition;
        }
        ActiveButtons(0);

    }
    /// <summary>
    /// 0 ∂•¿Ã Ω√¿€«ﬂ¿ª ∂ß 
    /// </summary>
    /// <param name="go"></param>
    public override void StartPlayingAnim(PlayerController go, int buttonIDX)
    {
        base.StartPlayingAnim(go, buttonIDX);

        ActiveButtons(buttonIDX);
        if (!isPlaying)
        {

            isPlaying = true;

            for (int i = 0; i < BGnum; i++)
            {
                Coroutine[i] = StartCoroutine(SmoothMove(i, i - 1,i));
            }
        }
        
    }

    void ActiveButtons(int buttonIDX)
    {
        for (int i = 0; i < (int)BGTiles.Maxnum; i++)
        {
            childObjects[i].SetActive(false);
        }
        for(int i = 0; i < BGnum; i++)
        {
            childObjects[buttonIDX * 3 + i].SetActive(true);
        }
        GameUI.Instance.RoadImage(buttonIDX);

    }

    /// <summary>
    /// 0 ∂•¿Ã ≥°≥µ¿ª ∂ß 
    /// </summary>
    /// <param name="go"></param>
    public override void EndPlayingAnim(PlayerController go) 
    {
        for (int i = 0; i < BGnum; i++)
        {
            childObjects[i].GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f);
            StopCoroutine(Coroutine[i]);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startIdx"></param>
    /// <param name="endIdx"></param>
    /// <param name="moveTime"></param>
    /// <param name="IDX">0,1,2 </param>
    /// <returns></returns>
    public IEnumerator SmoothMove(int startIdx, int endIdx, int IDX = 0,  float moveTime = MOVETIME)
    {
        while (true)
        {
            Vector3 start;
            Vector3 end;
            if (endIdx >= 0)
            {
                start = position[startIdx--];
                end = position[endIdx--];

                float startTime = Time.time;
                float endTime = startTime + moveTime;
                while (Time.time < endTime)
                {

                    float t = (Time.time - startTime) / moveTime;
                    //transform.localPosition = Vector3.Lerp(start, end, t);

                    for(int i = 0; i < 6; i++)
                    {
                        childObjects[3 * i + IDX].transform.localPosition = Vector3.Lerp(start, end, t);
                    }

                    yield return null;
                }

            }
            else
            {
                startIdx = BGnum-1;
                endIdx = BGnum-2;
                //ransform.localPosition = position[startIdx];
                for (int i = 0; i < 6; i++)
                {
                    childObjects[3 * i + IDX].transform.localPosition = position[startIdx];
                }
                yield return new WaitForSeconds(moveTime);

            }
        }
        
    }








}
