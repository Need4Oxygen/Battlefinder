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
    [SerializeField] private TMP_InputField levelInput = null;
    [SerializeField] private TMP_InputField playerNameInput = null;
    [SerializeField] private TMP_Text HPText = null;
    [SerializeField] private TMP_Text ACText = null;
    [SerializeField] private TMP_Text perceptionText = null;

    [Header("Build")]
    [SerializeField] private Transform buildContainer = null;
    [SerializeField] private Transform buildLevelSeparator = null;
    [SerializeField] private Transform buildButton = null;

    private List<GameObject> buildButtonList = null;
    private PF2E_PlayerData initialPlayer = null;
    public PF2E_PlayerData currentPlayer = null;

    private bool blockInputRefresh;

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
            ABCSelector.CloseSelectorPanel();

            ABCSelector.acceptButton.onClick = null;
            ABCSelector.backButton.onClick = null;

            // hacer algo lol

        }
    }

    private void NewPlayerProccessBack()
    {
        if (ABCSelector.currentlyDisplaying == E_PF2E_ABC.Ancestry)
        {
            ABCSelector.acceptButton.onClick = null;
            ABCSelector.backButton.onClick = null;

            // FALTA UN DELETE PLAYER
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
        blockInputRefresh = true;

        levelInput.text = currentPlayer.level.ToString();
        playerNameInput.text = currentPlayer.playerName;

        // ABC


    }

    /// <summary> Refresh player data with stuf?. </summary>
    private void RefreshPanelIntoPlayer()
    {
        currentPlayer.level = int.Parse(levelInput.text);
        currentPlayer.playerName = playerNameInput.text;
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

    public void SelectAncestry(string newAncestry)
    {

    }

}
