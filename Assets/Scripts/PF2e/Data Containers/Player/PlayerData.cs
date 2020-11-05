using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pathfinder2e;
using Pathfinder2e.Containers;
using UnityEngine;

namespace Pathfinder2e.Player
{

    public class PlayerData
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
        private APIC ac;

        public int ac_score
        {
            get
            {
                if (ac.playerData == null)
                    ac.playerData = this;
                return ac.score;
            }
        }

        public APIC AC_Get()
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
        private List<LectureFull> classDC_lectures = new List<LectureFull>();

        public string classDC { get { return DB.Prof_FindMax(classDC_lectures); } }

        public void ClassDC_ClearFrom(string from)
        {
            Lectures_ClearFrom(classDC_lectures, from);
        }

        public bool ClassDC_Train(Lecture lecture, string from)
        {
            classDC_lectures.Add(new LectureFull(lecture, from));
            return true;
        }


        //---------------------------------------------------SPEED--------------------------------------------------
        private int speed_ancestry = 0;

        public int speed_base { get { return speed_ancestry; } }
        public int speed_burrow = 0;
        public int speed_climp = 0;
        public int speed_fly = 0;
        public int speed_swim = 0;


        //---------------------------------------------------SIZE--------------------------------------------------
        private string size_ancestry = "M";
        private string size_temp = "";

