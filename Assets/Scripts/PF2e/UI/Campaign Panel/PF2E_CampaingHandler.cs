using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_CampaingHandler : MonoBehaviour
{
    // Controls the PF2e campaing retrieval and send thems to load and display
    // Controls the PF2e board, player, enemies and npcs buttons

    [SerializeField] private PF2E_CharacterCreation characterCreation = null;
    [SerializeField] private PF2E_BoardHandler boardHandler = null;
    [SerializeField] private ConfirmationController confirmation = null;

    [Header("Campaign Stuff")]
    [SerializeField] private CanvasGroup campaignPanel = null;
    [SerializeField] private TMP_Text campaignName = null;

    [Header("Boards")]
    [SerializeField] private Transform boardsContainer = null;
    [SerializeField] private GameObject boardsButtonPrefab = null;
    private List<GameObject> boardsButtonList = new List<GameObject>();

    [Header("Players")]
    [SerializeField] private Transform playersContainer = null;
    [SerializeField] private GameObject playersButtonPrefab = null;
    private List<GameObject> playersButtonList = new List<GameObject>();

    [Header("Enemies")]
    [SerializeField] private Transform enemiesContainer = null;
    [SerializeField] private GameObject enemiesButtonPrefab = null;
    private List<GameObject> enemiesButtonList = new List<GameObject>();

    [Header("NPCs")]
    [SerializeField] private Transform npcsContainer = null;
    [SerializeField] private GameObject npcsButtonPrefab = null;
    private List<GameObject> npcsButtonList = new List<GameObject>();

    void Awake()
    {
        PF2E_Globals.CampaignIDS = Json.LoadFromPlayerPrefs<List<PF2E_CampaignID>>("PF2e_campaignsIDList");
        if (PF2E_Globals.CampaignIDS == null)
            PF2E_Globals.CampaignIDS = new List<PF2E_CampaignID>();
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
    }

    private void CloseCampaingPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0.1f));
    }

    /// <summary> Called after asking the name for a new campaign has been promped and accepted. </summary>
    public void CreateCampaign(string name)
    {
        PF2E_Globals.CreateCampaign(name);
    }

    /// <summary> Load given Campaign. Called by campaign buttons that enumerate existing Campaigns. </summary>
    public void LoadCampaign(PF2E_CampaignID campaignID)
    {
        // Open Campaign Panel
        campaignName.text = campaignID.name;

        // Set current campaing and refresh boards, players, enemies and npcs
        PF2E_Globals.LoadCampaign(campaignID);
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
            PF2E_Globals.DeleteCampaign();
        }
    }

    #endregion

    #region --------BOARDS--------

    /// <summary> Called by the + button in the boards section of the campaign panel </summary>
    public void OnClickNewBoardButton()
    {
        boardHandler.NewBoard();
    }

    // Edition
    private void OnClickBoardButtonPlay(string board)
    {
        boardHandler.LoadBoard(PF2E_Globals.CurrentCampaign.boards[board]);
    }

    // Deletion
    private string boardToDelete = "";
    private void OnClickBoardButtonDelete(string board)
    {
        boardToDelete = board;
        confirmation.AskForConfirmation("Are you sure you want to delete this board?", OnClickBoardButtonDeleteCallback);
    }
    private void OnClickBoardButtonDeleteCallback(bool value)
    {
        if (value)
        {
            PF2E_Globals.CurrentCampaign.boards.Remove(boardToDelete);
            RefreshCampaignContainers();
            PF2E_Globals.SaveCampaign();
        }

        boardToDelete = "";
    }

    #endregion

    #region --------PLAYERS--------

    /// <summary> Called by the + button in the players section of the campaign panel </summary>
    public void OnClickNewPlayerButton()
    {
        characterCreation.NewPlayer();
    }

    // Edition
    private void OnClickPayerButtonEdit(string player)
    {
        characterCreation.LoadPlayer(PF2E_Globals.CurrentCampaign.players[player]);
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
            PF2E_Globals.CurrentCampaign.players.Remove(playerToDelete);
            RefreshCampaignContainers();
            PF2E_Globals.SaveCampaign();
        }

        playerToDelete = "";
    }

    #endregion



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

        if (PF2E_Globals.CurrentCampaign.boards != null)
            foreach (var board in PF2E_Globals.CurrentCampaign.boards)
            {
                Transform newBoardButton = Instantiate(boardsButtonPrefab, Vector3.zero, Quaternion.identity, boardsContainer).transform;
                PF2E_BoardButton newBoardButtonScript = newBoardButton.GetComponent<PF2E_BoardButton>();
                newBoardButtonScript.boardName.text = board.Value.boardName;

                newBoardButtonScript.playButton.onClick.AddListener(() => OnClickBoardButtonPlay(board.Key));
                newBoardButtonScript.deleteButton.onClick.AddListener(() => OnClickBoardButtonDelete(board.Key));

                boardsButtonList.Add(newBoardButton.gameObject);
            }
        if (PF2E_Globals.CurrentCampaign.players != null)
            foreach (var player in PF2E_Globals.CurrentCampaign.players)
            {
                Transform newPlayerButton = Instantiate(playersButtonPrefab, Vector3.zero, Quaternion.identity, playersContainer).transform;
                PF2E_PlayerButton newPlayerButtonScript = newPlayerButton.GetComponent<PF2E_PlayerButton>();
                newPlayerButtonScript.level.text = player.Value.level.ToString();
                newPlayerButtonScript.playerName.text = player.Value.playerName;

                newPlayerButtonScript.editButton.onClick.AddListener(() => OnClickPayerButtonEdit(player.Key));
                newPlayerButtonScript.deleteButton.onClick.AddListener(() => OnClickPayerButtonDelete(player.Key));

                playersButtonList.Add(newPlayerButton.gameObject);
            }
        for (int i = 0; i < PF2E_Globals.CurrentCampaign.enemies.Count; i++)
        {
            Transform newEnemyButton = Instantiate(enemiesButtonPrefab, Vector3.zero, Quaternion.identity, enemiesContainer).transform;
            PF2E_BoardButton newEnemyButtonScript = newEnemyButton.GetComponent<PF2E_BoardButton>();

            enemiesButtonList.Add(newEnemyButton.gameObject);
        }
        for (int i = 0; i < PF2E_Globals.CurrentCampaign.npcs.Count; i++)
        {
            Transform newNPCButton = Instantiate(npcsButtonPrefab, Vector3.zero, Quaternion.identity, npcsContainer).transform;
            PF2E_BoardButton newNPCButtonScript = newNPCButton.GetComponent<PF2E_BoardButton>();

            npcsButtonList.Add(newNPCButton.gameObject);
        }
    }


}
