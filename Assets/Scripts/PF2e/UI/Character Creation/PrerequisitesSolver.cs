using System.Collections;
using System.Collections.Generic;
using Pathfinder2e;
using Pathfinder2e.Containers;
using Pathfinder2e.Character;
using Tools;
using UnityEngine;

public class PrequisitesReport
{
    public bool isValidated = true;
    public List<int> errorList = new List<int>();

    public PrequisitesReport() { }
}

public static class PrerequisitesSolver
{

    public static PrequisitesReport Check_Feat(Feat feat, ref Character player)
    {
        PrequisitesReport report = new PrequisitesReport();

        for (int i = 0; i < feat.prerequisites.Count; i++)
            if (!ValidatePrereq(feat.prerequisites[i], ref player))
            {
                report.errorList.Add(i);
                report.isValidated = false;
            }

        return report;
    }

    private static bool ValidatePrereq(Prerequisite prereq, ref Character player)
    {
        switch (prereq.type)
        {
            case "proficiency":
                return Validate_Proficiency(ref player, prereq.descr);
            case "ability":
                return Validate_Ability(ref player, prereq.descr);


            case "heritage":
                return Validate_Heritage(prereq.descr, ref player);
            case "ancestry feat":
                return Validate_AncestryFeat(prereq.descr, ref player);
            case "ancestry feature":
                return Validate_AncestryFeature(prereq.descr, ref player);


            case "class feat":
                return Validate_ClassFeat(prereq.descr, ref player);
            case "class feature":
                return Validate_ClassFeature(prereq.descr, ref player);


            case "general feat":
                return Validate_GeneralFeat(prereq.descr, ref player);
            case "skill feat":
                return Validate_SkillFeat(prereq.descr, ref player);


            case "familiar":
                Logger.Log("PrereqChecker", $"Prerequisite type \"{prereq.type}\" not implemented!");
                return false;
            case "bloodline":
                Logger.Log("PrereqChecker", $"Prerequisite type \"{prereq.type}\" not implemented!");
                return false;
            case "rogue’s racket":
                Logger.Log("PrereqChecker", $"Prerequisite type \"{prereq.type}\" not implemented!");
                return false;


            case "special":
                Logger.Log("PrereqChecker", $"Prerequisite type \"{prereq.type}\" not implemented!");
                return false;


            default:
                Logger.Log("PrereqChecker", $"Prerequisite type \"{prereq.type}\" not recognized!");
                return false;
        }
    }

    private static bool Validate_Ability(ref Character playerData, string descr)
    {
        string[] split = descr.Split(' ');
        return playerData.Abl_GetScore(split[0].ToLowerFirst()) >= split[1].ToInt() ? true : false;
    }

    private static bool Validate_Proficiency(ref Character playerData, string profStr)
    {
        string[] split = profStr.Split(' ');
        if (split.Length < 3) return false;

        if (split.Length == 3) // For "expert in stealth" type of string
        {
            int maxProf = DB.Prof_Full2Int(split[0]);
            string item = split[2].ToLowerFirst();

            switch (item)
            {
                case "perception":
                    return DB.Prof_Abbr2Int(playerData.data.perception.prof) >= maxProf ? true : false;
                case "fortitude":
                    return DB.Prof_Abbr2Int(playerData.data.fortitude.prof) >= maxProf ? true : false;
                case "reflex":
                    return DB.Prof_Abbr2Int(playerData.data.reflex.prof) >= maxProf ? true : false;
                case "will":
                    return DB.Prof_Abbr2Int(playerData.data.will.prof) >= maxProf ? true : false;
                default:
                    return DB.Prof_Abbr2Int(playerData.Skill_Get(item).prof) >= maxProf ? true : false;
            }
        }
        else // Exceptions
        {
            switch (profStr)
            {
                case "trained in Arcana, Nature, or Religion": // Ancestry Herigage - Seer Elf
                    if (DB.Prof_Abbr2Int(playerData.Skill_Get("arcana").prof) > 1 ||
                        DB.Prof_Abbr2Int(playerData.Skill_Get("nature").prof) > 1 ||
                        DB.Prof_Abbr2Int(playerData.Skill_Get("religion").prof) > 1)
                        return true;
                    else
                        return false;

                // case "expert in your deity’s favored weapon":
                //     Debug.LogWarning($"<color=#a589e0>[PrereqChecker]</color> Prerequisite \"expert in your deity’s favored weapon\" not implemented!");
                //     return false;

                default:
                    Logger.Log("PrereqChecker", $"Prerequisite \"{profStr}\" not implemented!");
                    return false;
            }
        }
    }

    private static bool Validate_Heritage(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("heritage").Contains(feat);
    }

    private static bool Validate_AncestryFeat(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("ancestry feat").Contains(feat);
    }

    private static bool Validate_AncestryFeature(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("ancestry feature").Contains(feat);
    }

    private static bool Validate_ClassFeat(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("class feat").Contains(feat);
    }

    private static bool Validate_ClassFeature(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("class feature").Contains(feat);
    }

    private static bool Validate_GeneralFeat(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("general feat").Contains(feat);
    }

    private static bool Validate_SkillFeat(string feat, ref Character playerData)
    {
        return false;
        // return playerData.Build_GetFeatNames("skill feat").Contains(feat);
    }

}
