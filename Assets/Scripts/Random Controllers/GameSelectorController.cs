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

    /// <summary> Called by the diferent game buttons to select themselves </summary>
    public void SelectGame(string game)
    {
        if (game == E_Games.PF2E.ToString())
            PF2E_RefreshButtons();
    }

    #region Create Campaing

    private void OpenCreateCampaignPanel()
    {
        PanelFader.RescaleAndFade(createCampaignPanel.transform, createCampaignPanel, 1f, 1f, 0.2f);
        createCampaignAcceptButton.onClick.RemoveAllListeners();
        createCampaignCancelButton.onClick.RemoveAllListeners();
        createCampaignAcceptButton.onClick.AddListener(() => OnClickCreateCampaingPanelAccept());
        createCampaignCancelButton.onClick.AddListener(() => OnClickCreateCampaingPanelCancel());
        createCampaignInputField.text = "";
        createCampaignErrorText.text = "";
    }

    private void CloseCreateCampaignPanel()
    {
        PanelFader.RescaleAndFade(createCampaignPanel.transform, createCampaignPanel, 0.8f, 0f, 0.2f);
    }

    public void OnClickCreateCampaingPanelAccept()
    {
        // Create Campaign with the name in input field
        // Update buttons of selected game
        // Close campaign Creation Panel
        CloseCreateCampaignPanel();
    }

    public void OnClickCreateCampaingPanelCancel()
    {
        // Close campaign Creation Panel
        CloseCreateCampaignPanel();
    }

    #endregion

    #region Pathfinder 2e

    private List<GameObject> PF2eCampaignButtonList = new List<GameObject>();

    private void PF2E_RefreshButtons()
    {
        List<PF2E_CampaignID> PF2eCampaignIDs = new List<PF2E_CampaignID>();
        PF2eCampaignIDs = Json.LoadFromPlayerPrefs("PF2e_campaignsIDList") as List<PF2E_CampaignID>;
        foreach (var item in PF2eCampaignButtonList)
            Destroy(item, 0.001f);
        PF2eCampaignButtonList.Clear();

        // Campaign buttons
        if (PF2eCampaignIDs != null)
            foreach (var item in PF2eCampaignIDs)
            {
                Transform newButton = Instantiate(PF2E_campaingButtonPrefab, Vector3.zero, Quaternion.identity, PF2E_container);
                CampaignButton newButtonScript = newButton.GetComponent<CampaignButton>();
                newButtonScript.campaignNameText.text = item.name;
                newButtonScript.button.onClick.AddListener(() => OnClickPF2eCampaignButton(item));
            }

        // Add Campaign button
        Transform addButton = Instantiate(PF2E_campaingButtonPrefab, Vector3.zero, Quaternion.identity, PF2E_container);
        CampaignButton addButtonScript = addButton.GetComponent<CampaignButton>();
        addButtonScript.campaignNameText.text = "+";
        addButtonScript.button.onClick.AddListener(() => OnClickPF2eAddCampaignButton());
    }

    private void OnClickPF2eCampaignButton(PF2E_CampaignID campaignID)
    {
        PF2E_controller.LoadCampaign(campaignID);
    }

    private void OnClickPF2eAddCampaignButton()
    {
        // Abrir panel de creación de campaña
        createCampaignAcceptButton.onClick.AddListener(() => CreateCampaign(createCampaignInputField.text));
        OpenCreateCampaignPanel();
        // pedir el nombre de la campaña
        // mandar los datos al controller medainte PF2eController.CreateCampaign

    }

    private void CreateCampaign(string name)
    {
        PF2E_controller.CreateCampaign(name);
    }

    #endregion
}
