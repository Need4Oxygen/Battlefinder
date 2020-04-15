using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_Controller : MonoBehaviour
{
    public static List<PF2E_CampaignID> PF2eCampaignIDs;

    [SerializeField] private PF2E_PlayerCreationController playerCreationController = null;
    [SerializeField] private ConfirmationController confirmation = null;
    [SerializeField] private GameObject cameraBlur = null;
    [SerializeField] private CanvasGroup campaignPanel = null;
    [SerializeField] private TMP_Text campaignName = null;

    [Space(15)]
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

    public PF2E_CampaignID currentCampaignID = null;
    public PF2E_CampaignData currentCampaign = null;

    void Awake()
    {
        PF2eCampaignIDs = Json.LoadFromPlayerPrefs<List<PF2E_CampaignID>>("PF2e_campaignsIDList");
        if (PF2eCampaignIDs == null)
            PF2eCampaignIDs = new List<PF2E_CampaignID>();
    }

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0f));
    }

    #region Input
    public void OnClickBackButton()
    {
        CloseCampaingPanel();
    }

    public void OnClickDeleteButton()
    {
        DeleteCampaign();
    }

    public void OnClickNewPlayerButton()
    {
        playerCreationController.NewPlayer();
    }
    #endregion

    #region Campaigns
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

    public void CreateCampaign(string name)
    {
        string newGuid = Guid.NewGuid().ToString();

        PF2E_CampaignID newCampaignID = new PF2E_CampaignID();
        newCampaignID.guid = newGuid;
        newCampaignID.name = name;
        newCampaignID.order = 0;

        PF2E_CampaignData newCampaignData = new PF2E_CampaignData(newGuid, name, E_Games.PF2E);

        PF2eCampaignIDs.Add(newCampaignID);
        Json.SaveInPlayerPrefs("PF2e_campaignsIDList", PF2eCampaignIDs);
        Json.SaveInPlayerPrefs(newGuid, newCampaignData);
        PlayerPrefs.Save();
    }

    /// <summary> Load given Campaign. Called by UI buttons that enumerate existing Campaigns. </summary>
    public void LoadCampaign(PF2E_CampaignID campaignID)
    {
        // Open Campaign Panel
        campaignName.text = campaignID.name;

        // Set current campaing and refresh boards, players, enemies and npcs
        currentCampaignID = campaignID;
        currentCampaign = Json.LoadFromPlayerPrefs<PF2E_CampaignData>(campaignID.guid);
        RefreshCampaignContainers();

        OpenCampaingPanel();
    }

    /// <summary> Save current Campaign. </summary>
    public void SaveCampaign()
    {
        Json.SaveInPlayerPrefs(currentCampaignID.guid, currentCampaign);
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
            string guid = currentCampaignID.guid;
            PF2eCampaignIDs.Remove(currentCampaignID);
            Json.SaveInPlayerPrefs("PF2e_campaignsIDList", PF2eCampaignIDs);
            PlayerPrefs.DeleteKey(guid);
            PlayerPrefs.Save();
        }
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

        for (int i = 0; i < currentCampaign.boards.Count; i++)
        {
            Transform newBoardButton = Instantiate(boardsButtonPrefab, Vector3.zero, Quaternion.identity, boardsContainer).transform;
            PF2E_BoardButton newBoardButtonScript = newBoardButton.GetComponent<PF2E_BoardButton>();

            boardsButtonList.Add(newBoardButton.gameObject);
        }
        foreach (var player in currentCampaign.players)
        {
            Transform newPlayerButton = Instantiate(playersButtonPrefab, Vector3.zero, Quaternion.identity, playersContainer).transform;
            PF2E_PlayerButton newPlayerButtonScript = newPlayerButton.GetComponent<PF2E_PlayerButton>();
            newPlayerButtonScript.level.text = player.Value.level.ToString();
            newPlayerButtonScript.playerName.text = player.Value.playerName;
            // falta meterle el callback al botón para definir lo que pasa cuando lo pulsas el botón o los otros de editar y eliminar...
            newPlayerButtonScript.editButton.onClick.AddListener(() => OnClickPayerButtonEdit(player.Key));
            newPlayerButtonScript.deleteButton.onClick.AddListener(() => OnClickPayerButtonDelete(player.Key));

            playersButtonList.Add(newPlayerButton.gameObject);
        }
        for (int i = 0; i < currentCampaign.enemies.Count; i++)
        {
            Transform newEnemyButton = Instantiate(enemiesButtonPrefab, Vector3.zero, Quaternion.identity, enemiesContainer).transform;
            PF2E_BoardButton newEnemyButtonScript = newEnemyButton.GetComponent<PF2E_BoardButton>();

            enemiesButtonList.Add(newEnemyButton.gameObject);
        }
        for (int i = 0; i < currentCampaign.npcs.Count; i++)
        {
            Transform newNPCButton = Instantiate(npcsButtonPrefab, Vector3.zero, Quaternion.identity, npcsContainer).transform;
            PF2E_BoardButton newNPCButtonScript = newNPCButton.GetComponent<PF2E_BoardButton>();

            npcsButtonList.Add(newNPCButton.gameObject);
        }
    }

    // Send player to PlayerCreation to edit
    public void OnClickPayerButtonEdit(string player)
    {
        playerCreationController.LoadPlayer(currentCampaign.players[player]);
    }

    // Controlls player deletion
    private string playerToDelete = "";
    public void OnClickPayerButtonDelete(string player)
    {
        playerToDelete = player;
        confirmation.AskForConfirmation("Are you sure you want to delete this character?", OnClickPayerButtonDeleteCallback);
    }
    public void OnClickPayerButtonDeleteCallback(bool value)
    {
        if (value)
        {
            currentCampaign.players.Remove(playerToDelete);
            RefreshCampaignContainers();
            SaveCampaign();
        }
        else
            playerToDelete = "";
    }
}
