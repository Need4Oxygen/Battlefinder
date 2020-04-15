using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Feat", menuName = "Battlefinder/Pathfinder 2e/Feat", order = 0)]
public class SO_PF2E_Feats : ScriptableObject
{
    public string title;
    public E_PF2E_FeatType feat;
    public List<PF2E_Effect> effects;
}
