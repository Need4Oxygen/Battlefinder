using UnityEngine;

public class Positionable
{
    public string name;
    public E_Positionables type;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

public class PositionableActor : Positionable
{
    public E_Actors actorType;
    public string actorGuid;
    public string actorName;
}