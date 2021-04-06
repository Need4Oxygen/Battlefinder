using System;
using System.Collections.Generic;
using Pathfinder2e.Containers;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using Tools;

namespace Pathfinder2e.Character
{

    public class Character
    {
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ID
        [YamlIgnore] public string guid { get { return data.guid; } set { data.guid = value; } }
        [YamlIgnore] public string name { get { return data.name; } set { data.name = value; } }

        [YamlIgnore] public string ancestry { get { return data.ancestry; } }
        [YamlIgnore] public string background { get { return data.background; } }
        [YamlIgnore] public string class_name { get { return data.class_name; } }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LEVEL
        [YamlIgnore] public int level { get { return data.level; } set { data.level = value < 1 ? 1 : value > 20 ? 20 : value; Abl_UpdateValues(); Elements_SelectiveRefresh("all"); } }
        [YamlIgnore] public int experience { get { return data.experience; } set { data.experience = value; } }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- HIT POINTS
        [YamlIgnore] public int hp_current { get { return hp_max - data.hp_damage; } }
        [YamlIgnore] public int hp_max { get { return data.level * (data.hp_class + Abl_GetMod("con")) + data.hp_ancestry + hp_temp; } }

        [YamlIgnore] public int hp_damage { get { return data.hp_damage; } set { if (value >= 0) data.hp_damage = value; } }
        [YamlIgnore] public int hp_temp { get { return data.hp_temp; } }

        [YamlIgnore] public int hp_dying { get { return data.hp_dying; } set { data.hp_dying = value; } }
        [YamlIgnore] public int hp_dyingMax { get { return 4 - data.hp_doom; } }

        [YamlIgnore] public int hp_wounds { get { return data.hp_wounds; } set { data.hp_wounds = value; } }
        [YamlIgnore] public int hp_doom { get { return data.hp_doom; } }

        public void HitPoints_Refresh()
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == "hitpoints"
                where level >= a.level.ToInt()
                select a;

            RuleElement element;

            element = re.FirstOrDefault(a => a.from == "ancestry");
            data.hp_ancestry = element == null ? 0 : element.value.ToInt();

            element = re.FirstOrDefault(a => a.from == "class");
            data.hp_class = element == null ? 0 : element.value.ToInt();

            data.hp_temp = re.Where(a => a.key == "temp").Sum(a => a.value.ToInt());
        }

        public void Doom_Refresh()
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == "doom"
                select a;

            data.hp_doom = re.Where(a => a.key == "static").Sum(a => a.value.ToInt());
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- WEALTH
        [YamlIgnore] public int wealth { get { return data.wealth; } set { data.wealth = value; } }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- HERO POINTS
        [YamlIgnore] public int heroPoints { get { return data.heroPoints; } set { data.heroPoints = value < 0 ? 0 : value > 3 ? 3 : value; } }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CLASS DC
        [YamlIgnore] public string class_dc { get { return data.class_dc; } set { data.class_dc = value; } }

