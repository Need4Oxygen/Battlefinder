using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Feat", menuName = "Battlefinder/Pathfinder 2e/Feat", order = 0)]
public class SO_PF2E_Feats : ScriptableObject
{
    public string title;
    public E_PF2E_FeatType feat;
    public int level;
    [TextArea(1, 10)] public string description;
    public List<string> prequisites;
    public List<PF2E_Action> actions;
    public List<PF2E_Effect> effects;
    public List<PF2E_Lecture> lectures;
    public List<SO_PF2E_Feats> feats;
}
