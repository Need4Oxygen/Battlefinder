using UnityEngine;

public class PWallElement : Positionable
{
    public E_WallElement type;
    public string style;

    public PWallElement(string name, Vector3 position, Quaternion rotation, Vector3 localScale, E_WallElement type, string style)
    : base(name, position, rotation, localScale)
    {
        this.type = type;
        this.style = style;
    }
}
