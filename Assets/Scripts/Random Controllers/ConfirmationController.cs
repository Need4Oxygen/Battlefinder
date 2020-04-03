using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationController : MonoBehaviour
{
    [SerializeField] CanvasGroup confirmationPanel;
    [SerializeField] TMP_Text message;
    [SerializeField] Button acceptButton;
    [SerializeField] Button cancelButton;

    DBool callback;

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(confirmationPanel.transform, confirmationPanel, 0.85f, 0f, 0f));
    }

    private void OpenPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(confirmationPanel.transform, confirmationPanel, 1f, 1f, 0.1f));
    }

    private void ClosePanel()
    {
        callback = null;
        message.text = "";
        StartCoroutine(PanelFader.RescaleAndFade(confirmationPanel.transform, confirmationPanel, 0.85f, 0f, 0.1f));
    }

    public void AskForConfirmation(string message, DBool callback)
    {
        this.message.text = message;
        this.callback = callback;
        OpenPanel();
    }

    public void OnClickAcceptButton()
    {
        callback(true);
        ClosePanel();
    }

    public void OnClickCancelButton()
    {
        callback(false);
        ClosePanel();
    }
}
