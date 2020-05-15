using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_CampaingHandler : MonoBehaviour
{
    // Controls the PF2e campaing retrieval and send thems to load and display
    // Controls the PF2e board, player, enemies and npcs buttons

    [SerializeField] private PF2E_CharacterCreation characterCreation = null;
    [SerializeField] private ConfirmationController confirmation = null;
    [SerializeField] private GameObject cameraBlur = null;

    [Header("Campaign Stuff")]
    [SerializeField] private CanvasGroup campaignPanel = null;
    [SerializeField] private TMP_Text campaignName = null;
    [SerializeField] private Transform boardsContainer = null;
    [SerializeField] private Transform playersContainer = null;
    [SerializeField] private Transform enemiesContainer = null;
    [SerializeField] private Transform npcsContainer = null;
    [Space(15)]
    [SerializeField] private GameObject boardsButtonPrefab = null;
    [SerializeField] private GameObject playersButtonPrefab = null;
    [SerializeField] private GameObject enemiesButtonPrefab = null;
    [SerializeField] private GameObject npcsButtonPrefab = null;

    private List<GameObject> boardsButtonList = new List<GameObject>();
    private List<GameObject> playersButtonList = new List<GameObject>();
    private List<GameObject> enemiesButtonList = new List<GameObject>();
    private List<GameObject> npcsButtonList = new List<GameObject>();

    void Awake()
    {
        Globals.PF2eCampaignIDs = Json.LoadFromPlayerPrefs<List<PF2E_CampaignID>>("PF2e_campaignsIDList");
        if (Globals.PF2eCampaignIDs == null)
            Globals.PF2eCampaignIDs = new List<PF2E_CampaignID>();
    }

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0f));

        characterCreation.OnCharacterCreationClose += RefreshCampaignContainers;
    }

    public void OnClickBackButton()
    {
        CloseCampaingPanel();
    }

    public void OnClickDeleteButton()
    {
        DeleteCampaign();
    }

    #region --------CAMPAIGNS--------

    private void OpenCampaingPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 1f, 1f, 0.1f));
        cameraBlur.SetActive(true);
    }

    private void CloseCampaingPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0.1f));
        cameraBlur.SetActive(false);
    }

    /// <summary> Called after asking the name for a new campaign has been promped and accepted. </summary>
    public void CreateCampaign(string name)
    {
        Globals.CreateCampaign(name);
    }

    /// <summary> Load given Campaign. Called by campaign buttons that enumerate existing Campaigns. </summary>
    public void LoadCampaign(PF2E_CampaignID campaignID)
    {
        // Open Campaign Panel
        campaignName.text = campaignID.name;

        // Set current campaing and refresh boards, players, enemies and npcs
        Globals.LoadCampaign(campaignID);
        RefreshCampaignContainers();

        OpenCampaingPanel();
    }

    // Called by Delete Button in PF2E Campaign Panel
    private void DeleteCampaign()
    {
        confirmation.AskForConfirmation("Do you want to delete current campaign?", DeleteCampaignCallback);
    }
    private void DeleteCampaignCallback(bool value)
    {
        if (value)
        {
            CloseCampaingPanel();
            Globals.DeleteCampaign();
        }
    }

    #endregion


    #region --------PLAYERS--------

    // Clicking the + button in the players section of the campaing panel
    public void OnClickNewPlayerButton()
    {
        characterCreation.NewPlayer();
    }

    // Edition
    private void OnClickPayerButtonEdit(string player)
    {
        characterCreation.LoadPlayer(Globals.PF2eCurrentCampaign.players[player]);
    }

    // Deletion
    private string playerToDelete = "";
    private void OnClickPayerButtonDelete(string player)
    {
        playerToDelete = player;
        confirmation.AskForConfirmation("Are you sure you want to delete this character?", OnClickPayerButtonDeleteCallback);
    }
    private void OnClickPayerButtonDeleteCallback(bool value)
    {
        if (value)
        {
            Globals.PF2eCurrentCampaign.players.Remove(playerToDelete);
            RefreshCampaignContainers();
            Globals.SaveCampaign();
        }

        playerToDelete = "";
    }

    public void RefreshCampaignContainers()
    {
        // Delete all buttons
        foreach (var button in boardsButtonList)
            Destroy(button, 0.001f);
        foreach (var button in playersButtonList)
            Destroy(button, 0.001f);
        foreach (var button in enemiesButtonList)
            Destroy(button, 0.001f);
        foreach (var button in npcsButtonList)
            Destroy(button, 0.001f);

        boardsButtonList.Clear();
        playersButtonList.Clear();
        enemiesButtonList.Clear();
        npcsButtonList.Clear();

        for (int i = 0; i < Globals.PF2eCurrentCampaign.boards.Count; i++)
        {
            Transform newBoardButton = Instantiate(boardsButtonPrefab, Vector3.zero, Quaternion.identity, boardsContainer).transform;
            PF2E_BoardButton newBoardButtonScript = newBoardButton.GetComponent<PF2E_BoardButton>();

            boardsButtonList.Add(newBoardButton.gameObject);
        }
        foreach (var player in Globals.PF2eCurrentCampaign.players)
        {
            Transform newPlayerButton = Instantiate(playersButtonPrefab, Vector3.zero, Quaternion.identity, playersContainer).transform;
            PF2E_PlayerButton newPlayerButtonScript = newPlayerButton.GetComponent<PF2E_PlayerButton>();
            newPlayerButtonScript.level.text = player.Value.level.ToString();
            newPlayerButtonScript.playerName.text = player.Value.playerName;

            newPlayerButtonScript.editButton.onClick.AddListener(() => OnClickPayerButtonEdit(player.Key));
            newPlayerButtonScript.deleteButton.onClick.AddListener(() => OnClickPayerButtonDelete(player.Key));

            playersButtonList.Add(newPlayerButton.gameObject);
        }
        for (int i = 0; i < Globals.PF2eCurrentCampaign.enemies.Count; i++)
        {
            Transform newEnemyButton = Instantiate(enemiesButtonPrefab, Vector3.zero, Quaternion.identity, enemiesContainer).transform;
            PF2E_BoardButton newEnemyButtonScript = newEnemyButton.GetComponent<PF2E_BoardButton>();

            enemiesButtonList.Add(newEnemyButton.gameObject);
        }
        for (int i = 0; i < Globals.PF2eCurrentCampaign.npcs.Count; i++)
        {
            Transform newNPCButton = Instantiate(npcsButtonPrefab, Vector3.zero, Quaternion.identity, npcsContainer).transform;
            PF2E_BoardButton newNPCButtonScript = newNPCButton.GetComponent<PF2E_BoardButton>();

            npcsButtonList.Add(newNPCButton.gameObject);
        }
    }

    #endregion

}
