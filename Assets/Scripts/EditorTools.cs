using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class EditorTools : MonoBehaviour
{

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
        string bgBad = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/backgrounds.yaml";
        string bgGood = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/Trusted Yamls/Background/backgrounds.yaml";

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
            newString += $"  source:{"\n"}";

            foreach (var scr in bg.source)
            {
                newString += $"    - abbr: {scr.abbr}{"\n"}";
                newString += $"      page_start: { scr.page_start}{"\n"}";
                newString += $"      page_stop: { scr.page_stop}{"\n"}";
            }

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
        string aBad = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/actions.yaml";
        string aGood = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/Trusted Yamls/Actions/actions.yaml";
        string cGood = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/Trusted Yamls/Actions/action_categories.yaml";

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
                {
                    newActions += $"    - {trait}{"\n"}";
                }

            newActions += $"  source:{"\n"}";
            foreach (var scr in a.source)
            {
                newActions += $"    - abbr: {scr.abbr}{"\n"}";
                newActions += $"      page_start: { scr.page_start}{"\n"}";
                newActions += $"      page_stop: { scr.page_stop}{"\n"}";
            }

            newActions += "\n";
        }

        string newCategories = "";

        foreach (var c in categories)
        {
            newCategories += $"- name: {c.name}{"\n"}";
            newCategories += $"  descr: {"\""}{c.descr}{"\""}{"\n"}";
            newCategories += $"  source:{"\n"}";

            foreach (var scr in c.source)
            {
                newCategories += $"    - abbr: {scr.abbr}{"\n"}";
                newCategories += $"      page_start: { scr.page_start}{"\n"}";
                newCategories += $"      page_stop: { scr.page_stop}{"\n"}";
            }

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
        string lBad = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/langs.yaml";
        string lGood = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/Trusted Yamls/General/languages.yaml";

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

            newString += $"  source:{"\n"}";
            foreach (var scr in l.source)
            {
                newString += $"    - abbr: {scr.abbr}{"\n"}";
                newString += $"      page_start: { scr.page_start}{"\n"}";
                newString += $"      page_stop: { scr.page_stop}{"\n"}";
            }

            newString += "\n";
        }

        File.WriteAllText(lGood, newString);
    }


}
