using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Pathfinder2e;
using Pathfinder2e.Player;
using Pathfinder2e.Containers;

public class PF2E_ABCSelector : MonoBehaviour
{
    [HideInInspector] public E_PF2E_ABC currentlyDisplaying;

    [SerializeField] CharacterCreation characterCreation = null;

    [Header("ABC")]
    [SerializeField] CanvasGroup ABCSelectionPanel = null;
    [SerializeField] Transform buttonContainer = null;
    [SerializeField] Transform button = null;
    [SerializeField] Transform tabContainer = null;
    [SerializeField] Transform tab = null;

    [Space(15)]
    public Button acceptButton = null;
    public Button backButton = null;

    [Header("Ancestries Panel")]
    [SerializeField] Transform ancestryPanel = null;
    [SerializeField] TMP_Text ancestryTitle = null;
    [SerializeField] TMP_Text ancestryDescription = null;
    [SerializeField] TMP_Text ancestryHitPoints = null;
    [SerializeField] TMP_Text ancestrySpeed = null;
    [SerializeField] TMP_Text ancestrySize = null;
    [SerializeField] TMP_Text ancestryAbilityBoosts = null;
    [SerializeField] TMP_Text ancestryAbilityFlaws = null;
    [SerializeField] TMP_Text ancestryLanguages = null;
    [SerializeField] TMP_Text ancestryTraits = null;
    [SerializeField] TMP_Text ancestryFeatures = null;


    [Header("Backgrounds Panel")]
    [SerializeField] Transform backgroundPanel = null;
    [SerializeField] TMP_Text backgroundTitle = null;
    [SerializeField] TMP_Text backgroundDescription = null;
    [SerializeField] TMP_Text backgroundAbilityBoosts = null;
    [SerializeField] TMP_Text backgroundSkillTrain = null;
    [SerializeField] TMP_Text backgroundSkillFeat = null;


    [Header("Classes Panel")]
    [SerializeField] Transform classPanel = null;
    [SerializeField] Transform weaponArmorLeyendPanel = null;
    [SerializeField] TMP_Text classTitle = null;
    [SerializeField] TMP_Text classDescription = null;
    [SerializeField] TMP_Text classHitPoints = null;

    [SerializeField] TMP_Text classUnarmed = null;
    [SerializeField] TMP_Text classSimpleWeapons = null;
    [SerializeField] TMP_Text classMartialWeapons = null;
    [SerializeField] TMP_Text classAdvancedWeapons = null;

    [SerializeField] TMP_Text classUnarmored = null;
    [SerializeField] TMP_Text classLightArmor = null;
    [SerializeField] TMP_Text classMediumArmor = null;
    [SerializeField] TMP_Text classHeavyArmor = null;

    [SerializeField] TMP_Text classKeyAbility = null;
    [SerializeField] TMP_Text classSkillTrain = null;

