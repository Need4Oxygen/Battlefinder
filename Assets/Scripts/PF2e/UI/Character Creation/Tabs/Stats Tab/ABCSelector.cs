using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Pathfinder2e;
using Pathfinder2e.Character;
using Pathfinder2e.Containers;
using Tools;

namespace Pathfinder2e.GameData
{

    public class ABCSelector : MonoBehaviour
    {
        [HideInInspector] public string currentlyDisplaying;
        [SerializeField] private CharacterCreation characterCreation = null;

        [Header("ABC")]
        [SerializeField] private Window window = null;
        [SerializeField] private Transform buttonContainer = null;
        [SerializeField] private Transform button = null;
        [SerializeField] private Transform tabContainer = null;
        [SerializeField] private Transform tab = null;

        [Space(15)]
        public Button acceptButton = null;
        public Button backButton = null;

        [Header("Ancestries Panel")]
        [SerializeField] private Transform ancestryPanel = null;
        [SerializeField] private TMP_Text ancestryTitle = null;
        [SerializeField] private TMP_Text ancestryDescription = null;
        [SerializeField] private TMP_Text ancestryHitPoints = null;
        [SerializeField] private TMP_Text ancestrySpeed = null;
        [SerializeField] private TMP_Text ancestrySize = null;
        [SerializeField] private TMP_Text ancestryAbilityBoosts = null;
        [SerializeField] private TMP_Text ancestryAbilityFlaws = null;
        [SerializeField] private TMP_Text ancestryLanguages = null;
        [SerializeField] private TMP_Text ancestryTraits = null;
        [SerializeField] private TMP_Text ancestryFeatures = null;


        [Header("Backgrounds Panel")]
        [SerializeField] private Transform backgroundPanel = null;
        [SerializeField] private TMP_Text backgroundTitle = null;
        [SerializeField] private TMP_Text backgroundDescription = null;
        [SerializeField] private TMP_Text backgroundAbilityBoosts = null;
        [SerializeField] private TMP_Text backgroundSkillTrain = null;
        [SerializeField] private TMP_Text backgroundSkillFeat = null;


        [Header("Classes Panel")]
        [SerializeField] private Transform classPanel = null;
        [SerializeField] private Transform weaponArmorLeyendPanel = null;
        [SerializeField] private TMP_Text classTitle = null;
        [SerializeField] private TMP_Text classDescription = null;
        [SerializeField] private TMP_Text classHitPoints = null;

        [SerializeField] private TMP_Text classUnarmed = null;
        [SerializeField] private TMP_Text classSimpleWeapons = null;
        [SerializeField] private TMP_Text classMartialWeapons = null;
        [SerializeField] private TMP_Text classAdvancedWeapons = null;

        [SerializeField] private TMP_Text classUnarmored = null;
        [SerializeField] private TMP_Text classLightArmor = null;
        [SerializeField] private TMP_Text classMediumArmor = null;
        [SerializeField] private TMP_Text classHeavyArmor = null;

        [SerializeField] private TMP_Text classKeyAbility = null;
        [SerializeField] private TMP_Text classSkillTrain = null;

        [SerializeField] private TMP_Text classPerception = null;
        [SerializeField] private TMP_Text classFortitude = null;
        [SerializeField] private TMP_Text classReflex = null;
        [SerializeField] private TMP_Text classWill = null;


        [HideInInspector] public string selectedAncestry = "";
        [HideInInspector] public string selectedBackground = "";
        [HideInInspector] public string selectedClass = "";


        private Color selected = new Color(0.6f, 0.6f, 0.6f, 1f);
        private Color unselected = new Color(1f, 1f, 1f, 1f);

        private List<ButtonText> tabList = new List<ButtonText>();
        private List<ButtonText> buttonList = new List<ButtonText>();

        private ButtonText currentlySelected = null;

        void Awake()
        {
            CloseSubpanels();
        }

        public void OpenSelectorPanel()
        {
            window.OpenWindow();
        }

