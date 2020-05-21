using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_CharacterCreation : MonoBehaviour
{
    [Serializable] class SkillsWrapper { public List<Image> list = null; } // For Abilities serialization

    public DVoid OnCharacterCreationClose = null;

    [SerializeField] private PF2E_ABCSelector ABCSelector = null;
    [SerializeField] private PF2E_AblBoostsSelector ablBoostsSelector = null;
    [SerializeField] private CanvasGroup playerPanel = null;

    [Header("ABC Buttons")]
    [SerializeField] private PF2E_BuildButton ancestryButton = null;
    [SerializeField] private PF2E_BuildButton backgroundButton = null;
    [SerializeField] private PF2E_BuildButton classButton = null;

    [Header("Name & Level")]
    [SerializeField] private TMP_InputField levelInput = null;
    [SerializeField] private TMP_InputField playerNameInput = null;

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
    [SerializeField] private TMP_Text ACText = null;
    // Armor and Shield stuff goes here

    [Header("Perception & Saves")]
    [SerializeField] private PF2E_APICButton perceptionAPIC = null;
    [SerializeField] private PF2E_APICButton fortitudeAPIC = null;
    [SerializeField] private PF2E_APICButton reflexAPIC = null;
    [SerializeField] private PF2E_APICButton willAPIC = null;

    [Header("Random")]
    [SerializeField] private TMP_Text traitsText = null;
    [SerializeField] private List<SkillsWrapper> ablMapImages = null;
    [SerializeField] private List<PF2E_APICButton> skills = null;

    [Header("Build")]
    [SerializeField] private Transform buildContainer = null;
    [SerializeField] private Transform buildLevelSeparator = null;
    [SerializeField] private Transform buildButton = null;


    private List<GameObject> buildButtonList = new List<GameObject>();
    private PF2E_PlayerData initialPlayer = null;
    public PF2E_PlayerData currentPlayer = null;

    // private bool blockInputRefresh;

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(playerPanel.transform, playerPanel, 0.85f, 0f, 0f));
    }

    private void OpenPlayerCreationPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(playerPanel.transform, playerPanel, 1f, 1f, 0.1f));
    }

    private void ClosePlayerCreationPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(playerPanel.transform, playerPanel, 0.85f, 0f, 0.1f));
        initialPlayer = null;
        currentPlayer = null;

        if (OnCharacterCreationClose != null)
            OnCharacterCreationClose();
    }

    public void OnClickAcceptButton()
    {
        SavePlayer();
        ClosePlayerCreationPanel();
    }

    #region --------------------------------PLAYERS--------------------------------

    /// <summary> Generates new player and open the player panel. </summary>
    public void NewPlayer()
    {
        string newGuid = Guid.NewGuid().ToString();
        currentPlayer = new PF2E_PlayerData();
        currentPlayer.guid = newGuid;

        initialPlayer = currentPlayer;

        ABCSelector.OpenSelectorPanel();
        ABCSelector.Display(E_PF2E_ABC.Ancestry);
        ABCSelector.acceptButton.onClick.AddListener(() => NewPlayerProcessAccept());
        ABCSelector.backButton.onClick.AddListener(() => NewPlayerProcessBack());
    }

    private void NewPlayerProcessAccept()
    {
        if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Ancestry)
        {
            currentPlayer.ancestry = ABCSelector.selectedAncestry;
            ABCSelector.Display(E_PF2E_ABC.Background);
        }
        else if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Background)
        {
            currentPlayer.background = ABCSelector.selectedBackground;
            ABCSelector.Display(E_PF2E_ABC.Class);
        }
        else if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Class)
        {
            currentPlayer.class_name = ABCSelector.selectedClass;
            CloseABCPanel(true);
            OpenPlayerCreationPanel();
        }
    }

    private void NewPlayerProcessBack()
    {
        if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Ancestry)
        {
            currentPlayer = null;
            CloseABCPanel(false);
        }
        else if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Background)
        {
            currentPlayer.background = "";
            ABCSelector.Display(E_PF2E_ABC.Ancestry);

        }
        else if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Class)
        {
            currentPlayer.class_name = "";
            ABCSelector.Display(E_PF2E_ABC.Background);
        }
    }

    /// <summary> Load new player and open the player panel. </summary>
    public void LoadPlayer(PF2E_PlayerData player)
    {
        initialPlayer = player;
        currentPlayer = player;
        RefreshPlayerIntoPanel();
        OpenPlayerCreationPanel();
    }

    /// <summary> Save Player into campaing player list. </summary>
    public void SavePlayer()
    {
        if (PF2E_Globals.PF2eCurrentCampaign.players.ContainsKey(currentPlayer.guid))
            PF2E_Globals.PF2eCurrentCampaign.players[currentPlayer.guid] = currentPlayer;
        else
            PF2E_Globals.PF2eCurrentCampaign.players.Add(currentPlayer.guid, currentPlayer);

        PF2E_Globals.PF2E_SaveCampaign();
    }

    /// <summary> Refresh UI with player data. </summary>
    public void RefreshPlayerIntoPanel()
    {
        levelInput.SetTextWithoutNotify(currentPlayer.level.ToString());
        playerNameInput.SetTextWithoutNotify(currentPlayer.playerName);

        ancestryButton.subtitle.text = currentPlayer.ancestry;
        backgroundButton.subtitle.text = currentPlayer.background;
        classButton.subtitle.text = currentPlayer.class_name;

        HPCurrentText.text = currentPlayer.hp_current.ToString();
        HPMaxtText.text = currentPlayer.hp_max.ToString();
        HPTempText.text = currentPlayer.hp_temp.ToString();
        DyingMaxtText.text = currentPlayer.hp_dyingMax.ToString();

        ACText.text = currentPlayer.ac_score.ToString();

        perceptionAPIC.Refresh(currentPlayer.Perception_Get());
        fortitudeAPIC.Refresh(currentPlayer.Saves_Get("fortitude"));
        reflexAPIC.Refresh(currentPlayer.Saves_Get("reflex"));
        willAPIC.Refresh(currentPlayer.Saves_Get("will"));

        // Abilities
        Color active = Globals.Theme["text_2"]; Color unactive = Globals.Theme["background_1"];
        int[,] abl_map = currentPlayer.Abl_GetMap();
        for (int i = 0; i < abl_map.GetLength(0); i++)
            for (int j = 0; j < abl_map.GetLength(1); j++)
                if (abl_map[i, j] > 0)
                    ablMapImages[j].list[i].color = active;
                else if (abl_map[i, j] < 0)
                    ablMapImages[j].list[i].color = Color.red;
                else
                    ablMapImages[j].list[i].color = unactive;

        // Skills
        var list = currentPlayer.Skills_GetAllAsList();
        for (int i = 0; i < skills.Count; i++)
            skills[i].Refresh(list[i]);

        // Traits
        string traits = "";
        int count = 0; int total = traits.Length;
        foreach (var item in currentPlayer.traits_list)
        {
            if (count < total - 1)
                traits += item.name + ", ";
            else
                traits += item.name;
            count++;
        }
        traitsText.text = traits;

        // Build
        foreach (var item in buildButtonList)
        {
            item.gameObject.SetActive(false);
            Destroy(item.gameObject, 0.001f);
        }
        buildButtonList.Clear();
        foreach (var level in currentPlayer.build)
        {
            Transform separator = Instantiate(buildLevelSeparator, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(separator.gameObject);
            ButtonText separatorScript = separator.GetComponent<ButtonText>();
            separatorScript.text.text = level.Key;

            if (level.Value != null) // Separo choices de no choices
            {
                List<PF2E_BuildItem> choices = new List<PF2E_BuildItem>();
                List<PF2E_BuildItem> noChoices = new List<PF2E_BuildItem>();

                foreach (var item in level.Value)
                    if (item.Value.choice)
                        choices.Add(item.Value);
                    else
                        noChoices.Add(item.Value);

                foreach (var item in choices)
                    ChoiceButtonsAssigner(item, GenerateBuildButton());
                foreach (var item in noChoices)
                    NoChoiceButtonsAssigner(item, GenerateBuildButton());
            }
        }
    }

    private PF2E_BuildButton GenerateBuildButton()
    {
        Transform button = Instantiate(buildButton, Vector3.zero, Quaternion.identity, buildContainer);
        buildButtonList.Add(button.gameObject);
        return button.GetComponent<PF2E_BuildButton>();
    }

    private void ChoiceButtonsAssigner(PF2E_BuildItem buildItem, PF2E_BuildButton button)
    {
        if (buildItem.type == "Initial Ability Boosts")
        {
            PF2E_InitAblBoostData initAblData = currentPlayer.Build_Get<PF2E_InitAblBoostData>("Level 1", "Initial Ability Boosts");

            button.title.text = buildItem.type;
            button.button.onClick.AddListener(() => OnClickInitialAbilityBoosts());

            int x = initAblData.lvl1boosts.Count - 4;
            if (x < 0)
                button.subtitle.text = "Boosts not assigned: " + (x * -1);
            else
                button.subtitle.text = "Everything is correct.";
        }
        else
        {
            Debug.LogWarning("[Creation] Choice button: " + buildItem.type + " miss functionality!");

            button.title.text = buildItem.type;
            button.subtitle.text = buildItem.value;
        }
    }

    private void NoChoiceButtonsAssigner(PF2E_BuildItem buildItem, PF2E_BuildButton button)
    {
        button.title.text = buildItem.type;
        button.subtitle.text = buildItem.value;
    }

    private void OnClickInitialAbilityBoosts()
    {
        ablBoostsSelector.OpenPlayerInitialAblBoostsPanel();
    }

    #endregion


    #region --------------------------------BUTTONS & INPUT--------------------------------

    /// <summary> Called by the level inputfiedld. </summary>
    public void OnEndEditLevel()
    {
        currentPlayer.level = int.Parse(levelInput.text);
        RefreshPlayerIntoPanel();
    }

    /// <summary> Called by the name inputfiedld. </summary>
    public void OnEndEditName()
    {
        currentPlayer.playerName = playerNameInput.text;
        RefreshPlayerIntoPanel();
    }

    public void OnEndEditDamage()
    {
        int value = 0; int.TryParse(damageInput.text, out value);
        currentPlayer.hp_damage = value;
        RefreshPlayerIntoPanel();
    }
    public void OnEndEditDying()
    {
        int value = 0; int.TryParse(dyingInput.text, out value);
        currentPlayer.hp_dyingCurrent = value;
        RefreshPlayerIntoPanel();
    }
    public void OnEndEditWounds()
    {
        int value = 0; int.TryParse(woundsInput.text, out value);
        currentPlayer.hp_wounds = value;
        RefreshPlayerIntoPanel();
    }
    public void OnEndEditDoom()
    {
        int value = 0; int.TryParse(doomInput.text, out value);
        currentPlayer.hp_doom = value;
        RefreshPlayerIntoPanel();
    }

    /// <summary> Called by the ancestry selection button. </summary>
    public void OnClickSelectAncestry()
    {
        ABCSelector.OpenSelectorPanel();
        ABCSelector.Display(E_PF2E_ABC.Ancestry);
        ABCSelector.acceptButton.onClick.AddListener(() => SelectAncestryAccept());
        ABCSelector.backButton.onClick.AddListener(() => SelectAncestryCancel());
    }
    private void SelectAncestryAccept()
    {
        currentPlayer.ancestry = ABCSelector.selectedAncestry;
        CloseABCPanel(true);
    }
    private void SelectAncestryCancel()
    {
        CloseABCPanel(false);
    }

    /// <summary> Called by the backgrond selection button. </summary>
    public void OnClickSelectBackground()
    {
        ABCSelector.OpenSelectorPanel();
        ABCSelector.Display(E_PF2E_ABC.Background);
        ABCSelector.acceptButton.onClick.AddListener(() => SelectBackgroundAccept());
        ABCSelector.backButton.onClick.AddListener(() => SelectBackgroundCancel());
    }
    private void SelectBackgroundAccept()
    {
        currentPlayer.background = ABCSelector.selectedBackground;
        CloseABCPanel(true);
    }
    private void SelectBackgroundCancel()
    {
        CloseABCPanel(false);
    }

    /// <summary> Called by the class selection button. </summary>
    public void OnClickSelectedClass()
    {
        ABCSelector.OpenSelectorPanel();
        ABCSelector.Display(E_PF2E_ABC.Class);
        ABCSelector.acceptButton.onClick.AddListener(() => SelectClassAccept());
        ABCSelector.backButton.onClick.AddListener(() => SelectClassCancel());
    }
    private void SelectClassAccept()
    {
        currentPlayer.class_name = ABCSelector.selectedClass;
        CloseABCPanel(true);
    }
    private void SelectClassCancel()
    {
        CloseABCPanel(false);
    }

    private void CloseABCPanel(bool refreshPanels)
    {
        ABCSelector.CloseSelectorPanel();

        ABCSelector.acceptButton.onClick.RemoveAllListeners();
        ABCSelector.backButton.onClick.RemoveAllListeners();

        if (refreshPanels)
            RefreshPlayerIntoPanel();
    }

    #endregion

}
