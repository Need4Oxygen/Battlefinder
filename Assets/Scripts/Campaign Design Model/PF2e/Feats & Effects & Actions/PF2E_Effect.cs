using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PF2E_Effect
{
    public string name;
    [TextArea(1, 10)] public string description;

    [Space(15)]
    public E_PF2E_EffectAplication application;
    public List<PF2E_Fact> facts = new List<PF2E_Fact>();

    public List<PF2E_Fact> Resolve(ref PF2E_PlayerData playerData)
    {
        switch (application)
        {
            case E_PF2E_EffectAplication.None:
                return null;
            case E_PF2E_EffectAplication.Default:
                return facts;
            case E_PF2E_EffectAplication.Complex:
                return PF2E_EffectResolver.Resolve(ref playerData, name);
            case E_PF2E_EffectAplication.Both:
                facts.AddRange(PF2E_EffectResolver.Resolve(ref playerData, name));
                return facts;

            default:
                return facts;
        }
    }
}