        public void CloseSelectorPanel()
        {
            window.CloseWindow();
            ClearTabs();
            ClearButtons();

            currentlyDisplaying = "none";

            selectedAncestry = "";
            selectedBackground = "";
            selectedClass = "";
        }

        public void CloseSubpanels()
        {
            ancestryPanel.gameObject.SetActive(false);
            backgroundPanel.gameObject.SetActive(false);
            classPanel.gameObject.SetActive(false);
            weaponArmorLeyendPanel.gameObject.SetActive(false);
        }

        private void ClearTabs()
        {
            foreach (var item in tabList)
                Destroy(item.gameObject, 0.001f);
            tabList.Clear();
        }

        private void ClearButtons()
        {
            foreach (var item in buttonList)
                Destroy(item.gameObject, 0.001f);
            buttonList.Clear();
        }

        private void Select(ButtonText newSelection)
        {
            if (currentlySelected != null)
                currentlySelected.GetComponent<Image>().color = unselected;
            newSelection.GetComponent<Image>().color = selected;
            currentlySelected = newSelection;
        }

        public void Display(string abc)
        {
            ClearButtons();
            ClearTabs();
            CloseSubpanels();
            OpenSelectorPanel();

            switch (abc)
            {
                case "ancestry":
                    ancestryPanel.gameObject.SetActive(true);
                    DisplayAncestries();
                    break;
                case "background":
                    backgroundPanel.gameObject.SetActive(true);
                    DisplayBackgrounds();
                    break;
                case "class":
                    classPanel.gameObject.SetActive(true);
                    weaponArmorLeyendPanel.gameObject.SetActive(true);
                    DisplayClasses();
                    break;

                default:
                    break;
            }
        }

        //------------------------------------ANCESTRIES------------------------------------
        private void DisplayAncestries()
        {
            currentlyDisplaying = "ancestry";
            string currentAncestry = characterCreation.currentPlayer.ancestry;

            Transform newTab = Instantiate(tab, Vector3.zero, Quaternion.identity, tabContainer);
            ButtonText newTabScript = newTab.GetComponent<ButtonText>();
            newTabScript.text.text = "CRB";
            newTab.SetParent(tabContainer);
            tabList.Add(newTabScript);

            ButtonText currentAncestryButton = null;
            foreach (var item in DB.Ancestries)
            {
                Transform newButton = Instantiate(button, Vector3.zero, Quaternion.identity, buttonContainer);
                ButtonText newButtonScript = newButton.GetComponent<ButtonText>();
                newButtonScript.text.text = item.name;
                newButtonScript.button.onClick.AddListener(() => SelectAncestry(item.name, newButtonScript));
                buttonList.Add(newButtonScript);

                if (item.name == currentAncestry)
                    currentAncestryButton = newButtonScript;
            }

            if (currentAncestryButton != null)
                currentAncestryButton.button.onClick.Invoke();
            else
                buttonList[0].button.onClick.Invoke();
        }

