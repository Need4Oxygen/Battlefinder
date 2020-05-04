using System.Collections.Generic;

public class PF2E_APIC
{
    public string name = "";

    public PF2E_PlayerData playerData = null;

    private E_PF2E_Ability abilityEnum = E_PF2E_Ability.None;
    public List<PF2E_Lecture> lectures = new List<PF2E_Lecture>();
    private List<PF2E_Fact> itemModifiers = new List<PF2E_Fact>();
    private List<PF2E_Fact> circModifiers = new List<PF2E_Fact>();

    public PF2E_APIC(string name, E_PF2E_Ability abilityEnum)
    {
        this.name = name;
        this.abilityEnum = abilityEnum;
    }

    public int total
    {
        get
        {
            return ablScore + profScore;
        }
    }

    public int ablScore
    {
        get
        {
            if (playerData != null)
            {
                switch (abilityEnum)
                {
                    case E_PF2E_Ability.Strength:
                        return playerData.abl_strengthMod;
                    case E_PF2E_Ability.Dexterity:
                        return playerData.abl_dexterityMod;
                    case E_PF2E_Ability.Constitution:
                        return playerData.abl_constitutionMod;
                    case E_PF2E_Ability.Intelligence:
                        return playerData.abl_intelligenceMod;
                    case E_PF2E_Ability.Wisdom:
                        return playerData.abl_wisdomMod;
                    case E_PF2E_Ability.Charisma:
                        return playerData.abl_charismaMod;

                    default:
                        return 0;
                }
            }
            else
                return 0;
        }
    }

    public int profScore
    {
        get
        {
            if (playerData != null)
            {
                if (lectures.Count < 0)
                    return 0;

                bool T = false; bool E = false; bool M = false; bool L = false;
                foreach (var item in lectures)
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

    public E_PF2E_Proficiency profEnum { get { return PF2E_DataBase.GetMaxProfEnum(lectures); } }

    public int itemScore = 0;

    public int circScore = 0;
}
