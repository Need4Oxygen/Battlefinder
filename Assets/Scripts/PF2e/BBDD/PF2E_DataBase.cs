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
    // Class features
    // Class feats

    // [Header("Feats")]
    // [SerializeField] private TextAsset generalFeats = null;
    // [SerializeField] private TextAsset skillFeats = null;


    public static Dictionary<string, PF2E_Action> Actions = new Dictionary<string, PF2E_Action>();

    public static Dictionary<string, PF2E_Ancestry> Ancestries = new Dictionary<string, PF2E_Ancestry>();
    public static Dictionary<string, PF2E_Feat> AncestryFeatures = new Dictionary<string, PF2E_Feat>();
    public static Dictionary<string, PF2E_Feat> Heritages = new Dictionary<string, PF2E_Feat>();
    public static Dictionary<string, PF2E_Feat> AncestryFeats = new Dictionary<string, PF2E_Feat>();

    public static Dictionary<string, PF2E_Background> Backgrounds = new Dictionary<string, PF2E_Background>();

    public static Dictionary<string, PF2E_Class> Classes = new Dictionary<string, PF2E_Class>();

    public static Dictionary<string, PF2E_Armor> Armors = new Dictionary<string, PF2E_Armor>();
    public static Dictionary<string, PF2E_ArmorGroup> ArmorGroups = new Dictionary<string, PF2E_ArmorGroup>();

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

    public static string Abl_Abbr2Full(string abilityAbreviated)
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
                Debug.LogWarning("[PF2E_DataBase] Error: ability abreviation \"" + abilityAbreviated + "\" not recognized!");
                return "";
        }
    }

    public static string Abl_Full2Abbr(string abilityFullName)
    {
        switch (abilityFullName)
        {
            case "Strength":
                return "str";
            case "Dexterity":
                return "dex";
            case "Constitution":
                return "con";
            case "Intelligence":
                return "int";
            case "Wisdom":
                return "wis";
            case "Charisma":
                return "cha";
            case "Free":
                return "free";

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: ability abreviation (" + abilityFullName + ") not recognized!");
                return "";
        }
    }

    public static E_PF2E_Ability Abl_Abbr2Enum(string abilityAbreviated)
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

    /// <summary>Recieves a string "U", "T", "E", "M", "L" and returns corresponding enumerator. </summary>
    public static E_PF2E_Proficiency Prof_Abbr2Enum(string profAbbr)
    {
        switch (profAbbr)
        {
            case "U":
                return E_PF2E_Proficiency.Untrained;
            case "T":
                return E_PF2E_Proficiency.Trained;
            case "E":
                return E_PF2E_Proficiency.Expert;
            case "M":
                return E_PF2E_Proficiency.Master;
            case "L":
                return E_PF2E_Proficiency.Lengend;

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: poroficiency abreviation (" + profAbbr + ") not recognized!");
                return E_PF2E_Proficiency.Untrained;
        }
    }

    /// <summary>Recieves a proficiency enumerator and returns corresponding colored string. </summary>
    public static string Prof_Enum2ColoredAbbr(E_PF2E_Proficiency proficiency)
    {
        switch (proficiency)
        {
            case E_PF2E_Proficiency.Untrained:
                return ("<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"]) + ">U</color>");
            case E_PF2E_Proficiency.Trained:
                return ("<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"]) + ">T</color>");
            case E_PF2E_Proficiency.Expert:
                return ("<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"]) + ">E</color>");
            case E_PF2E_Proficiency.Master:
                return ("<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"]) + ">M</color>");
            case E_PF2E_Proficiency.Lengend:
                return ("<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"]) + ">L</color>");

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: poroficiency enum (" + proficiency + ") not recognized!");
                return ("<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"]) + ">T</color>");
        }
    }

    public static E_PF2E_Proficiency Prof_FindMax(List<PF2E_Lecture> lectures)
    {
        E_PF2E_Proficiency maxProf = E_PF2E_Proficiency.Untrained;

        if (lectures != null)
        {
            foreach (var item in lectures)
                if (item.proficiency == "T")
                    maxProf = E_PF2E_Proficiency.Trained;
                else if (item.proficiency == "E")
                    maxProf = E_PF2E_Proficiency.Expert;
                else if (item.proficiency == "M")
                    maxProf = E_PF2E_Proficiency.Master;
                else if (item.proficiency == "L")
                    maxProf = E_PF2E_Proficiency.Lengend;
        }

        return maxProf;
    }

    public static string Prof_FindMaxColoredAbbr(List<PF2E_Lecture> lectures)
    {
        string maxProf = "<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"]) + ">U</color>";

        if (lectures != null)
        {
            foreach (var item in lectures)
                if (item.proficiency == "T")
                    maxProf = "<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"]) + ">T</color>";
                else if (item.proficiency == "E")
                    maxProf = "<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"]) + ">E</color>";
                else if (item.proficiency == "M")
                    maxProf = "<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"]) + ">M</color>";
                else if (item.proficiency == "L")
                    maxProf = "<color=#" + ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"]) + ">L</color>";
        }

        return maxProf;
    }

    public static string Size_Abbr2Full(string abbr)
    {
        switch (abbr)
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
                Debug.LogWarning("[PF2E_DataBase] Error: size abreviation (" + abbr + ") not recognized!");
                return "";
        }
    }

    public static string Size_Full2Abbr(string full)
    {
        switch (full)
        {
            case "Tiny":
                return "T";
            case "Small":
                return "S";
            case "Medium":
                return "M";
            case "Large":
                return "L";
            case "Huge":
                return "H";
            case "Gargantuan":
                return "G";

            default:
                Debug.LogWarning("[PF2E_DataBase] Error: size abreviation (" + full + ") not recognized!");
                return "";
        }
    }

}