        private void ClassDC_Refresh()
        {
            class_dc = DB.Prof_FindMax(RE_GetBy_Sel("class_dc"), level).Item1;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SPEED
        [YamlIgnore] public int speed_land { get { return data.speed_land; } }
        [YamlIgnore] public int speed_burrow { get { return data.speed_burrow; } }
        [YamlIgnore] public int speed_climb { get { return data.speed_climb; } }
        [YamlIgnore] public int speed_fly { get { return data.speed_fly; } }
        [YamlIgnore] public int speed_swim { get { return data.speed_swim; } }

        private void Speed_Refresh(string type)
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == type
                where level >= a.level.ToInt()
                select a;

            int bass = 0; re.Where(a => a.key == "base").ForEach(a => { int value = a.value.ToInt(); bass = value > bass ? value : bass; });
            int bonus = re.Where(a => a.key == "bonus").Sum(a => a.value.ToInt());
            int penalty = re.Where(a => a.key == "penalty").Sum(a => a.value.ToInt());
            int forgiven = re.Where(a => a.key == "forgiven").Sum(a => a.value.ToInt());

            int result = bass + bonus - Mathf.Clamp(penalty - forgiven, 0, 99);

            switch (type)
            {
                case "speed_land": data.speed_land = result; break;
                case "speed_burrow": data.speed_burrow = result; break;
                case "speed_climb": data.speed_climb = result; break;
                case "speed_fly": data.speed_fly = result; break;
                case "speed_swim": data.speed_swim = result; break;
                default: break;
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SIZE
        [YamlIgnore] public string size { get { return data.size_current; } }

        private void Size_Refresh()
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == "size"
                select a;


            int baseSize = 2; var x = re.FirstOrDefault(a => a.key == "base"); baseSize = x == null ? 2 : x.value.ToInt();
            int tempSize = -1; re.Where(a => a.key == "temp").ForEach(a => { int size = DB.Size_Abbr2Int(a.value); tempSize = size > tempSize ? size : tempSize; });

            string currentSize = DB.Size_Int2Abbr(tempSize > -1 ? tempSize : baseSize);

            data.size_current = currentSize;
            data.size_bulkMod = DB.Size_Abbr2BulkMod(currentSize);
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BULK
        [YamlIgnore] public float bulk_current { get { return data.bulk_current; } }
        [YamlIgnore] public float bulk_encThreshold { get { return data.bulk_encThreshold; } }
        [YamlIgnore] public float bulk_maxThreshold { get { return data.bulk_maxThreshold; } }

        private void Bulk_Refresh()
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == "bulk"
                select a;

            int bonus = re.Where(a => a.key == "bonus").Sum(a => a.value.ToInt());
            int penalty = re.Where(a => a.key == "penalty").Sum(a => a.value.ToInt());
            int forgiven = re.Where(a => a.key == "forgiven").Sum(a => a.value.ToInt());

            int extraBulk = bonus - Mathf.Clamp(penalty - forgiven, 0, 99);

            data.bulk_encThreshold = data.size_bulkMod * (5 + Abl_GetMod("str")) + extraBulk;
            data.bulk_maxThreshold = data.size_bulkMod * (10 + Abl_GetMod("str")) + extraBulk;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LANGUAGES
        [YamlIgnore] public string language_str { get { return data.language_str; } }
        [YamlIgnore] public List<string> language_list { get { return data.language_list; } }

        private void Language_Refresh()
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == "language"
                select a;

            data.language_str = string.Join(", ", from a in re select a.value);
            data.language_list = (from a in re select a.value).ToList();
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ABILITIES
        public IEnumerable<RuleElement> elements_abl { get { return elements.Where(a => DB.Abl_AbbrList.Contains(a.selector)); } }

        /// <summary> Retrieve ability score in the shape of an interger. </summary>
        /// <param name="abl"> In the form of "str", "dex", "con", "int", "wis", "cha". </param>
        public int Abl_GetScore(string abl)
        {
            return data.abl_map[abl].x;
        }

        /// <summary> Retrieve ability modifier in the shape of an interger. </summary>
        /// <param name="abl"> In the form of "str", "dex", "con", "int", "wis", "cha". </param>
        public int Abl_GetMod(string abl)
        {
            return data.abl_map[abl].y;
        }

        public void Abl_Add(RuleElement boost) { Abl_Add(new List<RuleElement> { boost }); }
        public void Abl_Add(List<RuleElement> boosts)
        {
            data.elements.AddRange(boosts);
        }

        public void Abl_Remove(IEnumerable<RuleElement> elements)
        {
            data.elements.RemoveAll(elements);
        }

        public void Abl_RemoveFrom(string from)
        {
            data.elements.RemoveAll(a => a.from == from);
        }

        public RuleElement Abl_GetFrom(string from)
        {
            return data.elements.Find(x => x.from == from);
        }

        public IEnumerable<RuleElement> Abl_GetAllFrom(string from)
        {
            return from a in data.elements
                   where a.@from == @from
                   select a;
        }

        public void Abl_UpdateValues()
        {
            Dictionary<string, Vector2Int> abl_valuesPrev = new Dictionary<string, Vector2Int>(data.abl_map);

            List<RuleElement> re = elements_abl.Where(a => a.level.ToInt() <= level).ToList();

            foreach (var abl in DB.Abl_AbbrList)
            {
                int count = re.Where(a => a.selector == abl).Sum(a => a.value.ToInt());
                int score = 10, mod = 0;

                if (count >= 0)
                    for (int i = 0; i < count; i++)
                        score += score < 18 ? 2 : 1;
                else if (count < 0)
                    score += count * 2;
                mod = Mathf.FloorToInt((score - 10) / 2);

                data.abl_map[abl] = new Vector2Int(score, mod);
            }

            HashSet<string> selectors = new HashSet<string>();
            foreach (var abl in DB.Abl_AbbrList)
                if (data.abl_map[abl].x != abl_valuesPrev[abl].x)
                    selectors.Add(abl);
            Elements_SelectiveRefresh(selectors);
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SKILLS
        [YamlIgnore]
        public IEnumerable<RuleElement> elements_skills
        {
            get
            {
                return
                   data.skill_singles.Values.AsEnumerable()
                   .Concat(data.skill_unspent.Values.AsEnumerable())
                   .Concat(data.skill_choice.Values.AsEnumerable())
                   .Concat(data.skill_free.Values.SelectMany(a => a))
                   .Concat(data.skill_increase.Values.AsEnumerable())
                   .Where(a => !RuleElement.IsEmpty(a));
            }
        }

        public APIC_Skill Skill_Get(string full)
        {
            switch (full)
            {
                case "acrobatics": return data.acrobatics;
                case "arcana": return data.arcana;
                case "athletics": return data.athletics;
                case "crafting": return data.crafting;
                case "deception": return data.deception;
                case "diplomacy": return data.diplomacy;
                case "intimidation": return data.intimidation;
                case "medicine": return data.medicine;
                case "nature": return data.nature;
                case "occultism": return data.occultism;
                case "performance": return data.performance;
                case "religion": return data.religion;
                case "society": return data.society;
                case "stealth": return data.stealth;
                case "survival": return data.survival;
                case "thievery": return data.thievery;
                default: return null;
            }
        }

        public List<APIC_Skill> Skill_GetAll()
        {
            return new List<APIC_Skill> {
                data.acrobatics, data.arcana, data.athletics,  data.crafting, data.deception,
                data.diplomacy, data.intimidation, data.medicine, data.nature, data.occultism,
                data.performance, data.religion, data.society, data.stealth, data.survival, data.thievery };
        }

        private void Skill_Allocate(string from, string lvl, RuleElement element)
        {
            switch (element.key)
            {
                case "static":
                    if (Skill_OneLessProf(element.selector, element.proficiency))
                        data.skill_singles.Add(new RuleElement(from, lvl, element), new RuleElement(from, lvl, element));
                    else
                        data.skill_unspent.Add(new RuleElement(from, lvl, element), new RuleElement());
                    break;
                case "choice":
                    data.skill_choice.Add(new RuleElement(from, lvl, element), new RuleElement());
                    break;
                case "free":
                    data.skill_free.Add(new RuleElement(from, lvl, element), new List<RuleElement>());
                    break;
                case "increase":
                    data.skill_increase.Add(new RuleElement(from, lvl, element), new RuleElement());
                    break;
                default:
                    break;
            }
        }

        public void Skill_Set(RuleElement key, List<RuleElement> value)
        {
            HashSet<string> selectors = new HashSet<string>();
            RuleElement oldTrain = new RuleElement();

            foreach (var item in value.Where(a => a.selector != null))
                selectors.Add(item.selector);

            switch (key.key)
            {
                case "static":
                    data.skill_unspent.TryGetValue(key, out oldTrain);
                    if (oldTrain.selector != null) selectors.Add(oldTrain.selector);
                    data.skill_unspent[key] = value.Count > 0 ? value[0] : new RuleElement();
                    break;
                case "choice":
                    data.skill_choice.TryGetValue(key, out oldTrain);
                    if (oldTrain.selector != null) selectors.Add(oldTrain.selector);
                    data.skill_choice[key] = value.Count > 0 ? value[0] : new RuleElement();
                    break;
                case "free":
                    List<RuleElement> oldTraining = new List<RuleElement>();
                    oldTraining = data.skill_free[key];
                    foreach (var item in oldTraining) { if (item.selector != null) selectors.Add(item.selector); }
                    data.skill_free[key] = value;
                    break;
                case "increase":
                    data.skill_increase.TryGetValue(key, out oldTrain);
                    if (oldTrain.selector != null) selectors.Add(oldTrain.selector);
                    data.skill_increase[key] = value.Count > 0 ? value[0] : new RuleElement();
                    break;
            }

            Elements_SelectiveRefresh(selectors);
            Skill_CheckUnspents();
            Skill_CheckIncreases();
        }

        private void Skill_RemoveFrom(string from)
        {
            List<RuleElement> keys;
            HashSet<string> selectors = new HashSet<string>();

            keys = (from a in data.skill_singles where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) data.skill_singles.RemoveAll(keys);

            keys = (from a in data.skill_unspent where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) data.skill_unspent.RemoveAll(keys);

            keys = (from a in data.skill_choice where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) data.skill_choice.RemoveAll(keys);

            keys = (from a in data.skill_free where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) data.skill_free.RemoveAll(keys);

            keys = (from a in data.skill_increase where a.Key.@from == @from select a.Key).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) data.skill_increase.RemoveAll(keys);

            Elements_SelectiveRefresh(selectors);
            Skill_CheckUnspents();
            Skill_CheckIncreases();
        }

        private void Skill_CheckUnspents()
        {
            HashSet<string> selectors = new HashSet<string>();
            List<RuleElement> keys = (from a in data.skill_unspent where Skill_OneLessProf(a.Key.selector, a.Key.proficiency) select a.Key).ToList();

            foreach (var key in keys)
            {
                selectors.Add(key.selector);
                selectors.Add(data.skill_unspent[key].selector);

                RuleElement newRule = new RuleElement(key);
                data.skill_singles.Add(newRule, newRule);
                data.skill_unspent.Remove(key);
            }

            Elements_SelectiveRefresh(selectors);
        }

        private void Skill_CheckIncreases()
        {
            List<RuleElement> keys = (
                from a in data.skill_increase
                where !RuleElement.IsEmpty(a.Value)
                where DB.Prof_Abbr2Int(Skill_Get(a.Value.selector).profLvl20) < DB.Prof_Full2Int(a.Value.proficiency) ? true : false
                select a.Key).ToList();

            foreach (var key in keys)
                data.skill_increase[key] = new RuleElement();
        }

        /// <summary> Checks if given selector proficiency level is one under given proficiency level. Proficiency must be abbr as "U", "T"...</summary>
        public bool Skill_OneLessProf(string selector, string prof)
        {
            int selectorProf = DB.Prof_Abbr2Int(Skill_Get(selector).profLvl20);
            int givenProf = DB.Prof_Full2Int(prof);
            return (selectorProf + 1) == givenProf ? true : false;
        }

        /// <summary> Checks if given selector proficiency level is under given proficiency level. Proficiency must be abbr as "U", "T"...</summary>
        public bool Skill_UnderProf(string selector, string prof)
        {
            int selectorProf = DB.Prof_Abbr2Int(Skill_Get(selector).profLvl20);
            int givenProf = DB.Prof_Full2Int(prof);
            return (selectorProf) < givenProf ? true : false;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LORES
        private void Lores_Refresh()
        {
            data.lore_dic.Clear();

            IEnumerable<RuleElement> loreElements = RE_GetBy_Sel("lore");

            foreach (var item in loreElements)
                if (data.lore_dic.ContainsKey(item.value))
                    data.lore_dic.Add(item.value, new APIC_Lore("lore", item.value, this, "int", 0));

            foreach (var item in data.lore_dic)
                item.Value.Refresh();
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- WEAPONS/ARMOR PROFICIENCIES
        [YamlIgnore] public string unarmed { get { return data.unarmed; } }
        [YamlIgnore] public string simple_weapons { get { return data.simple_weapons; } }
        [YamlIgnore] public string martial_weapons { get { return data.martial_weapons; } }
        [YamlIgnore] public string advanced_weapons { get { return data.advanced_weapons; } }

        [YamlIgnore] public string unarmored { get { return data.unarmored; } }
        [YamlIgnore] public string light_armor { get { return data.light_armor; } }
        [YamlIgnore] public string medium_armor { get { return data.medium_armor; } }
        [YamlIgnore] public string heavy_armor { get { return data.heavy_armor; } }

        private void Unarmed_Refresh() { data.unarmed = DB.Prof_FindMax(RE_GetBy_Sel("unarmed"), level).Item1; }
        private void SimpleWeapons_Refresh() { data.simple_weapons = DB.Prof_FindMax(RE_GetBy_Sel("simple_weapons"), level).Item1; }
        private void MartialWeapons_Refresh() { data.martial_weapons = DB.Prof_FindMax(RE_GetBy_Sel("martial_weapons"), level).Item1; }
        private void AdvancedWeapons_Refresh() { data.advanced_weapons = DB.Prof_FindMax(RE_GetBy_Sel("advanced_weapons"), level).Item1; }

        private void Unarmored_Refresh() { data.unarmored = DB.Prof_FindMax(RE_GetBy_Sel("unarmored"), level).Item1; }
        private void LightArmor_Refresh() { data.light_armor = DB.Prof_FindMax(RE_GetBy_Sel("light_armor"), level).Item1; }
        private void MediumArmor_Refresh() { data.medium_armor = DB.Prof_FindMax(RE_GetBy_Sel("medium_armor"), level).Item1; }
        private void HeavyArmor_Refresh() { data.heavy_armor = DB.Prof_FindMax(RE_GetBy_Sel("heavy_armor"), level).Item1; }

        private void Weapons_Refresh()
        {
            IEnumerable<RuleElement> re =
                from a in elements
                where a.selector == "weapon_specific"
                select a;

            foreach (var element in re)
                foreach (var value in element.value_list)
                {
                    if (data.weapon_specific.ContainsKey(value.value))
                    {
                        if (DB.Prof_Full2Int(element.proficiency) > DB.Prof_Full2Int(data.weapon_specific[value.value]))
                            data.weapon_specific[value.value] = element.proficiency;
                    }
                    else
                        data.weapon_specific.Add(value.value, element.proficiency);
                }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ANCESTRY
        public void Ancestry_Set(string newAncestry)
        {
            if (newAncestry == ancestry) return;
            if (newAncestry == "") { Ancestry_Cleanse(true); return; }

            Ancestry ancestryData = DB.Ancestries.Find(a => a.name == newAncestry);

            if (ancestryData != null)
            {
                Ancestry_Cleanse(false);

                // Variables
                data.ancestry = newAncestry;

                // Add Rule Elements
                List<RuleElement> elementsToAdd = new List<RuleElement>()
                {
                    {new RuleElement() { key = "static", selector = "hitpoints", value = ancestryData.hp.ToString() }},
                    {new RuleElement() { key = "base", selector = "speed_land" , value= ancestryData.speed.ToString()}},
                    {new RuleElement() { key = "base", selector = "size", value = ancestryData.size }},
                };
                ancestryData.traits.ForEach(a => elementsToAdd.Add(new RuleElement() { key = "static", selector = "trait", value = a }));
                Elements_Add("ancestry", "1", elementsToAdd);

                // Abl Boosts Stuff
                List<RuleElement> ablBoostToAdd = new List<RuleElement>();

                foreach (var item in ancestryData.abl_boosts)
                    if (item != "free")
                        ablBoostToAdd.Add(new RuleElement() { from = "ancestry boost", key = "ability_modifier", selector = item, level = "1", value = "1" });
                if (ancestryData.abl_flaws != null)
                    foreach (var item in ancestryData.abl_flaws)
                        ablBoostToAdd.Add(new RuleElement() { from = "ancestry flaw", key = "ability_modifier", selector = item, level = "1", value = "-1" });

                List<RuleElement> previousFree = Abl_GetAllFrom("ancestry free").ToList(); // Save last data.ancestry free boosts into new data.ancestry
                if (previousFree.Count > 1) Abl_RemoveFrom("ancestry free");
                for (int i = 0; i < ancestryData.abl_boosts.Count(a => a == "free"); i++)
                    if (i < previousFree.Count())
                        ablBoostToAdd.Add(previousFree.ElementAt(i));

                Abl_Add(ablBoostToAdd);
                Abl_UpdateValues();
            }
            else
            {
                Logger.LogWarning("CharacterData", $"Couldn't find data.ancestry \"{newAncestry}\"");
            }
        }

        private void Ancestry_Cleanse(bool cleanseChoices)
        {
            data.ancestry = "";
            data.speed_ancestry = 0;

            Elements_RemoveFrom("ancestry");
            Abl_RemoveFrom("ancestry boost");
            Abl_RemoveFrom("ancestry flaw");
            if (cleanseChoices)
                Abl_RemoveFrom("ancestry free");
            Abl_UpdateValues();
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BACKGROUND
        public void Background_Set(string newBackground)
        {
            if (newBackground == background) return;
            if (newBackground == "") { Background_Cleanse(true); return; }

            Background backgroundData = DB.Backgrounds.Find(a => a.name == newBackground);

            if (backgroundData != null)
            {
                Background_Cleanse(false);

                // Variables
                data.background = newBackground;

                // Add Rule Elements
                Elements_Add("background", "1", new List<RuleElement>(backgroundData.elements));

                // Abl Boosts Stuff
                RuleElement previousChoice = Abl_GetFrom("background choice");
                bool match = false;
                if (previousChoice != null)
                    foreach (var item in backgroundData.abl_choices)
                        if (previousChoice.selector == item)
                            match = true;
                if (!match)
                    Abl_RemoveFrom("background choice");
                Abl_UpdateValues();
            }
            else
            {
                Logger.LogWarning("CharacterData", $"Couldn't find data.background \"{newBackground}\"");
            }
        }

        private void Background_Cleanse(bool cleanseChoices)
        {
            data.background = "";

            Elements_RemoveFrom("background");
            if (cleanseChoices)
            {
                Abl_RemoveFrom("background choice");
                Abl_RemoveFrom("background free");
                Abl_UpdateValues();
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CLASS
        public void Class_Set(string newClass)
        {
            if (newClass == class_name) return;
            if (newClass == "") { Class_Cleanse(true); return; }

            Class classData = DB.Classes.Find(a => a.name == newClass);
            ClassProgression classProg = DB.ClassProgression.Find(a => a.name == newClass);

            if (classData != null && classProg != null)
            {
                Class_Cleanse(false);

                // Variables
                data.class_name = newClass;

                // Add Rule Elements
                List<RuleElement> elementsToAdd = new List<RuleElement>();
                elementsToAdd.Add(new RuleElement() { key = "static", selector = "hitpoints", value = classData.hp.ToString() });
                elementsToAdd.AddRange(classData.elements);

                // Add skill increases into skillIncrease dictionary
                var skillIcreases = (from a in classProg.progression from b in a.items where b == "skill increase" select a.level).ToList();
                if (skillIcreases.Count > 0)
                    foreach (var item in skillIcreases)
                        elementsToAdd.Add(new RuleElement() { key = "increase", selector = "skills", level = item.ToString(), proficiency = DB.Skl_MaxTrainability(item) });

                Elements_Add("class", "1", elementsToAdd);

                // Abl Boosts Stuff
                if (classData.key_ability_choices.Count > 1)
                {
                    RuleElement previousChoice = Abl_GetFrom("class");
                    bool match = false;
                    if (previousChoice != null)
                        foreach (var item in classData.key_ability_choices)
                            if (previousChoice.selector == item)
                                match = true;
                    if (!match)
                        Abl_RemoveFrom("class");
                }
                else
                {
                    Abl_RemoveFrom("class");
                    Abl_Add(new RuleElement() { from = "class", selector = classData.key_ability_choices[0], level = "1", value = "1" });
                }
                Abl_UpdateValues();
            }
            else
            {
                Logger.LogWarning("CharacterData", $"Couldn't find class \"{newClass}\"");
            }
        }

        private void Class_Cleanse(bool cleanseChoices)
        {
            data.class_name = "";
            data.hp_class = 0;
            Elements_RemoveFrom("class");

            if (cleanseChoices)
            {
                Abl_RemoveFrom("class");
                Abl_UpdateValues();
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- RULE ELEMENTS
        public IEnumerable<RuleElement> elements { get { return data.elements; } }

        public void Elements_Add(string from, string lvl, RuleElement element) { Elements_Add(from, lvl, new List<RuleElement> { element }); }
        public void Elements_Add(string from, string lvl, List<RuleElement> list)
        {
            HashSet<string> selectors = new HashSet<string>();

            foreach (var element in list)
            {
                string newLvl = string.IsNullOrEmpty(element.level) ? lvl : element.level;

                if (DB.Skl_FullList.Contains(element.selector) || element.selector == "skills")
                    Skill_Allocate(from, newLvl, element);

                data.elements.Add(new RuleElement(from, newLvl, element));
                selectors.Add(element.selector);
            }

            Elements_SelectiveRefresh(selectors);
        }

        public IEnumerable<RuleElement> RE_GetBy_Sel(string selector)
        {
            return from a in data.elements
                   where a.selector == selector
                   select a;
        }

        public void Elements_RemoveFrom(string from)
        {
            List<RuleElement> keys;
            HashSet<string> selectors = new HashSet<string>();

            keys = (from a in data.elements where a.@from == @from select a).ToList();
            foreach (var key in keys) selectors.Add(key.selector);
            if (keys.Count > 0) data.elements.RemoveAll(keys);

            Elements_SelectiveRefresh(selectors);

            Skill_RemoveFrom(from);
        }

        public void Elements_SelectiveRefresh(string selectors) { Elements_SelectiveRefresh(new HashSet<string> { selectors }); }
        public void Elements_SelectiveRefresh(HashSet<string> selectors)
        {
            if (selectors.Count < 1) return;

            Logger.Log("CharacterData", $"Refreshing selectors \"{String.Join(", ", selectors)}\"");

            foreach (var selector in selectors)
                switch (selector)
                {
                    case "str":
                        data.athletics.Refresh();
                        break;
                    case "dex":
                        data.ac.Refresh();
                        data.reflex.Refresh();
                        data.acrobatics.Refresh();
                        data.stealth.Refresh();
                        data.thievery.Refresh();
                        break;
                    case "con":
                        data.fortitude.Refresh();
                        break;
                    case "int":
                        data.arcana.Refresh();
                        data.crafting.Refresh();
                        data.occultism.Refresh();
                        data.society.Refresh();
                        Lores_Refresh();
                        break;
                    case "wis":
                        data.perception.Refresh();
                        data.will.Refresh();
                        data.diplomacy.Refresh();
                        data.medicine.Refresh();
                        data.nature.Refresh();
                        data.religion.Refresh();
                        data.survival.Refresh();
                        break;
                    case "cha":
                        data.deception.Refresh();
                        data.diplomacy.Refresh();
                        data.intimidation.Refresh();
                        data.performance.Refresh();
                        break;

                    case "ac": data.ac.Refresh(); break;

                    case "perception": data.perception.Refresh(); break;

                    case "fortitude": data.fortitude.Refresh(); break;
                    case "reflex": data.reflex.Refresh(); break;
                    case "will": data.will.Refresh(); break;

                    case "acrobatics": data.acrobatics.Refresh(); break;
                    case "arcana": data.arcana.Refresh(); break;
                    case "athletics": data.athletics.Refresh(); break;
                    case "crafting": data.crafting.Refresh(); break;
                    case "deception": data.deception.Refresh(); break;
                    case "diplomacy": data.diplomacy.Refresh(); break;
                    case "intimidation": data.intimidation.Refresh(); break;
                    case "medicine": data.medicine.Refresh(); break;
                    case "nature": data.nature.Refresh(); break;
                    case "occultism": data.occultism.Refresh(); break;
                    case "performance": data.performance.Refresh(); break;
                    case "religion": data.religion.Refresh(); break;
                    case "society": data.society.Refresh(); break;
                    case "stealth": data.stealth.Refresh(); break;
                    case "survival": data.survival.Refresh(); break;
                    case "thievery": data.thievery.Refresh(); break;

                    case "class_dc": ClassDC_Refresh(); break;

                    case "lore": Lores_Refresh(); break;

                    case "weapon_specific": Weapons_Refresh(); break;

                    case "unarmed": Unarmed_Refresh(); break;
                    case "simple_weapons": SimpleWeapons_Refresh(); break;
                    case "martial_weapons": MartialWeapons_Refresh(); break;
                    case "advanced_weapons": AdvancedWeapons_Refresh(); break;

                    case "unarmored": Unarmored_Refresh(); break;
                    case "light_armor": LightArmor_Refresh(); break;
                    case "medium_armor": MediumArmor_Refresh(); break;
                    case "heavy_armor": HeavyArmor_Refresh(); break;

                    case "speed_land": Speed_Refresh(selector); break;
                    case "speed_burrow": Speed_Refresh(selector); break;
                    case "speed_limb": Speed_Refresh(selector); break;
                    case "speed_fly": Speed_Refresh(selector); break;
                    case "speed_swim": Speed_Refresh(selector); break;

                    case "hitpoints": HitPoints_Refresh(); break;
                    case "doom": Doom_Refresh(); break;

                    case "size": Size_Refresh(); break;
                    case "bulk": Bulk_Refresh(); break;

                    case "language": Language_Refresh(); break;

                    case "all":
                        HitPoints_Refresh(); Doom_Refresh(); Size_Refresh(); Bulk_Refresh(); Language_Refresh();
                        data.ac.Refresh(); data.perception.Refresh(); data.fortitude.Refresh(); data.reflex.Refresh(); data.will.Refresh();
                        Elements_SkillRefresh(); ClassDC_Refresh(); Lores_Refresh(); Weapons_Refresh();
                        Unarmed_Refresh(); SimpleWeapons_Refresh(); MartialWeapons_Refresh(); AdvancedWeapons_Refresh();
                        Unarmored_Refresh(); LightArmor_Refresh(); MediumArmor_Refresh(); HeavyArmor_Refresh();
                        Speed_Refresh("speed_land"); Speed_Refresh("speed_burrow"); Speed_Refresh("speed_limb"); Speed_Refresh("speed_fly"); Speed_Refresh("speed_swim");
                        break;
                    case "trait": break;
                    case "skills": break; // Handled by the skill dictionary system
                    case null: break;
                    default:
                        Logger.LogWarning("CharacterData", $"Selector \"{selector}\" couldn't be found!"); break;
                }
        }

        public void Elements_SkillRefresh()
        {
            data.acrobatics.Refresh(); data.arcana.Refresh(); data.athletics.Refresh(); data.crafting.Refresh();
            data.deception.Refresh(); data.diplomacy.Refresh(); data.intimidation.Refresh(); data.medicine.Refresh();
            data.nature.Refresh(); data.occultism.Refresh(); data.performance.Refresh(); data.religion.Refresh();
            data.society.Refresh(); data.stealth.Refresh(); data.survival.Refresh(); data.thievery.Refresh();
        }


        public CharacterData data = new CharacterData();

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- CONSTRUCTOR
        public Character() // Do NOT change the order
        {
            data = new CharacterData();

            // Needs to be declared before any APIC
            data.abl_map = new Dictionary<string, Vector2Int>()
            {
            {"str",new Vector2Int(10,0)}, {"dex",new Vector2Int(10,0)}, {"con",new Vector2Int(10,0)},
            {"int",new Vector2Int(10,0)}, {"wis",new Vector2Int(10,0)}, {"cha",new Vector2Int(10,0)},
            };

            data.ac = new APIC("ac", this, "dex", 10);

            data.perception = new APIC("perception", this, "wis", 0);

            data.fortitude = new APIC("fortitude", this, "con", 0);
            data.reflex = new APIC("reflex", this, "dex", 0);
            data.will = new APIC("will", this, "wis", 0);

            // Needs to be after skill APICs declaration
            data.skill_singles = new Dictionary<RuleElement, RuleElement>();
            data.skill_unspent = new Dictionary<RuleElement, RuleElement>();
            data.skill_choice = new Dictionary<RuleElement, RuleElement>();
            data.skill_free = new Dictionary<RuleElement, List<RuleElement>>();
            data.skill_increase = new Dictionary<RuleElement, RuleElement>();

            data.acrobatics = new APIC_Skill("acrobatics", this, "dex", 0);
            data.arcana = new APIC_Skill("arcana", this, "int", 0);
            data.athletics = new APIC_Skill("athletics", this, "str", 0);
            data.crafting = new APIC_Skill("crafting", this, "int", 0);
            data.deception = new APIC_Skill("deception", this, "cha", 0);
            data.diplomacy = new APIC_Skill("diplomacy", this, "cha", 0);
            data.intimidation = new APIC_Skill("intimidation", this, "cha", 0);
            data.medicine = new APIC_Skill("medicine", this, "wis", 0);
            data.nature = new APIC_Skill("nature", this, "wis", 0);
            data.occultism = new APIC_Skill("occultism", this, "int", 0);
            data.performance = new APIC_Skill("performance", this, "cha", 0);
            data.religion = new APIC_Skill("religion", this, "wis", 0);
            data.society = new APIC_Skill("society", this, "int", 0);
            data.stealth = new APIC_Skill("stealth", this, "dex", 0);
            data.survival = new APIC_Skill("survival", this, "wis", 0);
            data.thievery = new APIC_Skill("thievery", this, "dex", 0);

            data.lore_dic = new Dictionary<string, APIC>();

            data.weapon_specific = new Dictionary<string, string>();

            // Needs to be after ALL
            level = 1;
        }
    }

}
