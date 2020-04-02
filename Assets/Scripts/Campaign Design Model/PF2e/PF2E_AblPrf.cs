using UnityEngine;

public class PF2E_AblPrf
{
    public string name;
    public E_PF2E_Ability keyAbility;
    public E_PF2E_Proficiency proficiency;

    public PF2E_AblPrf(string name, E_PF2E_Ability keyAbility, E_PF2E_Proficiency proficiency)
    {
        this.name = name;
        this.keyAbility = keyAbility;
        this.proficiency = proficiency;
    }
}