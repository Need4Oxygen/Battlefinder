using System.Collections.Generic;
using UnityEngine;

public class PF2E_PlayerData : PlayerData
{
    public string guid;
    public int level = 0;
    public int experience = 0;

    public E_PF2E_Alignment alignment = E_PF2E_Alignment.NN;
    public E_PF2E_Ancestry ancestry = E_PF2E_Ancestry.Human;
    public E_PF2E_Background background = E_PF2E_Background.Artisan;
    public E_PF2E_Class class_ = E_PF2E_Class.Fighter;

    public Dictionary<string, int> abilities = new Dictionary<string, int>(6) {
        { "STR", 0 }, { "DEX", 0 }, { "CON", 0 },
        { "INT", 0 }, { "WIS", 0 }, { "CHA", 0 }};
    public Dictionary<E_PF2E_Skill, PF2E_AblPrf> skills = new Dictionary<E_PF2E_Skill, PF2E_AblPrf>()
    {
        {E_PF2E_Skill.Acrobatics,new PF2E_AblPrf("Acrobatics",E_PF2E_Ability.Dexterity,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Arcana,new PF2E_AblPrf("Arcana",E_PF2E_Ability.Intelligence,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Athletics,new PF2E_AblPrf("Athletics",E_PF2E_Ability.Strength,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Crafting,new PF2E_AblPrf("Crafting",E_PF2E_Ability.Intelligence,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Deception,new PF2E_AblPrf("Deception",E_PF2E_Ability.Charisma,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Diplomacy,new PF2E_AblPrf("Diplomacy",E_PF2E_Ability.Charisma,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Intimidation,new PF2E_AblPrf("Intimidation",E_PF2E_Ability.Charisma,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Medicine,new PF2E_AblPrf("Medicine",E_PF2E_Ability.Wisdom,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Nature,new PF2E_AblPrf("Nature",E_PF2E_Ability.Wisdom,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Occultism,new PF2E_AblPrf("Occultism",E_PF2E_Ability.Intelligence,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Performance,new PF2E_AblPrf("Performance",E_PF2E_Ability.Dexterity,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Religion,new PF2E_AblPrf("Religion",E_PF2E_Ability.Wisdom,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Society,new PF2E_AblPrf("Society",E_PF2E_Ability.Charisma,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Stealth,new PF2E_AblPrf("Stealth",E_PF2E_Ability.Dexterity,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Survival,new PF2E_AblPrf("Survival",E_PF2E_Ability.Wisdom,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Thievery,new PF2E_AblPrf("Thievery",E_PF2E_Ability.Dexterity,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Free_1,new PF2E_AblPrf("_____",E_PF2E_Ability.Intelligence,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Skill.Free_2,new PF2E_AblPrf("_____",E_PF2E_Ability.Intelligence,E_PF2E_Proficiency.Untrained)}
    };

    public E_PF2E_Proficiency classDC = E_PF2E_Proficiency.Untrained;
    public PF2E_AblPrf perception = new PF2E_AblPrf("Perception", E_PF2E_Ability.Wisdom, E_PF2E_Proficiency.Untrained);
    public Dictionary<E_PF2E_Saves, PF2E_AblPrf> saves = new Dictionary<E_PF2E_Saves, PF2E_AblPrf>(3)
    {
        {E_PF2E_Saves.Fortitude,new PF2E_AblPrf("Fortitude",E_PF2E_Ability.Constitution,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Saves.Fortitude,new PF2E_AblPrf("Reflex",E_PF2E_Ability.Dexterity,E_PF2E_Proficiency.Untrained)},
        {E_PF2E_Saves.Fortitude,new PF2E_AblPrf("Will",E_PF2E_Ability.Wisdom,E_PF2E_Proficiency.Untrained)}
    };
    public Dictionary<string, E_PF2E_Proficiency> weaponProficiencies = new Dictionary<string, E_PF2E_Proficiency>(3)
        { { "Simple", E_PF2E_Proficiency.Untrained }, { "Martial", E_PF2E_Proficiency.Untrained }, { "Advanced", E_PF2E_Proficiency.Untrained } };
    public Dictionary<string, E_PF2E_Proficiency> armorProficiencies = new Dictionary<string, E_PF2E_Proficiency>(4)
        { { "Unarmored", E_PF2E_Proficiency.Untrained }, { "Light", E_PF2E_Proficiency.Untrained }, { "Medium", E_PF2E_Proficiency.Untrained }, { "Heavy", E_PF2E_Proficiency.Untrained } };

}
