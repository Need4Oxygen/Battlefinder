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
    [SerializeField] private TMP_InputField levelInput = null;
    [SerializeField] private TMP_InputField playerNameInput = null;
    [SerializeField] private TMP_Text HPText = null;
    [SerializeField] private TMP_Text ACText = null;
    [SerializeField] private TMP_Text perceptionText = null;

    [Space(15)]
    [SerializeField] private PF2E_PCAbility stregth = null;
    [SerializeField] private PF2E_PCAbility dexterity = null;
    [SerializeField] private PF2E_PCAbility constitution = null;
    [SerializeField] private PF2E_PCAbility intelligence = null;
    [SerializeField] private PF2E_PCAbility wisdom = null;
    [SerializeField] private PF2E_PCAbility charisma = null;

    private PF2E_PlayerData initialPlayer = null;
    private PF2E_PlayerData currentPlayer = null;

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

    /// <summary> Executed whenever. </summary>
    public void OnValueChanged()
    {
        RefreshPanelIntoPlayer();
    }

    public void OnValueAbilityChanged(E_PF2E_Ability ability, int score)
    {
        // currentPlayer.abilities[ability] = score;
        RefreshPanelIntoPlayer();
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
    private void RefreshPlayerIntoPanel()
    {
        levelInput.text = currentPlayer.level.ToString();
        playerNameInput.text = currentPlayer.playerName;

        // stregth.score = currentPlayer.abilities[E_PF2E_Ability.Strength];
        // dexterity.score = currentPlayer.abilities[E_PF2E_Ability.Dexterity];
        // constitution.score = currentPlayer.abilities[E_PF2E_Ability.Constitution];
        // intelligence.score = currentPlayer.abilities[E_PF2E_Ability.Intelligence];
        // wisdom.score = currentPlayer.abilities[E_PF2E_Ability.Wisdom];
        // charisma.score = currentPlayer.abilities[E_PF2E_Ability.Charisma];
    }

    /// <summary> Refresh everything UI wise with player data. </summary>
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

        controller.RefreshCampaignContainers();
        controller.SaveCampaign();
    }

}
