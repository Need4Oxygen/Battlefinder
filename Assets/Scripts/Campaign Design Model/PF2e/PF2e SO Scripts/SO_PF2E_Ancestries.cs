using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ancestry", menuName = "Battlefinder/Pathfinder 2e/Ancestry", order = 0)]
public class SO_PF2E_Ancestries : ScriptableObject
{
    public string title;
    public E_PF2E_Ancestry ancestry;
    [TextArea(1, 10)] public string description;

    [Space(15)]
    public int healthPoints;
    public E_PF2E_Size size;
    public int speed;
    public List<E_PF2E_Ability> abilityBoosts;
    public List<E_PF2E_Ability> abilityFlaw;
    public List<E_PF2E_Languages> languages;
    public List<E_PF2E_CharacterTraits> traits;
    public List<SO_PF2E_Feats> heritages;
    public List<SO_PF2E_Feats> ancestryFeatures;
    public List<SO_PF2E_Feats> ancestryFeats;
}
