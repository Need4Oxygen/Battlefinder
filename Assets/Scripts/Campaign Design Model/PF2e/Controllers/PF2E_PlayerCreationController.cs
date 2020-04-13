using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_PlayerCreationController : MonoBehaviour
{
    public PF2E_Controller controller = null;
    [SerializeField] private CanvasGroup playerCreationPanel = null;

    [Space(15)]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text playerNameText;

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
    }

    public void NewPlayer()
    {
        string newGuid = Guid.NewGuid().ToString();
        currentPlayer = new PF2E_PlayerData();
        currentPlayer.guid = newGuid;
        OpenPlayerCreationPanel();
    }

    public void LoadPlayer(PF2E_PlayerData player)
    {
        currentPlayer = player;
        OpenPlayerCreationPanel();
    }

    /// <summary> Refresh everything UI wise with player data. </summary>
    public void Refresh()
    {

        levelText.text = currentPlayer.level.ToString();
        playerNameText.text = currentPlayer.playerName;

    }

    /// <summary> Save Player into campaing player list. </summary>
    public void SavePlayer()
    {
        if (controller.currentCampaign.players.ContainsKey(currentPlayer.guid))
            controller.currentCampaign.players[currentPlayer.guid] = currentPlayer;
        else
            controller.currentCampaign.players[currentPlayer.guid] = currentPlayer;

        controller.RefreshCampaignContainers();
        controller.SaveCampaign();
    }

    public void OnClickSavePlayerButton()
    {

    }

}
