using System;
using System.Collections.Generic;

namespace Pathfinder2e.Containers
{

    public class Effect
    {

        public string id; // Inherited from effect
        public int value; // Inherited from effect, like Clumsy 2 for example
        public string duration;
        public string type; // Circumstance, Status, Item, Untyped
        public string target; // HP, AC, Perception...
        public string[] triggers;

    }

}

// Type
//     None, Default, Circumstance, Status, Item, Untyped
//
// Duration
//     None, Default, EndOfTurn, 1Turn, 2Turn, 3Turn, 4Turn, 5Turn, 6Turn, 7Turn, 8Turn, 9Turn, 10Turn
//
// Triggers
//     None, Default,
//     EnviromentalHeat,
//     Moved, Triped, Shoved, Proned, Magicaly Proned,
//     Affected by Poison,
//     MagicalEffect, NecromancyEffect,
//     TriggeringCheck
//     Saving Throw VS Magical Effect, Saving Throw VS Necromancy Effect,
//     Pushed,
//     Recieving Poison Damage,
// 
// Effect Aplication
//     None, Default, Permanent, Temporal
// 
// Targets
//     None, Default,
//     HP, AC, Perception,
//     STR, DEX, CON, INT, WIS, CHA,
//     SavesFortitude, SavesReflex, SavesWill,
//     AttackMelee, AttackRange, AttackSpell, AttackDCs,
//     DamageMelee = 40, DamageRange = 41, DamageSpell = 42,
//     ResistanceFire = 50, ResistanceCold = 51, ResistanceSlash = 52, ResistanceBlunt = 53, ResistancePiercing = 54,
//     SpeedBase = 60, SpeedBasePenalty = 61, SpeedClimb = 62, SpeedClimbPenalty = 63, SpeedFly = 64, SpeedFlyPenalty = 65, SpeedSwim = 66, SpeedSwimPenalty = 67, SpeedBurrow = 68, SpeedBurrowPenalty = 69,
//     Acrobatics = 80, Arcana = 81, Athletics = 82, Crafting = 83, Deception = 84, Diplomacy = 85, Intimidation = 86, Medicine = 87, Nature = 88, Occultism = 89, Performance = 90, Religion = 91, Society = 92, Stealth = 93, Survival = 94, Thievery = 95,
//     Armor Speed Penalty,
//     Distance Moved,
//     Enviromental Heat Effect
//     Triggering Check
