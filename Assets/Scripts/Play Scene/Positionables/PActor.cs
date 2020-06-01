using UnityEngine;

public class PActor : Positionable
{
    public E_Actors type;
    public string guid;
    public string actorName;

    public PActor(string name, Vector3 position, Quaternion rotation, Vector3 localScale, E_Actors type, string guid, string actorName)
    : base(name, position, rotation, localScale)
    {
        this.type = type;
        this.guid = guid;
        this.actorName = actorName;
    }
}