        private void SelectAncestry(string ancestryName, ButtonText button)
        {
            Select(button);
            Ancestry ancestry = DB.Ancestries.Find(ctx => ctx.name == ancestryName);
            selectedAncestry = ancestryName;
            int count, total = 0;

            ancestryTitle.text = ancestry.name;
            ancestryDescription.text = ancestry.descr;
            ancestryHitPoints.text = ancestry.hp.ToString();
            ancestrySpeed.text = ancestry.speed.ToString();
            ancestrySize.text = DB.Size_Abbr2Full(ancestry.size);


            // Ability Boosts
            string abilityBoostString = "";
            count = 0; total = ancestry.abl_boosts.Count;
            foreach (var item in ancestry.abl_boosts)
            {
                if (count < total - 1)
                    abilityBoostString += DB.Abl_Abbr2Full(item) + ", ";
                else
                    abilityBoostString += DB.Abl_Abbr2Full(item);
                count++;
            }
            ancestryAbilityBoosts.text = abilityBoostString;


            // Ability Flaws
            string abilityFlawsString = "";
            if (ancestry.abl_flaws != null)
            {
                count = 0; total = ancestry.abl_flaws.Count;
                foreach (var item in ancestry.abl_flaws)
                {
                    if (count < total - 1)
                        abilityFlawsString += DB.Abl_Abbr2Full(item) + ", ";
                    else
                        abilityFlawsString += DB.Abl_Abbr2Full(item);
                    count++;
                }
            }
            ancestryAbilityFlaws.text = abilityFlawsString;


            // Languages
            string languagesString = "";
            for (int i = 0; i < ancestry.languages.Count; i++)
                if (i < ancestry.languages.Count - 1)
                    languagesString += ancestry.languages[i] + ", ";
                else
                    languagesString += ancestry.languages[i];
            ancestryLanguages.text = languagesString;


            // Traits
            string ancestryTraitsString = "";
            count = 0; total = ancestry.traits.Count;
            foreach (var item in ancestry.traits)
            {
                if (count < total - 1)
                    ancestryTraitsString += item + ", ";
                else
                    ancestryTraitsString += item;
                count++;
            }
            ancestryTraits.text = ancestryTraitsString;


            // Ancestry features
            string ancestryFeaturesString = "";
            if (ancestry.ancestry_features != null)
            {
                for (int i = 0; i < ancestry.ancestry_features.Count; i++)
                    if (i < ancestry.ancestry_features.Count - 1)
                        ancestryFeaturesString += ancestry.ancestry_features[i] + ", ";
                    else
                        ancestryFeaturesString += ancestry.ancestry_features[i];
            }
            ancestryFeatures.text = ancestryFeaturesString;
        }


        //------------------------------------BACKGROUNDS------------------------------------
        private void DisplayBackgrounds()
        {
            currentlyDisplaying = "background";
            string currentBackground = characterCreation.currentPlayer.background;

            Transform newTab = Instantiate(tab, Vector3.zero, Quaternion.identity, tabContainer);
            ButtonText newTabScript = newTab.GetComponent<ButtonText>();
            newTabScript.text.text = "CRB";
            newTab.SetParent(tabContainer);
            tabList.Add(newTabScript);

            ButtonText currentBackgroundButton = null;
            foreach (var item in DB.Backgrounds)
            {
                Transform newButton = Instantiate(button, Vector3.zero, Quaternion.identity, buttonContainer);
                ButtonText newButtonScript = newButton.GetComponent<ButtonText>();
                newButtonScript.text.text = item.name;
                newButtonScript.button.onClick.AddListener(() => SelectBackground(item.name, newButtonScript));
                buttonList.Add(newButtonScript);

                if (item.name == currentBackground)
                    currentBackgroundButton = newButtonScript;
            }

            if (currentBackgroundButton != null)
                currentBackgroundButton.button.onClick.Invoke();
            else
                buttonList[0].button.onClick.Invoke();
        }

        private void SelectBackground(string backgroundName, ButtonText button)
        {
            Select(button);
            Background background = DB.Backgrounds.Find(ctx => ctx.name == backgroundName);
            selectedBackground = backgroundName;

            backgroundTitle.text = background.name;
            backgroundDescription.text = background.descr;

            string backgroundAbilityBoostString = "";
            int count = 0; int total = background.abl_choices.Count;
            foreach (var item in background.abl_choices)
            {
                if (count < total - 1)
                    backgroundAbilityBoostString += DB.Abl_Abbr2Full(item) + ", ";
                else
                    backgroundAbilityBoostString += DB.Abl_Abbr2Full(item);
                count++;
            }
            backgroundAbilityBoosts.text = backgroundAbilityBoostString;

            // Extract feats to display in a string
            string backgroundSkillTrainString = "";
            List<string> skillNames = new List<string>();
            foreach (var item in background.elements)
                skillNames.Add(item.selector);
            for (int i = 0; i < skillNames.Count; i++)
                if (i < skillNames.Count - 1)
                    backgroundSkillTrainString += StrTools.ToUpperFirst(skillNames[i]) + ", ";
                else
                    backgroundSkillTrainString += StrTools.ToUpperFirst(skillNames[i]);
            backgroundSkillTrain.text = backgroundSkillTrainString;

            // Extract feats to display in a string
            string backgroundSkillFeatsString = "";
            List<string> skillFeats = new List<string>();
            foreach (var item in background.free_skill_feats)
                skillFeats.Add(item);
            for (int i = 0; i < skillFeats.Count; i++)
                if (i < skillFeats.Count - 1)
                    backgroundSkillFeatsString += skillFeats[i] + ", ";
                else
                    backgroundSkillFeatsString += skillFeats[i];
            backgroundSkillFeat.text = backgroundSkillFeatsString;
        }

