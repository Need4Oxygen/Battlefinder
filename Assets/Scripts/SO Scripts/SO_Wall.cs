using UnityEngine;

[CreateAssetMenu(fileName = "New Wall", menuName = "Board Decor/Wall", order = 1)]
public class SO_Wall : ScriptableObject
{
    public string wallName;
    public Transform knot;
    public Transform wall;
    public Sprite buttonSprite;
}
