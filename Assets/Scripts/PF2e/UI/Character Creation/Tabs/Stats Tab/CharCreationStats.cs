using System.Collections.Generic;
using System.Linq;
using Pathfinder2e.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinder2e.Character
{

    public class CharCreationStats : MonoBehaviour
    {
        [System.Serializable] private class ListWrapper { public List<Image> myList = null; }

        [SerializeField] private Window statsWindow = null;
        [SerializeField] private CharacterCreation creation = null;

        [Header("Health")]
        [SerializeField] private TMP_Text HPCurrentText = null;
        [SerializeField] private TMP_Text HPMaxtText = null;
        [SerializeField] private TMP_Text HPTempText = null;
        [SerializeField] private TMP_Text DyingMaxtText = null;
        [SerializeField] private TMP_InputField damageInput = null;
        [SerializeField] private TMP_InputField dyingInput = null;
        [SerializeField] private TMP_InputField woundsInput = null;
        [SerializeField] private TMP_InputField doomInput = null;

        [Header("AC")]
        [SerializeField] private APICButton armorAPIC = null;
        [SerializeField] private TMP_Text shieldHealth = null;
        [SerializeField] private TMP_Text shieldHard = null;
        [SerializeField] private TMP_InputField shieldDamage = null;
        [SerializeField] private TMP_Text shieldBonusAC = null;

        [Header("Perception & Saves")]
        [SerializeField] private APICButton perceptionAPIC = null;
        [SerializeField] private APICButton fortitudeAPIC = null;
        [SerializeField] private APICButton reflexAPIC = null;
        [SerializeField] private APICButton willAPIC = null;

        [Header("Speeds")]
        [SerializeField] private TMP_Text speedBaseText = null;
        [SerializeField] private TMP_Text speedFlyText = null;
        [SerializeField] private TMP_Text speedSwimText = null;
        [SerializeField] private TMP_Text speedClimbText = null;
        [SerializeField] private TMP_Text speedBurrowText = null;

        [Header("Weapon & Armor")]
        [SerializeField] private TMP_Text unarmoredText = null;
        [SerializeField] private TMP_Text lightArmorText = null;
        [SerializeField] private TMP_Text mediumArmorText = null;
        [SerializeField] private TMP_Text heavyArmorText = null;

        [SerializeField] private TMP_Text unarmedText = null;
        [SerializeField] private TMP_Text simpleWeaponText = null;
        [SerializeField] private TMP_Text martialWeaponText = null;
        [SerializeField] private TMP_Text advancedWeaponText = null;

        [Header("Random")]
        [SerializeField] private TMP_Text sizeText = null;
        [SerializeField] private TMP_Text bulkText = null;
        [SerializeField] private TMP_Text classDCText = null;
        [SerializeField] private TMP_Text traitsText = null;
        [SerializeField] private TMP_InputField languagesInput = null;
        [SerializeField] private TMP_InputField heroPointsInput = null;
        [SerializeField] private TMP_InputField wealthInput = null;
        [SerializeField] private List<APICButton> skills = null;
        [SerializeField] private List<ListWrapper> ablMap = null;
        [SerializeField] private List<TMP_Text> ablMapScores = null;

        [HideInInspector] public bool isOpen = false;

        #region --------------------------------------------GENERAL--------------------------------------------

        public void OpenStatsPanel()
        {
            isOpen = true;
            statsWindow.OpenWindow();
            RefreshPlayerIntoPanel();
        }

        public void CloseStatsPanel()
        {
            isOpen = false;
            statsWindow.CloseWindow();
        }

        #endregion

        #region --------------------------------------------PLAYERS--------------------------------------------

        /// <summary> Refresh UI with player data. </summary>
        public void RefreshPlayerIntoPanel()
        {
            HPCurrentText.text = creation.currentPlayer.hp_current.ToString();
            HPMaxtText.text = creation.currentPlayer.hp_max.ToString();
            HPTempText.text = creation.currentPlayer.hp_temp.ToString();
            DyingMaxtText.text = creation.currentPlayer.hp_dyingMax.ToString();

            // AC
            armorAPIC.Refresh(creation.currentPlayer.ac);
            shieldHealth.text = "0/0";
            shieldHard.text = "Hard 0";
            shieldDamage.SetTextWithoutNotify("0");
            shieldBonusAC.text = "+0";

            // Perception & Savesd
            perceptionAPIC.Refresh(creation.currentPlayer.perception);
            fortitudeAPIC.Refresh(creation.currentPlayer.fortitude);
            reflexAPIC.Refresh(creation.currentPlayer.reflex);
            willAPIC.Refresh(creation.currentPlayer.will);

            // ClassDC
            classDCText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.class_dc);

            // Size
            sizeText.text = creation.currentPlayer.size;

            // Bulk
            bulkText.text = creation.currentPlayer.bulk_current + "/" + creation.currentPlayer.bulk_encThreshold;

            // Hero Points
            heroPointsInput.SetTextWithoutNotify(creation.currentPlayer.heroPoints.ToString());

            // Wealth
            wealthInput.SetTextWithoutNotify(creation.currentPlayer.Wealth_Formated());

            // Abilities
            Color unactive = Globals.Theme["background_1"]; Color boostColor = Globals.Theme["text_2"]; Color flawColor = Globals.Theme["untrained"];
            foreach (var item in ablMap)
                foreach (var image in item.myList)
                    image.color = unactive;
            int[,] map = new int[8, 6];
            foreach (var boost in creation.currentPlayer.Abl_MapGet())
                switch (boost.from)
                {
                    case "ancestry boost": map[0, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "ancestry flaw": map[0, DB.Abl_Abbr2Int(boost.selector)] -= 1; break;
                    case "ancestry free": map[0, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "background choice": map[1, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "background free": map[1, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "class": map[2, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "lvl1": map[3, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "lvl5": map[4, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "lvl10": map[5, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "lvl15": map[6, DB.Abl_Abbr2Int(boost.selector)] += 1; break;
                    case "lvl20": map[7, DB.Abl_Abbr2Int(boost.selector)] += 1; break;

                    default: break;
                }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 6; j++)
                    if (map[i, j] < 0)
                        ablMap[i].myList[j].color = flawColor;
                    else if (map[i, j] > 0)
                        ablMap[i].myList[j].color = boostColor;
            ablMapScores[0].text = (creation.currentPlayer.Abl_GetMod("str") >= 0 ? "+" : "") + creation.currentPlayer.Abl_GetMod("str");
            ablMapScores[1].text = (creation.currentPlayer.Abl_GetMod("dex") >= 0 ? "+" : "") + creation.currentPlayer.Abl_GetMod("dex");
            ablMapScores[2].text = (creation.currentPlayer.Abl_GetMod("con") >= 0 ? "+" : "") + creation.currentPlayer.Abl_GetMod("con");
            ablMapScores[3].text = (creation.currentPlayer.Abl_GetMod("int") >= 0 ? "+" : "") + creation.currentPlayer.Abl_GetMod("int");
            ablMapScores[4].text = (creation.currentPlayer.Abl_GetMod("wis") >= 0 ? "+" : "") + creation.currentPlayer.Abl_GetMod("wis");
            ablMapScores[5].text = (creation.currentPlayer.Abl_GetMod("cha") >= 0 ? "+" : "") + creation.currentPlayer.Abl_GetMod("cha");

            // Skills
            var list = creation.currentPlayer.Skill_GetAll();
            for (int i = 0; i < skills.Count; i++)
                skills[i].Refresh(list[i]);

            // Traits
            string traits = "";
            List<string> traitList = (from a in creation.currentPlayer.RE_general where a.selector == "trait" select a.value).ToList() ?? new List<string>();
            if (traitList.Count > 0) traits = string.Join(", ", traitList);
            traitsText.text = traits;

            // Languages
            string languages = "";
            if (creation.currentPlayer.languages != null)
                if (creation.currentPlayer.languages != "")
                    languages = creation.currentPlayer.languages;
            languagesInput.SetTextWithoutNotify(languages);

            // Speeds
            speedBaseText.text = creation.currentPlayer.speed_base.ToString();
            speedFlyText.text = creation.currentPlayer.speed_fly.ToString();
            speedSwimText.text = creation.currentPlayer.speed_swim.ToString();
            speedClimbText.text = creation.currentPlayer.speed_climp.ToString();
            speedBurrowText.text = creation.currentPlayer.speed_burrow.ToString();

            // Weapon & Armor Profs
            unarmoredText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.unarmored);
            lightArmorText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.light_armor);
            mediumArmorText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.medium_armor);
            heavyArmorText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.heavy_armor);

            unarmedText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.unarmed);
            simpleWeaponText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.simple_weapons);
            martialWeaponText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.martial_weapons);
            advancedWeaponText.text = DB.Prof_Abbr2AbbrColored(creation.currentPlayer.advanced_weapons);
        }

        #endregion

        #region --------------------------------BUTTONS & INPUT--------------------------------

        public void OnEndEditDamage()
        {
            int value = 0; int.TryParse(damageInput.text, out value);
            creation.currentPlayer.hp_damage = value;
            RefreshPlayerIntoPanel();
        }
        public void OnEndEditDying()
        {
            int value = 0; int.TryParse(dyingInput.text, out value);
            creation.currentPlayer.hp_dyingCurrent = value;
            RefreshPlayerIntoPanel();
        }
        public void OnEndEditWounds()
        {
            int value = 0; int.TryParse(woundsInput.text, out value);
            creation.currentPlayer.hp_wounds = value;
            RefreshPlayerIntoPanel();
        }
        public void OnEndEditDoom()
        {
            int value = 0; int.TryParse(doomInput.text, out value);
            creation.currentPlayer.hp_doom = value;
            RefreshPlayerIntoPanel();
        }

        public void OnEndEditLanguages()
        {
            string value = languagesInput.text;
            creation.currentPlayer.languages = value;
            RefreshPlayerIntoPanel();
        }

        public void OnEndEditHeroPoints()
        {
            string value = heroPointsInput.text;
            int valueInt = 0; int.TryParse(value, out valueInt);
            creation.currentPlayer.heroPoints = valueInt;
            RefreshPlayerIntoPanel();
        }

        public void OnEndEditWealth()
        {
            string value = wealthInput.text;
            float valueF = 0; float.TryParse(value, out valueF);
            creation.currentPlayer.wealth = valueF;
            RefreshPlayerIntoPanel();
        }

        public void OnEndEditShieldDamage()
        {
            // string value = shieldDamage.text;u
            // creation.currentPlayer.shield = value;
            RefreshPlayerIntoPanel();
        }


        // money

        #endregion

    }

}
