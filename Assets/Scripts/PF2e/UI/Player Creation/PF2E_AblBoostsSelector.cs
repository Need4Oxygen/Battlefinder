using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_AblBoostsSelector : MonoBehaviour
{
    [SerializeField] private PF2E_CharacterCreation creation;
    [SerializeField] private Transform dropdownPrefab = null;

    [Header("Initial Abilities Boosts")]
    [SerializeField] private CanvasGroup iAblBoostsPanel = null;
    // [SerializeField] private GameObject iAblBoostsAlert = null;
    [SerializeField] private Transform ancestryContainer = null;
    [SerializeField] private Toggle[] level1Toggles = null;

    private List<string> ancestryBoosts = new List<string>();
    private List<string> ancestryFlaws = new List<string>();
    private List<TMP_Dropdown> ancestryFreeAssignsDropdowns = new List<TMP_Dropdown>();

    // [Header("Other Abilities Boosts")]
    // [SerializeField] private CanvasGroup ablBoostsPanel = null;
    // [SerializeField] private Button ablBoostsAcceptButotn = null;
    // [SerializeField] private Toggle[] toggles = null;


    private PF2E_InitAblBoostData currentData = null;
    private List<TMP_Dropdown> ancestryDrops = new List<TMP_Dropdown>();

    void Start()
    {
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
        // AssignBackgroundBoosts();
        // AssignClassBoosts();
    }

    private TMP_Dropdown GenerateDropdown(Transform parent)
    {
        Transform drop = Instantiate(dropdownPrefab, Vector3.zero, Quaternion.identity, parent);
        TMP_Dropdown dropdown = drop.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        return dropdown;
    }

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
            alreadyBoosted.Add(PF2E_DataBase.AbilityToFullName(item.Value.target));
        foreach (var item in ancestry.abilityFlaws)
            alreadyBoosted.Add(PF2E_DataBase.AbilityToFullName(item.Value.target));

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
            optionList.RemoveAll(i => alreadyBoosted.Contains(i.text));
            drop.AddOptions(optionList);
            drop.interactable = true;
            drop.onValueChanged.AddListener((v) => OnValueChangedAncestryDropdown(v));
            alreadyBoosted.Add(PF2E_DataBase.AbilityToFullName(item.Value.target));

            // Select dropdown value corresponding to last time
            string shouldSelect = ""; int shouldSelectIndex = 0;
            if (counter < currentData.ancestryFree.Count)
                shouldSelect = currentData.ancestryFree[counter];
            if (shouldSelect != "" && shouldSelect != "None" && shouldSelect != "Null" && shouldSelect != null)
                shouldSelectIndex = optionList.FindIndex(0, optionList.Count, i => i.text == PF2E_DataBase.AbilityToFullName(shouldSelect));
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


    public void OnValueChangedAncestryDropdown(int value)
    {
        SaveAncestryOptions();
    }

    public void SaveAncestryOptions()
    {
        currentData.ancestryBoosts.Clear();
        currentData.ancestryFlaws.Clear();
        currentData.ancestryFree.Clear();

        foreach (var item in ancestryBoosts)
            currentData.ancestryBoosts.Add(item);
        foreach (var item in ancestryFlaws)
            currentData.ancestryFlaws.Add(item);
        foreach (var item in ancestryFreeAssignsDropdowns)
            currentData.ancestryFree.Add(PF2E_DataBase.AbilityToAbbr(item.captionText.text));

        AssignAncestryBoosts();
    }

    public void OnClickAcceptButton()
    {
        creation.currentPlayer.Build_Set("Level 1", "Initial Ability Boosts", currentData);
        creation.currentPlayer.Build_Refresh();
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
                list.Add(new TMP_Dropdown.OptionData(PF2E_DataBase.AbilityToFullName(item)));
        else
            list = new List<TMP_Dropdown.OptionData> {
                new TMP_Dropdown.OptionData("None"),
                new TMP_Dropdown.OptionData("Strength"),
                new TMP_Dropdown.OptionData("Dexterity"),
                new TMP_Dropdown.OptionData("Constitution"),
                new TMP_Dropdown.OptionData("Intelligence"),
                new TMP_Dropdown.OptionData("Wisdom"),
                new TMP_Dropdown.OptionData("Charisma")
                };
        return (list);
    }
}

public class PF2E_InitAblBoostData
{
    public string name = "";
    public List<string> ancestryBoosts = new List<string>();
    public List<string> ancestryFlaws = new List<string>();
    public List<string> ancestryFree = new List<string>();

    public List<string> lvl1boosts = new List<string>();
}

public class PF2E_AblBoostData
{
    public string name = "";
    public List<string> boosts = new List<string>();
}

