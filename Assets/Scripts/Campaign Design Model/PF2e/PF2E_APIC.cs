using System.Collections.Generic;

public class PF2E_APIC
{
    public string name;

    public PF2E_PlayerData playerData;

    private E_PF2E_Ability abilityEnum;
    public List<PF2E_Lecture> profLectures;
    private List<PF2E_Fact> itemModifiers;
    private List<PF2E_Fact> circModifiers;

    public PF2E_APIC(string name, E_PF2E_Ability abilityEnum)
    {
        this.name = name;
        this.abilityEnum = abilityEnum;
    }

    public int total
    {
        get
        {
            return ability + proficiency;
        }
    }

    public int ability
    {
        get
        {
            if (playerData != null)
            {
                switch (abilityEnum)
                {
                    case E_PF2E_Ability.Strength:
                        return playerData.strengthMod;
                    case E_PF2E_Ability.Dexterity:
                        return playerData.dexterityMod;
                    case E_PF2E_Ability.Constitution:
                        return playerData.constitutionMod;
                    case E_PF2E_Ability.Intelligence:
                        return playerData.intelligenceMod;
                    case E_PF2E_Ability.Wisdom:
                        return playerData.wisdomMod;
                    case E_PF2E_Ability.Charisma:
                        return playerData.charismaMod;

                    default:
                        return 0;
                }
            }
            else
                return 0;
        }
    }

    public int proficiency
    {
        get
        {
            if (playerData != null)
            {
                if (profLectures.Count < 0)
                    return 0;

                bool T = false; bool E = false; bool M = false; bool L = false;
                foreach (var item in profLectures)
                    if (item.proficiency == "T")
                        T = true;
                    else if (item.proficiency == "E")
                        E = true;
                    else if (item.proficiency == "M")
                        M = true;
                    else if (item.proficiency == "L")
                        L = true;

                if (!T)
                    return 0;
                else if (!E)
                    return playerData.level + 2;
                else if (!M)
                    return playerData.level + 4;
                else if (!L)
                    return playerData.level + 6;
                else
                    return playerData.level + 8;
            }
            else
                return 0;
        }
    }

    public int item = 0;

    public int circumstantial = 0;
}