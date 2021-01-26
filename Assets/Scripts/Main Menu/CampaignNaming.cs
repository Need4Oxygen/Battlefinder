using System.Collections.Generic;
using Pathfinder2e.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignNaming : MonoBehaviour
{
    [SerializeField] private Window window = null;
    [SerializeField] private TMP_InputField createCampaignInputField = null;
    [SerializeField] private TMP_Text createCampaignErrorText = null;

    DString callback;

    private void OpenPanel()
    {
        createCampaignInputField.text = "";
        createCampaignErrorText.text = "";
        window.OpenWindow();
    }

    private void ClosePanel()
    {
        callback = null;
        window.CloseWindow();
    }

    public void AskForCampaignNaming(DString callback)
    {
        this.callback = callback;
        OpenPanel();
    }

    public void OnClickAcceptButton()
    {
        callback(createCampaignInputField.text);
        ClosePanel();
    }

    public void OnClickCancelButton()
    {
        callback("");
        ClosePanel();
    }
}
