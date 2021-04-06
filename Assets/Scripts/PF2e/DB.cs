using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PF2C = Pathfinder2e.Containers;
using Pathfinder2e.Containers;
using static TYaml.Serialization;

namespace Pathfinder2e
{

    public class DB : MonoBehaviour
    {
        public static DB Instance;

        [SerializeField] private TextAsset t_actions = null;
        [SerializeField] private TextAsset t_traits = null;
        [SerializeField] private TextAsset t_sources = null;
        [Header("Ancestry Stuff")]
        [SerializeField] private TextAsset t_ancestries = null;
        [SerializeField] private TextAsset t_ancestryFeatures = null;
        [SerializeField] private TextAsset t_ancestryHeritages = null;
        [SerializeField] private TextAsset t_ancestryFeats = null;
        [Header("Background Stuff")]
        [SerializeField] private TextAsset t_backgrounds = null;
        [Header("Classes Stuff")]
        [SerializeField] private TextAsset t_classes = null;
        [SerializeField] private TextAsset t_classFeatures = null;
        [SerializeField] private TextAsset t_classFeats = null;
        [SerializeField] private TextAsset t_classAdvancements = null;
        [Header("Dedications")]
        [SerializeField] private TextAsset t_dedicationFeats = null;
        [SerializeField] private TextAsset t_archetypeFeats = null;
        [Header("Skills Stuff")]
        [SerializeField] private TextAsset t_skillFeats = null;


        public static List<PF2C.Action> Actions = new List<PF2C.Action>();

        public static List<Ancestry> Ancestries = new List<Ancestry>();
        public static List<Feat> AncestryFeatures = new List<Feat>();
        public static AncestryHeritages AncestryHeritages = new AncestryHeritages();
        public static AncestryFeats AncestryFeats = new AncestryFeats();

        public static List<Background> Backgrounds = new List<Background>();

        public static List<Class> Classes = new List<Class>();
        public static ClassFeats ClassFeatures = new ClassFeats();
        public static ClassFeats ClassFeats = new ClassFeats();
        public static List<ClassProgression> ClassProgression = new List<ClassProgression>();

        public static List<Feat> Dedications = new List<Feat>();
        public static List<Feat> ArchetypeFeats = new List<Feat>();

        public static SkillFeats SkillFeats = new SkillFeats();
        public static List<Trait> Traits = new List<Trait>();
        public static List<SourceInfo> Sources = new List<SourceInfo>();