        public string size
        {
            get
            {
                if (size_temp != "")
                    return size_temp;
                else
                    return size_ancestry;
            }
            set
            {
                size_temp = value;
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
        public float bulk_current = 0f; // This has to be turn into full Inventory solution that calcs objects weight


        // ---------------------------------------------------CHARACTER TRAITS--------------------------------------------------
        public List<TraitFull> traits_list = new List<TraitFull>();

        private void Traits_ClearFrom(string from)
        {
            traits_list.RemoveAll(item => item.from == from);
        }


        // ---------------------------------------------------LANGUAGES--------------------------------------------------
        public string languages = "";

        // ---------------------------------------------------ABILITIES--------------------------------------------------
        // AblBoostData - "from" variable can contain this strings:
        //     ancestry boost, ancestry flaw, ancestry free
        //     background choice, background free
        //     class
        //     lvl1, lvl5, lvl10, lvl15, lvl20

        public int abl_strength { get { return Abl_ScoreCalc("str"); } }
        public int abl_dexterity { get { return Abl_ScoreCalc("dex"); } }
        public int abl_constitution { get { return Abl_ScoreCalc("con"); } }
        public int abl_intelligence { get { return Abl_ScoreCalc("int"); } }
        public int abl_wisdom { get { return Abl_ScoreCalc("wis"); } }
        public int abl_charisma { get { return Abl_ScoreCalc("cha"); } }

        public int abl_strengthMod { get { return Abl_ModCalc(abl_strength); } }
        public int abl_dexterityMod { get { return Abl_ModCalc(abl_dexterity); } }
        public int abl_constitutionMod { get { return Abl_ModCalc(abl_constitution); } }
        public int abl_intelligenceMod { get { return Abl_ModCalc(abl_intelligence); } }
        public int abl_wisdomMod { get { return Abl_ModCalc(abl_wisdom); } }
        public int abl_charismaMod { get { return Abl_ModCalc(abl_charisma); } }

        public List<AblBoostData> abl_boostList = new List<AblBoostData>();

        public int Abl_GetScore(string abl)
        {
            return Abl_ScoreCalc(abl);
        }

        public int Abl_GetMod(string abl)
        {
            return Abl_ModCalc(Abl_ScoreCalc(abl));
        }

        public void Abl_MapAdd(AblBoostData boost)
        {
            abl_boostList.Add(boost);
        }

        public void Abl_MapDelete(string name)
        {
            abl_boostList.RemoveAll(ctx => ctx.from == name);
        }

        public AblBoostData Abl_MapGet(string name)
        {
            return abl_boostList.Find(ctx => ctx.from == name);
        }

        public List<AblBoostData> Abl_MapGetAll(string name)
        {
            return abl_boostList.FindAll(ctx => ctx.from == name);
        }

        private int Abl_ScoreCalc(string abl)
        {
            List<AblBoostData> boosts = abl_boostList.FindAll(ctx => ctx.abl == abl);
            int count = 0, score = 10;

            foreach (var item in boosts)
                count += item.value;

            if (count >= 0)
                for (int i = 0; i < count; i++)
                    score += score < 18 ? 2 : 1;
            else if (count < 0)
                score += count * 2;

            return score;
        }

        private int Abl_ModCalc(int ablScore)
        {
            return Mathf.FloorToInt((ablScore - 10) / 2);
        }


        // ---------------------------------------------------SKILLS--------------------------------------------------
        private Dictionary<string, APIC> skills_dic;
        private Dictionary<string, APIC> lores_dic;

        public APIC Skills_Get(string skillName)
        {
            if (skills_dic.ContainsKey(skillName))
            {
                APIC skill = skills_dic[skillName];

                return skill;
            }
            else
            {
                Debug.LogWarning("[PlayerData] Couldn't find skill: " + skillName + "!");
                return null;
            }
        }

        public Dictionary<string, APIC> Skills_GetAll()
        {
            return skills_dic;
        }

        public List<APIC> Skills_GetAllAsList()
        {
            List<APIC> list = new List<APIC>();
            foreach (var item in skills_dic)
                list.Add(item.Value);

            return list;
        }

        ///<summary> Retrieve all Untrained skills. </summary>
        public Dictionary<string, APIC> Skills_GetUntrained()
        {
            Debug.LogWarning("[PlayerData] Not implemented!");
            return skills_dic;
        }

        ///<summary> Retrieve all skills with proficiency under Legendary. </summary>
        public Dictionary<string, APIC> Skills_GetTraineable()
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
        public bool Skills_Train(Lecture lecture, string from)
        {
            if (skills_dic.ContainsKey(lecture.target))
            {
                skills_dic[lecture.target].lectures.Add(new LectureFull(lecture, from));
                return true;
            }
            else
            {
                if (!lecture.target.Contains("determined") && !lecture.target.Contains("number"))
                    Debug.LogWarning($"[PlayerData] Tried to train skill: {lecture.target} but couldn't find it!");
                return false;
            }
        }


        // ---------------------------------------------------LORES--------------------------------------------------

        ///<summary> Train lore via lecture, saving a copy in an APIC object. </summary>
        ///<returns> True if it could be trained. False if it was already trained. </returns>
        public bool Lores_Train(Lecture lecture, string from)
        {
            if (lores_dic.ContainsKey(lecture.target))
            {
                lores_dic[lecture.target].lectures.Add(new LectureFull(lecture, from));
                return true;
            }
            else
            {
                APIC lore = new APIC("Acrobatics", this, "int", 0);
                lore.lectures.Add(new LectureFull(lecture, from));
                lores_dic.Add(lecture.target, lore);
                return false;
            }
        }

        public void Lores_ClearFrom(string from)
        {
            foreach (var item in lores_dic)
                Lectures_ClearFrom(item.Value.lectures, from);
        }


        // ---------------------------------------------------PERCEPTION--------------------------------------------------
        private APIC perception;

        public string perception_prof { get { return DB.Prof_FindMax(perception.lectures); } }

        public int perception_score { get { return perception.score; } }

        public APIC Perception_Get()
        {
            return perception;
        }

        public void Perception_ClearFrom(string from)
        {
            Lectures_ClearFrom(perception.lectures, from);
        }

        public bool Perception_Train(Lecture lecture, string from)
        {
            if (lecture.target == "perception")
            {
                perception.lectures.Add(new LectureFull(lecture, from));
                return true;
            }
            else
            {
                Debug.LogWarning("[PlayerData] Tried to train perception: " + lecture.target + " but wtf!");
                return false;
            }
        }


        // ---------------------------------------------------SAVES--------------------------------------------------
        private Dictionary<string, APIC> saves_dic;

        public APIC Saves_Get(string savesName)
        {
            if (saves_dic.ContainsKey(savesName))
            {
                APIC save = saves_dic[savesName];
                return save;
            }
            else
            {
                Debug.LogWarning("[PlayerData] Couldn't find save: " + savesName + "!");
                return null;
            }
        }

        public Dictionary<string, APIC> Saves_GetAll()
        {
            return saves_dic;
        }

        public List<APIC> Saves_GetAllAsList()
        {
            List<APIC> list = new List<APIC>();
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
        public bool Saves_Train(Lecture lecture, string from)
        {
            if (saves_dic.ContainsKey(lecture.target))
            {
                saves_dic[lecture.target].lectures.Add(new LectureFull(lecture, from));
                return true;
            }
            else
            {
                Debug.LogWarning("[PlayerData] Tried to train save: " + lecture.target + " but couldn't find it!");
                return false;
            }
        }


        // ---------------------------------------------------WEAPONS/ARMOR PROFICIENCIES--------------------------------------------------
        private Dictionary<string, List<LectureFull>> attackDefense_lectures = new Dictionary<string, List<LectureFull>>
    {
        {"unarmed attacks", new List<LectureFull>() },
        {"simple weapons", new List<LectureFull>() },
        {"martial weapons", new List<LectureFull>() },
        {"advanced weapons", new List<LectureFull>() },

        {"unarmored defense", new List<LectureFull>() },
        {"light armor", new List<LectureFull>() },
        {"medium armor", new List<LectureFull>() },
        {"heavy armor", new List<LectureFull>() },
    };

        public string unarmed { get { return DB.Prof_FindMax(attackDefense_lectures["unarmed attacks"]); } }
        public string simpleWeapons { get { return DB.Prof_FindMax(attackDefense_lectures["simple weapons"]); } }
        public string martialWeapons { get { return DB.Prof_FindMax(attackDefense_lectures["martial weapons"]); } }
        public string advancedWeapons { get { return DB.Prof_FindMax(attackDefense_lectures["advanced weapons"]); } }

        public string unarmored { get { return DB.Prof_FindMax(attackDefense_lectures["unarmored defense"]); } }
        public string lightArmor { get { return DB.Prof_FindMax(attackDefense_lectures["light armor"]); } }
        public string mediumArmor { get { return DB.Prof_FindMax(attackDefense_lectures["medium armor"]); } }
        public string heavyArmor { get { return DB.Prof_FindMax(attackDefense_lectures["heavy armor"]); } }

        public void WeaponArmor_ClearFrom(string from)
        {
            foreach (var item in attackDefense_lectures)
                Lectures_ClearFrom(item.Value, from);
        }

        public bool AttackDefense_Train(Lecture lecture, string from)
        {
            if (attackDefense_lectures.ContainsKey(lecture.target))
            {
                attackDefense_lectures[lecture.target].Add(new LectureFull(lecture, from));
                return true;
            }
            else if (lecture.target == "all armor")
            {
                attackDefense_lectures["unarmored defense"].Add(new LectureFull(lecture, from));
                attackDefense_lectures["light armor"].Add(new LectureFull(lecture, from));
                attackDefense_lectures["medium armor"].Add(new LectureFull(lecture, from));
                attackDefense_lectures["heavy armor"].Add(new LectureFull(lecture, from));
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
        public string ancestry { get { return _ancestry; } set { Ancestry_Set(value); } }

        private void Ancestry_Set(string newAncestry)
        {
            if (newAncestry == _ancestry)
                return;

            if (newAncestry != "")
            {
                Ancestry ancestryData = DB.Ancestries.Find(ctx => ctx.name == newAncestry);

                if (ancestryData != null)
                {
                    // Not choices
                    Ancestry_Cleanse(false);
                    _ancestry = newAncestry;
                    hp_ancestry = ancestryData.hp;
                    speed_ancestry = ancestryData.speed;
                    size_ancestry = ancestryData.size;
                    foreach (var item in ancestryData.traits)
                    {
                        Trait trait = DB.Traits.Find(ctx => ctx.name == item);
                        if (trait != null)
                            traits_list.Add(new TraitFull(trait, "ancestry"));
                    }
                    foreach (var item in ancestryData.abl_boosts)
                        if (item != "free")
                            Abl_MapAdd(new AblBoostData("ancestry boost", item, 1));
                    if (ancestryData.abl_flaws != null)
                        foreach (var item in ancestryData.abl_flaws)
                            Abl_MapAdd(new AblBoostData("ancestry flaw", item, -1));

                    // Choices
                    List<AblBoostData> previousFree = Abl_MapGetAll("ancestry free");
                    Abl_MapDelete("ancestry free");
                    for (int i = 0; i < ancestryData.abl_boosts.FindAll(ctx => ctx == "free").Count; i++)
                        if (i < previousFree.Count) // Saves as many free choices as it can, given what new ancestry gives you
                            Abl_MapAdd(previousFree[i]);
                }
                else
                {
                    Debug.LogWarning($"[PlayerData] Couldn't find ancestry {newAncestry} ");
                }
            }
            else
            {
                Ancestry_Cleanse(true);
            }
        }

        private void Ancestry_Cleanse(bool cleanseChoices)
        {
            _ancestry = "";
            hp_ancestry = 0;
            speed_ancestry = 0;
            size_ancestry = "M";

            Abl_MapDelete("ancestry boost");
            Abl_MapDelete("ancestry flaw");
            Traits_ClearFrom("ancestry");
            if (cleanseChoices)
                Abl_MapDelete("ancestry free");
        }


        // ---------------------------------------------------BACKGROUND--------------------------------------------------
        private string _background = "";
        public string background { get { return _background; } set { Background_Set(value); } }

        private void Background_Set(string newBackground)
        {
            if (newBackground == _background)
                return;

            if (newBackground != "")
            {
                Background backgroundData = DB.Backgrounds.Find(ctx => ctx.name == newBackground);

                if (backgroundData != null)
                {
                    // Not Choices
                    Background_Cleanse(false);
                    _background = newBackground;
                    foreach (var item in backgroundData.lectures)
                        if (item.target.Contains("lore"))
                            Lores_Train(item, "background");
                        else
                            Skills_Train(item, "background");

                    // Choices
                    AblBoostData previousChoice = Abl_MapGet("background choice");
                    bool match = false;
                    if (previousChoice != null)
                        foreach (var item in backgroundData.abl_choices)
                            if (previousChoice.abl == item)
                                match = true;
                    if (!match)
                        Abl_MapDelete("background choice");
                }
                else
                {
                    Debug.LogWarning($"[PlayerData] Couldn't find background {newBackground} ");
                }
            }
            else
            {
                Background_Cleanse(true);
            }
        }

        private void Background_Cleanse(bool cleanseChoices)
        {
            _background = "";

            Skills_ClearFrom("background");
            Lores_ClearFrom("background");
            if (cleanseChoices)
            {
                Abl_MapDelete("background choice");
                Abl_MapDelete("background free");
            }
        }


        // ---------------------------------------------------CLASS--------------------------------------------------
        private string _class = "";
        public string class_name { get { return _class; } set { SetClass(value); } }

        private void SetClass(string newClass)
        {
            if (newClass == _class)
                return;

            if (newClass != "")
            {
                Class classData = DB.Classes.Find(ctx => ctx.name == newClass);

                if (classData != null)
                {
                    // Not Choices
                    Class_Cleanse(false);
                    _class = newClass;
                    hp_class = classData.hp;
                    foreach (var item in classData.skills)
                        Skills_Train(item, "class");
                    foreach (var item in classData.perception)
                        Perception_Train(item, "class");
                    foreach (var item in classData.saves)
                        Saves_Train(item, "class");
                    foreach (var item in classData.attacks)
                        AttackDefense_Train(item, "class");
                    foreach (var item in classData.defenses)
                        AttackDefense_Train(item, "class");
                    foreach (var item in classData.class_dc_and_spells)
                        ClassDC_Train(item, "class");

                    // Choices
                    // Build_SetNewProgression(classData.name);
                    if (classData.key_ability_choices.Count > 1)
                    {
                        AblBoostData previousChoice = Abl_MapGet("class");
                        bool match = false;
                        if (previousChoice != null)
                            foreach (var item in classData.key_ability_choices)
                                if (previousChoice.abl == item)
                                    match = true;
                        if (!match)
                            Abl_MapDelete("class");
                    }
                    else
                    {
                        Abl_MapDelete("class");
                        Abl_MapAdd(new AblBoostData("class", classData.key_ability_choices[0], 1));
                    }
                }
                else
                {
                    Debug.LogWarning($"[PlayerData] Couldn't find background {newClass} ");
                }
            }
            else
            {
                Class_Cleanse(true);
            }
        }

        private void Class_Cleanse(bool cleanseChoices)
        {
            _class = "";
            hp_class = 0;

            Skills_ClearFrom("class");
            Perception_ClearFrom("class");
            Saves_ClearFrom("class");
            WeaponArmor_ClearFrom("class");
            ClassDC_ClearFrom("class");

            if (cleanseChoices)
            {
                Abl_MapDelete("class");
            }
        }


        // ---------------------------------------------------BUILD--------------------------------------------------
        public Dictionary<int, Dictionary<string, BuildBlock>> build = new Dictionary<int, Dictionary<string, BuildBlock>>();

        public T Build_GetFromBlock<T>(int level, string key)
        {
            try
            {
                if (build.ContainsKey(level))
                    if (build[level].ContainsKey(key))
                        if (build[level][key].value != null)
                            return JsonConvert.DeserializeObject<T>(build[level][key].value);

                Debug.LogWarning($"[PlayerData] Couldn't find key {key} ");
                return default(T);
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerData] Couldn't retrieve object {key} from build\n{e.Message}\n{e.StackTrace}");
                return default(T);
            }
        }

        private void Build_SetNewProgression(string className)
        {
            Debug.Log($"[PlayerData] changing [{playerName}] class progression");

            Dictionary<int, Dictionary<string, BuildBlock>> newBuild = new Dictionary<int, Dictionary<string, BuildBlock>>();
            ClassProgression prog = DB.ClassProgression.Find(ctx => ctx.name == className);
            List<BuildBlock> coincidences = new List<BuildBlock>();

            // Search for coincidences in old build with new class progression
            foreach (var lvlDic in build)
                foreach (var block in lvlDic.Value)
                    if (block.Value.value == prog.progression[block.Value.level].items.Find(ctx => ctx == block.Value.name))
                        coincidences.Add(block.Value);

            // Generate new build
            foreach (var block in coincidences)
            {
                if (!newBuild.ContainsKey(block.level))
                    newBuild.Add(block.level, new Dictionary<string, BuildBlock>());

                newBuild[block.level].Add(block.name, block);
            }

            build = newBuild;
            Build_Refresh();
        }

        public void Build_Refresh()
        {
            Debug.LogWarning($"[PlayerData] Refreshing [{playerName}] build - NOT IMPLEMENTED");

            // Remove all data saved

            // Reapply all data from build

        }


        // ---------------------------------------------------LECTURES MANAGEMENT--------------------------------------------------
        private List<LectureFull> lectures_unused = new List<LectureFull>();

        // public bool Lectures_Allocate(Lecture lecture, string from)
        // {
        //     bool trainSuccessful = false;

        //     if (lecture.target == "unarmed" || lecture.target == "simple weapons" || lecture.target == "martial weapons" ||
        //     lecture.target == "advanced weapons" || lecture.target == "unarmored defense" || lecture.target == "light armor" ||
        //     lecture.target == "medium armor" || lecture.target == "heavy armor")
        //     {
        //         trainSuccessful = AttackDefense_Train(lecture, from);
        //     }
        //     else if (lecture.target == "fortitude" || lecture.target == "reflex" || lecture.target == "will")
        //     {
        //         trainSuccessful = Saves_Train(lecture, from);
        //     }
        //     else if (lecture.target == "perception")
        //     {
        //         trainSuccessful = Perception_Train(lecture, from);
        //     }
        //     else if (lecture.target == "acrobatics" || lecture.target == "arcana" || lecture.target == "athletics" || lecture.target == "crafting" ||
        //     lecture.target == "deception" || lecture.target == "diplomacy" || lecture.target == "intimidation" || lecture.target == "medicine" ||
        //     lecture.target == "nature" || lecture.target == "occultism" || lecture.target == "performance" || lecture.target == "religion" ||
        //     lecture.target == "society" || lecture.target == "stealth" || lecture.target == "survival" || lecture.target == "thievery" ||
        //     lecture.target == "lore 1" || lecture.target == "lore 2")
        //     {
        //         trainSuccessful = Skills_Train(lecture, from);
        //     }

        //     return trainSuccessful;
        // }

        public void Lectures_ClearFrom(List<LectureFull> lectures, string from)
        {
            lectures.RemoveAll(item => item.from == from || item.from == "");
        }


        // ---------------------------------------------------EFFECTS--------------------------------------------------


        // ---------------------------------------------------CONSTRUCTOR--------------------------------------------------
        public PlayerData()
        {
            ac = new APIC("Armor Class", this, "dex", 10);

            skills_dic = new Dictionary<string, APIC>()
        {
            {"acrobatics",new APIC("Acrobatics" , this , "dex", 0)},
            {"arcana",new APIC("Arcana" , this , "int", 0)},
            {"athletics",new APIC("Athletics" , this , "str", 0)},
            {"crafting",new APIC("Crafting" , this , "int", 0)},
            {"deception",new APIC("Deception" , this , "cha", 0)},
            {"diplomacy",new APIC("Diplomacy" , this , "cha", 0)},
            {"intimidation",new APIC("Intimidation" , this , "cha", 0)},
            {"medicine",new APIC("Medicine" , this , "wis", 0)},
            {"nature",new APIC("Nature" , this , "wis", 0)},
            {"occultism",new APIC("Occultism" , this , "int", 0)},
            {"performance",new APIC("Performance" , this , "dex", 0)},
            {"religion",new APIC("Religion" , this , "wis", 0)},
            {"society",new APIC("Society" , this , "cha", 0)},
            {"stealth",new APIC("Stealth" , this , "dex", 0)},
            {"survival",new APIC("Survival" , this , "wis", 0)},
            {"thievery",new APIC("Thievery" , this , "dex", 0)},
        };

            lores_dic = new Dictionary<string, APIC>() { };
            saves_dic = new Dictionary<string, APIC>
        {
            {"fortitude",new APIC("Fortitude" , this , "con", 0)},
            {"reflex",new APIC("Reflex" , this , "dex", 0)},
            {"will",new APIC("Will" , this , "wis", 0)},
        };

            perception = new APIC("perception", this, "wis", 0);
        }

    }

}
