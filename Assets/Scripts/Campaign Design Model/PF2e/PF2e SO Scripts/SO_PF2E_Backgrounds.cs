using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Background", menuName = "Battlefinder/Pathfinder 2e/Background", order = 0)]
public class SO_PF2E_Backgrounds : ScriptableObject
{
    public string title;
    public E_PF2E_Background background;
    [TextArea(1, 10)]
    public string description;
    public List<E_PF2E_Ability> abilitiesBoost;               // STR or DEX + Free
    public List<E_PF2E_Skill> trainedSkills;                  // Trained in Religion
    public List<string> trainedLores;                         // Trained in some Lore
    public List<SO_PF2E_Feats> skillFeats;                    // Gain the Student of the Canon skill feat
}