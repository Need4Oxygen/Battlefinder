using UnityEngine;

public class SetIdleAnim : MonoBehaviour
{
    private enum AnimationType { Sitting, Standing, Table };

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int standingAnimsTotal;
    [SerializeField]
    private int sittingAnimsTotal;
    [SerializeField]
    private int tableAnimsTotal;

    [Space(12)]

    [SerializeField]
    private AnimationType animationType;
    [SerializeField]
    private bool walking;
    [SerializeField]
    private bool laidBack;
    [SerializeField]
    private bool cupHand;

    private string anim = null;

    void OnEnable()
    {
        switch (animationType)
        {
            case AnimationType.Standing:
                if (walking)
                    anim = "walking";
                else
                    anim = "standing" + Random.Range(1, standingAnimsTotal + 1);
                break;
            case AnimationType.Sitting:
                if (laidBack)
                    anim = "laidBack";
                else if (cupHand)
                    anim = "cupHand";
                else
                    anim = "sitting" + Random.Range(1, sittingAnimsTotal + 1);
                break;
            case AnimationType.Table:
                anim = "table" + Random.Range(1, tableAnimsTotal + 1);
                break;
        }
        Invoke("DelayedAnim", Random.Range(0f, 3f));
    }

    void DelayedAnim()
    {
        animator.SetTrigger(anim);
    }
}