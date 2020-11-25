using UnityEngine;
using System.Collections.Generic;
using YamlTools;
using Pathfinder2e.Containers;

namespace Pathfinder2e
{

    public class DB : MonoBehaviour
    {
        public static DB Instance;

        [SerializeField] private TextAsset t_actions = null;
        [SerializeField] private TextAsset t_traits = null;
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
        [Header("Skills Stuff")]
        [SerializeField] private TextAsset t_skillFeats = null;




        public static List<Action> Actions = new List<Action>();

        public static List<Ancestry> Ancestries = new List<Ancestry>();
        public static List<Feat> AncestryFeatures = new List<Feat>();
        public static AncestryHeritages AncestryHeritages = new AncestryHeritages();
        public static AncestryFeats AncestryFeats = new AncestryFeats();

        public static List<Background> Backgrounds = new List<Background>();

        public static List<Class> Classes = new List<Class>();
        public static ClassFeats ClassFeatures = new ClassFeats();
        public static ClassFeats ClassFeats = new ClassFeats();
        public static List<ClassProgression> ClassProgression = new List<ClassProgression>();

        public static SkillFeats SkillFeats = new SkillFeats();
        public static List<Trait> Traits = new List<Trait>();

        public static List<string> SkillNames = new List<string>() { "acrobatics", "athletics", "crafting", "deception", "diplomacy", "intimidation", "medicine", "nature", "occultism", "performance", "religion", "society", "stealth", "survival", "thievery" };

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
            Actions = YamlConvert.DeserializeObject<List<Action>>(t_actions.text);

            Ancestries = YamlConvert.DeserializeObject<List<Ancestry>>(t_ancestries.text);
            AncestryFeatures = YamlConvert.DeserializeObject<List<Feat>>(t_ancestryFeatures.text);
            AncestryHeritages = YamlConvert.DeserializeObject<AncestryHeritages>(t_ancestryHeritages.text);
            AncestryFeats = YamlConvert.DeserializeObject<AncestryFeats>(t_ancestryFeats.text);

            Backgrounds = YamlConvert.DeserializeObject<List<Background>>(t_backgrounds.text);

            Classes = YamlConvert.DeserializeObject<List<Class>>(t_classes.text);
            ClassFeatures = YamlConvert.DeserializeObject<ClassFeats>(t_classFeatures.text);
            ClassFeats = YamlConvert.DeserializeObject<ClassFeats>(t_classFeats.text);
            ClassProgression = YamlConvert.DeserializeObject<List<ClassProgression>>(t_classAdvancements.text);

            SkillFeats = YamlConvert.DeserializeObject<SkillFeats>(t_skillFeats.text);
            Traits = YamlConvert.DeserializeObject<List<Trait>>(t_traits.text);
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
        }

        // ---------------------------------------------------ABILITIES--------------------------------------------------

        public static string Abl_Abbr2Full(string abilityAbreviated)
        {
            switch (abilityAbreviated)
            {
                case "str": return "Strength";
                case "dex": return "Dexterity";
                case "con": return "Constitution";
                case "int": return "Intelligence";
                case "wis": return "Wisdom";
                case "cha": return "Charisma";
                case "free": return "Free";

                default:
                    Debug.LogWarning("[DB] Error: ability abreviation \"" + abilityAbreviated + "\" not recognized!"); return "";
            }
        }

        public static int Abl_Abbr2Int(string abilityAbreviated)
        {
            switch (abilityAbreviated)
            {
                case "str": return 0;
                case "dex": return 1;
                case "con": return 2;
                case "int": return 3;
                case "wis": return 4;
                case "cha": return 5;
                case "free": return 6;

                default:
                    Debug.LogWarning("[DB] Error: ability abreviation \"" + abilityAbreviated + "\" not recognized!");
                    return 0;
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
                    Debug.LogWarning("[DB] Error: ability int \"" + abilityInt + "\" not recognized!");
                    return "";
            }
        }

        public static string Abl_Full2Abbr(string abilityFullName)
        {
            switch (abilityFullName)
            {
                case "Strength": return "str";
                case "Dexterity": return "dex";
                case "Constitution": return "con";
                case "Intelligence": return "int";
                case "Wisdom": return "wis";
                case "Charisma": return "cha";
                case "Free": return "free";

                default:
                    Debug.LogWarning($"[DB] Ability abreviation \"{abilityFullName}\" not recognized!"); return "";
            }
        }


        // ---------------------------------------------------PROFICIENCIES--------------------------------------------------

        /// <summary>Recieves a string "U", "T", etc, and returns int as 0, 1, etc. </summary>
        public static int Prof_Abbr2Int(string profAbbr)
        {
            switch (profAbbr)
            {
                case "U": return 0;
                case "T": return 1;
                case "E": return 2;
                case "M": return 3;
                case "L": return 4;

                default:
                    Debug.LogWarning($"[DB] Error: proficiency abreviation ({profAbbr}) not recognized!");
                    return 0;
            }
        }

