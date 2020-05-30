using System.Collections;
using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    [SerializeField] private Light lightSource = null;
    [SerializeField] public float minIntensity = 1f;
    [SerializeField] public float maxIntensity = 3f;
    [SerializeField] public float minRange = 1f;
    [SerializeField] public float maxRange = 3f;
    [SerializeField] public Vector2 randomTimeBetweenChanges = new Vector2(0.1f, 0.8f);
    [SerializeField] public float changeSpeed = 1f;
    [SerializeField] public bool stopFlickering = default;

    public AnimationCurve curve;
    public Vector3 distance;
    public Vector2 randomSpeedRange;
    
    private float speed;

    private Vector3 startPos, targetPos;
    private float timeStart;

    private float targetRange;
    private float targetIntensity;

    void Start()
    {
        startPos = transform.position;
        GetRandomPos();
        StartCoroutine(DoFlicker());
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

        if (!stopFlickering)
        {
            lightSource.range = Mathf.Lerp(lightSource.range, targetRange, Time.deltaTime * changeSpeed);
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, Time.deltaTime * changeSpeed);
        }
    }

    private IEnumerator DoFlicker()
    {
        while (!stopFlickering)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            targetRange = Random.Range(minRange, maxRange);
            yield return new WaitForSeconds(Random.Range(randomTimeBetweenChanges.x, randomTimeBetweenChanges.y));
        }
    }

    private void GetRandomPos()
    {
        speed = Random.Range(randomSpeedRange.x, randomSpeedRange.y);
        targetPos = startPos;
        targetPos.x += Random.Range(-1.0f, +1.0f) * distance.x;
        targetPos.y += Random.Range(-1.0f, +1.0f) * distance.y;
        targetPos.z += Random.Range(-1.0f, +1.0f) * distance.z;
        timeStart = Time.time;
    }
}
