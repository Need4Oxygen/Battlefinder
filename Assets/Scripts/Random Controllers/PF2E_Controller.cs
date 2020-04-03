using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_Controller : MonoBehaviour
{

    #region Campaign Logic

    public static List<PF2E_CampaignID> PF2eCampaignIDs;

    [SerializeField] private ConfirmationController confirmation = null;

    [SerializeField] private CanvasGroup campaignPanel = null;
    [SerializeField] private TMP_Text campaignName = null;

    [SerializeField] private Transform boardsCreationPanel = null;
    [SerializeField] private Transform playersCreationPanel = null;
    [SerializeField] private Transform enemiesCreationPanel = null;
    [SerializeField] private Transform npcsCreationPanel = null;

    [SerializeField] private Transform boardsContainer = null;
    [SerializeField] private Transform playersContainer = null;
    [SerializeField] private Transform enemiesContainer = null;
    [SerializeField] private Transform npcsContainer = null;

    [SerializeField] private GameObject boardsButtonPrefab = null;
    [SerializeField] private GameObject playersButtonPrefab = null;
    [SerializeField] private GameObject enemiesButtonPrefab = null;
    [SerializeField] private GameObject npcsButtonPrefab = null;

    private List<GameObject> boardsButtonList = new List<GameObject>();
    private List<GameObject> playersButtonList = new List<GameObject>();
    private List<GameObject> enemiesButtonList = new List<GameObject>();
    private List<GameObject> npcsButtonList = new List<GameObject>();

    private PF2E_CampaignID currentCampaignID = null;
    private PF2E_CampaignData currentCampaign = null;

    void Awake()
    {
        PF2eCampaignIDs = Json.LoadFromPlayerPrefs<List<PF2E_CampaignID>>("PF2e_campaignsIDList");
        if (PF2eCampaignIDs == null)
            PF2eCampaignIDs = new List<PF2E_CampaignID>();
    }

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0f));
        campaignPanel.gameObject.SetActive(false);
    }

    private void OpenCampaingPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 1f, 1f, 0.1f));
    }

    private void CloseCampaingPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0.1f));
    }

    /// <summary> Load given Campaign. Called by UI buttons that enumerate existing Campaigns. </summary>
    public void LoadCampaign(PF2E_CampaignID campaignID)
    {
        OpenCampaingPanel();

        // Open Campaign Panel
        campaignPanel.gameObject.SetActive(true);
        campaignName.text = campaignID.name;

        // Set current campaing and refresh boards, players, enemies and npcs
        currentCampaignID = campaignID;
        currentCampaign = Json.LoadFromPlayerPrefs<PF2E_CampaignData>(campaignID.guid);
        RefreshCampaignContainers();
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

    public void OnClickBackButton()
    {
        CloseCampaingPanel();
    }

    public void OnClickDeleteButton()
    {
        DeleteCampaign();
    }

    private void RefreshCampaignContainers()
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
        }

        // Spawn players buttons
        // Spawn enemies buttons
        // Spawn npcs buttons
    }

    #endregion
}
