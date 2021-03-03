using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pathfinder2e;
using Pathfinder2e.Containers;
using Pathfinder2e.GameData;
using Tools;
using UnityEngine;
using YamlDotNet.Serialization;

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

                Abl_UpdateValues();
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- HIT POINTS
        private int hp_class = 0;
        private int hpancestry = 0;

        public int hp_current { get { return hp_max - hp_damage; } }
        public int hp_max { get { return level * (hp_class + Abl_GetMod("con")) + hpancestry + hp_temp; } }

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
        private float _wealth = 0;
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
        public int speed_ancestry = 0;

        public int speed_base { get { return speed_ancestry; } }
        public int speed_burrow = 0;
        public int speed_climp = 0;
        public int speed_fly = 0;
        public int speed_swim = 0;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SIZE
        public string sizeancestry = "M";
        public string size_temp = "";

        public string size
        {
            get
            {
                if (size_temp != "")
                    return size_temp;
                else
                    return sizeancestry;
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
                    case "T": return 0.5f;
                    case "S": return 1f;
                    case "M": return 1f;
                    case "L": return 2f;
                    case "H": return 4f;
                    case "G": return 8f;
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


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LANGUAGES
        public string languages = "";


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ABILITIES
        public List<RuleElement> RE_abilities;

        public Dictionary<string, Vector2Int> abl_values;
        private static List<string> abl_sources = new List<string> { "str", "dex", "con", "int", "wis", "cha" };

        /// <summary> Retrieve ability score in the shape of an interger. </summary>
        /// <param name="abl"> In the form of "str", "dex", "con", "int", "wis", "cha". </param>
        public int Abl_GetScore(string abl)
        {
            return abl_values[abl].x;
        }

        /// <summary> Retrieve ability modifier in the shape of an interger. </summary>
        /// <param name="abl"> In the form of "str", "dex", "con", "int", "wis", "cha". </param>
        public int Abl_GetMod(string abl)
        {
            return abl_values[abl].y;
        }

        public void Abl_MapAdd(RuleElement boost) { Abl_MapAdd(new List<RuleElement> { boost }); }
        public void Abl_MapAdd(List<RuleElement> boosts)
        {
            foreach (var boost in boosts)
                RE_abilities.Add(boost);
            Abl_UpdateValues();
        }

        public void Abl_MapSet(List<RuleElement> list)
        {
            RE_abilities = new List<RuleElement>(list);
            Abl_UpdateValues();
        }

        public void Abl_MapClearFrom(string from)
        {
            RE_abilities.RemoveAll(x => x.from == from);
            Abl_UpdateValues();
        }

        public IEnumerable<RuleElement> Abl_MapGet()
        {
            return RE_abilities.AsEnumerable();
        }

        public RuleElement Abl_MapGetFrom(string from)
        {
            return RE_abilities.Find(x => x.from == from);
        }

        public IEnumerable<RuleElement> Abl_MapGetAllFrom(string from)
        {
            return from a in RE_abilities
                   where a.@from == @from
                   select a;
        }

        private void Abl_UpdateValues()
        {
            RE_abilities.RemoveAll(x => x.from == null || string.IsNullOrEmpty(x.from));

            if (RE_abilities.Count < 1) return;

            Dictionary<string, Vector2Int> abl_valuesPrev = new Dictionary<string, Vector2Int>(abl_values);

            foreach (var abl in abl_sources)
            {
                IEnumerable<RuleElement> all = RE_abilities.Where(x => int.Parse(x.level) <= level && x.selector == abl);
                int count = all.Sum(x => int.Parse(x.value));

                int score = 10, mod = 0;
                if (count >= 0)
                    for (int i = 0; i < count; i++)
                        score += score < 18 ? 2 : 1;
                else if (count < 0)
                    score += count * 2;
                mod = Mathf.FloorToInt((score - 10) / 2);

                abl_values[abl] = new Vector2Int(score, mod);
            }

            HashSet<string> selectors = new HashSet<string>();
            foreach (var abl in abl_sources)
                if (abl_values[abl].x != abl_valuesPrev[abl].x)
                    selectors.Add(StrExtensions.ToLowerFirst(DB.Abl_Abbr2Full(abl)));
            RE_SelectiveRefresh(selectors);
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- PERCEPTION
        public APIC perception;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SAVES
        public APIC fortitude, reflex, will;


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SKILLS
        public APIC_Skill acrobatics, arcana, athletics, crafting, deception, diplomacy, intimidation, medicine, nature, occultism, performance, religion, society, stealth, survival, thievery;

        // All this track the skills proficiency allocation
        public Dictionary<RuleElement, RuleElement> skill_singles;
        public Dictionary<RuleElement, RuleElement> skill_unspent;
        public Dictionary<RuleElement, RuleElement> skill_choice;
        public Dictionary<RuleElement, List<RuleElement>> skill_free;
        public Dictionary<RuleElement, RuleElement> skill_improve;

        [YamlIgnore]
        public IEnumerable<RuleElement> RE_skills
        {
            get
            {
                return
                    skill_singles.Values.AsEnumerable()
                    .Concat(skill_unspent.Values.AsEnumerable())
                    .Concat(skill_choice.Values.AsEnumerable())
                    .Concat(skill_free.Values.SelectMany(x => x))
                    .Concat(skill_improve.Values.AsEnumerable())
                    .Where(x => !RuleElement.IsEmpty(x) && int.Parse(x.level) <= level);
            }
            set { }
        }

        public APIC_Skill Skill_Get(string full)
        {
            switch (full)
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

        public List<APIC_Skill> Skill_GetAll()
        {
            return new List<APIC_Skill> {
                acrobatics, arcana, athletics,  crafting, deception,
                diplomacy, intimidation, medicine, nature, occultism,
                performance, religion, society, stealth, survival, thievery };
        }

        public void Skill_Add(string from, string lvl, RuleElement element)
        {
            switch (element.key)
            {
                case "skill_static":
                    if (Skill_TraineableTo(element.selector, element.proficiency))
                    {
                        skill_singles.Add(new RuleElement(from, lvl, element), new RuleElement(from, lvl, element));
                        RE_SelectiveRefresh(new HashSet<string> { element.selector });
                    }
                    else
                        skill_unspent.Add(new RuleElement(from, lvl, element), new RuleElement());
                    break;
                case "skill_choice":
                    skill_choice.Add(new RuleElement(from, lvl, element), new RuleElement());
                    break;
                case "skill_free":
                    skill_free.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    break;
                case "skill_improve":
                    skill_improve.Add(new RuleElement(from, lvl, element), new RuleElement());
                    break;
                default:
                    break;
            }
        }

        public void Skill_Set(RuleElement key, List<RuleElement> value)
        {
            HashSet<string> selectors = new HashSet<string>();
            RuleElement oldTrain = new RuleElement();

            foreach (var item in value.Where(x => x.selector != null))
                selectors.Add(item.selector);

            switch (key.key)
            {
                case "skill_static":
                    skill_unspent.TryGetValue(key, out oldTrain);
                    if (oldTrain.selector != null) selectors.Add(oldTrain.selector);
                    skill_unspent[key] = value.Count > 0 ? value[0] : new RuleElement();
                    break;
                case "skill_choice":
                    skill_choice.TryGetValue(key, out oldTrain);
                    if (oldTrain.selector != null) selectors.Add(oldTrain.selector);
                    skill_choice[key] = value.Count > 0 ? value[0] : new RuleElement();
                    break;
                case "skill_free":
                    List<RuleElement> oldTraining = new List<RuleElement>();
                    oldTraining = skill_free[key];
                    foreach (var item in oldTraining) { if (item.selector != null) selectors.Add(item.selector); }
                    skill_free[key] = value;
                    break;
                case "skill_improve":
                    skill_improve.TryGetValue(key, out oldTrain);
                    if (oldTrain.selector != null) selectors.Add(oldTrain.selector);
                    skill_improve[key] = value.Count > 0 ? value[0] : new RuleElement();
                    break;
            }

            RE_SelectiveRefresh(selectors);
            Skill_CheckUnspents();
        }

        private void Skill_RemoveFrom(string from)
        {
            List<RuleElement> keys;
            HashSet<string> selectors = new HashSet<string>();

            keys = (from a in skill_singles where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) skill_singles.RemoveAll(keys);

            keys = (from a in skill_unspent where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) skill_unspent.RemoveAll(keys);

            keys = (from a in skill_choice where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) skill_choice.RemoveAll(keys);

            keys = (from a in skill_free where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) skill_free.RemoveAll(keys);

            keys = (from a in skill_improve where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) skill_improve.RemoveAll(keys);

            RE_SelectiveRefresh(selectors);
            Skill_CheckUnspents();
        }

        private void Skill_CheckUnspents()
        {
            HashSet<string> selectors = new HashSet<string>();
            List<RuleElement> keys = (from a in skill_unspent where Skill_TraineableTo(a.Key.selector, a.Key.proficiency) select a.Key).ToList();

            foreach (var key in keys)
            {
                selectors.Add(key.selector);
                selectors.Add(skill_unspent[key].selector);

                RuleElement newRule = new RuleElement(key);
                skill_singles.Add(newRule, newRule);
                skill_unspent.Remove(key);
            }

            RE_SelectiveRefresh(selectors);
        }

        /// <summary> Compares selector proficiency against some proficiency. Profficiency must be abbr as "T" "E" "M"...</summary>
        /// <returns> True if selector is > or = to the proficiency that the rule wants to train. AKA selector can't be trained by this rule. </returns>
        public bool Skill_TraineableTo(string selector, string prof)
        {
            int selectorProf = DB.Prof_Abbr2Int(Skill_Get(selector).prof);
            int trainProf = DB.Prof_Full2Int(prof);
            return (selectorProf + 1) == trainProf ? true : false;
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
        public string ancestry = "";

        public void Ancestry_Set(string newAncestry)
        {
            if (newAncestry == ancestry) return;
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

            Ancestry ancestryData = DB.Ancestries.Find(x => x.name == newAncestry);

            Ancestry_Cleanse(false);
            ancestry = newAncestry;
            hpancestry = ancestryData.hp;
            speed_ancestry = ancestryData.speed;
            sizeancestry = ancestryData.size;
            foreach (var item in ancestryData.traits)
            {
                Trait trait = DB.Traits.Find(x => x.name == item);
                if (trait != null) RE_Add("ancestry", "1", new RuleElement() { key = "trait", selector = "trait", value = trait.name });
            }

            // Abl Boosts Stuff
            List<RuleElement> ablBoostToAdd = new List<RuleElement>();

            foreach (var item in ancestryData.abl_boosts)
                if (item != "free")
                    ablBoostToAdd.Add(new RuleElement() { from = "ancestry boost", selector = item, level = "1", value = "1" });
            if (ancestryData.abl_flaws != null)
                foreach (var item in ancestryData.abl_flaws)
                    ablBoostToAdd.Add(new RuleElement() { from = "ancestry flaw", selector = item, level = "1", value = "-1" });

            IEnumerable<RuleElement> previousFree = Abl_MapGetAllFrom("ancestry free"); // Save last ancestry free boosts into new ancestry
            Abl_MapClearFrom("ancestry free");
            for (int i = 0; i < ancestryData.abl_boosts.Count(x => x == "free"); i++)
                if (i < previousFree.Count())
                    ablBoostToAdd.Add(previousFree.ElementAt(i));

            Abl_MapAdd(ablBoostToAdd);
        }

        private void Ancestry_Cleanse(bool cleanseChoices)
        {
            ancestry = "";
            hpancestry = 0;
            speed_ancestry = 0;
            sizeancestry = "M";

            Abl_MapClearFrom("ancestry boost");
            Abl_MapClearFrom("ancestry flaw");
            RE_RemoveFrom("ancestry");
            if (cleanseChoices)
                Abl_MapClearFrom("ancestry free");
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BACKGROUND
        public string background = "";

        public void Background_Set(string newBackground)
        {
            if (newBackground == background)
                return;

            if (newBackground != "")
            {
                Background backgroundData = DB.Backgrounds.Find(x => x.name == newBackground);

                if (backgroundData != null)
                {
                    Background_Cleanse(false);
                    background = newBackground;
                    RE_Add("background", "1", new List<RuleElement>(backgroundData.elements));

                    // Abl Boosts Stuff
                    RuleElement previousChoice = Abl_MapGetFrom("background choice");
                    bool match = false;
                    if (previousChoice != null)
                        foreach (var item in backgroundData.abl_choices)
                            if (previousChoice.selector == item)
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
            background = "";

            RE_RemoveFrom("background");
            if (cleanseChoices)
            {
                Abl_MapClearFrom("background choice");
                Abl_MapClearFrom("background free");
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CLASS
        public string class_name = "";

        public void Class_Set(string newClass)
        {
            if (newClass == class_name)
                return;

            if (newClass != "")
            {
                Class classData = DB.Classes.Find(x => x.name == newClass);

                if (classData != null)
                {
                    Class_Cleanse(false);
                    class_name = newClass;
                    hp_class = classData.hp;
                    RE_Add("class", "1", new List<RuleElement>(classData.elements));

                    // Abl Boosts Stuff
                    if (classData.key_ability_choices.Count > 1)
                    {
                        RuleElement previousChoice = Abl_MapGetFrom("class");
                        bool match = false;
                        if (previousChoice != null)
                            foreach (var item in classData.key_ability_choices)
                                if (previousChoice.selector == item)
                                    match = true;
                        if (!match)
                            Abl_MapClearFrom("class");
                    }
                    else
                    {
                        Abl_MapClearFrom("class");
                        Abl_MapAdd(new RuleElement() { from = "class", selector = classData.key_ability_choices[0], level = "1", value = "1" });
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
            class_name = "";
            hp_class = 0;
            RE_RemoveFrom("class");

            if (cleanseChoices)
            {
                Abl_MapClearFrom("class");
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- RULE ELEMENTS
        public List<RuleElement> RE_general;

        public void RE_Add(string from, string lvl, RuleElement element) { RE_Add(from, lvl, new List<RuleElement> { element }); }
        public void RE_Add(string from, string lvl, List<RuleElement> list)
        {
            HashSet<string> selectors = new HashSet<string>();

            foreach (var element in list)
            {
                switch (element.key)
                {
                    case "skill_static":
                        if (Skill_TraineableTo(element.selector, element.proficiency))
                        {
                            skill_singles.Add(new RuleElement(from, lvl, element), new RuleElement(from, lvl, element));
                            RE_SelectiveRefresh(new HashSet<string> { element.selector });
                        }
                        else
                            skill_unspent.Add(new RuleElement(from, lvl, element), new RuleElement());
                        break;
                    case "skill_choice":
                        skill_choice.Add(new RuleElement(from, lvl, element), new RuleElement());
                        break;
                    case "skill_free":
                        skill_free.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                        break;
                    case "skill_improve":
                        skill_improve.Add(new RuleElement(from, lvl, element), new RuleElement());
                        break;

                    default:
                        RE_general.Add(new RuleElement(from, lvl, element));
                        selectors.Add(element.selector);
                        break;
                }
            }

            RE_SelectiveRefresh(selectors);
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
        public IEnumerable<RuleElement> RE_GetFromSkill(string selector)
        {
            return from a in RE_skills
                   where a.selector == selector
                   select a;
        }

        public void RE_RemoveFrom(string from)
        {
            List<RuleElement> keys;
            HashSet<string> selectors = new HashSet<string>();

            keys = (from a in RE_general where a.@from == @from select a).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) RE_general.RemoveAll(keys);

            RE_SelectiveRefresh(selectors);

            Skill_RemoveFrom(from);
        }

        public void RE_SelectiveRefresh(string selectors) { RE_SelectiveRefresh(new HashSet<string> { selectors }); }
        public void RE_SelectiveRefresh(HashSet<string> selectors)
        {
            if (selectors.Count < 1) return;

            Debug.Log($"[PlayerData] Refreshing selectors {String.Join(", ", selectors)} ");

            foreach (var selector in selectors)
                switch (selector)
                {
                    case "strength":
                        athletics?.Refresh();
                        break;
                    case "dexterity":
                        ac?.Refresh();
                        reflex?.Refresh();
                        acrobatics?.Refresh();
                        stealth?.Refresh();
                        thievery?.Refresh();
                        break;
                    case "constitution":
                        fortitude?.Refresh();
                        break;
                    case "intelligence":
                        arcana?.Refresh();
                        crafting?.Refresh();
                        occultism?.Refresh();
                        society?.Refresh();
                        Lores_Refresh();
                        break;
                    case "wisdom":
                        perception?.Refresh();
                        will?.Refresh();
                        diplomacy?.Refresh();
                        medicine?.Refresh();
                        nature?.Refresh();
                        religion?.Refresh();
                        survival?.Refresh();
                        break;
                    case "charisma":
                        deception?.Refresh();
                        diplomacy?.Refresh();
                        intimidation?.Refresh();
                        performance?.Refresh();
                        break;

                    case "ac": ac?.Refresh(); break;

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

                    case "skills": break;

                    case null: break;
                    default: Debug.LogWarning($"[PlayerData] Selector \"{selector}\" couldn't be found!"); break;
                }
        }

        public void RE_SkillRefresh()
        {
            acrobatics.Refresh();
            arcana.Refresh();
            athletics.Refresh();
            crafting.Refresh();
            deception.Refresh();
            diplomacy.Refresh();
            intimidation.Refresh();
            medicine.Refresh();
            nature.Refresh();
            occultism.Refresh();
            performance.Refresh();
            religion.Refresh();
            society.Refresh();
            stealth.Refresh();
            survival.Refresh();
            thievery.Refresh();
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CONSTRUCTOR
        public CharacterData() // Do NOT change the order
        {
            RE_general = new List<RuleElement>();
            RE_abilities = new List<RuleElement>();

            // Needs to be declared before any APIC
            abl_values = new Dictionary<string, Vector2Int>()
            {
            {"str",new Vector2Int(10,0)},
            {"dex",new Vector2Int(10,0)},
            {"con",new Vector2Int(10,0)},
            {"int",new Vector2Int(10,0)},
            {"wis",new Vector2Int(10,0)},
            {"cha",new Vector2Int(10,0)},
            };

            ac = new APIC("ac", this, "dex", 10);

            perception = new APIC("perception", this, "wis", 0);

            fortitude = new APIC("fortitude", this, "con", 0);
            reflex = new APIC("reflex", this, "dex", 0);
            will = new APIC("will", this, "wis", 0);

            // Needs to be after skill APICs declaration
            skill_singles = new Dictionary<RuleElement, RuleElement>();
            skill_unspent = new Dictionary<RuleElement, RuleElement>();
            skill_choice = new Dictionary<RuleElement, RuleElement>();
            skill_free = new Dictionary<RuleElement, List<RuleElement>>();
            skill_improve = new Dictionary<RuleElement, RuleElement>();

            acrobatics = new APIC_Skill("acrobatics", this, "dex", 0);
            arcana = new APIC_Skill("arcana", this, "int", 0);
            athletics = new APIC_Skill("athletics", this, "str", 0);
            crafting = new APIC_Skill("crafting", this, "int", 0);
            deception = new APIC_Skill("deception", this, "cha", 0);
            diplomacy = new APIC_Skill("diplomacy", this, "cha", 0);
            intimidation = new APIC_Skill("intimidation", this, "cha", 0);
            medicine = new APIC_Skill("medicine", this, "wis", 0);
            nature = new APIC_Skill("nature", this, "wis", 0);
            occultism = new APIC_Skill("occultism", this, "int", 0);
            performance = new APIC_Skill("performance", this, "dex", 0);
            religion = new APIC_Skill("religion", this, "wis", 0);
            society = new APIC_Skill("society", this, "cha", 0);
            stealth = new APIC_Skill("stealth", this, "dex", 0);
            survival = new APIC_Skill("survival", this, "wis", 0);
            thievery = new APIC_Skill("thievery", this, "dex", 0);

            lore_dic = new Dictionary<string, APIC>();

            // Needs to be after ALL
            level = 1;
        }
    }

}
