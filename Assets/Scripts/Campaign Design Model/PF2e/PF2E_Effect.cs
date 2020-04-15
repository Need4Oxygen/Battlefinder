using System;
using UnityEngine;

[Serializable]
public class PF2E_Effect
{
    public string name;                            // Name
    [TextArea(1, 10)]
    public string description;                     // Where it comes from
    public E_PF2E_EffectAplication application;    // What does it affect
    public E_PF2E_EffectTarget target;             // What does it affect
    public E_PF2E_EffectType effectType;           // How does it affect
    public int value;                              // How much does it affect
}