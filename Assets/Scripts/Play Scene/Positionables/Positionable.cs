using UnityEngine;

public class Positionable
{
    public string name;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;

    public Positionable(string name, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.localScale = localScale;
    }
}
