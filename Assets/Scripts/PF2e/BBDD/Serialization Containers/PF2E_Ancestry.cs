using System.Collections.Generic;

public class PF2E_Ancestry
{
    public string name;
    public string description;
    public int hitPoints;
    public int speed;
    public string size;
    public Dictionary<string, PF2E_AblModifier> abilityBoosts;
    public Dictionary<string, PF2E_AblModifier> abilityFlaws;
    public string[] languages;
    public Dictionary<string, PF2E_Trait> traits;
    public string[] ancestryFeatures;
    public string[] heritages;
    public string[] ancestryFeats;
}
