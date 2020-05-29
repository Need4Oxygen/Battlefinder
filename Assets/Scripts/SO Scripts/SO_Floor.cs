using UnityEngine;

[CreateAssetMenu(fileName = "New Floor", menuName = "Board Decor/Floor", order = 1)]
public class SO_Floor : ScriptableObject
{
    public string floorName;
    public int terrainLayer;
    public Sprite buttonSprite;
}
