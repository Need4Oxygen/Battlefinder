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

    public static PrequisitesReport Check_Feat(Feat feat, ref CharacterData player)
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

    private static bool ValidatePrereq(Prerequisite prereq, ref CharacterData player)
    {
        switch (prereq.type)
        {
            case "proficiency":
                return Validate_Proficiency(prereq.descr, ref player);
            case "ability":
                return Validate_Ability(prereq.descr, ref player);


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
                Debug.LogWarning($"[PrereqChecker] Prerequisite type \"{prereq.type}\" not implemented!");
                return false;
            case "bloodline":
                Debug.LogWarning($"[PrereqChecker] Prerequisite type \"{prereq.type}\" not implemented!");
                return false;
            case "rogue’s racket":
                Debug.LogWarning($"[PrereqChecker] Prerequisite type \"{prereq.type}\" not implemented!");
                return false;


            case "special":
                Debug.LogWarning($"[PrereqChecker] Prerequisite type \"{prereq.type}\" not implemented!");
                return false;


            default:
                Debug.LogWarning($"[PrereqChecker] Prerequisite type \"{prereq.type}\" not recognized!");
                return false;
        }
    }

    private static bool Validate_Ability(string descr, ref CharacterData playerData)
    {
        string[] split = descr.Split(' ');
        return playerData.Abl_GetScore(StrTools.ToLowerFirst(split[0])) >= int.Parse(split[1]) ? true : false;
    }

    private static bool Validate_Proficiency(string descr, ref CharacterData playerData)
    {
        string[] split = descr.Split(' ');
        if (split.Length < 3) return false;

        if (split.Length == 3) // For "expert in stealth" type of string
        {
            int maxProf = DB.Prof_Full2Int(split[0]);
            string item = StrTools.ToLowerFirst(split[2]);

            switch (item)
            {
                case "perception":
                    return DB.Prof_Abbr2Int(playerData.perception_prof) >= maxProf ? true : false;
                case "fortitude":
                    return DB.Prof_Abbr2Int(playerData.Saves_Get("fortitude").prof) >= maxProf ? true : false;
                case "reflex":
                    return DB.Prof_Abbr2Int(playerData.Saves_Get("reflex").prof) >= maxProf ? true : false;
                case "wisdom":
                    return DB.Prof_Abbr2Int(playerData.Saves_Get("wisdom").prof) >= maxProf ? true : false;
                default:
                    return DB.Prof_Abbr2Int(playerData.Skills_Get(item).prof) >= maxProf ? true : false;
            }
        }
        else // Exceptions
        {
            switch (descr)
            {
                case "trained in Arcana, Nature, or Religion":
                    if (DB.Prof_Abbr2Int(playerData.Skills_Get("arcana").prof) > 1 ||
                        DB.Prof_Abbr2Int(playerData.Skills_Get("nature").prof) > 1 ||
                        DB.Prof_Abbr2Int(playerData.Skills_Get("religion").prof) > 1)
                        return true;
                    else
                        return false;

                case "expert in your deity’s favored weapon":
                    Debug.LogWarning($"[PrereqChecker] Prerequisite \"expert in your deity’s favored weapon\" not implemented!");
                    return false;

                default:
                    return false;
            }
        }
    }

    private static bool Validate_Heritage(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("heritage").Contains(feat); ;
    }

    private static bool Validate_AncestryFeat(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("ancestry feat").Contains(feat); ;
    }

    private static bool Validate_AncestryFeature(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("ancestry feature").Contains(feat); ;
    }

    private static bool Validate_ClassFeat(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("class feat").Contains(feat); ;
    }

    private static bool Validate_ClassFeature(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("class feature").Contains(feat); ;
    }

    private static bool Validate_GeneralFeat(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("general feat").Contains(feat); ;
    }

    private static bool Validate_SkillFeat(string feat, ref CharacterData playerData)
    {
        return playerData.Build_GetFeatNames("skill feat").Contains(feat); ;
    }

}
