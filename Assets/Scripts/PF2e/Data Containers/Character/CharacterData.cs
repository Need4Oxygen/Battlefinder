using System.Collections.Generic;
using Pathfinder2e.Containers;
using UnityEngine;

namespace Pathfinder2e.Character
{

    public class CharacterData
    {
        public CharacterData() { }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ID
        public string guid = "";
        public string name = "";

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LEVEL
        public int level = 0;
        public int experience = 0;

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- HIT POINTS
        public int hp_damage = 0;
        public int hp_temp = 0;

        public int hp_class = 0;
        public int hp_ancestry = 0;

        public int hp_dying = 0;
        public int hp_wounds = 0;
        public int hp_doom = 0;

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ABC
        public string ancestry = "";
        public string background = "";
        public string class_name = "";

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- RANDOM
        public int wealth = 0;
        public int heroPoints = 0;

        public int speed_ancestry = 0;
        public int speed_land = 0;
        public int speed_burrow = 0;
        public int speed_climb = 0;
        public int speed_fly = 0;
        public int speed_swim = 0;

        public string size_current = "";
        public float size_bulkMod = 0f;

        public float bulk_current = 0f;
        public float bulk_encThreshold = 0f;
        public float bulk_maxThreshold = 0f;

        public string language_str = "";
        public List<string> language_list = new List<string>();

        public Dictionary<string, string> weapon_specific;

        public string unarmed = "";
        public string simple_weapons = "";
        public string martial_weapons = "";
        public string advanced_weapons = "";

        public string unarmored = "";
        public string light_armor = "";
        public string medium_armor = "";
        public string heavy_armor = "";

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ABILITIES
        public Dictionary<string, Vector2Int> abl_map;

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SKILLS
        public Dictionary<RuleElement, RuleElement> skill_singles;
        public Dictionary<RuleElement, RuleElement> skill_unspent;
        public Dictionary<RuleElement, RuleElement> skill_choice;
        public Dictionary<RuleElement, List<RuleElement>> skill_free;
        public Dictionary<RuleElement, RuleElement> skill_increase;

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- APICs
        public APIC ac, perception, fortitude, reflex, will;
        public APIC_Skill acrobatics, arcana, athletics, crafting,
            deception, diplomacy, intimidation, medicine, nature,
            occultism, performance, religion, society, stealth,
            survival, thievery;

        public Dictionary<string, APIC> lore_dic;

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- DCs
        public string class_dc = "";

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- RULE ELEMENTS
        public List<RuleElement> elements = new List<RuleElement>();

    }

}