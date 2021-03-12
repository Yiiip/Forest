using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Dog : MonoBehaviour
{
    public UILineRenderer Path;
    public float Duration = 5f;
    [Range(0, 5)] public float DurationStartAt = 4f;
    public bool IsForward = true;
    public bool IsLoop = true;

    public float startAtNormalized;
    public int startP = 1;
    public float pieceProgress;
    public float allDis;

    public List<Vector2> points;

    void OnEnable()
    {
        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        while (IsLoop)
        {
            while (Path == null)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);

            points.Clear();
            points.AddRange(Path.DrawingPoints);


            allDis = 0f;
            for (int i = 1; i < points.Count; i++)
            {
                if (!points.IsIndexValid(i) || !points.IsIndexValid(i - 1))
                {
                    continue;
                }
                allDis += Vector2.Distance(points[i - 1], points[i]);
            }

            startAtNormalized = DurationStartAt / Duration;


            if (IsForward)
            {

                startP = 1;
                float t = 0f;
                for (int i = 1; i < points.Count; i++)
                {
                    if (!points.IsIndexValid(i) || !points.IsIndexValid(i - 1))
                    {
                        continue;
                    }
                    var begin = points[i - 1];
                    var end = points[i];
                    var dur = Duration * Vector2.Distance(begin, end) / allDis;
                    t += dur;
                    float t0 = t - dur;
                    float t1 = t;
                    float t0n = t0 / Duration;
                    float t1n = t1 / Duration;
                    if (startAtNormalized >= t0n && startAtNormalized < t1n)
                    {
                        startP = i;
                        pieceProgress = (DurationStartAt - t0) / (t1 - t0);
                        break;
                    }
                }
                // transform.localPosition = points[startP]; //rough position

                //正向
                for (int i = startP; i < points.Count; i++)
                {
                    if (!points.IsIndexValid(i) || !points.IsIndexValid(i - 1))
                    {
                        continue;
                    }
                    var begin = points[i - 1];
                    var end = points[i];
                    float dur = Duration * Vector2.Distance(begin, end) / allDis;
                    transform.localPosition = Vector2.Lerp(begin, end, pieceProgress);
                    while (pieceProgress < 1f)
                    {
                        pieceProgress += Time.deltaTime / dur;
                        transform.localPosition = Vector2.Lerp(begin, end, pieceProgress);
                        yield return null;
                    }
                    pieceProgress = 0f;
                }

            }
            else
            {

                startP = points.Count - 2;
                float t = 0f;
                for (int i = points.Count - 2; i >= 0; i--)
                {
                    if (!points.IsIndexValid(i) || !points.IsIndexValid(i + 1))
                    {
                        continue;
                    }
                    var begin = points[i + 1];
                    var end = points[i];
                    var dur = Duration * Vector2.Distance(begin, end) / allDis;
                    t += dur;
                    float t0 = t - dur;
                    float t1 = t;
                    float t0n = t0 / Duration;
                    float t1n = t1 / Duration;
                    if (startAtNormalized >= t0n && startAtNormalized < t1n)
                    {
                        startP = i;
                        pieceProgress = (DurationStartAt - t0) / (t1 - t0);
                        break;
                    }
                }

                //反向
                for (int i = startP; i >= 0; i--)
                {
                    if (!points.IsIndexValid(i) || !points.IsIndexValid(i + 1))
                    {
                        continue;
                    }
                    var begin = points[i + 1];
                    var end = points[i];
                    float dur = Duration * Vector2.Distance(begin, end) / allDis;
                    transform.localPosition = Vector2.Lerp(begin, end, pieceProgress);
                    while (pieceProgress < 1f)
                    {
                        pieceProgress += Time.deltaTime / dur;
                        transform.localPosition = Vector2.Lerp(begin, end, pieceProgress);
                        yield return null;
                    }
                    pieceProgress = 0f;
                }

            }
        }
    }


}
