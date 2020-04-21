using System;
using UnityEngine;

[Serializable]
public class PF2E_Action
{
    public string title;
    public E_PF2E_ActionType type;
    [TextArea(1, 10)] public string trigger;
    [TextArea(1, 10)] public string description;
}
