using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "Battlefinder/Pathfinder 2e/Class", order = 0)]
public class SO_PF2E_Classes : ScriptableObject
{
    public string title;
    public E_PF2E_Class gameClass;
    [TextArea(1, 10)] public string description;

    [Space(15)]
    public int healthPoints;
    public List<E_PF2E_Ability> keyAbility;
    [Space(15)]
    public int freeSkillTrains;
    public List<PF2E_Lecture> lectures;
}