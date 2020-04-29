using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class PF2E_DataBase : MonoBehaviour
{
    public static PF2E_DataBase Instance;

    [SerializeField] private TextAsset actions = null;

    [Header("Ancestry Stuff")]
    [SerializeField] private TextAsset ancestries = null;
    [SerializeField] private TextAsset ancestryFeatures = null;
    [SerializeField] private TextAsset heritages = null;
    [SerializeField] private TextAsset ancestryFeats = null;

    [Header("Background Stuff")]
    [SerializeField] private TextAsset backgrounds = null;

    [Header("Classes Stuff")]
    [SerializeField] private TextAsset classes = null;

    [Header("Feats")]
    [SerializeField] private TextAsset generalFeats = null;
    [SerializeField] private TextAsset skillFeats = null;


    public static Dictionary<string, PF2E_Action> Actions = new Dictionary<string, PF2E_Action>();

    public static Dictionary<string, PF2E_Ancestry> Ancestries = new Dictionary<string, PF2E_Ancestry>();
    public static Dictionary<string, PF2E_Feat> AncestryFeatures = new Dictionary<string, PF2E_Feat>();
    public static Dictionary<string, PF2E_Feat> Heritages = new Dictionary<string, PF2E_Feat>();
    public static Dictionary<string, PF2E_Feat> AncestryFeats = new Dictionary<string, PF2E_Feat>();

    public static Dictionary<string, PF2E_Background> Backgrounds = new Dictionary<string, PF2E_Background>();

    public static Dictionary<string, PF2E_Class> Classes = new Dictionary<string, PF2E_Class>();

    void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);


        Initialize();
    }

    public void Initialize()
    {
        Actions = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Action>>(actions.text);

        Ancestries = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Ancestry>>(ancestries.text);
        AncestryFeatures = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Feat>>(ancestryFeatures.text);
        Heritages = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Feat>>(heritages.text);
        AncestryFeats = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Feat>>(ancestryFeats.text);

        Backgrounds = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Background>>(backgrounds.text);

        Classes = JsonConvert.DeserializeObject<Dictionary<string, PF2E_Class>>(classes.text);
    }

    public void Clear()
    {
        Actions.Clear();

        Ancestries.Clear();
        AncestryFeatures.Clear();
        Heritages.Clear();
        AncestryFeats.Clear();

        Classes.Clear();
    }

    public static string AbilityToFullName(string abilityAbreviated)
    {
        switch (abilityAbreviated)
        {
            case "str":
                return "Strength";
            case "dex":
                return "Dexterity";
            case "con":
                return "Constitution";
            case "int":
                return "Intelligence";
            case "wis":
                return "Wisdom";
            case "cha":
                return "Charisma";
            case "free":
                return "Free";

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: ability abreviation (" + abilityAbreviated + ") not recognized!");
                return "Null";
        }
    }

    public static E_PF2E_Ability AbilityToEnum(string abilityAbreviated)
    {
        switch (abilityAbreviated)
        {
            case "str":
                return E_PF2E_Ability.Strength;
            case "dex":
                return E_PF2E_Ability.Dexterity;
            case "con":
                return E_PF2E_Ability.Constitution;
            case "int":
                return E_PF2E_Ability.Intelligence;
            case "wis":
                return E_PF2E_Ability.Wisdom;
            case "cha":
                return E_PF2E_Ability.Charisma;
            case "free":
                return E_PF2E_Ability.Free;

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: ability abreviation (" + abilityAbreviated + ") not recognized!");
                return E_PF2E_Ability.Free;
        }
    }

    public static string SizeFullName(string sizeAbreviated)
    {
        switch (sizeAbreviated)
        {
            case "T":
                return "Tiny";
            case "S":
                return "Small";
            case "M":
                return "Medium";
            case "L":
                return "Large";
            case "H":
                return "Huge";
            case "G":
                return "Gargantuan";

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: size abreviation (" + sizeAbreviated + ") not recognized!");
                return "Null";
        }
    }

}

public class PF2E_Action
{
    public string type;
    public string name;
    public string[] traits;
    public string cost;
    public string requirement;
    public string trigger;
    public string description;
    public string failure;
}

public class PF2E_Ancestry
{
    public string name;
    public string description;
    public int hitPoints;
    public int speed;
    public string size;
    public string[] abilityBoosts;
    public string[] abilityFlaws;
    public string[] languages;
    public string[] traits;
    public string[] ancestryFeatures;
    public string[] heritages;
    public string[] ancestryFeats;
}

public class PF2E_Background
{
    public string name;
    public string description;
    public string[] abilityBoostsChoice;
    public Dictionary<string, PF2E_Lecture> lectures;
    public string skillFeat;
}

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
    public Dictionary<string, PF2E_BuildItem> build;
}

public class PF2E_BuildItem
{
    public string[] choices;
    public string[] classFeatures;
    public string[] actions;
}

public class PF2E_Feat
{
    public string type;
    public string name;
    public string description;
    public int level;
    public string[] traits;
    public string[] prequisites;
    public string[] actions;
    public string[] feats;
    public Dictionary<string, PF2E_Lecture> lectures;
    public Dictionary<string, PF2E_Effect> effects;
}

public class PF2E_Lecture
{
    public string name;
    public string description;
    public string target;
    public string proficiency;
    public bool canTurnFree;
}

public class PF2E_Effect
{
    public string name;
    public string description;
}
