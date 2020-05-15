using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_AblBoostsSelector : MonoBehaviour
{
    [SerializeField] private PF2E_CharacterCreation creation = null;
    [SerializeField] private Transform dropdownPrefab = null;

    [Header("Initial Abilities Boosts")]
    [SerializeField] private CanvasGroup iAblBoostsPanel = null;
    [SerializeField] private Transform ancestryContainer = null;
    [SerializeField] private Transform backgroundContainer = null;
    [SerializeField] private Transform classContainer = null;
    [SerializeField] private List<Toggle> level1Toggles = null;

    private List<string> ancestryBoosts = new List<string>();
    private List<string> ancestryFlaws = new List<string>();
    private List<TMP_Dropdown> ancestryFreeAssignsDropdowns = new List<TMP_Dropdown>();
    private List<TMP_Dropdown> ancestryDrops = new List<TMP_Dropdown>();
    private List<TMP_Dropdown> backgroundDrops = new List<TMP_Dropdown>();
    private List<TMP_Dropdown> classDrops = new List<TMP_Dropdown>();

    // [Header("Other Abilities Boosts")]
    // [SerializeField] private CanvasGroup ablBoostsPanel = null;
    // [SerializeField] private Button ablBoostsAcceptButotn = null;
    // [SerializeField] private Toggle[] toggles = null;

    private PF2E_InitAblBoostData currentData = null;


    void Start()
    {
        AssignLvl1BoostsFunctionality();

        StartCoroutine(PanelFader.RescaleAndFade(iAblBoostsPanel.transform, iAblBoostsPanel, 0.85f, 0f, 0f));
        // StartCoroutine(PanelFader.RescaleAndFade(ablBoostsPanel.transform, ablBoostsPanel, 0.85f, 0f, 0f));
    }

    #region --------------------------------INITIAL BOOSTS--------------------------------

    public void OpenPlayerInitialAblBoostsPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(iAblBoostsPanel.transform, iAblBoostsPanel, 1f, 1f, 0.1f));

        currentData = creation.currentPlayer.Build_Get<PF2E_InitAblBoostData>("Level 1", "Initial Ability Boosts");
        if (currentData == null)
            currentData = new PF2E_InitAblBoostData();

        AssignInitialAblBoosts();
    }

    public void ClosePlayerInitialAblBoostsPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(iAblBoostsPanel.transform, iAblBoostsPanel, 0.85f, 0f, 0.1f));

        currentData = null;
    }

    private void AssignInitialAblBoosts()
    {
        AssignAncestryBoosts();
        AssignBackgroundBoosts();
        AssignClassBoosts();
        AssignLvl1Boosts();
    }

    private TMP_Dropdown GenerateDropdown(Transform parent)
    {
        Transform drop = Instantiate(dropdownPrefab, Vector3.zero, Quaternion.identity, parent);
        TMP_Dropdown dropdown = drop.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        return dropdown;
    }

    //--------------------------------------------ANCESTRIES ASSIGMENT STUFF--------------------------------------------
    private void AssignAncestryBoosts()
    {
        PF2E_Ancestry ancestry = PF2E_DataBase.Ancestries[creation.currentPlayer.ancestry];
        List<string> alreadyBoosted = new List<string>();
        int currentDrop = 0;

        // Delete previous drops
        foreach (var item in ancestryDrops)
        {
            item.gameObject.SetActive(false); // This prevents flickering
            Destroy(item.gameObject, 0.001f);
        }
        ancestryDrops.Clear();

        // Set already boosted/flawed abilities so user can't boost them
        foreach (var item in ancestry.abilityBoosts)
            alreadyBoosted.Add(AbilityToFullName(item.Value.target));
        foreach (var item in ancestry.abilityFlaws)
            alreadyBoosted.Add(AbilityToFullName(item.Value.target));

        // Instantiate ability boosts drops
        ancestryBoosts.Clear();
        foreach (var item in ancestry.abilityBoosts)
        {
            TMP_Dropdown drop = GenerateDropdown(ancestryContainer);
            ancestryDrops.Add(drop);
            drop.interactable = false;
            drop.AddOptions(CreateOptionList(new string[] { item.Value.target }));
            ancestryBoosts.Add(item.Value.target);
            currentDrop++;
        }

        // Instantiate free ability boosts drops
        int freeBoostsCount = ancestry.freeAbilityBoosts.Count;
        int counter = 0;
        ancestryFreeAssignsDropdowns.Clear();
        foreach (var item in ancestry.freeAbilityBoosts)
        {
            TMP_Dropdown drop = GenerateDropdown(ancestryContainer);
            ancestryDrops.Add(drop);
            ancestryFreeAssignsDropdowns.Add(drop);

            // Ready a dropdown
            List<TMP_Dropdown.OptionData> optionList = CreateOptionList();
            optionList.RemoveAll(v => alreadyBoosted.Contains(v.text) && v.text != AbilityToFullName(currentData.ancestryFree[counter]));
            drop.AddOptions(optionList);
            drop.interactable = true;
            drop.onValueChanged.AddListener((v) => OnValueChangedAncestryDropdown(v));
            alreadyBoosted.Add(AbilityToFullName(item.Value.target));

            // Select dropdown value corresponding to last time
            string shouldSelect = ""; int shouldSelectIndex = 0;
            if (counter < currentData.ancestryFree.Count)
                shouldSelect = currentData.ancestryFree[counter];
            if (shouldSelect != "" && shouldSelect != "None" && shouldSelect != "Null" && shouldSelect != null)
                shouldSelectIndex = optionList.FindIndex(0, optionList.Count, i => i.text == AbilityToFullName(shouldSelect));
            drop.SetValueWithoutNotify(shouldSelectIndex);

            currentDrop++;
            counter++;
        }

        // Instantiate ability flaws drops
        ancestryFlaws.Clear();
        foreach (var item in ancestry.abilityFlaws)
        {
            TMP_Dropdown drop = GenerateDropdown(ancestryContainer);
            ancestryDrops.Add(drop);
            drop.interactable = false;
            drop.AddOptions(CreateOptionList(new string[] { item.Value.target }));
            drop.captionText.color = Color.red;
            ancestryFlaws.Add(item.Value.target);
            currentDrop++;
        }
    }


    private void OnValueChangedAncestryDropdown(int value)
    {
        SaveAncestryOptions();
    }

    private void SaveAncestryOptions()
    {
        currentData.ancestryBoosts.Clear();
        currentData.ancestryFlaws.Clear();
        currentData.ancestryFree.Clear();

        foreach (var item in ancestryBoosts)
            currentData.ancestryBoosts.Add(item);
        foreach (var item in ancestryFlaws)
            currentData.ancestryFlaws.Add(item);
        foreach (var item in ancestryFreeAssignsDropdowns)
            currentData.ancestryFree.Add(AbilityToAbbr(item.captionText.text));

        AssignAncestryBoosts();
    }


    //--------------------------------------------BACKGROUND ASSIGMENT STUFF--------------------------------------------
    private void AssignBackgroundBoosts()
    {
        PF2E_Background background = PF2E_DataBase.Backgrounds[creation.currentPlayer.background];
        List<string> alreadyBoosted = new List<string>();

        // Delete previous drops
        foreach (var item in backgroundDrops)
        {
            item.gameObject.SetActive(false); // This prevents flickering
            Destroy(item.gameObject, 0.001f);
        }
        backgroundDrops.Clear();

        // Set background boosted/flawed abilities so user can't boost them
        foreach (var item in currentData.backgroundBoosts)
            alreadyBoosted.Add(AbilityToFullName(item));

        for (int i = 0; i < 2; i++)
        {
            List<TMP_Dropdown.OptionData> optionList = CreateOptionList();

            if (i == 0)
            {
                List<string> choices = new List<string>();
                choices.Add("None");
                foreach (var item in background.abilityBoostsChoice)
                    choices.Add(item.Value.target);
                optionList = CreateOptionList(choices.ToArray());
                optionList.RemoveAll(v => alreadyBoosted.Contains(v.text) && v.text != AbilityToFullName(currentData.backgroundBoosts[i]));
            }
            else
            {
                optionList.RemoveAll(v => alreadyBoosted.Contains(v.text) && v.text != AbilityToFullName(currentData.backgroundBoosts[i]));
            }

            TMP_Dropdown drop = GenerateDropdown(backgroundContainer);
            backgroundDrops.Add(drop);
            drop.interactable = true;
            drop.onValueChanged.AddListener((v) => OnValueChangedBackgroundDropdown(v));
            drop.AddOptions(optionList);

            string shouldSelect = ""; int shouldSelectIndex = 0;
            if (i < currentData.backgroundBoosts.Count)
                shouldSelect = currentData.backgroundBoosts[i];
            if (shouldSelect != "" && shouldSelect != "None" && shouldSelect != null)
                shouldSelectIndex = optionList.FindIndex(0, optionList.Count, v => v.text == AbilityToFullName(shouldSelect));
            drop.SetValueWithoutNotify(shouldSelectIndex);
        }
    }

    private void OnValueChangedBackgroundDropdown(int value)
    {
        SaveBackgroundOptions();
    }

    private void SaveBackgroundOptions()
    {
        currentData.backgroundBoosts.Clear();

        foreach (var item in backgroundDrops)
            currentData.backgroundBoosts.Add(AbilityToAbbr(item.captionText.text));

        AssignBackgroundBoosts();
    }


    //--------------------------------------------CLASS ASSIGMENT STUFF--------------------------------------------
    private void AssignClassBoosts()
    {
        PF2E_Class classObj = PF2E_DataBase.Classes[creation.currentPlayer.class_name];

        // Delete previous drops
        foreach (var item in classDrops)
        {
            item.gameObject.SetActive(false); // This prevents flickering
            Destroy(item.gameObject, 0.001f);
        }
        classDrops.Clear();

        List<string> choices = new List<string>();

        if (classObj.keyAbility.Count > 1)
        {
            choices.Add("None");
            foreach (var item in classObj.keyAbility)
                choices.Add(item.Value.target);
        }
        else
        {
            foreach (var item in classObj.keyAbility)
                choices.Add(item.Value.target);
        }

        TMP_Dropdown drop = GenerateDropdown(classContainer);
        classDrops.Add(drop);
        drop.onValueChanged.AddListener((v) => OnValueChangedClassDropdown(v));
        drop.AddOptions(CreateOptionList(choices.ToArray()));

        if (choices.Count > 1)
            drop.interactable = true;
        else
            drop.interactable = false;

        string shouldSelect = ""; int shouldSelectIndex = 0;
        if (currentData.classBoosts.Count > 0)
            shouldSelect = currentData.classBoosts[0];
        if (shouldSelect != "" && shouldSelect != "None" && shouldSelect != null)
            shouldSelectIndex = choices.FindIndex(0, choices.Count, v => v == shouldSelect);
        drop.SetValueWithoutNotify(shouldSelectIndex);
    }

    private void OnValueChangedClassDropdown(int value)
    {
        SaveClassOptions();
    }

    private void SaveClassOptions()
    {
        currentData.classBoosts.Clear();

        foreach (var item in classDrops)
            currentData.classBoosts.Add(AbilityToAbbr(item.captionText.text));

        AssignClassBoosts();
    }


    //--------------------------------------------CLASS ASSIGMENT STUFF--------------------------------------------
    private void AssignLvl1BoostsFunctionality()
    {
        foreach (var item in level1Toggles)
        {
            item.onValueChanged.RemoveAllListeners();
            item.onValueChanged.AddListener(v => OnValueChangedLvl1BoostsToggle(item, v));
        }
    }

    private void AssignLvl1Boosts()
    {
        foreach (var item in currentData.lvl1boosts)
            switch (item)
            {
                case "str":
                    level1Toggles[0].SetIsOnWithoutNotify(true);
                    break;
                case "dex":
                    level1Toggles[2].SetIsOnWithoutNotify(true);
                    break;
                case "con":
                    level1Toggles[4].SetIsOnWithoutNotify(true);
                    break;
                case "int":
                    level1Toggles[1].SetIsOnWithoutNotify(true);
                    break;
                case "wis":
                    level1Toggles[3].SetIsOnWithoutNotify(true);
                    break;
                case "cha":
                    level1Toggles[5].SetIsOnWithoutNotify(true);
                    break;

                default:
                    break;
            }
    }

    public void OnValueChangedLvl1BoostsToggle(Toggle toggle, bool value)
    {
        if (level1Toggles.FindAll(v => v.isOn == true).Count <= 4 && value)
            toggle.SetIsOnWithoutNotify(true);
        else
            toggle.SetIsOnWithoutNotify(false);

        SaveLvl1Boosts();
    }

    private void SaveLvl1Boosts()
    {
        currentData.lvl1boosts.Clear();

        List<Toggle> activeToggles = level1Toggles.FindAll(v => v.isOn == true);

        foreach (var item in activeToggles)
            currentData.lvl1boosts.Add(AbilityToAbbr(item.GetComponentInChildren<TMP_Text>().text));
    }


    //--------------------------------------------INPUT--------------------------------------------
    public void OnClickAcceptButton()
    {
        creation.currentPlayer.Build_Set("Level 1", "Initial Ability Boosts", currentData);
        creation.RefreshPlayerIntoPanel();
        creation.SavePlayer();
        ClosePlayerInitialAblBoostsPanel();
    }

    public void OnClickCancelButton()
    {
        ClosePlayerInitialAblBoostsPanel();
    }

    #endregion

    #region --------------------------------OTHER BOOSTS--------------------------------

    // public void OpenPlayerAblBoostsPanel()
    // {
    //     StartCoroutine(PanelFader.RescaleAndFade(ablBoostsPanel.transform, ablBoostsPanel, 1f, 1f, 0.1f));
    // }

    // public void ClosePlayerAblBoostsPanel()
    // {
    //     StartCoroutine(PanelFader.RescaleAndFade(ablBoostsPanel.transform, ablBoostsPanel, 0.85f, 0f, 0.1f));
    // }

    #endregion

    private List<TMP_Dropdown.OptionData> CreateOptionList() { return CreateOptionList(null); }
    private List<TMP_Dropdown.OptionData> CreateOptionList(string[] options)
    {
        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
        if (options != null)
            foreach (var item in options)
                if (item == "None")
                    list.Add(new TMP_Dropdown.OptionData("None"));
                else
                    list.Add(new TMP_Dropdown.OptionData(AbilityToFullName(item)));
        else
            list = new List<TMP_Dropdown.OptionData> {
                new TMP_Dropdown.OptionData("None"),
                new TMP_Dropdown.OptionData("Strength"),
                new TMP_Dropdown.OptionData("Dexterity"),
                new TMP_Dropdown.OptionData("Constitution"),
                new TMP_Dropdown.OptionData("Intelligence"),
                new TMP_Dropdown.OptionData("Wisdom"),
                new TMP_Dropdown.OptionData("Charisma")};
        return (list);
    }

    private string AbilityToAbbr(string ablFullName)
    {
        if (ablFullName != "" && ablFullName != "None")
            return PF2E_DataBase.AbilityToAbbr(ablFullName);
        else
            return "";
    }

    private string AbilityToFullName(string ablAbbr)
    {
        if (ablAbbr != "" && ablAbbr != "None")
            return PF2E_DataBase.AbilityToFullName(ablAbbr);
        else
            return "";
    }

}
