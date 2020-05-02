using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_PlayerCreationController : MonoBehaviour
{
    public PF2E_Controller controller = null;
    [SerializeField] private PF2E_ABCSelector ABCSelector = null;
    [SerializeField] private ConfirmationController confirmation = null;
    [SerializeField] private CanvasGroup playerPanel = null;

    [Space(15)]
    [SerializeField] private PF2E_BuildButton ancestryButton = null;
    [SerializeField] private PF2E_BuildButton backgroundButton = null;
    [SerializeField] private PF2E_BuildButton classButton = null;

    [Space(15)]
    [SerializeField] private TMP_InputField levelInput = null;
    [SerializeField] private TMP_InputField playerNameInput = null;

    [SerializeField] private TMP_Text HPText = null;
    // [SerializeField] private TMP_Text ACText = null;
    [SerializeField] private TMP_Text perceptionText = null;
    [SerializeField] private TMP_Text speedText = null;
    [SerializeField] private TMP_Text sizeText = null;
    [SerializeField] private TMP_Text classDCText = null;
    [SerializeField] private TMP_Text traitsText = null;

    [Header("Build")]
    // [SerializeField] private Transform buildContainer = null;
    // [SerializeField] private Transform buildLevelSeparator = null;
    // [SerializeField] private Transform buildButton = null;

    // private List<GameObject> buildButtonList = null;
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
        controller.RefreshCampaignContainers();
        initialPlayer = null;
        currentPlayer = null;
    }

    public void OnClickSaveButton()
    {
        SavePlayer();
        ClosePlayerCreationPanel();
    }

    public void OnClickBackButton()
    {
        if (initialPlayer != currentPlayer)
            confirmation.AskForConfirmation("You have unsaved changes, do you really want to go back?", OnClickBackButtonCallback);
        else
            ClosePlayerCreationPanel();
    }
    private void OnClickBackButtonCallback(bool value)
    {
        if (value)
            ClosePlayerCreationPanel();
    }


    #region --------PLAYERS--------

    /// <summary> Generates new player and open the player panel. </summary>
    public void NewPlayer()
    {
        string newGuid = Guid.NewGuid().ToString();
        currentPlayer = new PF2E_PlayerData();
        currentPlayer.guid = newGuid;

        initialPlayer = currentPlayer;

        ABCSelector.OpenSelectorPanel();
        ABCSelector.Display(E_PF2E_ABC.Ancestry);
        ABCSelector.acceptButton.onClick.AddListener(() => NewPlayerProccessAccept());
        ABCSelector.backButton.onClick.AddListener(() => NewPlayerProccessBack());
    }

    private void NewPlayerProccessAccept()
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
            currentPlayer.playerClass = ABCSelector.selectedClass;
            GenericClose(true);
            OpenPlayerCreationPanel();
        }
    }

    private void NewPlayerProccessBack()
    {
        if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Ancestry)
        {
            currentPlayer = null;
            GenericClose(false);
        }
        else if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Background)
        {
            currentPlayer.background = "";
            ABCSelector.Display(E_PF2E_ABC.Ancestry);

        }
        else if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Class)
        {
            currentPlayer.playerClass = "";
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

    /// <summary> Refresh UI with player data. </summary>
    private void RefreshPlayerIntoPanel()
    {
        levelInput.SetTextWithoutNotify(currentPlayer.level.ToString());
        playerNameInput.SetTextWithoutNotify(currentPlayer.playerName);

        ancestryButton.subtitle.text = currentPlayer.ancestry;
        backgroundButton.subtitle.text = currentPlayer.background;
        classButton.subtitle.text = currentPlayer.playerClass;

        levelInput.text = currentPlayer.level.ToString();
        playerNameInput.text = currentPlayer.playerName;

        HPText.text = currentPlayer.hp_max.ToString();
        perceptionText.text = currentPlayer.perception_score.ToString();
        speedText.text = currentPlayer.speed_base.ToString();
        sizeText.text = currentPlayer.size;
        classDCText.text = currentPlayer.classDC.ToString();

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
    }

    /// <summary> Refresh player data with stuf?. </summary>
    private void RefreshPanelIntoPlayer()
    {
        // currentPlayer.level = int.Parse(levelInput.text);
        // currentPlayer.playerName = playerNameInput.text;
    }

    /// <summary> Save Player into campaing player list. </summary>
    private void SavePlayer()
    {
        if (controller.currentCampaign.players.ContainsKey(currentPlayer.guid))
            controller.currentCampaign.players[currentPlayer.guid] = currentPlayer;
        else
            controller.currentCampaign.players.Add(currentPlayer.guid, currentPlayer);

        controller.SaveCampaign();
    }

    #endregion


    #region --------BUTTONS & INPUT--------

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
        GenericClose(true);
    }
    private void SelectAncestryCancel()
    {
        GenericClose(false);
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
        GenericClose(true);
    }
    private void SelectBackgroundCancel()
    {
        GenericClose(false);
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
        currentPlayer.playerClass = ABCSelector.selectedClass;
        GenericClose(true);
    }
    private void SelectClassCancel()
    {
        GenericClose(false);
    }

    private void GenericClose(bool refreshPanels)
    {
        ABCSelector.CloseSelectorPanel();

        ABCSelector.acceptButton.onClick.RemoveAllListeners();
        ABCSelector.backButton.onClick.RemoveAllListeners();

        if (refreshPanels)
            RefreshPlayerIntoPanel();
    }

    #endregion

}
