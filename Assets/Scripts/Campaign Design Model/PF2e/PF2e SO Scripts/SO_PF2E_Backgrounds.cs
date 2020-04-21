using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Background", menuName = "Battlefinder/Pathfinder 2e/Background", order = 0)]
public class SO_PF2E_Backgrounds : ScriptableObject
{
    public string title;
    public E_PF2E_Background background;
    [TextArea(1, 10)] public string description;

    [Space(15)]
    public List<E_PF2E_Ability> abilitiesBoost;
    public List<PF2E_Lecture> lectures;
    public List<SO_PF2E_Feats> skillFeats;
}
