using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "Battlefinder/Pathfinder 2e/Class", order = 0)]
public class SO_PF2E_Classes : ScriptableObject
{
    public string title;
    public E_PF2E_Class gameClass;
    [TextArea(1, 10)]
    public string description;
    [Space(15)]
    public List<E_PF2E_Ability> keyAbility;               // STR or DEX or whatever
    public int healthPoints;
    [Space(15)]
    public E_PF2E_Proficiency perception;
    [Space(15)]
    public E_PF2E_Proficiency fortitude;
    public E_PF2E_Proficiency reflex;
    public E_PF2E_Proficiency will;
    [Space(15)]
    public int freeSkillTrains;                               // Free skills + inteligence modifier
    public List<E_PF2E_Skill> givenSkillTrain;
    public bool giveAllGivenSkillTrains;
    [Space(15)]
    public E_PF2E_Proficiency unarmed;
    public E_PF2E_Proficiency simpleWeapons;
    public E_PF2E_Proficiency martialWeapons;
    public E_PF2E_Proficiency advancedWeapons;
    [Space(15)]
    public E_PF2E_Proficiency unarmored;
    public E_PF2E_Proficiency lightArmor;
    public E_PF2E_Proficiency mediumArmor;
    public E_PF2E_Proficiency heavyArmor;
    [Space(15)]
    public E_PF2E_Proficiency arcaneSpells;
    public E_PF2E_Proficiency divineSpells;
    public E_PF2E_Proficiency occultSpells;
    public E_PF2E_Proficiency primalSpells;
}