        /// <summary>Recieves a string "untrained", "trained", etc, and returns int as 0, 1, etc. </summary>
        public static int Prof_Full2Int(string profAbbr)
        {
            switch (profAbbr)
            {
                case "untrained": return 0;
                case "trained": return 1;
                case "expert": return 2;
                case "master": return 3;
                case "legendary": return 4;

                default:
                    Debug.LogWarning($"[DB] Error: proficiency abreviation ({profAbbr}) not recognized!");
                    return 0;
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
                    Debug.LogWarning($"[DB] Error: proficiency int ({profInt}) not recognized!");
                    return "U";
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns full name as "Untrained", "Trained", etc. </summary>
        public static string Prof_Abbr2Full(string profAbbr)
        {
            switch (profAbbr)
            {
                case "U": return "Untrained";
                case "T": return "Trained";
                case "E": return "Expert";
                case "M": return "Master";
                case "L": return "Lengend";

                default:
                    Debug.LogWarning($"[DB] Error: proficiency abreviation ({profAbbr}) not recognized!");
                    return "Untrained";
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns colored full name as "Untrained", "Trained", etc. </summary>
        public static string Prof_Abbr2FullColored(string profAbbr)
        {
            switch (profAbbr)
            {
                case "U": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>Untrained</color>";
                case "T": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"])}>Trained</color>";
                case "E": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"])}>Expert</color>";
                case "M": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"])}>Master</color>";
                case "L": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"])}>Leyend</color>";

                default:
                    Debug.LogWarning($"[DB] Error: proficiency abreviation ({profAbbr}) not recognized!");
                    return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>Untrained</color>";
            }
        }

        /// <summary>Recieves a string "U", "T", etc, and returns colored abbr as "U", "T", etc. </summary>
        public static string Prof_Abbr2AbbrColored(string profAbbr)
        {
            switch (profAbbr)
            {
                case "U": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
                case "T": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"])}>T</color>";
                case "E": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"])}>E</color>";
                case "M": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"])}>M</color>";
                case "L": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"])}>L</color>";

                default:
                    Debug.LogWarning($"[DB] Error: proficiency abreviation ({profAbbr}) not recognized!");
                    return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
            }
        }

        /// <summary>Recieves a string "untrained", "trained", etc, and returns colored abbr as "U", "T", etc. </summary>
        public static string Prof_Full2AbbrColored(string profAbbr)
        {
            switch (profAbbr)
            {
                case "untrained": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
                case "trained": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["trained"])}>T</color>";
                case "expert": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["expert"])}>E</color>";
                case "master": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["master"])}>M</color>";
                case "legendary": return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["leyend"])}>L</color>";

                default:
                    Debug.LogWarning($"[DB] Error: proficiency abreviation ({profAbbr}) not recognized!");
                    return $"<color=#{ColorUtility.ToHtmlStringRGBA(Globals.Theme["untrained"])}>U</color>";
            }
        }

        private static List<Lecture> LectureFullList2LectureList(List<LectureFull> lecturesFull)
        {
            List<Lecture> list = new List<Lecture>(lecturesFull.Count);
            foreach (var item in lecturesFull)
                list.Add(item as Lecture);
            return list;
        }

        /// <summary>Recieves a lecture list and returns max proficiency as "U", "T", etc. </summary>
        public static string Prof_FindMax(List<LectureFull> lectures)
        {
            int maxProf = 0;

            if (lectures != null)
                foreach (var item in lectures)
                {
                    int currentProf = Prof_Full2Int(item.prof);
                    if (currentProf > maxProf)
                        maxProf = currentProf;
                }
            else
                Debug.LogWarning($"[APIC] Error: provided lecture list was empty!");

            return DB.Prof_Int2Abbr(maxProf);
        }

        /// <summary>Recieves a lecture list and returns colored max proficiency as "U", "T", etc. </summary>
        public static string Prof_FindMaxColored(List<LectureFull> lectures) { return Prof_Abbr2AbbrColored(Prof_FindMax(lectures)); }


        // ---------------------------------------------------SIZES--------------------------------------------------

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

                default:
                    Debug.LogWarning("[DB] Error: size abreviation (" + abbr + ") not recognized!"); return "";
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

                default:
                    Debug.LogWarning("[DB] Error: size abreviation (" + full + ") not recognized!"); return "";
            }
        }

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
                    Debug.LogWarning($"[DB] Action Cost abreviation \"{full}\" not recognized!");
                    return "";
            }
        }

        public static string ToUpperFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ToLowerFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }

        // ---------------------------------------------------SOURCES--------------------------------------------------

        // public static string Src_Abbr2Full(string abbr)
        // {
        //     switch (abbr)
        //     {
        //         sources.

        //         default:
        //             Debug.LogWarning("[DB] Error: size abreviation (" + abbr + ") not recognized!"); return "";
        //     }
        // }
    }

}
