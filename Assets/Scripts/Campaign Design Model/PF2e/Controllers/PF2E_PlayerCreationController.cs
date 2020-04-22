using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_PlayerCreationController : MonoBehaviour
{
    public PF2E_Controller controller = null;
    [SerializeField] private ConfirmationController confirmation = null;
    [SerializeField] private CanvasGroup playerPanel = null;

    [Header("Build")]
    [SerializeField] private Transform buildContainer = null;
    [SerializeField] private Transform buildButton = null;
    [SerializeField] private List<GameObject> buildButtonList = null;

    [Space(15)]
    [SerializeField] private TMP_InputField levelInput = null;
    [SerializeField] private TMP_InputField playerNameInput = null;
    [SerializeField] private TMP_Text HPText = null;
    [SerializeField] private TMP_Text ACText = null;
    [SerializeField] private TMP_Text perceptionText = null;

    private PF2E_PlayerData initialPlayer = null;
    private PF2E_PlayerData currentPlayer = null;

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
        RefreshPlayerIntoPanel();
        OpenPlayerCreationPanel();
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
        levelInput.text = currentPlayer.level.ToString();
        playerNameInput.text = currentPlayer.playerName;
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

}