    [SerializeField] TMP_Text classPerception = null;
    [SerializeField] TMP_Text classFortitude = null;
    [SerializeField] TMP_Text classReflex = null;
    [SerializeField] TMP_Text classWill = null;


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
        StartCoroutine(PanelFader.RescaleAndFade(ABCSelectionPanel.transform, ABCSelectionPanel, 0.85f, 0f, 0f));
        CloseSubpanels();
    }

    public void OpenSelectorPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(ABCSelectionPanel.transform, ABCSelectionPanel, 1f, 1f, 0.1f));
    }

    public void CloseSelectorPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(ABCSelectionPanel.transform, ABCSelectionPanel, 0.85f, 0f, 0.1f));
        ClearTabs();
        ClearButtons();

        currentlyDisplaying = E_PF2E_ABC.None;

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

    public void Display(E_PF2E_ABC display)
    {
        ClearButtons();
        ClearTabs();
        CloseSubpanels();
        OpenSelectorPanel();

        switch (display)
        {
            case E_PF2E_ABC.Ancestry:
                ancestryPanel.gameObject.SetActive(true);
                DisplayAncestries();
                break;
            case E_PF2E_ABC.Background:
                backgroundPanel.gameObject.SetActive(true);
                DisplayBackgrounds();
                break;
            case E_PF2E_ABC.Class:
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
        currentlyDisplaying = E_PF2E_ABC.Ancestry;
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
        count = 0; total = ancestry.abl_flaw.Count;
        foreach (var item in ancestry.abl_flaw)
        {
            if (count < total - 1)
                abilityFlawsString += DB.Abl_Abbr2Full(item) + ", ";
            else
                abilityFlawsString += DB.Abl_Abbr2Full(item);
            count++;
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
        for (int i = 0; i < ancestry.ancestry_features.Count; i++)
            if (i < ancestry.ancestry_features.Count - 1)
                ancestryFeaturesString += ancestry.ancestry_features[i] + ", ";
            else
                ancestryFeaturesString += ancestry.ancestry_features[i];
        ancestryFeatures.text = ancestryFeaturesString;
    }


    //------------------------------------BACKGROUNDS------------------------------------
    private void DisplayBackgrounds()
    {
        currentlyDisplaying = E_PF2E_ABC.Background;
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
        foreach (var item in background.lectures)
            skillNames.Add(item.target);
        for (int i = 0; i < skillNames.Count; i++)
            if (i < skillNames.Count - 1)
                backgroundSkillTrainString += skillNames[i] + ", ";
            else
                backgroundSkillTrainString += skillNames[i];
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
        currentlyDisplaying = E_PF2E_ABC.Class;
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
        classSkillTrain.text = ClassSkillTrainStringProcessor(classObj.skills);

        string classKeyAbilityString = "";
        for (int i = 0; i < classObj.key_ability_choices.Count; i++)
            if (i == 0)
                classKeyAbilityString = DB.Abl_Abbr2Full(classObj.key_ability_choices[i]);
            else
                classKeyAbilityString += $" or {DB.Abl_Abbr2Full(classObj.key_ability_choices[i])}";
        classKeyAbility.text = classKeyAbilityString;

        classUnarmed.text = "U"; classUnarmored.text = "U"; classPerception.text = "U";
        classSimpleWeapons.text = "U"; classLightArmor.text = "U"; classFortitude.text = "U";
        classMartialWeapons.text = "U"; classMediumArmor.text = "U"; classReflex.text = "U";
        classAdvancedWeapons.text = "U"; classHeavyArmor.text = "U"; classWill.text = "U";

        List<Lecture> lectures = new List<Lecture>(classObj.attacks.Concat<Lecture>(classObj.defenses).Concat<Lecture>(classObj.perception).Concat<Lecture>(classObj.saves));
        foreach (var item in lectures)
            switch (item.target)
            {
                case "unarmed":
                    classUnarmed.text = item.prof;
                    break;
                case "simpleWeapons":
                    classSimpleWeapons.text = item.prof;
                    break;
                case "martialWeapons":
                    classMartialWeapons.text = item.prof;
                    break;
                case "advancedWeapons":
                    classAdvancedWeapons.text = item.prof;
                    break;
                case "unarmored":
                    classUnarmored.text = item.prof;
                    break;
                case "lightArmor":
                    classLightArmor.text = item.prof;
                    break;
                case "mediumArmor":
                    classMediumArmor.text = item.prof;
                    break;
                case "heavyArmor":
                    classHeavyArmor.text = item.prof;
                    break;
                case "perception":
                    classPerception.text = item.prof;
                    break;
                case "fortitude":
                    classFortitude.text = item.prof;
                    break;
                case "reflex":
                    classReflex.text = item.prof;
                    break;
                case "will":
                    classWill.text = item.prof;
                    break;
                default:
                    break;
            }
    }

    static string ClassSkillTrainStringProcessor(List<Lecture> lectures)
    {
        string str = "";

        foreach (var item in lectures)
        {
            if (DB.SkillNames.Contains(item.target))
                str += $"{DB.ToUpperFirst(item.prof)} in {DB.ToUpperFirst(item.target)}. ";
            else if (item.target.Substring(0, 8) == "a number")
                str += $"{DB.ToUpperFirst(item.prof)} in a number of additional skills equal to {new string(item.target.Where(char.IsDigit).ToArray())} plus your intelligence modifier. ";
            else if (item.target == "your choice of acrobatics or athletics") // for fighter choice of acrobatics or athletics
                str += $"{DB.ToUpperFirst(item.prof)} in your choice of acrobatics or athletics";
            else if (item.target == "one skill determined by your choice of deity") // for champion cleric deity
                str += $"{DB.ToUpperFirst(item.prof)} in one skill determined by your choice of deity";
            else if (item.target == "one skill determined by your druidic order") // for druid order
                str += $"{DB.ToUpperFirst(item.prof)} in one skill determined by your druidic order";
            else if (item.target == "one or more skills determined by your rogue's racket") // for rogues racket
                str += $"{DB.ToUpperFirst(item.prof)} in one or more skills determined by your rogue's racket";
            else if (item.target == "one or more skills determined by your bloodline") // for sorcerer bloodline
                str += $"{DB.ToUpperFirst(item.prof)} in one or more skills determined by your bloodline";
        }

        return str;
    }



}
