using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectorController : MonoBehaviour
{

    [Header("Create Campaign")]
    [SerializeField] private CanvasGroup createCampaignPanel;
    [SerializeField] private TMP_InputField createCampaignInputField;
    [SerializeField] private TMP_Text createCampaignErrorText;
    [SerializeField] private Button createCampaignAcceptButton;
    [SerializeField] private Button createCampaignCancelButton;

    [Header("Pathfinder 2e")]
    [SerializeField] private PF2E_Controller PF2E_controller;
    [SerializeField] private Transform PF2E_campaingButtonPrefab;
    [SerializeField] private Transform PF2E_container;

    private List<GameObject> PF2E_buttons = new List<GameObject>();
    private E_Games currentGame = E_Games.None;

    /// <summary> Called by the diferent game buttons to select themselves </summary>
    public void GameButtonPressed(string game)
    {
        switch (game)
        {
            case "PF2E":
                if (currentGame != E_Games.PF2E)
                {
                    currentGame = E_Games.PF2E;
                    PF2E_ExtendGameButtons();
                }
                else
                {
                    currentGame = E_Games.None;
                    PF2E_RetractGameButtons();
                }
                break;
            default: // Closes all Buttons
                currentGame = E_Games.None;
                PF2E_RetractGameButtons();
                break;
        }
    }

    void Start()
    {
        // Close Create Campaign panel instantly, in case it was open
        StartCoroutine(PanelFader.RescaleAndFade(createCampaignPanel.transform, createCampaignPanel, 0.85f, 0f, 0f));
    }

    #region Create Campaing

    private void OpenCreateCampaignPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(createCampaignPanel.transform, createCampaignPanel, 0.85f, 1f, 0f, 1f, 0.1f));
        createCampaignAcceptButton.onClick.RemoveAllListeners();
        createCampaignCancelButton.onClick.RemoveAllListeners();
        createCampaignAcceptButton.onClick.AddListener(() => OnClickCreateCampaingPanelAccept());
        createCampaignCancelButton.onClick.AddListener(() => OnClickCreateCampaingPanelCancel());
        createCampaignInputField.text = "";
        createCampaignErrorText.text = "";
    }

    private void CloseCreateCampaignPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(createCampaignPanel.transform, createCampaignPanel, 0.85f, 0f, 0.1f));
    }

    public void OnClickCreateCampaingPanelAccept()
    {
        CloseCreateCampaignPanel();
    }

    public void OnClickCreateCampaingPanelCancel()
    {
        CloseCreateCampaignPanel();
    }

    #endregion

    #region Pathfinder 2e

    private List<GameObject> PF2eCampaignButtonList = new List<GameObject>();

    private void PF2E_RefreshButtons()
    {
        PF2E_RetractGameButtons();
        PF2E_ExtendGameButtons();
    }

    private void PF2E_ExtendGameButtons()
    {
        // Campaign buttons
        foreach (var item in PF2E_Controller.PF2eCampaignIDs)
        {
            Transform newButton = Instantiate(PF2E_campaingButtonPrefab, PF2E_container.position, Quaternion.identity, PF2E_container);
            CampaignButton newButtonScript = newButton.GetComponent<CampaignButton>();
            newButtonScript.campaignNameText.text = item.name;
            newButtonScript.button.onClick.AddListener(() => PF2E_OnClickCampaignButton(item));
            PF2eCampaignButtonList.Add(newButton.gameObject);
        }

        // Add Campaign button
        Transform addButton = Instantiate(PF2E_campaingButtonPrefab, PF2E_container.position, Quaternion.identity, PF2E_container);
        CampaignButton addButtonScript = addButton.GetComponent<CampaignButton>();
        addButtonScript.campaignNameText.text = "+";
        addButtonScript.button.onClick.AddListener(() => PF2E_OnClickAddCampaignButton());
        PF2eCampaignButtonList.Add(addButton.gameObject);
    }

    private void PF2E_RetractGameButtons()
    {
        currentGame = E_Games.None;
        foreach (var item in PF2eCampaignButtonList)
            Destroy(item, 0.001f);
        PF2eCampaignButtonList.Clear();
    }

    // Click on existing campaign, open campaign panel
    private void PF2E_OnClickCampaignButton(PF2E_CampaignID campaignID)
    {
        PF2E_controller.LoadCampaign(campaignID);
        PF2E_RetractGameButtons();
    }

    // Click on add campaign, open add campaign panel
    private void PF2E_OnClickAddCampaignButton()
    {
        OpenCreateCampaignPanel(); // This remove Listeners
        createCampaignAcceptButton.onClick.AddListener(() => PF2E_CreateCampaign(createCampaignInputField.text));
    }

    private void PF2E_CreateCampaign(string name)
    {
        PF2E_controller.CreateCampaign(name);
        PF2E_RefreshButtons();
    }

    #endregion
}
