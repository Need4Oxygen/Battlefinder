using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PF2E_PlayerData
{
    public string guid = "";
    public string playerName = "";


    //---------------------------------------------------LEVEL--------------------------------------------------
    private int _experience = 0;
    public int experience
    {
        get { return _experience; }
        set { }
    }

    private int _level = 1;
    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            if (value < 1)
                _level = 1;
            else if (value > 20)
                _level = 20;
            else
                _level = value;
        }
    }


    //---------------------------------------------------HIT POINTS--------------------------------------------------
    // maxHitPoints = level*(classHP+constitution)+ancestryHP+temp

    private int hp_class = 0;
    private int hp_ancestry = 0;
    private int hp_temp = 0;

    private int _damage = 0;
    public int hp_damage
    {
        get { return _damage; }
        set
        {
            _damage += value;
            if (_damage < 0) hp_damage = 0;
        }
    }

    public int hp_current { get { return hp_max - hp_damage; } }
    public int hp_max { get { return level * (hp_class + abl_constitutionMod) + hp_ancestry + hp_temp; } }


    //---------------------------------------------------CLASS DC--------------------------------------------------
    private List<PF2E_Lecture> classDC_lectures = new List<PF2E_Lecture>();

    public E_PF2E_Proficiency classDC { get { return PF2E_DataBase.GetMaxProfEnum(classDC_lectures); } }

    public void ClassDC_ClearFrom(string from)
    {
        ClearLecturesFrom(classDC_lectures, from);
    }

    public bool ClassDC_Train(PF2E_Lecture lecture)
    {
        if (lecture.target == "classDC")
        {
            classDC_lectures.Add(lecture);
            return true;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Tried to train classDC: " + lecture.target + " but wtf!");
            return false;
        }
    }

    //---------------------------------------------------SPEED--------------------------------------------------
    private int speed_ancestry = 0;

    public int speed_base { get { return speed_ancestry; } }
    public int speed_burrow = 0;
    public int speed_climp = 0;
    public int speed_fly = 0;
    public int speed_swim = 0;


    //---------------------------------------------------SIZE--------------------------------------------------
    private string _size_ancestry = "Medium";
    private string _size_temp = "";

    public string size
    {
        get
        {
            if (_size_temp != "")
                return _size_temp;
            else
                return _size_ancestry;
        }
        set
        {
            _size_temp = value;
        }
    }


    //---------------------------------------------------CHARACTER TRAITS--------------------------------------------------
    public List<PF2E_Trait> traits_list = new List<PF2E_Trait>();

    private void Traits_ClearFrom(string from)
    {
        traits_list.RemoveAll(item => item.from == from);
    }


    //---------------------------------------------------LANGUAGES--------------------------------------------------
    public List<string> languages_list = new List<string>();

    public void Languages_Add(string newLang)
    {
        if (!languages_list.Contains(newLang))
            languages_list.Add(newLang);
    }

    public void Languages_Remove(string newLang)
    {
        if (languages_list.Contains(newLang))
            languages_list.Remove(newLang);
    }


    //---------------------------------------------------ABILITIES--------------------------------------------------

    //       |Ancestry|Background|Class|Lvl1Boost|Lvl5Boost|Lvl10Boost|Lvl15Boost|Lvl20Boost|
    //    STR|        |          |     |         |         |          |          |          |
    //    DEX|        |          |     |         |         |          |          |          |
    //    CON|        |          |     |         |         |          |          |          |
    //    INT|        |          |     |         |         |          |          |          |
    //    WIS|        |          |     |         |         |          |          |          |
    //    CHA|        |          |     |         |         |          |          |          |

    private bool[,] abl_map = new bool[8, 6];

    public int abl_strength { get { return Abl_ScoreCalc(E_PF2E_Ability.Strength); } }
    public int abl_dexterity { get { return Abl_ScoreCalc(E_PF2E_Ability.Dexterity); } }
    public int abl_constitution { get { return Abl_ScoreCalc(E_PF2E_Ability.Constitution); } }
    public int abl_intelligence { get { return Abl_ScoreCalc(E_PF2E_Ability.Intelligence); } }
    public int abl_wisdom { get { return Abl_ScoreCalc(E_PF2E_Ability.Wisdom); } }
    public int abl_charisma { get { return Abl_ScoreCalc(E_PF2E_Ability.Charisma); } }

    public int abl_strengthMod { get { return Abl_ModCalc(abl_strength); } }
    public int abl_dexterityMod { get { return Abl_ModCalc(abl_dexterity); } }
    public int abl_constitutionMod { get { return Abl_ModCalc(abl_constitution); } }
    public int abl_intelligenceMod { get { return Abl_ModCalc(abl_intelligence); } }
    public int abl_wisdomMod { get { return Abl_ModCalc(abl_wisdom); } }
    public int abl_charismaMod { get { return Abl_ModCalc(abl_charisma); } }

    private List<PF2E_AblModifier> abl_flawsList = new List<PF2E_AblModifier>();

    private int Abl_ScoreCalc(E_PF2E_Ability abl)
    {
        int count = 0;
        int score = 10;

        foreach (var item in abl_flawsList)    // FLAWS
            if (PF2E_DataBase.AbilityToEnum(item.target) == abl)
                count -= item.value;

        for (int i = 0; i < 8; i++)           // BOOSTS
            if (abl_map[i, (int)abl - 2])
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

    private int Abl_ModCalc(int ablScore)
    {
        return Mathf.FloorToInt((ablScore - 10) / 2);
    }

    public void Abl_Add(E_PF2E_AbilityBoost boost, E_PF2E_Ability abl)
    {
        abl_map[(int)boost - 2, (int)abl - 2] = true;
    }

    public void Abl_Remove(E_PF2E_AbilityBoost boost, E_PF2E_Ability abl)
    {
        abl_map[(int)boost - 2, (int)abl - 2] = false;
    }

    public void Abl_ClearBoost(E_PF2E_AbilityBoost boost)
    {
        for (int i = 0; i < 6; i++)
            abl_map[(int)boost - 2, i] = false;
    }

    public void Abl_ClearFlawsFrom(string from)
    {
        abl_flawsList.RemoveAll(item => item.from == from);
    }


    //---------------------------------------------------SKILLS--------------------------------------------------
    private Dictionary<string, PF2E_APIC> skills_dic = new Dictionary<string, PF2E_APIC>()
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

    public Dictionary<string, PF2E_APIC> Skills_GetAll()
    {
        foreach (var item in skills_dic)
        {
            if (item.Value.playerData == null)
                item.Value.playerData = this;
        }
        return skills_dic;
    }

    ///<summary> Retrieve all Untrained skills. </summary>
    public Dictionary<string, PF2E_APIC> Skills_GetUntrained()
    {
        Debug.LogWarning("[PlayerData] Not implemented!");
        return skills_dic;
    }

    ///<summary> Retrieve all skills with proficiency under Legendary. </summary>
    public Dictionary<string, PF2E_APIC> Skills_GetTraineable()
    {
        Debug.LogWarning("[PlayerData] Not implemented!");
        return skills_dic;
    }

    public PF2E_APIC Skills_Get(string skillName)
    {
        if (skills_dic.ContainsKey(skillName))
        {
            PF2E_APIC skill = skills_dic[skillName];

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

    public void Skills_ClearFrom(string from)
    {
        foreach (var item in skills_dic)
            ClearLecturesFrom(item.Value.lectures, "background");
    }

    ///<summary> Train skill via lecture, saving a copy in an APIC object. </summary>
    ///<returns> True if it could be trained. False if it was already trained. </returns>
    public bool Skills_Train(PF2E_Lecture lecture)
    {
        if (skills_dic.ContainsKey(lecture.target))
        {
            if (skills_dic[lecture.target].profEnum < PF2E_DataBase.ProficiencyToEnum(lecture.proficiency))
            {
                skills_dic[lecture.target].lectures.Add(lecture);
                if (lecture.target == "lore 1")
                    skills_dic[lecture.target].name = lecture.name;
                return true;
            }
            else
            {
                lectures_unused.Add(lecture);
                return false;
            }
        }
        else
        {
            Debug.LogWarning("[PlayerData] Tried to train skill: " + lecture.target + " but couldn't find it!");
            return false;
        }
    }


    //---------------------------------------------------PERCEPTION--------------------------------------------------
    private PF2E_APIC perception = new PF2E_APIC("perception", E_PF2E_Ability.Wisdom);

    public E_PF2E_Proficiency perception_prof
    {
        get
        {
            if (perception.playerData == null)
                perception.playerData = this;
            return PF2E_DataBase.GetMaxProfEnum(perception.lectures);
        }
    }
    public int perception_score
    {
        get
        {
            if (perception.playerData == null)
                perception.playerData = this;
            return perception.total;
        }
    }

    public void Perception_ClearFrom(string from)
    {
        ClearLecturesFrom(perception.lectures, from);
    }

    public bool Perception_Train(PF2E_Lecture lecture)
    {
        if (lecture.target == "perception")
        {
            perception.lectures.Add(lecture);
            return true;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Tried to train perception: " + lecture.target + " but wtf!");
            return false;
        }
    }


    //---------------------------------------------------SAVES--------------------------------------------------
    private Dictionary<string, List<PF2E_Lecture>> saves_lectures = new Dictionary<string, List<PF2E_Lecture>>
    {
        {"fortitude", new List<PF2E_Lecture>() },
        {"reflex", new List<PF2E_Lecture>() },
        {"will", new List<PF2E_Lecture>() },
    };

    public E_PF2E_Proficiency saves_fortitude { get { return PF2E_DataBase.GetMaxProfEnum(saves_lectures["fortitude"]); } }
    public E_PF2E_Proficiency saves_reflex { get { return PF2E_DataBase.GetMaxProfEnum(saves_lectures["reflex"]); } }
    public E_PF2E_Proficiency saves_will { get { return PF2E_DataBase.GetMaxProfEnum(saves_lectures["will"]); } }

    public void Saves_ClearFrom(string from)
    {
        foreach (var item in saves_lectures)
            ClearLecturesFrom(item.Value, from);
    }

    public bool Saves_Train(PF2E_Lecture lecture)
    {
        if (saves_lectures.ContainsKey(lecture.target))
        {
            saves_lectures[lecture.target].Add(lecture);
            return true;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Tried to train save throw: " + lecture.target + " but couldn't find it!");
            return false;
        }
    }

    //---------------------------------------------------WEAPONS/ARMOR PROFICIENCIES--------------------------------------------------
    private Dictionary<string, List<PF2E_Lecture>> weaponArmor_lectures = new Dictionary<string, List<PF2E_Lecture>>
    {
        {"unarmed", new List<PF2E_Lecture>() },
        {"simpleWeapons", new List<PF2E_Lecture>() },
        {"martialWeapons", new List<PF2E_Lecture>() },
        {"advancedWeapons", new List<PF2E_Lecture>() },

        {"unarmored", new List<PF2E_Lecture>() },
        {"lightArmor", new List<PF2E_Lecture>() },
        {"mediumArmor", new List<PF2E_Lecture>() },
        {"heavyArmor", new List<PF2E_Lecture>() },
    };

    public E_PF2E_Proficiency unarmed { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["unarmed"]); } }
    public E_PF2E_Proficiency simpleWeapons { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["simpleWeapons"]); } }
    public E_PF2E_Proficiency martialWeapons { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["martialWeapons"]); } }
    public E_PF2E_Proficiency advancedWeapons { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["advancedWeapons"]); } }

    public E_PF2E_Proficiency unarmored { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["unarmored"]); } }
    public E_PF2E_Proficiency lightArmor { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["lightArmor"]); } }
    public E_PF2E_Proficiency mediumArmor { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["mediumArmor"]); } }
    public E_PF2E_Proficiency heavyArmor { get { return PF2E_DataBase.GetMaxProfEnum(weaponArmor_lectures["heavyArmor"]); } }

    public void WeaponArmor_ClearFrom(string from)
    {
        foreach (var item in weaponArmor_lectures)
            ClearLecturesFrom(item.Value, from);
    }

    public bool WeaponArmor_Train(PF2E_Lecture lecture)
    {
        if (weaponArmor_lectures.ContainsKey(lecture.target))
        {
            weaponArmor_lectures[lecture.target].Add(lecture);
            return true;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Tried to train weapon/armor: " + lecture.target + " but couldn't find it!");
            return false;
        }
    }


    //---------------------------------------------------LECTURES MANAGEMENT--------------------------------------------------
    private List<PF2E_Lecture> lectures_unused = new List<PF2E_Lecture>();

    public bool Lectures_Allocate(PF2E_Lecture lecture)
    {
        bool trainSuccessful = false;

        List<PF2E_Lecture> lmao = new List<PF2E_Lecture>();

        List<PF2E_Lecture> lmao2 = lmao.FindAll(item => item.from == "lmao");


        if (lecture.target == "unarmed" || lecture.target == "simpleWeapons" || lecture.target == "martialWeapons" ||
        lecture.target == "advancedWeapons" || lecture.target == "unarmored" || lecture.target == "lightArmor" ||
        lecture.target == "mediumArmor" || lecture.target == "heavyArmor")
        {
            trainSuccessful = WeaponArmor_Train(lecture);
        }
        else if (lecture.target == "fortitude" || lecture.target == "reflex" || lecture.target == "will")
        {
            trainSuccessful = Saves_Train(lecture);
        }
        else if (lecture.target == "perception")
        {
            trainSuccessful = Perception_Train(lecture);
        }
        else if (lecture.target == "acrobatics" || lecture.target == "arcana" || lecture.target == "athletics" || lecture.target == "crafting" ||
        lecture.target == "deception" || lecture.target == "diplomacy" || lecture.target == "intimidation" || lecture.target == "medicine" ||
        lecture.target == "nature" || lecture.target == "occultism" || lecture.target == "performance" || lecture.target == "religion" ||
        lecture.target == "society" || lecture.target == "stealth" || lecture.target == "survival" || lecture.target == "thievery" ||
        lecture.target == "lore 1" || lecture.target == "lore 2")
        {
            trainSuccessful = Skills_Train(lecture);
        }

        return trainSuccessful;
    }


    //---------------------------------------------------ANCESTRY--------------------------------------------------
    private string _ancestry = "";
    public string ancestry { get { return _ancestry; } set { SetAncestry(value); } }

    private void SetAncestry(string newAncestry)
    {
        if (PF2E_DataBase.Ancestries.ContainsKey(newAncestry))
        {
            PF2E_Ancestry ancestry = PF2E_DataBase.Ancestries[newAncestry];
            _ancestry = newAncestry;

            hp_ancestry = ancestry.hitPoints;

            speed_ancestry = ancestry.speed;

            _size_ancestry = ancestry.size;

            Abl_ClearFlawsFrom("ancestry");
            foreach (var item in ancestry.abilityFlaws)
                abl_flawsList.Add(item.Value);

            Traits_ClearFrom("ancestry");
            foreach (var item in ancestry.traits)
                traits_list.Add(item.Value);
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

            Skills_ClearFrom("background");
            Skills_Get("lore 1").name = "";
            foreach (var item in background.lectures)
                Skills_Train(item.Value);
        }
    }


    //---------------------------------------------------CLASS--------------------------------------------------
    private string _playerClass = "";
    public string playerClass { get { return _playerClass; } set { SetClass(value); } }

    public int class_freeSkillTrains = 0;

    private void SetClass(string newClass)
    {
        if (PF2E_DataBase.Classes.ContainsKey(newClass))
        {
            PF2E_Class classObj = PF2E_DataBase.Classes[newClass];
            _playerClass = newClass;

            hp_class = classObj.hitPoints;

            class_freeSkillTrains = classObj.freeSkillTrains;

            Skills_ClearFrom("class");
            foreach (var item in classObj.classSkillsTrains)
                Skills_Train(item.Value);

            Perception_ClearFrom("class");
            Saves_ClearFrom("class");
            WeaponArmor_ClearFrom("class");
            ClassDC_ClearFrom("class");
            foreach (var item in classObj.classSkillsTrains)
                Lectures_Allocate(item.Value);

        }
    }


    //---------------------------------------------------TOOLS--------------------------------------------------
    public void ClearLecturesFrom(List<PF2E_Lecture> lectures, string from)
    {
        lectures.RemoveAll(item => item.from == from);
    }

}
