using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ancestry", menuName = "Battlefinder/Pathfinder 2e/Ancestry", order = 0)]
public class SO_PF2E_Ancestries : ScriptableObject
{
    public string title;
    public E_PF2E_Ancestry ancestry;
    [TextArea(1, 10)]
    public string description;
    [Space(15)]
    public int healthPoints;
    public E_PF2E_Size size;

}
