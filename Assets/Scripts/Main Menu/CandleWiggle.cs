using UnityEngine;
using System.Collections;

public class CandleWiggle : MonoBehaviour
{

    public AnimationCurve curve;
    public Vector3 distance;
    public float speed;

    private Vector3 startPos, targetPos;
    private float timeStart;

    void Start()
    {
        startPos = transform.position;
        GetRandomPos();
    }

    void Update()
    {
        float d = (Time.time - timeStart) / speed, m = curve.Evaluate(d);
        if (d > 1)
        {
            GetRandomPos();
        }
        else if (d < 0.5)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, m * 2.0f);
        }
        else
        {
            transform.position = Vector3.Lerp(targetPos, startPos, (m - 0.5f) * 2.0f);
        }
    }

    private void GetRandomPos()
    {
        targetPos = startPos;
        targetPos.x += Random.Range(-1.0f, +1.0f) * distance.x;
        targetPos.y += Random.Range(-1.0f, +1.0f) * distance.y;
        targetPos.z += Random.Range(-1.0f, +1.0f) * distance.z;
        timeStart = Time.time;
    }
}