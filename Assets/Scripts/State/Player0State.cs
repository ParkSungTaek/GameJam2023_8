using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player0State : State
{
    const float MOVETIME =22f;
    Vector3[] position = new Vector3[4];

    [SerializeField]
    GameObject[] childObjects;
    const int BGnum = 3;

    Coroutine[] Coroutine;
    bool isPlaying = false;
    private void Start()
    {
        childObjects = new GameObject[BGnum];
        Coroutine = new Coroutine[BGnum];
        for (int i = 0; i < BGnum; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
            position[i] = childObjects[i].transform.localPosition;
        }

    }
    /// <summary>
    /// 0 ∂•¿Ã Ω√¿€«ﬂ¿ª ∂ß 
    /// </summary>
    /// <param name="go"></param>
    public override void StartPlayingAnim(PlayerController go, ButtonData buttonData = null)
    {
        base.StartPlayingAnim(go);


        GameUI.Instance.SetRoadColor(new Color((193f / 255f), (209f / 255f), 0));
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
