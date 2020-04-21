using System;
using UnityEngine;

[Serializable]
public class PF2E_Lecture
{
    public string name;
    [TextArea(1, 10)] public string description;

    public E_PF2E_Traineable traineable;
    public E_PF2E_Proficiency adquiredProficiency;
    public bool turnFreeIfAlreadyTrained;
}
