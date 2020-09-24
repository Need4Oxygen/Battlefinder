using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class EditorTools : MonoBehaviour
{

    const string path = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/";

    public class Background_Wrapper
    {
        public string name { get; set; }
        public string descr { get; set; }
        public List<string> ability_boost_choices { get; set; }
        public string is_comty_use { get; set; }
        public string is_specific_to_adv { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    [MenuItem("Tools/Update Backgrounds")]
    public static void UpdateBackgrounds()
    {
        string bgBad = $"{path}Pathfinder 2 Sqlite/backgrounds.yaml";
        string bgGood = $"{path}Trusted Yamls/Background/backgrounds.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
        var input = new StringReader(File.ReadAllText(bgBad));
        var backgrounds = deserializer.Deserialize<List<Background_Wrapper>>(input);

        Debug.Log($"[EditorTools] Updating Backgrounds with size: {backgrounds.Count}{"\n"}");

        string newString = "";

        foreach (var bg in backgrounds)
        {
            newString += $"- name: {bg.name}{"\n"}";
            newString += $"  descr: {"\""}{bg.descr}{"\""}{"\n"}";
            newString += $"  ability_boost_choices:{"\n"}";

            foreach (var choice in bg.ability_boost_choices)
            {
                newString += $"    - {PF2E_DataBase.Abl_Full2Abbr(choice)}{"\n"}";
            }

            newString += $"  community_licenced: {bg.is_comty_use}{"\n"}";
            newString += $"  is_specific_to_adv: {bg.is_specific_to_adv}{"\n"}";

            WriteSources(ref newString, bg.source);

            newString += "\n";
        }

        File.WriteAllText(bgGood, newString);
    }

    public class ActionFile_Wrapper
    {
        public List<Actions_Wrapper> action { get; set; }
        public List<ActionCategory_Wrapper> actioncategory { get; set; }
    }

    public class Actions_Wrapper
    {
        public string name { get; set; }
        public string descr { get; set; }
        public string actioncategory { get; set; }
        public string actioncost_name { get; set; }
        public string trigger { get; set; }
        public string req { get; set; }
        public List<string> trait { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    public class ActionCategory_Wrapper
    {
        public string name { get; set; }
        public string descr { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    [MenuItem("Tools/Update Actions")]
    public static void UpdateActions()
    {
        string aBad = $"{path}Pathfinder 2 Sqlite/actions.yaml";
        string aGood = $"{path}Trusted Yamls/Actions/actions.yaml";
        string cGood = $"{path}Trusted Yamls/Actions/action_categories.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
        var input = new StringReader(File.ReadAllText(aBad));
        var yaml = deserializer.Deserialize<ActionFile_Wrapper>(input);
        List<Actions_Wrapper> actions = yaml.action;
        List<ActionCategory_Wrapper> categories = yaml.actioncategory;

        Debug.Log($"[EditorTools] Updating Actions with size: {actions.Count}{"\n"}");
        Debug.Log($"[EditorTools] Updating Actions Categories with size: {categories.Count}{"\n"}");

        string newActions = "";

        foreach (var a in actions)
        {
            newActions += $"- name: {a.name}{"\n"}";
            newActions += $"  descr: {"\""}{a.descr}{"\""}{"\n"}";
            newActions += $"  action_category: {a.actioncategory}{"\n"}";
            newActions += $"  actioncost: {PF2E_DataBase.ActionCost_Full2Abbr(a.actioncost_name)}{"\n"}";
            newActions += $"  trigger: {a.trigger}{"\n"}";
            newActions += $"  requirement: {a.req}{"\n"}";

            newActions += $"  traits:{"\n"}";
            if (a.trait != null)
                foreach (var trait in a.trait)
                    newActions += $"    - {trait}{"\n"}";

            WriteSources(ref newActions, a.source);

            newActions += "\n";
        }

        string newCategories = "";

        foreach (var c in categories)
        {
            newCategories += $"- name: {c.name}{"\n"}";
            newCategories += $"  descr: {"\""}{c.descr}{"\""}{"\n"}";

            WriteSources(ref newCategories, c.source);

            newCategories += "\n";
        }

        File.WriteAllText(aGood, newActions);
        File.WriteAllText(cGood, newCategories);
    }

    public class Language_Wrapper
    {
        public string name { get; set; }
        public string rarity { get; set; }
        public string speakers { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    [MenuItem("Tools/Update Languages")]
    public static void UpdateLanguages()
    {
        string lBad = $"{path}Pathfinder 2 Sqlite/langs.yaml";
        string lGood = $"{path}Trusted Yamls/General/languages.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
        var input = new StringReader(File.ReadAllText(lBad));
        var languages = deserializer.Deserialize<List<Language_Wrapper>>(input);

        Debug.Log($"[EditorTools] Updating Languages with size: {languages.Count}{"\n"}");

        string newString = "";

        foreach (var l in languages)
        {
            newString += $"- name: {l.name}{"\n"}";
            newString += $"  rarity: {l.rarity}{"\n"}";
            newString += $"  speakers: {l.speakers}{"\n"}";

            WriteSources(ref newString, l.source);

            newString += "\n";
        }

        File.WriteAllText(lGood, newString);
    }

    public class Senses_Wrapper
    {
        public string name { get; set; }
        public string descr { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    [MenuItem("Tools/Update Senses")]
    public static void UpdateSenses()
    {
        string sBad = $"{path}Pathfinder 2 Sqlite/senses.yaml";
        string sGood = $"{path}Trusted Yamls/General/senses.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
        var input = new StringReader(File.ReadAllText(sBad));
        var senses = deserializer.Deserialize<List<Senses_Wrapper>>(input);

        Debug.Log($"[EditorTools] Updating Senses with size: {senses.Count}{"\n"}");

        string newString = "";

        foreach (var s in senses)
        {
            newString += $"- name: {s.name}{"\n"}";
            newString += $"  descr: {"\""}{s.descr}{"\""}{"\n"}";

            WriteSources(ref newString, s.source);

            newString += "\n";
        }

        File.WriteAllText(sGood, newString);
    }

    public class TraitsFile_Wrapper
    {
        public List<Traits_Wrapper> trait { get; set; }
        public List<string> traittype { get; set; }
    }

    public class Traits_Wrapper
    {
        public string name { get; set; }
        public string descr { get; set; }
        public string type { get; set; }
    }

    [MenuItem("Tools/Update Traits")]
    public static void UpdateTraits()
    {
        string tBad = $"{path}Pathfinder 2 Sqlite/traits.yaml";
        string tGood = $"{path}Trusted Yamls/General/traits.yaml";
        string cGood = $"{path}Trusted Yamls/General/traits_categories.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
        var input = new StringReader(File.ReadAllText(tBad));
        var yaml = deserializer.Deserialize<TraitsFile_Wrapper>(input);
        List<Traits_Wrapper> traits = yaml.trait;
        List<string> categories = yaml.traittype;

        Debug.Log($"[EditorTools] Updating Traits with size: {traits.Count}{"\n"}");
        Debug.Log($"[EditorTools] Updating Traits Categories with size: {categories.Count}{"\n"}");

        string newTraits = "";

        foreach (var t in traits)
        {
            newTraits += $"- name: {t.name}{"\n"}";
            newTraits += $"  descr: {"\""}{t.descr}{"\""}{"\n"}";
            newTraits += $"  type: {t.type}{"\n"}";

            newTraits += "\n";
        }

        string newCategories = "";

        foreach (var c in categories)
        {
            newCategories += $"- {c}{"\n"}";
        }

        File.WriteAllText(tGood, newTraits);
        File.WriteAllText(cGood, newCategories);
    }

    public class ClassesFile_Wrapper
    {
        public List<Lecture_Wrapper> attacks { get; set; }
        public List<Lecture_Wrapper> class_dc_and_spells { get; set; }
        public List<Lecture_Wrapper> defenses { get; set; }
        public int hp { get; set; }
        public List<string> key_ability_choices { get; set; }
        public string name { get; set; }
        public string perception { get; set; }
        public List<Lecture_Wrapper> saving_throws { get; set; }
        public List<Lecture_Wrapper> skills { get; set; }
    }

    [MenuItem("Tools/Update Classes")]
    public static void UpdateClassess()
    {
        string tBad = $"{path}Pathfinder 2 Sqlite/classes.yaml";
        string cGood = $"{path}Trusted Yamls/Classes/classes.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).IgnoreUnmatchedProperties().Build();
        var input = new StringReader(File.ReadAllText(tBad));
        var classes = deserializer.Deserialize<List<ClassesFile_Wrapper>>(input);

        Debug.Log($"[EditorTools] Updating Classes with size: {classes.Count}{"\n"}");

        string newString = "";

        foreach (var c in classes)
        {
            newString += $"- name: {c.name}{"\n"}";
            newString += $"  descr: null{"\n"}";
            newString += $"  hp: {c.hp}{"\n"}";
            newString += $"  key_ability_choices:{"\n"}";
            foreach (var abl in c.key_ability_choices)
                newString += $"    - {abl}{"\n"}";

            Lecture_Wrapper perception = new Lecture_Wrapper();
            perception.degree = c.perception;
            perception.type = "perception";
            List<Lecture_Wrapper> perceptionList = new List<Lecture_Wrapper> { perception };
            WriteLectures(ref newString, "perception", perceptionList);
            WriteLectures(ref newString, "saves", c.saving_throws);
            WriteLectures(ref newString, "attacks", c.attacks);
            WriteLectures(ref newString, "defenses", c.defenses);
            WriteLectures(ref newString, "skills", c.skills);
            WriteLectures(ref newString, "class_dc_and_spells", c.class_dc_and_spells);

            PF2E_Source source = new PF2E_Source();
            source.abbr = "CRB";
            source.page_start = 0;
            source.page_stop = 0;
            List<PF2E_Source> sourceList = new List<PF2E_Source> { source };
            WriteSources(ref newString, sourceList);

            newString += "\n";
        }

        File.WriteAllText(cGood, newString);
    }

    public class ConditionsFile_Wrapper
    {
        public string name { get; set; }
        public string descr { get; set; }
        public string short_descr { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    [MenuItem("Tools/Update Conditions")]
    public static void UpdateConditions()
    {
        string tBad = $"{path}Pathfinder 2 Sqlite/conditions.yaml";
        string cGood = $"{path}Trusted Yamls/General/conditions.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).IgnoreUnmatchedProperties().Build();
        var input = new StringReader(File.ReadAllText(tBad));
        var conditions = deserializer.Deserialize<List<ConditionsFile_Wrapper>>(input);

        Debug.Log($"[EditorTools] Updating Conditions with size: {conditions.Count}{"\n"}");

        string newString = "";

        foreach (var c in conditions)
        {
            newString += $"- name: {c.name}{"\n"}";
            newString += $"  descr: { "\""}{c.descr}{ "\""}{"\n"}";
            newString += $"  short_descr: { "\""}{c.short_descr}{ "\""}{"\n"}";

            PF2E_Source source = new PF2E_Source();
            source.abbr = "CRB";
            source.page_start = 0;
            source.page_stop = 0;
            List<PF2E_Source> sourceList = new List<PF2E_Source> { source };
            WriteSources(ref newString, sourceList);

            newString += "\n";
        }

        File.WriteAllText(cGood, newString);
    }





    // General Stuff

    static void WriteSources(ref string mainString, List<PF2E_Source> source)
    { WriteSources(ref mainString, 0, source); }
    static void WriteSources(ref string mainString, int indent, List<PF2E_Source> source)
    {
        string i = "  ";
        for (int j = 0; j < indent; j++)
            i += "  ";

        mainString += $"{i}source:{"\n"}";
        foreach (var scr in source)
        {
            mainString += $"{i}  - abbr: {scr.abbr}{"\n"}";
            mainString += $"{i}    page_start: { scr.page_start}{"\n"}";
            mainString += $"{i}    page_stop: { scr.page_stop}{"\n"}";
        }
    }

    public class Lecture_Wrapper
    {
        public string degree { get; set; }
        public string type { get; set; }
    }

    static void WriteLectures(ref string mainString, string propertyName, List<Lecture_Wrapper> lecture)
    { WriteLectures(ref mainString, propertyName, 0, lecture); }
    static void WriteLectures(ref string mainString, string propertyName, int indent, List<Lecture_Wrapper> lecture)
    {
        string i = "  ";
        for (int j = 0; j < indent; j++)
            i += "  ";

        mainString += $"{i}{propertyName}:{"\n"}";
        foreach (var l in lecture)
        {
            mainString += $"{i}  - target: {l.type.ToLowerInvariant()}{"\n"}";
            mainString += $"{i}    prof: {l.degree.ToLowerInvariant()}{"\n"}";
        }
    }

}
