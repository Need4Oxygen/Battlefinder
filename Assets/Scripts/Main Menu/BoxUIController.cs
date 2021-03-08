using System.Collections.Generic;
using Pathfinder2e.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxUIController : MonoBehaviour
{
    [Header("Pathfinder 2e")]
    [SerializeField] private CampaignNaming campaignNaming = null;
    [SerializeField] private CampaignHandler campaingHandler = null;
    [SerializeField] private Transform campaingButtonPrefab = null;
    [SerializeField] private Transform buttonContainer = null;

    private List<GameObject> PF2E_CampaignButtons = new List<GameObject>();
    private E_Game currentGame = E_Game.None;

    /// <summary> Called by the diferent game buttons to show their campaings </summary>
    public void OnClickGameButton(string game)
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

    /// <summary> Called by the exit game button </summary>
    public void OnClickExitGame()
    {
        Application.Quit();
    }

    private void PF2E_RefreshButtons()
    {
        PF2E_RetractGameButtons();
        PF2E_ExtendGameButtons();
    }

    private void PF2E_ExtendGameButtons()
    {
        // Campaign buttons
        foreach (var item in Pathfinder2e.GameData.Globals_PF2E.CampaignIDs)
        {
            Transform newButton = Instantiate(campaingButtonPrefab, buttonContainer.position, buttonContainer.rotation, buttonContainer);
            ButtonText newButtonScript = newButton.GetComponent<ButtonText>();
            newButtonScript.text.text = item.Key.Replace(".yaml", "");

            newButtonScript.button.onClick.AddListener(() => PF2E_OnClickUI_ButtonText(item.Key));
            PF2E_CampaignButtons.Add(newButton.gameObject);
        }

        // Add Campaign button
        Transform addButton = Instantiate(campaingButtonPrefab, buttonContainer.position, buttonContainer.rotation, buttonContainer);
        ButtonText addButtonScript = addButton.GetComponent<ButtonText>();
        addButtonScript.text.text = "+";
        addButtonScript.button.onClick.AddListener(() => PF2E_OnClickAddUI_ButtonText());
        PF2E_CampaignButtons.Add(addButton.gameObject);
    }

    private void PF2E_RetractGameButtons()
    {
        currentGame = E_Game.None;
        foreach (var item in PF2E_CampaignButtons)
            Destroy(item, 0.001f);
        PF2E_CampaignButtons.Clear();
    }

    // Click on existing campaign button, open campaign panel
    private void PF2E_OnClickUI_ButtonText(string campaignID)
    {
        campaingHandler.LoadCampaign(campaignID);
        PF2E_RetractGameButtons();
    }

    // Click on add campaign button (the one with the +), open add campaign panel
    private void PF2E_OnClickAddUI_ButtonText()
    {
        campaignNaming.AskForCampaignNaming(PF2E_CreateCampaign);
    }

    private void PF2E_CreateCampaign(string name)
    {
        if (name != "")
        {
            PF2E_RetractGameButtons();
            campaingHandler.CreateCampaign(name);
            PF2E_RefreshButtons();
        }
    }

}
