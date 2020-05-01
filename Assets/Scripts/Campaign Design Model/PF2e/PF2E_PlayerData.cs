using System.Collections.Generic;
using UnityEngine;

public class PF2E_PlayerData : PlayerData
{
    public string guid = "";
    public int level = 0;
    public int experience = 0;

    public string playerClass = "";


    //---------------------------------------------------HIT-POINTS--------------------------------------------------

    // maxHitPoints = level*(classHP+constitution)+ancestryHP+temp

    private int classHP = 0;
    private int ancestryHP = 0;
    private int tempHP = 0;

    private int _damage = 0;
    public int damage
    {
        get { return _damage; }
        set
        {
            _damage += value;
            if (_damage < 0) damage = 0;
        }
    }

    public int hitPoints { get { return maxHitPoints - damage; } }
    public int maxHitPoints { get { return level * (classHP + constitution) + ancestryHP + tempHP; } }


    //---------------------------------------------------SPEED--------------------------------------------------
    private int ancestrySpeed = 0;

    public int baseSpeed { get { return ancestrySpeed; } }
    public int burrowSpeed = 0;
    public int climbSpeed = 0;
    public int flySpeed = 0;
    public int swimSpeed = 0;


    //---------------------------------------------------SIZE--------------------------------------------------
    private string ancestrySize = "Medium";

    public string size { get { return ancestrySize; } }


    //---------------------------------------------------CHARACTER TRAITS--------------------------------------------------
    private List<PF2E_Trait> traits = new List<PF2E_Trait>();

    private void ClearCharacterTraitFrom(string identifier)
    {
        List<PF2E_Trait> toRemove = new List<PF2E_Trait>();
        foreach (var item in traits)
            if (item.from == identifier)
                toRemove.Add(item);
        foreach (var item in toRemove)
            traits.Remove(item);
    }


    //---------------------------------------------------LANGUAGES--------------------------------------------------
    public List<string> languages = new List<string>();

    public void AddLanguage(string newLang)
    {
        if (!languages.Contains(newLang))
            languages.Add(newLang);
    }

    public void RemoveLanguage(string newLang)
    {
        if (languages.Contains(newLang))
            languages.Remove(newLang);
    }


    //---------------------------------------------------ABILITIES--------------------------------------------------

    //       |Ancestry|Background|Class|Lvl1Boost|Lvl5Boost|Lvl10Boost|Lvl15Boost|Lvl20Boost|
    //    STR|        |          |     |         |         |          |          |          |
    //    DEX|        |          |     |         |         |          |          |          |
    //    CON|        |          |     |         |         |          |          |          |
    //    INT|        |          |     |         |         |          |          |          |
    //    WIS|        |          |     |         |         |          |          |          |
    //    CHA|        |          |     |         |         |          |          |          |

    private bool[,] ablMap = new bool[8, 6];

    public int strength { get { return AblScoreCalc(E_PF2E_Ability.Strength); } }
    public int dexterity { get { return AblScoreCalc(E_PF2E_Ability.Dexterity); } }
    public int constitution { get { return AblScoreCalc(E_PF2E_Ability.Constitution); } }
    public int intelligence { get { return AblScoreCalc(E_PF2E_Ability.Intelligence); } }
    public int wisdom { get { return AblScoreCalc(E_PF2E_Ability.Wisdom); } }
    public int charisma { get { return AblScoreCalc(E_PF2E_Ability.Charisma); } }

    public int strengthMod { get { return AblModCalc(strength); } }
    public int dexterityMod { get { return AblModCalc(dexterity); } }
    public int constitutionMod { get { return AblModCalc(constitution); } }
    public int intelligenceMod { get { return AblModCalc(intelligence); } }
    public int wisdomMod { get { return AblModCalc(wisdom); } }
    public int charismaMod { get { return AblModCalc(charisma); } }

    private List<PF2E_AblModifier> abilityFlaws = new List<PF2E_AblModifier>();

    private int AblScoreCalc(E_PF2E_Ability abl)
    {
        int count = 0;
        int score = 10;

        foreach (var item in abilityFlaws)    // FLAWS
            if (PF2E_DataBase.AbilityToEnum(item.target) == abl)
                count -= item.value;

        for (int i = 0; i < 8; i++)           // BOOSTS
            if (ablMap[i, (int)abl - 2])
                count++;

        if (count >= 0)
            for (int i = 0; i < count; i++)
                if (score < 18)
                    score += 2;
                else
                    score++;
        else
            for (int i = 0; i > count; i--)
                score -= 2;

        return score;
    }

    private int AblModCalc(int ablScore)
    {
        return Mathf.FloorToInt((ablScore - 10) / 2);
    }

