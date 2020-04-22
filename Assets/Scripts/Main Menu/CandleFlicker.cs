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

    private float startRange;
    private float startIntensity;
    private float targetRange;
    private float targetIntensity;

    public void Start()
    {
        StartCoroutine(DoFlicker());
    }

    void Update()
    {
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
}
