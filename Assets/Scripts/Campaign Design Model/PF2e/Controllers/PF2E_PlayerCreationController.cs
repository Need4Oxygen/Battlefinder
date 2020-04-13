using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_PlayerCreationController : MonoBehaviour
{
    public PF2E_Controller controller = null;
    [SerializeField] private ConfirmationController confirmation = null;
    [SerializeField] private CanvasGroup playerCreationPanel = null;

    [Space(15)]
    [SerializeField] private TMP_InputField levelText = null;
    [SerializeField] private TMP_InputField playerNameText = null;

    public PF2E_PlayerData initialPlayer = null;
    public PF2E_PlayerData currentPlayer = null;

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(playerCreationPanel.transform, playerCreationPanel, 0.85f, 0f, 0f));
    }

    private void OpenPlayerCreationPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(playerCreationPanel.transform, playerCreationPanel, 1f, 1f, 0.1f));
    }

    private void ClosePlayerCreationPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(playerCreationPanel.transform, playerCreationPanel, 0.85f, 0f, 0.1f));

        initialPlayer = null;
        currentPlayer = null;
    }

    public void OnClickSaveButton()
    {
        RefreshPanelIntoPlayer(); // esto se tendría que hacer dinámicamente al cambiar cosas o con un botón de refresh

        // if (initialPlayer != currentPlayer)
        {
            SavePlayer();
            ClosePlayerCreationPanel();
        }
        // else
        {
            // ClosePlayerCreationPanel();
        }
    }

    public void OnClickBackButton()
    {
        if (initialPlayer != currentPlayer)
            confirmation.AskForConfirmation("You have unsaved changes, do you really want to go back?", OnClickBackButtonCallback);
        else
            ClosePlayerCreationPanel();
    }
    public void OnClickBackButtonCallback(bool value)
    {
        if (value)
            ClosePlayerCreationPanel();
    }

    public void NewPlayer()
    {
        string newGuid = Guid.NewGuid().ToString();
        currentPlayer = new PF2E_PlayerData();
        currentPlayer.guid = newGuid;
        initialPlayer = currentPlayer;
        RefreshPlayerIntoPanel();
        OpenPlayerCreationPanel();
    }

    public void LoadPlayer(PF2E_PlayerData player)
    {
        initialPlayer = player;
        currentPlayer = player;
        OpenPlayerCreationPanel();
    }

    /// <summary> Refresh everything UI wise with player data. </summary>
    public void RefreshPlayerIntoPanel()
    {
        levelText.text = currentPlayer.level.ToString();
        playerNameText.text = currentPlayer.playerName;
    }

    /// <summary> Refresh everything UI wise with player data. </summary>
    public void RefreshPanelIntoPlayer()
    {
        currentPlayer.level = int.Parse(levelText.text);
        currentPlayer.playerName = playerNameText.text;
    }

    /// <summary> Save Player into campaing player list. </summary>
    public void SavePlayer()
    {
        if (controller.currentCampaign.players.ContainsKey(currentPlayer.guid))
            controller.currentCampaign.players[currentPlayer.guid] = currentPlayer;
        else
            controller.currentCampaign.players.Add(currentPlayer.guid, currentPlayer);

        controller.RefreshCampaignContainers();
        controller.SaveCampaign();
    }

}