    public void AddAbility(E_PF2E_AbilityBoost boost, E_PF2E_Ability abl)
    {
        ablMap[(int)boost - 2, (int)abl - 2] = true;
    }

    public void RemoveAbility(E_PF2E_AbilityBoost boost, E_PF2E_Ability abl)
    {
        ablMap[(int)boost - 2, (int)abl - 2] = false;
    }

    public void ClearAbilityBoost(E_PF2E_AbilityBoost boost)
    {
        for (int i = 0; i < 6; i++)
            ablMap[(int)boost - 2, i] = false;
    }

    public void ClearAblFlawsFrom(string identifier)
    {
        List<PF2E_AblModifier> toRemove = new List<PF2E_AblModifier>();
        foreach (var item in abilityFlaws)
            if (item.from == identifier)
                toRemove.Add(item);
        foreach (var item in toRemove)
            abilityFlaws.Remove(item);
    }


    //---------------------------------------------------SKILLS--------------------------------------------------
    private Dictionary<string, PF2E_APIC> skills = new Dictionary<string, PF2E_APIC>()
    {
        {"acrobatics",new PF2E_APIC("Acrobatics" ,E_PF2E_Ability.Dexterity)},
        {"arcana",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Intelligence)},
        {"athletics",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Strength)},
        {"crafting",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Intelligence)},
        {"deception",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Charisma)},
        {"diplomacy",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Charisma)},
        {"intimidation",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Charisma)},
        {"medicine",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Wisdom)},
        {"nature",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Wisdom)},
        {"occultism",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Intelligence)},
        {"performance",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Dexterity)},
        {"religion",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Wisdom)},
        {"society",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Charisma)},
        {"stealth",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Dexterity)},
        {"survival",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Wisdom)},
        {"thievery",new PF2E_APIC("Athletics" ,E_PF2E_Ability.Default)},
        {"lore 1",new PF2E_APIC("" ,E_PF2E_Ability.Intelligence)},
        {"lore 2",new PF2E_APIC("" ,E_PF2E_Ability.Intelligence)},
    };

    public Dictionary<string, PF2E_APIC> GetSkills()
    {
        return skills;
    }

    public PF2E_APIC GetSkill(string skillName)
    {
        if (skills.ContainsKey(skillName))
        {
            PF2E_APIC skill = skills[skillName];

            if (skill.playerData == null)
                skill.playerData = this;

            return skill;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Couldn't find skill: " + skillName + "!");
            return null;
        }
    }

    public void TrainSkill(PF2E_Lecture lecture)
    {
        if (skills.ContainsKey(lecture.target))
        {
            skills[lecture.target].profLectures.Add(lecture);
        }
        else
        {
            Debug.LogWarning("[PlayerData] Tried to train skill: " + lecture.target + " but couldn't find it!");
        }
    }



    //---------------------------------------------------SKILLS--------------------------------------------------
    //---------------------------------------------------LECTURES--------------------------------------------------
    private List<PF2E_Lecture> unusedLectures = new List<PF2E_Lecture>();

    // private



    //---------------------------------------------------ANCESTRY--------------------------------------------------
    private string _ancestry = "";
    public string ancestry { get { return _ancestry; } set { SetAncestry(value); } }

    private void SetAncestry(string newAncestry)
    {
        if (PF2E_DataBase.Ancestries.ContainsKey(newAncestry))
        {
            PF2E_Ancestry ancestry = PF2E_DataBase.Ancestries[newAncestry];
            _ancestry = newAncestry;

            ancestryHP = ancestry.hitPoints;

            ancestrySpeed = ancestry.speed;

            ancestrySize = ancestry.size;

            ClearAblFlawsFrom("ancestry");
            foreach (var item in ancestry.abilityFlaws)
                abilityFlaws.Add(item.Value);

            ClearCharacterTraitFrom("ancestry");
            foreach (var item in ancestry.traits)
                traits.Add(item.Value);
        }
        else
        {
            Debug.LogWarning("[PlayerData] (" + playerName + ") Can't find ancestry: " + newAncestry + "!");
        }
    }


    //---------------------------------------------------BACKGROUND--------------------------------------------------
    private string _background = "";
    public string background { get { return _background; } set { SetBackground(value); } }

    private void SetBackground(string newBackground)
    {
        if (PF2E_DataBase.Backgrounds.ContainsKey(newBackground))
        {
            PF2E_Background background = PF2E_DataBase.Backgrounds[newBackground];
            _background = newBackground;

            // Lectures



            // Skill Feats

        }

    }







}

public class skills
{
    public E_PF2E_Ability ability;
    public List<PF2E_Lecture> prof;
    public List<PF2E_Effect> effects;
}
