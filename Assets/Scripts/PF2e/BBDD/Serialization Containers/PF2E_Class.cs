using System.Collections.Generic;

public class PF2E_Class
{
    public string name;
    public string description;
    public string keyAbility;
    public string keyAbilityAlt;
    public int hitPoints;
    public int freeSkillTrains;
    public string freeSkillTrainsString;
    public string[] classFeats;
    public Dictionary<string, PF2E_Lecture> classSkillsTrains;
    public Dictionary<string, PF2E_Lecture> lectures;
    public Dictionary<string, Dictionary<string, PF2E_BuildItem>> build;
}
