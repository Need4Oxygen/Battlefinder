using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pathfinder2e;
using Pathfinder2e.Containers;
using Pathfinder2e.GameData;
using UnityEngine;

namespace Pathfinder2e.Character
{

    public class CharacterData
    {
        public string guid = "";
        public string name = "";


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LEVEL
        private int _experience = 0;
        public int experience
        {
            get { return _experience; }
            set { }
        }

        private int _level = 0;
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

                Abl_SetLvlFilter(_level);
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- HIT POINTS
        private int hp_class = 0;
        private int hp_ancestry = 0;

        public int hp_current { get { return hp_max - hp_damage; } }
        public int hp_max { get { return level * (hp_class + Abl_GetMod("con")) + hp_ancestry + hp_temp; } }

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


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- AC
        public APIC ac;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- WEALTH
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


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- HERO POINTS
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


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CLASS DC
        public string class_dc = "T";

        private void ClassDC_Refresh()
        {
            class_dc = DB.Prof_FindMax(RE_Get("class_dc"));
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SPEED
        private int speed_ancestry = 0;

        public int speed_base { get { return speed_ancestry; } }
        public int speed_burrow = 0;
        public int speed_climp = 0;
        public int speed_fly = 0;
        public int speed_swim = 0;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SIZE
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


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BULK
        public float bulk_bonus = 0f; // This has to be turn into effect list
        public float bulk_encThreshold { get { return Size_BulkMod() * (5 + Abl_GetMod("str")) + bulk_bonus; } }
        public float bulk_maxThreshold { get { return Size_BulkMod() * (10 + Abl_GetMod("str")) + bulk_bonus; } }
        public float bulk_current = 0f; // This has to be turn into full Inventory solution that calcs objects weight


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CHARACTER TRAITS
        public List<TraitFull> traits_list = new List<TraitFull>();

        private void Traits_ClearFrom(string from)
        {
            traits_list.RemoveAll(item => item.from == from);
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LANGUAGES
        public string languages = "";

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ABILITIES
        private static List<string> abl_sources = new List<string> { "str", "dex", "con", "int", "wis", "cha" };

        private Dictionary<string, Vector2Int> abl_values;
        private List<AblBoostData> abl_boostList = new List<AblBoostData>();
        private List<string> abl_lvlFilter = new List<string>();

        public int Abl_GetScore(string abl)
        {
            return abl_values[abl].x;
        }

        public int Abl_GetMod(string abl)
        {
            return abl_values[abl].y;
        }

        public void Abl_MapAdd(AblBoostData boost)
        {
            abl_boostList.Add(boost);
            Abl_UpdateValues();
        }

        public void Abl_MapSet(List<AblBoostData> list)
        {
            abl_boostList = new List<AblBoostData>(list);
            Abl_UpdateValues();
        }

        public void Abl_MapClearFrom(string source)
        {
            abl_boostList.RemoveAll(ctx => ctx.source == source);
            Abl_UpdateValues();
        }

        public List<AblBoostData> Abl_MapGet()
        {
            return new List<AblBoostData>(abl_boostList);
        }

        public AblBoostData Abl_MapGetFrom(string name)
        {
            return abl_boostList.Find(ctx => ctx.source == name);
        }

        public List<AblBoostData> Abl_MapGetAllFrom(string name)
        {
            return new List<AblBoostData>(abl_boostList.FindAll(ctx => ctx.source == name));
        }

        private void Abl_SetLvlFilter(int lvl)
        {
            abl_lvlFilter.Clear();

            if (lvl < 5)
            { abl_lvlFilter.Add("lvl5"); abl_lvlFilter.Add("lvl10"); abl_lvlFilter.Add("lvl15"); abl_lvlFilter.Add("lvl20"); }
            else if (lvl < 10)
            { abl_lvlFilter.Add("lvl10"); abl_lvlFilter.Add("lvl15"); abl_lvlFilter.Add("lvl20"); }
            else if (lvl < 15)
            { abl_lvlFilter.Add("lvl15"); abl_lvlFilter.Add("lvl20"); }
            else if (lvl < 20)
            { abl_lvlFilter.Add("lvl20"); }

            Abl_UpdateValues();
        }

        private void Abl_UpdateValues()
        {
            abl_boostList.RemoveAll(ctx => ctx.source == null || string.IsNullOrEmpty(ctx.source));

            foreach (var abl in abl_sources)
            {
                List<AblBoostData> boosts = abl_boostList.FindAll(ctx => ctx.abl == abl);
                int count = 0, score = 10, mod = 0;

                foreach (var item in boosts)
                    if (!abl_lvlFilter.Contains(item.source))
                        count += item.value;

                if (count >= 0)
                    for (int i = 0; i < count; i++)
                        score += score < 18 ? 2 : 1;
                else if (count < 0)
                    score += count * 2;
                mod = Mathf.FloorToInt((score - 10) / 2);

                abl_values[abl] = new Vector2Int(score, mod);
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- PERCEPTION
        public APIC perception;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SAVES
        public APIC fortitude, reflex, will;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SKILLS
        public APIC acrobatics, arcana, athletics, crafting, deception, diplomacy, intimidation, medicine, nature, occultism, performance, religion, society, stealth, survival, thievery;

        // This dictionaries track the skills proficiency allocation
        public Dictionary<RuleElement, List<RuleElement>> skill_unspent;
        public Dictionary<RuleElement, List<RuleElement>> skill_choice;
        public Dictionary<RuleElement, List<RuleElement>> skill_free;
        public Dictionary<RuleElement, List<RuleElement>> skill_improve;

        public APIC Skill_Get(string skill)
        {
            switch (skill)
            {
                case "acrobatics": return acrobatics;
                case "arcana": return arcana;
                case "athletics": return athletics;
                case "crafting": return crafting;
                case "deception": return deception;
                case "diplomacy": return diplomacy;
                case "intimidation": return intimidation;
                case "medicine": return medicine;
                case "nature": return nature;
                case "occultism": return occultism;
                case "performance": return performance;
                case "religion": return religion;
                case "society": return society;
                case "stealth": return stealth;
                case "survival": return survival;
                case "thievery": return thievery;
                default: return null;
            }
        }

        public List<APIC> Skill_GetAll()
        {
            return new List<APIC> {
                acrobatics, athletics, arcana, crafting, deception,
                diplomacy, intimidation, medicine, nature, occultism,
                performance, religion, society, stealth, survival, thievery };
        }

        public void Skill_Allocate(string from, string lvl, RuleElement element)
        {
            switch (element.key)
            {
                case "skill_static":
                    skill_unspent.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    break;
                case "skill_choice":
                    int alreadyTrainedCount = 0;
                    foreach (var item in element.value_list)
                        if (AlreadyTrained(item.value, element.proficiency)) alreadyTrainedCount++;
                    if (alreadyTrainedCount >= element.value_list.Count) // If all choices are already trained
                        skill_unspent.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    else
                        skill_choice.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    break;
                case "skill_free":
                    skill_free.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    break;
                case "skill_improve":
                    skill_improve.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    break;
                default:
                    break;
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LORES
        public Dictionary<string, APIC> lore_dic;

        public void Lores_Refresh()
        {
            lore_dic.Clear();

            IEnumerable<RuleElement> loreElements = RE_Get("lore");

            foreach (var item in loreElements)
                if (lore_dic.ContainsKey(item.value))
                    lore_dic.Add(item.value, new APIC_Lore("lore", item.value, this, "int", 0));

            foreach (var item in lore_dic)
                item.Value.Refresh();
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- WEAPONS/ARMOR PROFICIENCIES
        public string unarmed = "U";
        public string simple_weapons = "U";
        public string martial_weapons = "U";
        public string advanced_weapons = "U";

        public string unarmored = "U";
        public string light_armor = "U";
        public string medium_armor = "U";
        public string heavy_armor = "U";

        public void Unarmed_Refresh() { unarmed = DB.Prof_FindMax(RE_Get("unarmed")); }
        public void SimpleWeapons_Refresh() { simple_weapons = DB.Prof_FindMax(RE_Get("simple_weapons")); }
        public void MartialWeapons_Refresh() { martial_weapons = DB.Prof_FindMax(RE_Get("martial_weapons")); }
        public void AdvancedWeapons_Refresh() { advanced_weapons = DB.Prof_FindMax(RE_Get("advanced_weapons")); }

        public void Unarmored_Refresh() { unarmored = DB.Prof_FindMax(RE_Get("unarmored")); }
        public void LightArmor_Refresh() { light_armor = DB.Prof_FindMax(RE_Get("light_armor")); }
        public void MediumArmor_Refresh() { medium_armor = DB.Prof_FindMax(RE_Get("medium_armor")); }
        public void HeavyArmor_Refresh() { heavy_armor = DB.Prof_FindMax(RE_Get("heavy_armor")); }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ANCESTRY
        private string _ancestry = "";
        public string ancestry { get { return _ancestry; } set { Ancestry_Set(value); } }

        private void Ancestry_Set(string newAncestry)
        {
            if (newAncestry == _ancestry) return;
            if (newAncestry == "")
            {
                Ancestry_Cleanse(true);
                return;
            }
            if (!DB.Ancestries.Any(x => x.name == newAncestry))
            {
                Debug.LogWarning($"[PlayerData] Couldn't find ancestry {newAncestry} ");
                return;
            }

            Ancestry ancestryData = DB.Ancestries.Find(ctx => ctx.name == newAncestry);

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

            // Abl Boosts Stuff
            List<AblBoostData> previousFree = Abl_MapGetAllFrom("ancestry free");
            Abl_MapClearFrom("ancestry free");
            for (int i = 0; i < ancestryData.abl_boosts.FindAll(ctx => ctx == "free").Count; i++)
                if (i < previousFree.Count) // Saves as many free choices as it can, given what new ancestry gives you
                    Abl_MapAdd(previousFree[i]);
        }

        private void Ancestry_Cleanse(bool cleanseChoices)
        {
            _ancestry = "";
            hp_ancestry = 0;
            speed_ancestry = 0;
            size_ancestry = "M";

            Abl_MapClearFrom("ancestry boost");
            Abl_MapClearFrom("ancestry flaw");
            Traits_ClearFrom("ancestry");
            if (cleanseChoices)
                Abl_MapClearFrom("ancestry free");
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BACKGROUND
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
                    Background_Cleanse(false);
                    _background = newBackground;
                    RE_Add("background", "1", new List<RuleElement>(backgroundData.elements));

                    // Abl Boosts Stuff
                    AblBoostData previousChoice = Abl_MapGetFrom("background choice");
                    bool match = false;
                    if (previousChoice != null)
                        foreach (var item in backgroundData.abl_choices)
                            if (previousChoice.abl == item)
                                match = true;
                    if (!match)
                        Abl_MapClearFrom("background choice");
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

            RE_RemoveFrom("background");
            if (cleanseChoices)
            {
                Abl_MapClearFrom("background choice");
                Abl_MapClearFrom("background free");
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CLASS
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
                    Class_Cleanse(false);
                    _class = newClass;
                    hp_class = classData.hp;
                    RE_Add("class", "1", new List<RuleElement>(classData.elements));

                    // Abl Boosts Stuff
                    if (classData.key_ability_choices.Count > 1)
                    {
                        AblBoostData previousChoice = Abl_MapGetFrom("class");
                        bool match = false;
                        if (previousChoice != null)
                            foreach (var item in classData.key_ability_choices)
                                if (previousChoice.abl == item)
                                    match = true;
                        if (!match)
                            Abl_MapClearFrom("class");
                    }
                    else
                    {
                        Abl_MapClearFrom("class");
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
            RE_RemoveFrom("class");

            if (cleanseChoices)
            {
                Abl_MapClearFrom("class");
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BUILD
        public Dictionary<int, Dictionary<string, BuildBlock>> build = new Dictionary<int, Dictionary<string, BuildBlock>>();

        public T Build_GetFromBlock<T>(int level, string key)
        {
            try
            {
                if (build.ContainsKey(level))
                    if (build[level].ContainsKey(key))
                        if (build[level][key].data != null)
                            return JsonConvert.DeserializeObject<T>(build[level][key].data);

                Debug.LogWarning($"[PlayerData] Couldn't find key {key} ");
                return default(T);
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerData] Couldn't retrieve object {key} from build\n{e.Message}\n{e.StackTrace}");
                return default(T);
            }
        }

        /// <summary>
        /// Retrieve feats of specific type in the current build. Types can be: "heritage", "ancestry feat", "ancestry feature", "class feat", "class feature", "general feat" and "skill feat"
        /// </summary>
        /// <param name="type"> heritage, ancestry feat, ancestry feature, class feat, class feature, general feat and skill feat </param>
        public List<Feat> Build_GetFeats(string type)
        {
            List<string> featNames = Build_GetFeatNames(type);
            List<Feat> feats = new List<Feat>();

            switch (type)
            {
                case "heritage":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.AncestryHeritages.Find(ancestry).Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                case "ancestry feat":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.AncestryFeats.Find(ancestry).Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                case "ancestry feature":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.AncestryFeatures.Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                case "class feat":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.ClassFeats.Find(class_name).Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                case "class feature":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.ClassFeatures.Find(class_name).Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                case "general feat":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.SkillFeats.Find("general feat").Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                case "skill feat":
                    foreach (var item in featNames)
                    {
                        Feat feat = DB.SkillFeats.Find("skill feat").Find(x => x.name == item);
                        if (feat != null)
                            feats.Add(feat);
                    }
                    break;
                default: break;
            }

            if (feats.Count == 0)
                Debug.LogWarning($"[PlayerData] Couldn't find feats with type \"{type}\"");

            return feats;
        }

        /// <summary>
        /// Retrieve feat names of specific type in the current build. Types can be: "heritage", "ancestry feat", "ancestry feature", "class feat", "class feature", "general feat" and "skill feat"
        /// </summary>
        /// <param name="type">heritage, ancestry feat, ancestry feature, class feat, class feature, general feat and skill feat </param>
        public List<string> Build_GetFeatNames(string type)
        {
            List<string> featNames = new List<string>();

            foreach (var lvl in build)
                foreach (var block in lvl.Value)
                    if (block.Key == type)
                        featNames.Add(block.Value.name);

            return featNames;
        }

        private void Build_SetNewProgression(string className)
        {
            Debug.Log($"[PlayerData] changing [{name}] class progression");

            Dictionary<int, Dictionary<string, BuildBlock>> newBuild = new Dictionary<int, Dictionary<string, BuildBlock>>();
            ClassProgression prog = DB.ClassProgression.Find(ctx => ctx.name == className);
            List<BuildBlock> coincidences = new List<BuildBlock>();

            // Search for coincidences in old build with new class progression
            foreach (var lvlDic in build)
                foreach (var block in lvlDic.Value)
                    if (block.Value.data == prog.progression[block.Value.level].items.Find(ctx => ctx == block.Value.name))
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
            Debug.LogWarning($"[PlayerData] Refreshing [{name}] build - NOT IMPLEMENTED");
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- RULE ELEMENTS
        private List<RuleElement> RE_general = new List<RuleElement>();

        public void RE_Add(string from, string lvl, RuleElement element) { RE_Add(from, lvl, new List<RuleElement> { element }); }
        public void RE_Add(string from, string lvl, List<RuleElement> list)
        {
            List<string> selectors = new List<string>();

            foreach (var element in list)
            {
                switch (element.key)
                {
                    case "skill_static":
                        if (AlreadyTrained(element.selector, element.proficiency))
                        {
                            Skill_Allocate(from, lvl, element);
                        }
                        else
                        {
                            RE_general.Add(new RuleElement(from, lvl, element));
                            if (!selectors.Contains(element.selector))
                                selectors.Add(element.selector);
                        }
                        break;
                    case "skill_choice":
                        Skill_Allocate(from, lvl, element);
                        break;
                    case "skill_free":
                        Skill_Allocate(from, lvl, element);
                        break;
                    case "skill_improve":
                        Skill_Allocate(from, lvl, element);
                        break;
                    default:
                        break;
                }
            }

            RE_SelectiveRefresh(selectors);
        }

        private void RE_Filter(string from, string lvl, RuleElement element)
        {

            RE_general.Add(new RuleElement(from, lvl, element));

        }

        public IEnumerable<RuleElement> RE_Get(string selector)
        {
            return from a in RE_general
                   where a.selector == selector
                   select a;
        }

        public IEnumerable<RuleElement> RE_Get(string selector, string value)
        {
            return from a in RE_general
                   where a.selector == selector && a.value == value
                   select a;
        }

        public void RE_RemoveFrom(string from)
        {
            // if its a skill, needs to check if it was unspent and detrain that
            RE_general.RemoveAll(ctx => ctx.from == from);
        }

        public void RE_SelectiveRefresh(List<string> selectors)
        {
            List<string> alreadyRefreshed = new List<string>();

            foreach (var item in selectors)
                if (!alreadyRefreshed.Contains(item))
                {
                    alreadyRefreshed.Add(item);
                    switch (item)
                    {
                        case "ac": ac.Refresh(); break;

                        case "perception": perception.Refresh(); break;

                        case "fortitude": fortitude.Refresh(); break;
                        case "reflex": reflex.Refresh(); break;
                        case "will": will.Refresh(); break;

                        case "acrobatics": acrobatics.Refresh(); break;
                        case "arcana": arcana.Refresh(); break;
                        case "athletics": athletics.Refresh(); break;
                        case "crafting": crafting.Refresh(); break;
                        case "deception": deception.Refresh(); break;
                        case "diplomacy": diplomacy.Refresh(); break;
                        case "intimidation": intimidation.Refresh(); break;
                        case "medicine": medicine.Refresh(); break;
                        case "nature": nature.Refresh(); break;
                        case "occultism": occultism.Refresh(); break;
                        case "performance": performance.Refresh(); break;
                        case "religion": religion.Refresh(); break;
                        case "society": society.Refresh(); break;
                        case "stealth": stealth.Refresh(); break;
                        case "survival": survival.Refresh(); break;
                        case "thievery": thievery.Refresh(); break;

                        case "class_dc": ClassDC_Refresh(); break;

                        case "lore": Lores_Refresh(); break;

                        case "unarmed": Unarmed_Refresh(); break;
                        case "simple_weapons": SimpleWeapons_Refresh(); break;
                        case "martial_weapons": MartialWeapons_Refresh(); break;
                        case "advanced_weapons": AdvancedWeapons_Refresh(); break;

                        case "unarmored": Unarmored_Refresh(); break;
                        case "light_armor": LightArmor_Refresh(); break;
                        case "medium_armor": MediumArmor_Refresh(); break;
                        case "heavy_armor": HeavyArmor_Refresh(); break;


                        default: return;
                    }
                }
        }

        /// <summary> Compares selector proficiency against rule proficiency. </summary>
        /// <returns> True if selector is > or = to the proficiency that the rule wants to train. AKA selector can't be trained by this rule. </returns>
        private bool AlreadyTrained(string selector, string ruleProf)
        {
            return DB.Prof_Abbr2Int(Skill_Get(selector).prof) <= DB.Prof_Full2Int(ruleProf) ? true : false;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CONSTRUCTOR
        public CharacterData() // Do NOT change the order
        {
            // Needs to be declared before any APIC
            abl_values = new Dictionary<string, Vector2Int>()
            {
            {"str",new Vector2Int(0,0)},
            {"dex",new Vector2Int(0,0)},
            {"con",new Vector2Int(0,0)},
            {"int",new Vector2Int(0,0)},
            {"wis",new Vector2Int(0,0)},
            {"cha",new Vector2Int(0,0)},
            };

            // Needs to be after abl_values declaration
            level = 1;

            ac = new APIC("ac", this, "dex", 10);

            perception = new APIC("perception", this, "wis", 0);

            fortitude = new APIC("fortitude", this, "con", 0);
            reflex = new APIC("reflex", this, "dex", 0);
            will = new APIC("will", this, "wis", 0);

            acrobatics = new APIC("acrobatics", this, "dex", 0);
            arcana = new APIC("arcana", this, "int", 0);
            athletics = new APIC("athletics", this, "str", 0);
            crafting = new APIC("crafting", this, "int", 0);
            deception = new APIC("deception", this, "cha", 0);
            diplomacy = new APIC("diplomacy", this, "cha", 0);
            intimidation = new APIC("intimidation", this, "cha", 0);
            medicine = new APIC("medicine", this, "wis", 0);
            nature = new APIC("nature", this, "wis", 0);
            occultism = new APIC("occultism", this, "int", 0);
            performance = new APIC("performance", this, "dex", 0);
            religion = new APIC("religion", this, "wis", 0);
            society = new APIC("society", this, "cha", 0);
            stealth = new APIC("stealth", this, "dex", 0);
            survival = new APIC("survival", this, "wis", 0);
            thievery = new APIC("thievery", this, "dex", 0);

            lore_dic = new Dictionary<string, APIC>();

            skill_choice = new Dictionary<RuleElement, List<RuleElement>>();
            skill_free = new Dictionary<RuleElement, List<RuleElement>>();
            skill_unspent = new Dictionary<RuleElement, List<RuleElement>>();
        }
    }

}
