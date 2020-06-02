using System.Collections;
using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    [SerializeField] private Light lightSource = null;

    [Header("Light Flicker Variation")]
    [SerializeField] private bool isFlickering = true;
    [SerializeField] private float flickerSpeed = 1f;
    [Space(5)]
    [SerializeField] private float intensityMultiplier = 3f;
    [SerializeField] private float rangeMultiplier = 3f;

    [Header("Light Shake Variation")]
    [SerializeField] private bool isShaking = true;
    [SerializeField] private float shakeSpeed = 1f;
    [Space(5)]
    [SerializeField] private Vector3 shakeMultiplier = Vector3.one;

    private float shakeTimeCount;
    private float flickerTimeCount;

    private Vector3 startPos;
    private float startIntensity;
    private float startRange;

    void Awake()
    {
        startPos = transform.localPosition;
        startIntensity = lightSource.intensity;
        startRange = lightSource.range;
    }

    void Update()
    {
        if (isFlickering)
        {
            flickerTimeCount += Time.deltaTime * flickerSpeed;

            float newIntensity = startIntensity + (GetPerlin(50, flickerTimeCount) * intensityMultiplier);
            float newRange = startRange + (GetPerlin(5, flickerTimeCount) * rangeMultiplier);
            lightSource.intensity = newIntensity;
            lightSource.range = newRange;

        }
        else
        {
            lightSource.intensity = startIntensity;
            lightSource.range = startRange;
        }

        if (isShaking)
        {
            shakeTimeCount += Time.deltaTime * shakeSpeed;

            Vector3 newPos = startPos + GetShakeVector();
            transform.localPosition = newPos;
        }
        else if (transform.localPosition != startPos)
        {
            //lerp back towards default position.
            Vector3 newPos = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 5f);
            transform.localPosition = newPos;
            if (Vector3.Distance(transform.localPosition, startPos) < 0.01f)
                transform.localPosition = startPos;
        }
    }

    //Perlin float between -1 and 1.
    private float GetPerlin(float seed, float counter)
    {
        return (Mathf.PerlinNoise(seed, counter) - 0.5f) * 2f;
    }

    //Generate a Vector3, using different seeds to ensure different numbers
    private Vector3 GetShakeVector()
    {
        return new Vector3(
            GetPerlin(1, shakeTimeCount) * shakeMultiplier.x,
            GetPerlin(10, shakeTimeCount) * shakeMultiplier.y,
            GetPerlin(100, shakeTimeCount) * shakeMultiplier.z
            );
    }
}
