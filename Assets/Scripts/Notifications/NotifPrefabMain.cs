using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifPrefabMain : MonoBehaviour
{
    [HideInInspector]
    public NotificationManager notificationManager;
    public CanvasGroup fadeCanvasGroup;
    public Image background;
    public Image image;
    public TMP_Text text;
    public float speed;
    public float lifespan;
    public float fadeDuration;

    RectTransform self;
    int positionInQueue = 0;
    float height;

    public NotifPrefabMain(Image _image, TMP_Text _text)
    {
        image = _image;
        text = _text;
    }

    //suscribo esto al NotificationManager para saber cuando sale otra notificacion
    //se suscribe al instanciarlo en Notification Manager
    public void Rearange(int i)
    {
        positionInQueue = i;
    }

    public void Start()
    {
        self = GetComponent<RectTransform>();
        height = self.sizeDelta.y;

        //posiciono el objeto por encima de la pantalla rozando
        self.anchoredPosition = new Vector2(0, height);
        self.localPosition = new Vector3(self.localPosition.x, self.localPosition.y, 0);//pongo la z a cero

        //comienzo el fadeout
        StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 0f, fadeDuration));
    }

    void Update()
    {
        Vector2 targetPos = new Vector2(0, -positionInQueue * height);
        Vector2 distance = targetPos - self.anchoredPosition;

        //me mueve el objeto
        Vector2 newPos = self.anchoredPosition + distance * speed * Time.unscaledDeltaTime;
        self.anchoredPosition = newPos;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float alphaTarget, float duration)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        yield return new WaitForSecondsRealtime(lifespan);

        float counter = 0;
        float completion = 0;
        float alphaInit = cg.alpha;
        float alphaDif = alphaInit - alphaTarget;

        if (alphaDif != 0)
            while (counter < duration)
            {
                counter += Time.unscaledDeltaTime;
                completion = counter / duration;

                cg.alpha = -Mathf.Clamp(completion, 0f, 1f) * alphaDif + alphaInit;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        else
            Debug.Log("Fade failed: initial alpha equal to target");

        cg.alpha = alphaTarget;

        //le digo al manager que me he muerto
        if (notificationManager != null)
            notificationManager.NotifDeath(gameObject);

        Destroy(gameObject);
    }

}