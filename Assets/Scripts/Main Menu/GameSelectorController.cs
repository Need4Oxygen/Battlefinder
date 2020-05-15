using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectorController : MonoBehaviour
{
    [Header("Campaign")]
    [SerializeField] private CanvasGroup createCampaignPanel = null;
    [SerializeField] private TMP_InputField createCampaignInputField = null;
    [SerializeField] private TMP_Text createCampaignErrorText = null;
    [SerializeField] private Button createCampaignAcceptButton = null;
    [SerializeField] private Button createCampaignCancelButton = null;

    [Header("Pathfinder 2e")]
    [SerializeField] private PF2E_CampaingHandler PF2E_controller = null;
    [SerializeField] private Transform PF2E_campaingButtonPrefab = null;
    [SerializeField] private Transform PF2E_container = null;

    private List<GameObject> PF2E_buttons = new List<GameObject>();
    private E_Game currentGame = E_Game.None;

    /// <summary> Called by the diferent game buttons to select themselves </summary>
    public void GameButtonPressed(string game)
    {
        switch (game)
        {
            case "PF2E":
                if (currentGame != E_Game.PF2E)
                {
                    currentGame = E_Game.PF2E;
                    PF2E_ExtendGameButtons();
                }
                else
                {
                    currentGame = E_Game.None;
                    PF2E_RetractGameButtons();
                }
                break;
            default: // Closes all Buttons
                currentGame = E_Game.None;
                PF2E_RetractGameButtons();
                break;
        }
    }

    void Start()
    {
        // Close Create Campaign panel instantly, in case it was open
        StartCoroutine(PanelFader.RescaleAndFade(createCampaignPanel.transform, createCampaignPanel, 0.85f, 0f, 0f));
    }

    #region --------CAMPAIGNS CREATION--------

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

    private void OnClickCreateCampaingPanelAccept()
    {
        CloseCreateCampaignPanel();
    }

    private void OnClickCreateCampaingPanelCancel()
    {
        CloseCreateCampaignPanel();
    }

    #endregion

    #region --------PF2E--------

    private List<GameObject> PF2eUI_ButtonTextList = new List<GameObject>();

    private void PF2E_RefreshButtons()
    {
        PF2E_RetractGameButtons();
        PF2E_ExtendGameButtons();
    }

    private void PF2E_ExtendGameButtons()
    {
        // Campaign buttons
        foreach (var item in Globals.PF2eCampaignIDs)
        {
            Transform newButton = Instantiate(PF2E_campaingButtonPrefab, PF2E_container.position, PF2E_container.rotation, PF2E_container);
            ButtonText newButtonScript = newButton.GetComponent<ButtonText>();
            newButtonScript.text.text = item.name;
            newButtonScript.button.onClick.AddListener(() => PF2E_OnClickUI_ButtonText(item));
            PF2eUI_ButtonTextList.Add(newButton.gameObject);
        }

        // Add Campaign button
        Transform addButton = Instantiate(PF2E_campaingButtonPrefab, PF2E_container.position, PF2E_container.rotation, PF2E_container);
        ButtonText addButtonScript = addButton.GetComponent<ButtonText>();
        addButtonScript.text.text = "+";
        addButtonScript.button.onClick.AddListener(() => PF2E_OnClickAddUI_ButtonText());
        PF2eUI_ButtonTextList.Add(addButton.gameObject);
    }

    private void PF2E_RetractGameButtons()
    {
        currentGame = E_Game.None;
        foreach (var item in PF2eUI_ButtonTextList)
            Destroy(item, 0.001f);
        PF2eUI_ButtonTextList.Clear();
    }

    // Click on existing campaign button, open campaign panel
    private void PF2E_OnClickUI_ButtonText(PF2E_CampaignID campaignID)
    {
        PF2E_controller.LoadCampaign(campaignID);
        PF2E_RetractGameButtons();
    }

    // Click on add campaign button (the one with the +), open add campaign panel
    private void PF2E_OnClickAddUI_ButtonText()
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
