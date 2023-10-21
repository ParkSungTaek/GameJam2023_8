using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player0State : State
{
    const float MOVETIME =22f;
    Vector3[] position = new Vector3[4];

    [SerializeField]
    GameObject[,] childObjects;
    const int BGnum = 3;
    const int BGType = 6;


    Coroutine[,] Coroutine;
    bool isPlaying = false;
    private void Start()
    {
        GameManager.InGameData.PlayerState[(int)Define.Types.Land] = this;
        childObjects = new GameObject[BGType,BGnum];
        Coroutine = new Coroutine[BGType,BGnum];
        for (int i = 0; i < BGnum; i++)
        {
            for(int type = 0;type < BGType; type++)
            {
                childObjects[type,i] = transform.Find($"{type}_{i}").gameObject;
                
            }
            position[i] = childObjects[0,i].transform.localPosition;
        }



    }
    void ActiveIDX(int idx)
    {
        for (int i = 0; i < BGnum; i++)
        {
            for (int type = 0; type < BGType; type++)
            {
                childObjects[type, i].SetActive(false);
            }
        }
        for (int i = 0; i < BGnum; i++)
        {
                childObjects[idx, i].SetActive(true);

        }
    }

    /// <summary>
    /// 0 ∂•¿Ã Ω√¿€«ﬂ¿ª ∂ß 
    /// </summary>
    /// <param name="go"></param>
    public override void StartPlayingAnim(int buttonData = 0)
    {
        base.StartPlayingAnim();


        if (!isPlaying)
        {
            isPlaying = true;
            for (int i = 0; i < BGnum; i++)
            {
                Coroutine[buttonData, i] = StartCoroutine(SmoothMove(childObjects[buttonData, i].transform, i, i - 1));   
            }
        }
        
    }

    /// <summary>
    /// 0 ∂•¿Ã ≥°≥µ¿ª ∂ß 
    /// </summary>
    /// <param name="go"></param>
    public override void EndPlayingAnim(int buttonData) 
    {
        for (int i = 0; i < BGnum; i++)
        {
            StopCoroutine(Coroutine[buttonData, i]);
        }
    }
    public IEnumerator SmoothMove(Transform transform, int startIdx, int endIdx, float moveTime = MOVETIME)
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
                    transform.localPosition = Vector3.Lerp(start, end, t);
                    yield return null;
                }

            }
            else
            {
                startIdx = BGnum-1;
                endIdx = BGnum-2;
                transform.localPosition = position[startIdx];
                yield return new WaitForSeconds(moveTime);

            }
        }
        
    }








}
