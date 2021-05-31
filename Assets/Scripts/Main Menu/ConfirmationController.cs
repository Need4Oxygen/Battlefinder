using TMPro;
using UnityEngine;

public class ConfirmationController : MonoBehaviour
{
    [SerializeField] WindowRIP window = null;
    [SerializeField] TMP_Text message = null;

    DBool callback;

    private void OpenPanel()
    {
        window.OpenWindow();
    }

    private void ClosePanel()
    {
        callback = null;
        message.text = "";
        window.CloseWindow();
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
