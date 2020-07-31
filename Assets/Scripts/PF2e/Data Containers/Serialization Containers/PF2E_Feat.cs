using System.Collections.Generic;

public class PF2E_Feat
{
    public string type;
    public string name;
    public string description;
    public int level;
    public string[] traits;
    public string[] prequisites;
    public string[] actions;
    public string[] spells;
    public string[] feats;
    public Dictionary<string, PF2E_Lecture> lectures;
    public Dictionary<string, PF2E_Effect> effects;
}
