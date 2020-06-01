using UnityEngine;

[CreateAssetMenu(fileName = "New Wall", menuName = "Board Decor/Wall", order = 1)]
public class SO_WallStyle : ScriptableObject
{
    public string styleName;
    public Transform knot;
    public Transform wall;
    public Sprite buttonSprite;
}
