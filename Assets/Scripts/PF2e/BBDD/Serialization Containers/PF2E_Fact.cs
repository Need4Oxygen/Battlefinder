using System;
using System.Collections.Generic;

[Serializable]
public class PF2E_Fact
{
    public string name;
    public string description;
    public int value;
    public E_PF2E_EffectType effectType;
    public E_PF2E_EffectTarget target;
    public List<string> effectTriggers;

    public PF2E_Fact(string name, string description, int value, E_PF2E_EffectTarget target, E_PF2E_EffectType effectType)
    {
        this.name = name;
        this.description = description;
        this.value = value;
        this.target = target;
        this.effectType = effectType;
    }

    public PF2E_Fact(string name, string description, int value, E_PF2E_EffectTarget target, E_PF2E_EffectType effectType, List<string> effectTriggers)
    {
        this.name = name;
        this.description = description;
        this.value = value;
        this.target = target;
        this.effectType = effectType;
        this.effectTriggers = effectTriggers;
    }
}
