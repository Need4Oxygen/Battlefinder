using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_ABCSelector : MonoBehaviour
{
    [HideInInspector] public E_PF2E_ABC currentlyDisplaying;

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
    [SerializeField] TMP_Text backgroundAbilityBoosts = null;        // Strenght or Dexterity, Free
    [SerializeField] TMP_Text backgroundSkillTrain = null;
    [SerializeField] TMP_Text backgroundSkillFeat = null;


    [Header("Classes Panel")]
    [SerializeField] Transform classPanel = null;
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


    private List<UI_ButtonText> tabList = new List<UI_ButtonText>();
    private List<UI_ButtonText> buttonList = new List<UI_ButtonText>();


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
        selectedAncestry = "";
        selectedBackground = "";
        selectedClass = "";
    }

    public void CloseSubpanels()
    {
        ancestryPanel.gameObject.SetActive(false);
        backgroundPanel.gameObject.SetActive(false);
        classPanel.gameObject.SetActive(false);
    }

    #region --------Tabs & Buttons--------

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

    #endregion

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

        Transform newTab = Instantiate(tab, Vector3.zero, Quaternion.identity, tabContainer);
        UI_ButtonText newTabScript = newTab.GetComponent<UI_ButtonText>();
        newTabScript.text.text = "Core Rulebook";
        newTab.SetParent(tabContainer);
        tabList.Add(newTabScript);

        foreach (var item in PF2E_DataBase.Ancestries)
        {
            Transform newButton = Instantiate(button, Vector3.zero, Quaternion.identity, buttonContainer);
            UI_ButtonText newButtonScript = newButton.GetComponent<UI_ButtonText>();
            newButtonScript.text.text = item.Value.name;
            newButtonScript.button.onClick.AddListener(() => SelectAncestry(item.Value.name));
            buttonList.Add(newButtonScript);
        }

        buttonList[0].button.onClick.Invoke();
    }

    private void SelectAncestry(string ancestryName)
    {
        PF2E_Ancestry ancestry = PF2E_DataBase.Ancestries[ancestryName];
        selectedAncestry = ancestryName;
        int count, total = 0;

        ancestryTitle.text = ancestry.name;
        ancestryDescription.text = ancestry.description;
        ancestryHitPoints.text = ancestry.hitPoints.ToString();
        ancestrySpeed.text = ancestry.speed.ToString();
        ancestrySize.text = PF2E_DataBase.SizeFullName(ancestry.size);


        // Ability Boosts
        string abilityBoostString = "";
        count = 0; total = ancestry.abilityBoosts.Count;
        foreach (var item in ancestry.abilityBoosts)
        {
            if (count < total - 1)
                abilityBoostString += PF2E_DataBase.AbilityToFullName(item.Value.target) + ", ";
            else
                abilityBoostString += PF2E_DataBase.AbilityToFullName(item.Value.target);
            count++;
        }
        ancestryAbilityBoosts.text = abilityBoostString;


        // Ability Flaws
        string abilityFlawsString = "";
        count = 0; total = ancestry.abilityFlaws.Count;
        foreach (var item in ancestry.abilityFlaws)
        {
            if (count < total - 1)
                abilityFlawsString += PF2E_DataBase.AbilityToFullName(item.Value.target) + ", ";
            else
                abilityFlawsString += PF2E_DataBase.AbilityToFullName(item.Value.target);
            count++;
        }
        ancestryAbilityFlaws.text = abilityFlawsString;


        // Languages
        string languagesString = "";
        for (int i = 0; i < ancestry.languages.Length; i++)
            if (i < ancestry.languages.Length - 1)
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
                ancestryTraitsString += item.Value.name + ", ";
            else
                ancestryTraitsString += item.Value.name;
            count++;
        }
        ancestryTraits.text = ancestryTraitsString;


        // Ancestry features
        string ancestryFeaturesString = "";
        for (int i = 0; i < ancestry.ancestryFeatures.Length; i++)
            if (i < ancestry.ancestryFeatures.Length - 1)
                ancestryFeaturesString += ancestry.ancestryFeatures[i] + ", ";
            else
                ancestryFeaturesString += ancestry.ancestryFeatures[i];
        ancestryFeatures.text = ancestryFeaturesString;
    }


    //------------------------------------BACKGROUNDS------------------------------------
    private void DisplayBackgrounds()
    {
        currentlyDisplaying = E_PF2E_ABC.Background;

        Transform newTab = Instantiate(tab, Vector3.zero, Quaternion.identity, tabContainer);
        UI_ButtonText newTabScript = newTab.GetComponent<UI_ButtonText>();
        newTabScript.text.text = "Core Rulebook";
        newTab.SetParent(tabContainer);
        tabList.Add(newTabScript);

        foreach (var item in PF2E_DataBase.Backgrounds)
        {
            Transform newButton = Instantiate(button, Vector3.zero, Quaternion.identity, buttonContainer);
            UI_ButtonText newButtonScript = newButton.GetComponent<UI_ButtonText>();
            newButtonScript.text.text = item.Value.name;
            newButtonScript.button.onClick.AddListener(() => SelectBackground(item.Value.name));
            buttonList.Add(newButtonScript);
        }

        buttonList[0].button.onClick.Invoke();
    }

    private void SelectBackground(string backgroundName)
    {
        PF2E_Background background = PF2E_DataBase.Backgrounds[backgroundName];
        selectedBackground = backgroundName;

        backgroundTitle.text = background.name;
        backgroundDescription.text = background.description;

        string backgroundAbilityBoostString = "";
        int count = 0; int total = background.abilityBoostsChoice.Count;
        foreach (var item in background.abilityBoostsChoice)
        {
            if (count < total - 1)
                backgroundAbilityBoostString += PF2E_DataBase.AbilityToFullName(item.Value.target) + ", ";
            else
                backgroundAbilityBoostString += PF2E_DataBase.AbilityToFullName(item.Value.target);
            count++;
        }
        backgroundAbilityBoosts.text = backgroundAbilityBoostString;

        string backgroundSkillTrainString = "";
        List<string> sNames = new List<string>();
        foreach (var item in background.lectures)
            sNames.Add(item.Value.name);
        for (int i = 0; i < sNames.Count; i++)
            if (i < sNames.Count - 1)
                backgroundSkillTrainString += sNames[i] + ", ";
            else
                backgroundSkillTrainString += sNames[i];
        backgroundSkillTrain.text = backgroundSkillTrainString;

        backgroundSkillFeat.text = background.skillFeat;
    }

    //------------------------------------CLASSES------------------------------------
    private void DisplayClasses()
    {
        currentlyDisplaying = E_PF2E_ABC.Class;

        Transform newTab = Instantiate(tab, Vector3.zero, Quaternion.identity, tabContainer);
        UI_ButtonText newTabScript = newTab.GetComponent<UI_ButtonText>();
        newTabScript.text.text = "Core Rulebook";
        newTab.SetParent(tabContainer);
        tabList.Add(newTabScript);

        foreach (var item in PF2E_DataBase.Classes)
        {
            Transform newButton = Instantiate(button, Vector3.zero, Quaternion.identity, buttonContainer);
            UI_ButtonText newButtonScript = newButton.GetComponent<UI_ButtonText>();
            newButtonScript.text.text = item.Value.name;
            newButtonScript.button.onClick.AddListener(() => SelectClass(item.Value.name));
            buttonList.Add(newButtonScript);
        }

        buttonList[0].button.onClick.Invoke();
    }

    private void SelectClass(string className)
    {
        PF2E_Class classObj = PF2E_DataBase.Classes[className];
        selectedClass = className;

        classTitle.text = classObj.name;
        classDescription.text = classObj.description;
        classHitPoints.text = classObj.hitPoints.ToString();
        classSkillTrain.text = classObj.freeSkillTrainsString;

        if (className == "Fighter")
            classKeyAbility.text = "Strength or Dexterity";
        else
            classKeyAbility.text = PF2E_DataBase.AbilityToFullName(classObj.keyAbility);

        classUnarmed.text = "U"; classUnarmored.text = "U"; classPerception.text = "U";
        classSimpleWeapons.text = "U"; classLightArmor.text = "U"; classFortitude.text = "U";
        classMartialWeapons.text = "U"; classMediumArmor.text = "U"; classReflex.text = "U";
        classAdvancedWeapons.text = "U"; classHeavyArmor.text = "U"; classWill.text = "U";

        foreach (var item in classObj.lectures)
        {
            switch (item.Value.target)
            {
                case "unarmed":
                    classUnarmed.text = item.Value.proficiency;
                    break;
                case "simpleWeapons":
                    classSimpleWeapons.text = item.Value.proficiency;
                    break;
                case "martialWeapons":
                    classMartialWeapons.text = item.Value.proficiency;
                    break;
                case "advancedWeapons":
                    classAdvancedWeapons.text = item.Value.proficiency;
                    break;
                case "unarmored":
                    classUnarmored.text = item.Value.proficiency;
                    break;
                case "lightArmor":
                    classLightArmor.text = item.Value.proficiency;
                    break;
                case "mediumArmor":
                    classMediumArmor.text = item.Value.proficiency;
                    break;
                case "heavyArmor":
                    classHeavyArmor.text = item.Value.proficiency;
                    break;
                case "perception":
                    classPerception.text = item.Value.proficiency;
                    break;
                case "fortitude":
                    classFortitude.text = item.Value.proficiency;
                    break;
                case "reflex":
                    classReflex.text = item.Value.proficiency;
                    break;
                case "will":
                    classWill.text = item.Value.proficiency;
                    break;
                default:
                    break;
            }
        }
    }

}
