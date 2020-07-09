using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class PF2E_PlayerData
{
    public string guid = "";
    public string playerName = "";


    // ---------------------------------------------------LEVEL--------------------------------------------------
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


    // ---------------------------------------------------HIT POINTS--------------------------------------------------
    private int hp_class = 0;
    private int hp_ancestry = 0;

    public int hp_current { get { return hp_max - hp_damage; } }
    public int hp_max { get { return level * (hp_class + abl_constitutionMod) + hp_ancestry + hp_temp; } }

    private int _damage = 0;
    public int hp_damage
    {
        get { return _damage; }
        set
        {
            if (value >= 0) _damage = value;
        }
    }

    private int _temp = 0;
    public int hp_temp
    {
        get { return _temp; }
        set { _temp = value; }
    }

    private int _dyingCurrent = 0;
    public int hp_dyingCurrent
    {
        get { return _dyingCurrent; }
        set { if (value >= 0) _dyingCurrent = value; }
    }
    public int hp_dyingMax { get { return 4 - _doom; } }

    private int _wounds = 0;
    public int hp_wounds
    {
        get { return _wounds; }
        set { if (value >= 0) _wounds = value; }
    }

    private int _doom = 0;
    public int hp_doom
    {
        get { return _doom; }
        set { if (value >= 0) _doom = value; }
    }


    // ---------------------------------------------------AC--------------------------------------------------
    private PF2E_APIC ac;

    public int ac_score
    {
        get
        {
            if (ac.playerData == null)
                ac.playerData = this;
            return ac.score;
        }
    }

    public PF2E_APIC AC_Get()
    {
        return ac;
    }


    // ---------------------------------------------------HERO POINTS--------------------------------------------------
    private float _wealth = 15;
    public float wealth
    {
        get { return _wealth; }
        set { _wealth = value; }
    }

    public string Wealth_Formated()
    {
        return _wealth.ToString("F2");
    }


    // ---------------------------------------------------HERO POINTS--------------------------------------------------
    private int _heroPoints = 0;
    public int heroPoints
    {
        get { return _heroPoints; }
        set
        {
            if (value > 9)
                _heroPoints = 9;
            else if (value < 0)
                _heroPoints = 0;
            else
                _heroPoints = value;
        }
    }


    //---------------------------------------------------CLASS DC--------------------------------------------------
    private List<PF2E_Lecture> classDC_lectures = new List<PF2E_Lecture>();

    public E_PF2E_Proficiency classDC { get { return PF2E_DataBase.Prof_FindMax(classDC_lectures); } }

    public void ClassDC_ClearFrom(string from)
    {
        Lectures_ClearFrom(classDC_lectures, from);
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
    private string _size_ancestry = "M";
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

    public float Size_BulkMod()
    {
        string sizeStr = size;
        if (sizeStr != "")
            switch (sizeStr)
            {
                case "T":
                    return 0.5f;
                case "S":
                    return 1f;
                case "M":
                    return 1f;
                case "L":
                    return 2f;
                case "H":
                    return 4f;
                case "G":
                    return 8f;

                default:
                    Debug.LogWarning("[PF2E_DataBase] Error: size (" + sizeStr + ") not recognized!");
                    return 1f;
            }
        else
            return 1f;
    }


    // ---------------------------------------------------BULK--------------------------------------------------
    public float bulk_bonus = 0f; // This has to be turn into effect list
    public float bulk_encThreshold { get { return Size_BulkMod() * (5 + abl_strengthMod) + bulk_bonus; } }
    public float bulk_maxThreshold { get { return Size_BulkMod() * (10 + abl_strengthMod) + bulk_bonus; } }
    public float bulk_current = 5f; // This has to be turn into full Inventory solution that calcs objects weight


    // ---------------------------------------------------CHARACTER TRAITS--------------------------------------------------
    public List<PF2E_Trait> traits_list = new List<PF2E_Trait>();

    private void Traits_ClearFrom(string from)
    {
        traits_list.RemoveAll(item => item.from == from);
    }


    // ---------------------------------------------------LANGUAGES--------------------------------------------------
    public string languages = "";

    // ---------------------------------------------------ABILITIES--------------------------------------------------

    //       |Ancestry|Background|Class|Lvl1Boost|Lvl5Boost|Lvl10Boost|Lvl15Boost|Lvl20Boost|
    //    STR|        |          |     |         |         |          |          |          |
    //    DEX|        |          |     |         |         |          |          |          |
    //    CON|        |          |     |         |         |          |          |          |  x = boosts
    //    INT|        |          |     |         |         |          |          |          |  y = abilities
    //    WIS|        |          |     |         |         |          |          |          |
    //    CHA|        |          |     |         |         |          |          |          |

    public int abl_strength { get { return Abl_ScoreCalc("str"); } }
    public int abl_dexterity { get { return Abl_ScoreCalc("dex"); } }
    public int abl_constitution { get { return Abl_ScoreCalc("con"); } }
    public int abl_intelligence { get { return Abl_ScoreCalc("int"); } }
    public int abl_wisdom { get { return Abl_ScoreCalc("wis"); } }
    public int abl_charisma { get { return Abl_ScoreCalc("con"); } }

    public int abl_strengthMod { get { return Abl_ModCalc(abl_strength); } }
    public int abl_dexterityMod { get { return Abl_ModCalc(abl_dexterity); } }
    public int abl_constitutionMod { get { return Abl_ModCalc(abl_constitution); } }
    public int abl_intelligenceMod { get { return Abl_ModCalc(abl_intelligence); } }
    public int abl_wisdomMod { get { return Abl_ModCalc(abl_wisdom); } }
    public int abl_charismaMod { get { return Abl_ModCalc(abl_charisma); } }

    private PF2E_InitAblBoostData initAblBoosts = new PF2E_InitAblBoostData();
    private PF2E_AblBoostData lvl5AblBoosts = new PF2E_AblBoostData();
    private PF2E_AblBoostData lvl10AblBoosts = new PF2E_AblBoostData();
    private PF2E_AblBoostData lvl15AblBoosts = new PF2E_AblBoostData();
    private PF2E_AblBoostData lvl20AblBoosts = new PF2E_AblBoostData();

    private int Abl_ScoreCalc(string abl)
    {
        int count = 0;
        int score = 10;

        if (initAblBoosts != null)
        {
            count += initAblBoosts.ancestryBoosts.FindAll(boost => boost == abl).Count;
            count -= initAblBoosts.ancestryFlaws.FindAll(flaw => flaw == abl).Count;
            count += initAblBoosts.ancestryFree.FindAll(boost => boost == abl).Count;
            count += initAblBoosts.backgroundBoosts.FindAll(boost => boost == abl).Count;
            count += initAblBoosts.classBoosts.FindAll(boost => boost == abl).Count;
            count += initAblBoosts.lvl1boosts.FindAll(boost => boost == abl).Count;
        }

        if (lvl5AblBoosts != null)
            count += lvl5AblBoosts.boosts.FindAll(boost => boost == abl).Count;
        if (lvl10AblBoosts != null)
            count += lvl5AblBoosts.boosts.FindAll(boost => boost == abl).Count;
        if (lvl15AblBoosts != null)
            count += lvl5AblBoosts.boosts.FindAll(boost => boost == abl).Count;
        if (lvl20AblBoosts != null)
            count += lvl5AblBoosts.boosts.FindAll(boost => boost == abl).Count;

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

    /// <summary>Return ability int[8,6] map where first dimensions is the boost and the second is the affected ability. </summary>
    public int[,] Abl_GetMap()
    {
        int[,] map = new int[8, 6];

        if (initAblBoosts != null)
        {
            MapConstructor(0, 1, initAblBoosts.ancestryBoosts, ref map);
            MapConstructor(0, 1, initAblBoosts.ancestryFree, ref map);
            MapConstructor(0, -1, initAblBoosts.ancestryFlaws, ref map);
            MapConstructor(1, 1, initAblBoosts.backgroundBoosts, ref map);
            MapConstructor(2, 1, initAblBoosts.classBoosts, ref map);
            MapConstructor(3, 1, initAblBoosts.lvl1boosts, ref map);
        }

        return map;
    }

    private void MapConstructor(int boostIndex, int value, List<string> strings, ref int[,] map)
    {
        foreach (var item in strings)
        {
            if (item == "str")
                map[boostIndex, 0] = value;
            else if (item == "dex")
                map[boostIndex, 1] = value;
            else if (item == "con")
                map[boostIndex, 2] = value;
            else if (item == "int")
                map[boostIndex, 3] = value;
            else if (item == "wis")
                map[boostIndex, 4] = value;
            else if (item == "cha")
                map[boostIndex, 5] = value;
        }
    }


    // ---------------------------------------------------SKILLS--------------------------------------------------
    private Dictionary<string, PF2E_APIC> skills_dic;

    public PF2E_APIC Skills_Get(string skillName)
    {
        if (skills_dic.ContainsKey(skillName))
        {
            PF2E_APIC skill = skills_dic[skillName];

            return skill;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Couldn't find skill: " + skillName + "!");
            return null;
        }
    }

    public Dictionary<string, PF2E_APIC> Skills_GetAll()
    {
        return skills_dic;
    }

    public List<PF2E_APIC> Skills_GetAllAsList()
    {
        List<PF2E_APIC> list = new List<PF2E_APIC>();
        foreach (var item in skills_dic)
            list.Add(item.Value);

        return list;
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

    public void Skills_ClearFrom(string from)
    {
        foreach (var item in skills_dic)
            Lectures_ClearFrom(item.Value.lectures, from);
    }

    ///<summary> Train skill via lecture, saving a copy in an APIC object. </summary>
    ///<returns> True if it could be trained. False if it was already trained. </returns>
    public bool Skills_Train(PF2E_Lecture lecture)
    {
        if (skills_dic.ContainsKey(lecture.target))
        {
            if (skills_dic[lecture.target].profEnum < PF2E_DataBase.Prof_Abbr2Enum(lecture.proficiency))
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


    // ---------------------------------------------------PERCEPTION--------------------------------------------------
    private PF2E_APIC perception;

    public E_PF2E_Proficiency perception_prof { get { return PF2E_DataBase.Prof_FindMax(perception.lectures); } }

    public int perception_score { get { return perception.score; } }

    public PF2E_APIC Perception_Get()
    {
        return perception;
    }

    public void Perception_ClearFrom(string from)
    {
        Lectures_ClearFrom(perception.lectures, from);
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


    // ---------------------------------------------------SAVES--------------------------------------------------
    private Dictionary<string, PF2E_APIC> saves_dic;

    public PF2E_APIC Saves_Get(string savesName)
    {
        if (saves_dic.ContainsKey(savesName))
        {
            PF2E_APIC save = saves_dic[savesName];
            return save;
        }
        else
        {
            Debug.LogWarning("[PlayerData] Couldn't find save: " + savesName + "!");
            return null;
        }
    }

    public Dictionary<string, PF2E_APIC> Saves_GetAll()
    {
        return saves_dic;
    }

    public List<PF2E_APIC> Saves_GetAllAsList()
    {
        List<PF2E_APIC> list = new List<PF2E_APIC>();
        foreach (var item in saves_dic)
            list.Add(item.Value);

        return list;
    }

    public void Saves_ClearFrom(string from)
    {
        foreach (var item in saves_dic)
            Lectures_ClearFrom(item.Value.lectures, from);
    }

    ///<summary> Train save via lecture, saving a copy in an APIC object. </summary>
    ///<returns> True if it could be trained. False if it was already trained. </returns>
    public bool Saves_Train(PF2E_Lecture lecture)
    {
        if (saves_dic.ContainsKey(lecture.target))
        {
            if (saves_dic[lecture.target].profEnum < PF2E_DataBase.Prof_Abbr2Enum(lecture.proficiency))
            {
                saves_dic[lecture.target].lectures.Add(lecture);
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
            Debug.LogWarning("[PlayerData] Tried to train save: " + lecture.target + " but couldn't find it!");
            return false;
        }
    }


    // ---------------------------------------------------WEAPONS/ARMOR PROFICIENCIES--------------------------------------------------
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

    public E_PF2E_Proficiency unarmed { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["unarmed"]); } }
    public E_PF2E_Proficiency simpleWeapons { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["simpleWeapons"]); } }
    public E_PF2E_Proficiency martialWeapons { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["martialWeapons"]); } }
    public E_PF2E_Proficiency advancedWeapons { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["advancedWeapons"]); } }

    public E_PF2E_Proficiency unarmored { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["unarmored"]); } }
    public E_PF2E_Proficiency lightArmor { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["lightArmor"]); } }
    public E_PF2E_Proficiency mediumArmor { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["mediumArmor"]); } }
    public E_PF2E_Proficiency heavyArmor { get { return PF2E_DataBase.Prof_FindMax(weaponArmor_lectures["heavyArmor"]); } }

    public void WeaponArmor_ClearFrom(string from)
    {
        foreach (var item in weaponArmor_lectures)
            Lectures_ClearFrom(item.Value, from);
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


    // ---------------------------------------------------ANCESTRY--------------------------------------------------
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

            Traits_ClearFrom("ancestry");
            foreach (var item in ancestry.traits)
                traits_list.Add(item.Value);
        }
        else
        {
            Debug.LogWarning("[PlayerData] (" + playerName + ") Can't find ancestry: " + newAncestry + "!");
        }
    }


    // ---------------------------------------------------BACKGROUND--------------------------------------------------
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


    // ---------------------------------------------------CLASS--------------------------------------------------
    private string _class = "";
    public string class_name { get { return _class; } set { SetClass(value); } }

    public int class_freeSkillTrains = 0;

    private void SetClass(string newClass)
    {
        if (PF2E_DataBase.Classes.ContainsKey(newClass))
        {
            PF2E_Class classObj = PF2E_DataBase.Classes[newClass];
            _class = newClass;

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
                Skills_Train(item.Value);
            foreach (var item in classObj.lectures)
                Lectures_Allocate(item.Value);

            // Try to rescue data from the old build
            Dictionary<string, Dictionary<string, PF2E_BuildItem>> newBuild = classObj.build;
            Build_SetNewBuild(newBuild);
        }
    }


    // ---------------------------------------------------BUILD--------------------------------------------------
    // build[level][feat]
    public Dictionary<string, Dictionary<string, PF2E_BuildItem>> build = new Dictionary<string, Dictionary<string, PF2E_BuildItem>>();

    public T Build_Get<T>(string level, string itemKey)
    {
        try
        {
            if (build.ContainsKey(level))
                if (build[level].ContainsKey(itemKey))
                    if (build[level][itemKey].obj != null)
                        return JsonConvert.DeserializeObject<T>(build[level][itemKey].obj);
                    else
                        return default(T);
                else
                    return default(T);
            else
                return default(T); // Can't do all of this in one line because null exception

        }
        catch (Exception e)
        {
            Debug.LogError("[PlayerData] ERROR: Couldn't retrieve object " + itemKey + " from build\n" + e.Message + "\n" + e.StackTrace);
            return default(T);
        }
    }

    public void Build_Set(string level, string itemkey, object obj)
    {
        string jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
        Build_Set(level, itemkey, jsonString);

        Build_Refresh();
    }
    public void Build_Set(string level, string itemkey, string obj)
    {
        if (build.ContainsKey(level))
            if (build[level].ContainsKey(itemkey))
                build[level][itemkey].obj = obj;
    }

    public void Build_Refresh()
    {
        Debug.Log("[PlayerData] [" + playerName + "] Build Refresh...");

        initAblBoosts = Build_Get<PF2E_InitAblBoostData>("Level 1", "Initial Ability Boosts");
    }

    private void Build_SetNewBuild(Dictionary<string, Dictionary<string, PF2E_BuildItem>> classBuild)
    {
        Debug.Log("[PlayerData] [" + playerName + "] Setting Build...");

        var newBuild = new Dictionary<string, Dictionary<string, PF2E_BuildItem>>(classBuild);

        foreach (var level in classBuild) // Try to rescue stuff from the last build into the new one
        {
            string levelString = level.Key;
            if (build.ContainsKey(levelString))
                foreach (var item in level.Value)
                {
                    string itemName = item.Key;
                    if (build[levelString].ContainsKey(itemName))
                    {
                        newBuild[levelString][itemName].value = build[levelString][itemName].value;
                        newBuild[levelString][itemName].obj = build[levelString][itemName].obj;
                    }
                }
        }

        build = newBuild;
        Build_Refresh();
    }


    // ---------------------------------------------------LECTURES MANAGEMENT--------------------------------------------------
    private List<PF2E_Lecture> lectures_unused = new List<PF2E_Lecture>();

    public bool Lectures_Allocate(PF2E_Lecture lecture)
    {
        bool trainSuccessful = false;

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

    public void Lectures_ClearFrom(List<PF2E_Lecture> lectures, string from)
    {
        lectures.RemoveAll(item => item.from == from || item.from == "");
    }


    // ---------------------------------------------------EFFECTS--------------------------------------------------
    public Dictionary<string, List<PF2E_Effect>> effects_library = new Dictionary<string, List<PF2E_Effect>>();


    // ---------------------------------------------------CONSTRUCTOR--------------------------------------------------
    public PF2E_PlayerData()
    {
        ac = new PF2E_APIC("Armor Class", this, E_PF2E_Ability.Dexterity, 10);

        skills_dic = new Dictionary<string, PF2E_APIC>()
        {
            {"acrobatics",new PF2E_APIC("Acrobatics" , this , E_PF2E_Ability.Dexterity, 0)},
            {"arcana",new PF2E_APIC("Arcana" , this , E_PF2E_Ability.Intelligence, 0)},
            {"athletics",new PF2E_APIC("Athletics" , this , E_PF2E_Ability.Strength, 0)},
            {"crafting",new PF2E_APIC("Crafting" , this , E_PF2E_Ability.Intelligence, 0)},
            {"deception",new PF2E_APIC("Deception" , this , E_PF2E_Ability.Charisma, 0)},
            {"diplomacy",new PF2E_APIC("Diplomacy" , this , E_PF2E_Ability.Charisma, 0)},
            {"intimidation",new PF2E_APIC("Intimidation" , this , E_PF2E_Ability.Charisma, 0)},
            {"medicine",new PF2E_APIC("Medicine" , this , E_PF2E_Ability.Wisdom, 0)},
            {"nature",new PF2E_APIC("Nature" , this , E_PF2E_Ability.Wisdom, 0)},
            {"occultism",new PF2E_APIC("Occultism" , this , E_PF2E_Ability.Intelligence, 0)},
            {"performance",new PF2E_APIC("Performance" , this , E_PF2E_Ability.Dexterity, 0)},
            {"religion",new PF2E_APIC("Religion" , this , E_PF2E_Ability.Wisdom, 0)},
            {"society",new PF2E_APIC("Society" , this , E_PF2E_Ability.Charisma, 0)},
            {"stealth",new PF2E_APIC("Stealth" , this , E_PF2E_Ability.Dexterity, 0)},
            {"survival",new PF2E_APIC("Survival" , this , E_PF2E_Ability.Wisdom, 0)},
            {"thievery",new PF2E_APIC("Thievery" , this , E_PF2E_Ability.Default, 0)},
            {"lore 1",new PF2E_APIC("" , this , E_PF2E_Ability.Intelligence, 0)},
            {"lore 2",new PF2E_APIC("" , this , E_PF2E_Ability.Intelligence, 0)}
        };

        saves_dic = new Dictionary<string, PF2E_APIC>
        {
            {"fortitude",new PF2E_APIC("Fortitude" , this , E_PF2E_Ability.Constitution, 0)},
            {"reflex",new PF2E_APIC("Reflex" , this , E_PF2E_Ability.Dexterity, 0)},
            {"will",new PF2E_APIC("Will" , this , E_PF2E_Ability.Wisdom, 0)},
        };

        perception = new PF2E_APIC("perception", this, E_PF2E_Ability.Wisdom, 0);
    }

}
