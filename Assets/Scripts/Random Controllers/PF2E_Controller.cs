using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PF2E_Controller : MonoBehaviour
{

    #region Campaign Logic

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

    // Guid.NewGuid().ToString();

    private PF2E_CampaignID currentCampaignID = null;
    private PF2E_CampaignData currentCampaign = null;

    void Start()
    {
        PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.8f, 0f, 0f);
        campaignPanel.gameObject.SetActive(false);
    }

    /// <summary> Load given Campaign. Called by UI buttons that enumerate existing Campaigns. </summary>
    public void LoadCampaign(PF2E_CampaignID campaignID)
    {
        PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 1f, 1f, 0.2f);

        // Open Campaign Panel
        campaignPanel.gameObject.SetActive(true);
        campaignName.text = campaignID.name;

        // Set current campaing and refresh boards, players, enemies and npcs
        currentCampaignID = campaignID;
        currentCampaign = Json.LoadFromPlayerPrefs(campaignID.guid) as PF2E_CampaignData;
        RefreshContainers();
    }

    public void CreateCampaign(string name)
    {
        PF2E_CampaignID newCampaignID = new PF2E_CampaignID();
        newCampaignID.guid = Guid.NewGuid().ToString();
        newCampaignID.name = name;
        newCampaignID.order = 0;


    }

    private void DeleteCampaign()
    {
        // Open the confirmation panel
    }

    private PF2E_CampaignData RetrieveCampaign(string guid)
    {
        return Json.LoadFromPlayerPrefs(guid) as PF2E_CampaignData;
    }

    private void SaveCampaign(string guid, PF2E_CampaignData campaign)
    {

        Json.SaveInPlayerPrefs(currentCampaign.guid, campaign);
    }

    private void RefreshContainers()
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
