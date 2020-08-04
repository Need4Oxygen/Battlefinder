using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class EditorTools : MonoBehaviour
{

    public class Bg
    {
        public string name { get; set; }
        public string descr { get; set; }
        public List<string> ability_boost_choices { get; set; }
        public string is_comty_use { get; set; }
        public string is_specific_to_adv { get; set; }
        public List<PF2E_Source> source { get; set; }
    }

    [MenuItem("Tools/ConvertBackgrounds")]
    public static void ConvertBackgrounds()
    {
        string bgBad = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/backgrounds.yaml";
        string bgGood = "C:/Repos/Battlefinder/Assets/Scripts/PF2e/BBDD/YAMLs/Trusted Yamls/Background/backgrounds.yaml";

        var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
        var input = new StringReader(File.ReadAllText(bgBad));
        var backgrounds = deserializer.Deserialize<List<Bg>>(input);

        Debug.Log(backgrounds.Count);

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

}