        //------------------------------------CLASSES------------------------------------
        private void DisplayClasses()
        {
            currentlyDisplaying = "class";
            string currentClass = characterCreation.currentPlayer.class_name;

            Transform newTab = Instantiate(tab, Vector3.zero, Quaternion.identity, tabContainer);
            ButtonText newTabScript = newTab.GetComponent<ButtonText>();
            newTabScript.text.text = "CRB";
            newTab.SetParent(tabContainer);
            tabList.Add(newTabScript);

            ButtonText currentClassButton = null;
            foreach (var item in DB.Classes)
            {
                Transform newButton = Instantiate(button, Vector3.zero, Quaternion.identity, buttonContainer);
                ButtonText newButtonScript = newButton.GetComponent<ButtonText>();
                newButtonScript.text.text = item.name;
                newButtonScript.button.onClick.AddListener(() => SelectClass(item.name, newButtonScript));
                buttonList.Add(newButtonScript);

                if (item.name == currentClass)
                    currentClassButton = newButtonScript;
            }

            if (currentClassButton != null)
                currentClassButton.button.onClick.Invoke();
            else
                buttonList[0].button.onClick.Invoke();
        }

        private void SelectClass(string className, ButtonText button)
        {
            Select(button);
            Class classObj = DB.Classes.Find(ctx => ctx.name == className);
            selectedClass = className;

            classTitle.text = classObj.name;
            classDescription.text = classObj.descr;
            classHitPoints.text = classObj.hp.ToString();
            classSkillTrain.text = ClassSkillTrainStringProcessor(classObj);

            string classKeyAbilityString = "";
            for (int i = 0; i < classObj.key_ability_choices.Count; i++)
                if (i == 0)
                    classKeyAbilityString = DB.Abl_Abbr2Full(classObj.key_ability_choices[i]);
                else
                    classKeyAbilityString += $" or {DB.Abl_Abbr2Full(classObj.key_ability_choices[i])}";
            classKeyAbility.text = classKeyAbilityString;

            string untrained = DB.Prof_Full2AbbrColored("untrained");
            classUnarmed.text = untrained; classUnarmored.text = untrained; classPerception.text = untrained;
            classSimpleWeapons.text = untrained; classLightArmor.text = untrained; classFortitude.text = untrained;
            classMartialWeapons.text = untrained; classMediumArmor.text = untrained; classReflex.text = untrained;
            classAdvancedWeapons.text = untrained; classHeavyArmor.text = untrained; classWill.text = untrained;

            foreach (var element in classObj.elements)
            {
                switch (element.selector)
                {
                    case "perception": classPerception.text = DB.Prof_Full2AbbrColored(element.proficiency); break;

                    case "fortitude": classFortitude.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "reflex": classReflex.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "will": classWill.text = DB.Prof_Full2AbbrColored(element.proficiency); break;

                    case "unarmed": classUnarmed.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "simple_weapons": classSimpleWeapons.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "martial_weapons": classMartialWeapons.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "advanced_weapons": classAdvancedWeapons.text = DB.Prof_Full2AbbrColored(element.proficiency); break;

                    case "unarmored": classUnarmored.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "light_armor": classLightArmor.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "medium_armor": classMediumArmor.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    case "heavy_armor": classHeavyArmor.text = DB.Prof_Full2AbbrColored(element.proficiency); break;
                    default: break;
                }
            }
        }

        static string ClassSkillTrainStringProcessor(Class classObj)
        {
            string str = "";

            foreach (var item in classObj.skill_train_strings)
                str += $"{item}. ";

            return str;
        }
    }

}