        void Awake()
        {
            if (Instance != null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(gameObject);

            InitializeDB();
        }

        public void InitializeDB()
        {
            Actions = Deserialize<List<PF2C.Action>>(t_actions.text);

            Ancestries = Deserialize<List<Ancestry>>(t_ancestries.text);
            AncestryFeatures = Deserialize<List<Feat>>(t_ancestryFeatures.text);
            AncestryHeritages = Deserialize<AncestryHeritages>(t_ancestryHeritages.text);
            AncestryFeats = Deserialize<AncestryFeats>(t_ancestryFeats.text);

            Backgrounds = Deserialize<List<Background>>(t_backgrounds.text);

            Classes = Deserialize<List<Class>>(t_classes.text);
            ClassFeatures = Deserialize<ClassFeats>(t_classFeatures.text);
            ClassFeats = Deserialize<ClassFeats>(t_classFeats.text);
            ClassProgression = Deserialize<List<ClassProgression>>(t_classAdvancements.text);

            Dedications = Deserialize<List<Feat>>(t_dedicationFeats.text);
            ArchetypeFeats = Deserialize<List<Feat>>(t_archetypeFeats.text);

            SkillFeats = Deserialize<SkillFeats>(t_skillFeats.text);
            Traits = Deserialize<List<Trait>>(t_traits.text);
            Sources = Deserialize<List<SourceInfo>>(t_sources.text);
        }

        public static void Clear()
        {
            Actions.Clear();

            Ancestries.Clear();
            AncestryFeatures.Clear();
            AncestryHeritages = null;
            AncestryFeats = null;

            Backgrounds.Clear();

            Classes.Clear();
            ClassFeatures = null;
            ClassFeats = null;
            ClassProgression.Clear();

            SkillFeats = null;
            Traits.Clear();
            Sources.Clear();
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ABILITIES
        public static List<string> Abl_AbbrList = new List<string> { "str", "dex", "con", "int", "wis", "cha" };

        public static string Abl_Abbr2Full(string abbr)
        {
            switch (abbr)
            {
                case "str": return "Strength";
                case "dex": return "Dexterity";
                case "con": return "Constitution";
                case "int": return "Intelligence";
                case "wis": return "Wisdom";
                case "cha": return "Charisma";
                case "free": return "Free";

                default:
                    Logger.LogWarning("DB", $"Ability abbr \"{abbr}\" not recognized!"); return "";
            }
        }

        public static int Abl_Abbr2Int(string abbr)
        {
            switch (abbr)
            {
                case "str": return 0;
                case "dex": return 1;
                case "con": return 2;
                case "int": return 3;
                case "wis": return 4;
                case "cha": return 5;
                case "free": return 6;
                default:
                    Logger.LogWarning("DB", $"Ability abbr \"{abbr}\" not recognized!"); return 0;
            }
        }

        public static string Abl_Int2Abbr(int abilityInt)
        {
            switch (abilityInt)
            {
                case 0: return "str";
                case 1: return "dex";
                case 2: return "con";
                case 3: return "int";
                case 4: return "wis";
                case 5: return "cha";
                case 6: return "free";
                default:
                    Logger.LogWarning("DB", $"Ability int \"{abilityInt}\" not recognized!"); return "";
            }
        }

        public static string Abl_Full2Abbr(string full)
        {
            switch (full)
            {
                case "Strength": return "str";
                case "Dexterity": return "dex";
                case "Constitution": return "con";
                case "Intelligence": return "int";
                case "Wisdom": return "wis";
                case "Charisma": return "cha";
                case "Free": return "free";
                default:
                    Logger.LogWarning("DB", $"Prof full \"{full}\" not recognized!"); return "";
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- PROFICIENCIES

        /// <summary>Recieves a string "U", "T", etc, and returns score as 0, 2, 4, 6 or 8. </summary>
        public static int Prof_Abbr2Score(string abbr)
        {
            switch (abbr)
            {
                case "L": return 8;
                case "M": return 6;
                case "E": return 4;
                case "T": return 2;
                case "U": return 0;
                default:
                    Logger.LogWarning("DB", $"Prof abbr \"{abbr}\" not recognized!"); return 0;
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns int as 0, 1, etc. </summary>
        public static int Prof_Abbr2Int(string abbr)
        {
            switch (abbr)
            {
                case "U": return 0;
                case "T": return 1;
                case "E": return 2;
                case "M": return 3;
                case "L": return 4;
                default:
                    Logger.LogWarning("DB", $"Prof abbr \"{abbr}\" not recognized!"); return 0;
            }
        }

        /// <summary>Recieves a string "untrained", "trained", etc, and returns int as 0, 1, etc. </summary>
        public static int Prof_Full2Int(string full)
        {
            switch (full)
            {
                case "untrained": return 0;
                case "trained": return 1;
                case "expert": return 2;
                case "master": return 3;
                case "legendary": return 4;
                default:
                    Logger.LogWarning("DB", $"Prof full \"{full}\" not recognized!"); return 0;
            }
        }

        /// <summary>Recieves an int 0, 1, etc, and returns string as "U", "T", etc. </summary>
        public static string Prof_Int2Abbr(int profInt)
        {
            switch (profInt)
            {
                case 0: return "U";
                case 1: return "T";
                case 2: return "E";
                case 3: return "M";
                case 4: return "L";
                default:
                    Logger.LogWarning("DB", $"Prof int \"{profInt}\" not recognized!"); return "U";
            }
        }

        /// <summary>Recieves an int 0, 1, etc, and returns string as "U", "T", etc. </summary>
        public static string Prof_Int2Full(int profInt)
        {
            switch (profInt)
            {
                case 0: return "untrained";
                case 1: return "trained";
                case 2: return "expert";
                case 3: return "master";
                case 4: return "legendary";
                default:
                    Logger.LogWarning("DB", $"Prof int \"{profInt}\" not recognized!"); return "untrained";
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns full name as "Untrained", "Trained", etc. </summary>
        public static string Prof_Abbr2Full(string abbr)
        {
            switch (abbr)
            {
                case "U": return "untrained";
                case "T": return "trained";
                case "E": return "expert";
                case "M": return "master";
                case "L": return "lengend";
                default:
                    Logger.LogWarning("DB", $"Prof abbr \"{abbr}\" not recognized!"); return "Untrained";
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns colored full name as "Untrained", "Trained", etc. </summary>
        public static string Prof_Abbr2FullColored(string abbr)
        {
            switch (abbr)
            {
                case "U": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>Untrained</color>";
                case "T": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"])}>Trained</color>";
                case "E": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"])}>Expert</color>";
                case "M": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"])}>Master</color>";
                case "L": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"])}>Leyend</color>";
                default:
                    Logger.LogWarning("DB", $"Prof abbr \"{abbr}\" not recognized!");
                    return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>Untrained</color>";
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns colored abbr as "U", "T", etc. </summary>
        public static string Prof_Abbr2AbbrColored(string abbr)
        {
            switch (abbr)
            {
                case "U": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
                case "T": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"])}>T</color>";
                case "E": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"])}>E</color>";
                case "M": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"])}>M</color>";
                case "L": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"])}>L</color>";
                default:
                    Logger.LogWarning("DB", $"Prof abbr \"{abbr}\" not recognized!");
                    return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
            }
        }

        /// <summary>Recieves a string "untrained", "trained", etc, and returns colored abbr as "U", "T", etc. </summary>
        public static string Prof_Full2Abbr(string full)
        {
            switch (full)
            {
                case "untrained": return "U";
                case "trained": return "T";
                case "expert": return "E";
                case "master": return "M";
                case "legendary": return "L";
                default:
                    Logger.LogWarning("DB", $"Prof full \"{full}\" not recognized!"); return "U";
            }
        }

        /// <summary>Recieves a string "untrained", "trained", etc, and returns colored abbr as "U", "T", etc. </summary>
        public static string Prof_Full2AbbrColored(string abbr)
        {
            switch (abbr)
            {
                case "untrained": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
                case "trained": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"])}>T</color>";
                case "expert": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"])}>E</color>";
                case "master": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"])}>M</color>";
                case "legendary": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"])}>L</color>";
                default:
                    Logger.LogWarning("DB", $"Prof abbr \"{abbr}\" not recognized!");
                    return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
            }
        }

        /// <summary> Recieves a lecture list and returns max proficiency as "U", "T", etc for specified level and level 20. </summary>
        /// <param name="list"> List from where to extract proficiencies. </param>
        /// <param name="playerLevel"> Current level of the player beign checked. </param>
        /// <returns> Returns a Tuple where Item1 is bound to player level, but Item2 isn't. </returns>
        public static (string, string) Prof_FindMax(IEnumerable<RuleElement> list, int playerLevel)
        {
            if (list == null) return ("U", "U");
            if (list.Count() < 1) return ("U", "U");

            int prof = 0;
            int profLvl20 = 0;

            IEnumerable<RuleElement> statics = list.Where(x => x.key == "proficiency_static" || x.key == "skill_static");
            IEnumerable<RuleElement> increases = list.Where(x => x.key == "proficiency_increase" || x.key == "skill_increase");

            // Set max static prof
            foreach (var element in statics)
            {
                if (int.Parse(element.level) > playerLevel) continue;
                int currentProf = Prof_Full2Int(element.proficiency);
                if (currentProf > prof)
                    prof = currentProf;
            }
            foreach (var element in statics)
            {
                int currentProf = Prof_Full2Int(element.proficiency);
                if (currentProf > profLvl20)
                    profLvl20 = currentProf;
            }

            // Add increases
            HashSet<int> incUniq = new HashSet<int>();
            foreach (var element in increases) // For current level
            {
                if (int.Parse(element.level) > playerLevel) continue;
                incUniq.Add(Prof_Full2Int(element.proficiency));
            }
            while (incUniq.Contains(prof + 1))
            {
                prof++;
            }
            if (incUniq.Count > 0) incUniq.Clear();
            foreach (var element in increases) // For level 20
            {
                incUniq.Add(Prof_Full2Int(element.proficiency));
            }
            while (incUniq.Contains(profLvl20 + 1))
            {
                profLvl20++;
            }

            return (DB.Prof_Int2Abbr(prof), DB.Prof_Int2Abbr(profLvl20));
        }

        /// <summary>Recieves a lecture list and returns colored max proficiency as "U", "T", etc. </summary>
        public static string Prof_FindMaxColored(List<RuleElement> elements, int playerLevel) { return Prof_Abbr2AbbrColored(Prof_FindMax(elements, playerLevel).Item1); }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SKILLS
        public static List<string> Skl_FullList = new List<string>() { "acrobatics", "arcana", "athletics", "crafting", "deception", "diplomacy", "intimidation", "medicine", "nature", "occultism", "performance", "religion", "society", "stealth", "survival", "thievery" };

        public static int Skl_Full2Int(string full)
        {
            switch (full)
            {
                case "acrobatics": return 0;
                case "arcana": return 1;
                case "athletics": return 2;
                case "crafting": return 3;
                case "deception": return 4;
                case "diplomacy": return 5;
                case "intimidation": return 6;
                case "medicine": return 7;
                case "nature": return 8;
                case "occultism": return 9;
                case "performance": return 10;
                case "religion": return 11;
                case "society": return 12;
                case "stealth": return 13;
                case "survival": return 14;
                case "thievery": return 15;
                default:
                    Logger.LogWarning("DB", $"Skill full \"{full}\" not recognized!"); return -1;
            }
        }

        public static string Skl_Int2Full(int value)
        {
            switch (value)
            {
                case 0: return "acrobatics";
                case 1: return "arcana";
                case 2: return "athletics";
                case 3: return "crafting";
                case 4: return "deception";
                case 5: return "diplomacy";
                case 6: return "intimidation";
                case 7: return "medicine";
                case 8: return "nature";
                case 9: return "occultism";
                case 10: return "performance";
                case 11: return "religion";
                case 12: return "society";
                case 13: return "stealth";
                case 14: return "survival";
                case 15: return "thievery";
                default:
                    Logger.LogWarning("DB", $"Skill int \"{value}\" not recognized!"); return "";
            }
        }

        public static string Skl_MaxTrainability(int value)
        {
            if (value >= 1 && value <= 6)
                return "expert";
            else if (value >= 7 && value <= 14)
                return "master";
            else if (value >= 15 && value <= 20)
                return "legendary";
            else
                return "trained";
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- SIZES
        public static string Size_Abbr2Full(string abbr)
        {
            switch (abbr)
            {
                case "T": return "Tiny";
                case "S": return "Small";
                case "M": return "Medium";
                case "L": return "Large";
                case "H": return "Huge";
                case "G": return "Gargantuan";
                default: Logger.LogWarning("DB", $"Size abbr \"{abbr}\" not recognized!"); return "";
            }
        }

        public static string Size_Full2Abbr(string full)
        {
            switch (full)
            {
                case "Tiny": return "T";
                case "Small": return "S";
                case "Medium": return "M";
                case "Large": return "L";
                case "Huge": return "H";
                case "Gargantuan": return "G";
                default: Logger.LogWarning("DB", $"Size full \"{full}\" not recognized!"); return "";
            }
        }

        public static string Size_Int2Abbr(int value)
        {
            switch (value)
            {
                case 0: return "T";
                case 1: return "S";
                case 2: return "M";
                case 3: return "L";
                case 4: return "H";
                case 5: return "G";
                default: Logger.LogWarning("DB", $"Size int \"{value}\" not recognized!"); return "";
            }
        }

        public static int Size_Abbr2Int(string abbr)
        {
            switch (abbr)
            {
                case "T": return 0;
                case "S": return 1;
                case "M": return 2;
                case "L": return 3;
                case "H": return 4;
                case "G": return 5;
                default: Logger.LogWarning("DB", $"Size abbr \"{abbr}\" not recognized!"); return 0;
            }
        }

        public static float Size_Abbr2BulkMod(string size)
        {
            string sizeStr = size;
            if (sizeStr != "")
                switch (sizeStr)
                {
                    case "T": return 0.5f;
                    case "S": return 1f;
                    case "M": return 1f;
                    case "L": return 2f;
                    case "H": return 4f;
                    case "G": return 8f;
                    default:
                        Logger.LogWarning("CharacterData", $"Size \"{sizeStr}\" not recognized!");
                        return 1f;
                }
            else
                return 1f;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- ACTIONCOST
        public static string ActionCost_Full2Abbr(string full)
        {
            switch (full)
            {
                case "Free Action": return "F";
                case "Reaction": return "R";
                case "Single Action": return "1";
                case "Two Actions": return "2";
                case "Three Actions": return "3";
                case "Varies": return "V";

                default:
                    Logger.LogWarning("DB", $"Action Cost full \"{full}\" not recognized!"); return "";
            }
        }

    }

}
