using UnityEngine;

public class WallElement : MonoBehaviour, IDestroyable
{
    [HideInInspector] public WallTool wallTool;

    public E_WallElement type;
    public SO_WallStyle style;

    public void Destroy()
    {
        if (wallTool != null)
            wallTool.DestroyElement(this);
    }
}
