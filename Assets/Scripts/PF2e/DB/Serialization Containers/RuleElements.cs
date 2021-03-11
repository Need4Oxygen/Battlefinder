using System;
using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public class RuleElement
    {
        public string from { get; set; }
        public string key { get; set; } // Type of element
        public string selector { get; set; } // What does it affect
        public string level { get; set; } // What does it affect
        public string type { get; set; } // How does it stack
        public string duration { get; set; }
        public string frecuency { get; set; }
        public string until { get; set; } // When does it end

        public string dice_number { get; set; }
        public string die_size { get; set; }

        public Strike strike { get; set; }

        public string proficiency { get; set; }

        public string value { get; set; }
        public List<Bracket> value_list { get; set; }

        public Predicate predicate { get; set; }

        public RuleElement() { }

        public RuleElement(string from, string key, string selector, string level,
        string type, string duration, string frecuency, string until, string dice_number,
        string die_size, Strike strike, string proficiency, string value, List<Bracket> value_list,
        Predicate predicate)
        {
            this.from = from;
            this.key = key;
            this.selector = selector;
            this.level = level;
            this.type = type;
            this.duration = duration;
            this.frecuency = frecuency;
            this.until = until;
            this.dice_number = dice_number;
            this.die_size = die_size;
            this.strike = strike;
            this.proficiency = proficiency;
            this.value = value;
            this.value_list = value_list;
            this.predicate = predicate;
        }

        public RuleElement(string from, string level, RuleElement element)
        {
            this.from = from;
            this.level = level;
            this.selector = element.selector;
            this.key = element.key;
            this.type = element.type;
            this.duration = element.duration;
            this.frecuency = element.frecuency;
            this.until = element.until;
            this.dice_number = element.dice_number;
            this.die_size = element.die_size;
            this.strike = element.strike;
            this.proficiency = element.proficiency;
            this.value = element.value;
            this.value_list = element.value_list;
            this.predicate = element.predicate;
        }

        public RuleElement(RuleElement element)
        {
            this.from = element.from;
            this.level = element.level;
            this.selector = element.selector;
            this.key = element.key;
            this.type = element.type;
            this.duration = element.duration;
            this.frecuency = element.frecuency;
            this.until = element.until;
            this.dice_number = element.dice_number;
            this.die_size = element.die_size;
            this.strike = element.strike;
            this.proficiency = element.proficiency;
            this.value = element.value;
            this.value_list = element.value_list;
            this.predicate = element.predicate;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            RuleElement otherRule = obj as RuleElement;
            if (otherRule == null)
                return false;

            if (otherRule.from == from &&
            otherRule.key == key &&
            otherRule.selector == selector &&
            otherRule.level == level &&
            otherRule.value == value)
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool IsNullOrEmpty(RuleElement rule)
        {
            if (IsNull(rule)) return true;
            if (IsEmpty(rule)) return true;
            return false;
        }

        public static bool IsNull(RuleElement rule)
        {
            if (rule == null)
                return true;
            else
                return false;
        }

        public static bool IsEmpty(RuleElement rule)
        {
            if (rule.key == null && rule.selector == null)
                return true;
            else
                return false;
        }

        public static bool operator ==(RuleElement a, RuleElement b) => a is null ? (b is null ? true : false) : a.Equals(b);
        public static bool operator !=(RuleElement a, RuleElement b) => a is null ? (b is null ? false : true) : !a.Equals(b);
    }

    public class Strike
    {
        public string label { get; set; }
        public string category { get; set; }
        public string damage_type { get; set; }
        public string dice_number { get; set; }
        public string die_size { get; set; }
        public string group { get; set; }
        public string range { get; set; }
        public List<string> traits { get; set; }
    }

    public class Predicate
    {
        public List<string> all { get; set; }
        public List<string> any { get; set; }
        public List<string> not { get; set; }
    }

    public class ValueList
    {
        public List<Bracket> all { get; set; }
    }

    public class Bracket
    {
        public string start { get; set; }
        public string end { get; set; }
        public string value { get; set; }
    }

}

//
//  List of Keys:
//      Abilities
//          abl_static
//      Skills
//          +skill_static - Set skill proficiency start proficiency to given value
//          skill_free - Let the player train a number of skills they choose
//          skill_choice - Let the player decide which of the offered skill should be trained
//          skill_improve - Improve skill proficiency adding a value
//      Proficiency
//          +proficiency_static - Set proficiency start proficiency to given value
//          proficiency_fixed - Override proficiency modifiers and fixes a value
//          proficiency_weapon - Set proficiency for individual weapons
//          proficiency_armor - Set proficiency for individual armors
//
//  List of Selectors:
//      hp, hp_per_level
//      ac
//      +class_dc
//      speed, land_speed, burrow_speed, climb_speed, fly_speed, swim_speed
//      attack, attack_roll, damage
//      saving_throw, fortitude, reflex, will
//      initiative, +perception
//      skill_check, acrobatics, arcana, athletics, crafting, deception, diplomacy, intimidation, medicine, nature, occultism, performance, religion, society, stealth, survival, thievery
//      unarmed, simple_weapons, martial_weapons, advanced_weapons
//      unarmored, light_armor, medium_armor, heavy_armor
//      armors, weapons - for individual pieces proficiency tracking
//