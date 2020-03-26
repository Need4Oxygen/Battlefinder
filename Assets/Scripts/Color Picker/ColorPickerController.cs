using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerController : MonoBehaviour
{
    public static ColorPickerController Instance;

    public Transform colorPickerPanelPrefab;

    private Transform colorPickerPanel;
    private ColorPickerPanel colorPickerPanelScript;
    private Coroutine askingCorou;

    [HideInInspector] public bool isAskingForColor = false;

    void Awake()
    {
        // Singletone Declaration
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (colorPickerPanel == null)
        {
            colorPickerPanel = Instantiate(colorPickerPanel, Vector3.zero, Quaternion.identity, null);
            colorPickerPanelScript = colorPickerPanel.GetComponent<ColorPickerPanel>();
        }
        colorPickerPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAskingForColor)
            if (Input.GetButtonDown("Cancel"))
                colorPickerPanelScript.OnCancelButtonPress();
    }

    public void RequestPickColor(Transform anchor, Color initialColor, PickColorCallback callback)
    {
        if (!isAskingForColor)
        {
            isAskingForColor = true;

            colorPickerPanel.position = anchor.position;
            colorPickerPanel.parent = anchor;
            colorPickerPanel.gameObject.SetActive(true);
            colorPickerPanelScript.AskForColors(initialColor, callback);
        }
    }
}
