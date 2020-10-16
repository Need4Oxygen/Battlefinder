using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pathfinder2e;
using Pathfinder2e.Containers;
using UnityEngine;

namespace Pathfinder.PlayerData
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
            if (lecture.target == "classDC")
            {
                classDC_lectures.Add(new LectureFull(lecture, from));
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
        public float bulk_current = 5f; // This has to be turn into full Inventory solution that calcs objects weight


        // ---------------------------------------------------CHARACTER TRAITS--------------------------------------------------
        public List<Trait> traits_list = new List<Trait>();

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

        public Dictionary<Vector2, AblBoostData> abl_map = new Dictionary<Vector2, AblBoostData>();

        public int Abl_GetScore(string abl)
        {
            switch (abl)
            {
                case "str": return abl_strength;
                case "dex": return abl_dexterity;
                case "con": return abl_constitution;
                case "int": return abl_intelligence;
                case "wis": return abl_wisdom;
                case "cha": return abl_charisma;
                default: return 0;
            }
        }

        public int Abl_GetMod(string abl)
        {
            switch (abl)
            {
                case "str": return abl_strengthMod;
                case "dex": return abl_dexterityMod;
                case "con": return abl_constitutionMod;
                case "int": return abl_intelligenceMod;
                case "wis": return abl_wisdomMod;
                case "cha": return abl_charismaMod;
                default: return 0;
            }
        }

        public bool Abl_SetMap(Vector2 coords, AblBoostData boost)
        {

            // set a boost data in the dicctionary

            return true;
        }

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
        {"unarmed", new List<LectureFull>() },
        {"simpleWeapons", new List<LectureFull>() },
        {"martialWeapons", new List<LectureFull>() },
        {"advancedWeapons", new List<LectureFull>() },

        {"unarmored", new List<LectureFull>() },
        {"lightArmor", new List<LectureFull>() },
        {"mediumArmor", new List<LectureFull>() },
        {"heavyArmor", new List<LectureFull>() },
    };

        public string unarmed { get { return DB.Prof_FindMax(attackDefense_lectures["unarmed"]); } }
        public string simpleWeapons { get { return DB.Prof_FindMax(attackDefense_lectures["simpleWeapons"]); } }
        public string martialWeapons { get { return DB.Prof_FindMax(attackDefense_lectures["martialWeapons"]); } }
        public string advancedWeapons { get { return DB.Prof_FindMax(attackDefense_lectures["advancedWeapons"]); } }

        public string unarmored { get { return DB.Prof_FindMax(attackDefense_lectures["unarmored"]); } }
        public string lightArmor { get { return DB.Prof_FindMax(attackDefense_lectures["lightArmor"]); } }
        public string mediumArmor { get { return DB.Prof_FindMax(attackDefense_lectures["mediumArmor"]); } }
        public string heavyArmor { get { return DB.Prof_FindMax(attackDefense_lectures["heavyArmor"]); } }

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
            Ancestry ancestry = DB.Ancestries.Find(ctx => ctx.name == newAncestry);
            if (ancestry != null)
            {
                _ancestry = newAncestry;
                hp_ancestry = ancestry.hp;
                speed_ancestry = ancestry.speed;
                size_ancestry = ancestry.size;

                Traits_ClearFrom("ancestry");
                foreach (var item in ancestry.traits)
                    traits_list.Add(new Trait(item, "ancestry"));
            }
            else
            {
                Debug.LogWarning($"[PlayerData] ({playerName}) Can't find ancestry: {newAncestry}!");
            }
        }


        // ---------------------------------------------------BACKGROUND--------------------------------------------------
        private string _background = "";
        public string background { get { return _background; } set { SetBackground(value); } }

        private void SetBackground(string newBackground)
        {
            Background background = DB.Backgrounds.Find(ctx => ctx.name == newBackground);
            if (background != null)
            {
                _background = newBackground;

                Skills_ClearFrom("background");
                Lores_ClearFrom("background");
                foreach (var item in background.lectures)
                    if (item.target.Contains("lore"))
                        Lores_Train(item, "background");
                    else
                        Skills_Train(item, "background");
            }
        }


        // ---------------------------------------------------CLASS--------------------------------------------------
        private string _class = "";
        public string class_name { get { return _class; } set { SetClass(value); } }

        private void SetClass(string newClass)
        {
            Class classObj = DB.Classes.Find(ctx => ctx.name == newClass);
            if (classObj != null)
            {
                _class = newClass;
                hp_class = classObj.hp;

                Skills_ClearFrom("class");
                Perception_ClearFrom("class");
                Saves_ClearFrom("class");
                WeaponArmor_ClearFrom("class");
                ClassDC_ClearFrom("class");

                foreach (var item in classObj.skills)
                    Skills_Train(item, "class");
                foreach (var item in classObj.perception)
                    Perception_Train(item, "class");
                foreach (var item in classObj.saves)
                    Saves_Train(item, "class");
                foreach (var item in classObj.attacks)
                    AttackDefense_Train(item, "class");
                foreach (var item in classObj.defenses)
                    AttackDefense_Train(item, "class");
                foreach (var item in classObj.class_dc_and_spells)
                    ClassDC_Train(item, "class");

                // Try to rescue data from the old build
                Dictionary<string, Dictionary<string, BuildBlock>> newBuild = classObj.build;
                Build_SetNewBuild(newBuild);
            }
        }


        // ---------------------------------------------------BUILD--------------------------------------------------
        // build[level][feat]
        public List<BuildBlock> build = new List<BuildBlock>();

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
            Debug.Log($"[PlayerData] Refreshing [{playerName}] build");

            initAblBoosts = Build_Get<AblBoostData>("Level 1", "Initial Ability Boosts");
        }

        private void Build_SetNewProgression(string className)
        {
            Debug.Log($"[PlayerData] changing [{playerName}] class progression");

            ClassProgression prog = DB.ClassProgression.Find(ctx => ctx.name == className);
            List<BuildBlock> newBuild = new List<BuildBlock>();

            foreach (var block in build) // If old blocks targets coincides with new class progression targets, they are saved
                if (block.key == prog.progression[block.level].items.Find(ctx => ctx == block.key))
                    newBuild.Add(block);

            build = newBuild;

            Build_Refresh();
        }


        // ---------------------------------------------------LECTURES MANAGEMENT--------------------------------------------------
        private List<LectureFull> lectures_unused = new List<LectureFull>();

        public bool Lectures_Allocate(Lecture lecture, string from)
        {
            bool trainSuccessful = false;

            if (lecture.target == "unarmed" || lecture.target == "simpleWeapons" || lecture.target == "martialWeapons" ||
            lecture.target == "advancedWeapons" || lecture.target == "unarmored" || lecture.target == "lightArmor" ||
            lecture.target == "mediumArmor" || lecture.target == "heavyArmor")
            {
                trainSuccessful = AttackDefense_Train(lecture, from);
            }
            else if (lecture.target == "fortitude" || lecture.target == "reflex" || lecture.target == "will")
            {
                trainSuccessful = Saves_Train(lecture, from);
            }
            else if (lecture.target == "perception")
            {
                trainSuccessful = Perception_Train(lecture, from);
            }
            else if (lecture.target == "acrobatics" || lecture.target == "arcana" || lecture.target == "athletics" || lecture.target == "crafting" ||
            lecture.target == "deception" || lecture.target == "diplomacy" || lecture.target == "intimidation" || lecture.target == "medicine" ||
            lecture.target == "nature" || lecture.target == "occultism" || lecture.target == "performance" || lecture.target == "religion" ||
            lecture.target == "society" || lecture.target == "stealth" || lecture.target == "survival" || lecture.target == "thievery" ||
            lecture.target == "lore 1" || lecture.target == "lore 2")
            {
                trainSuccessful = Skills_Train(lecture, from);
            }

            return trainSuccessful;
        }

        public void Lectures_ClearFrom(List<LectureFull> lectures, string from)
        {
            lectures.RemoveAll(item => item.from == from || item.from == "");
        }


        // ---------------------------------------------------EFFECTS--------------------------------------------------
        public Dictionary<string, List<PF2E_Effect>> effects_library = new Dictionary<string, List<PF2E_Effect>>();


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
