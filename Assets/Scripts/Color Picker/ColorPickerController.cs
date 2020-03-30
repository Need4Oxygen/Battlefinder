using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerController : MonoBehaviour
{
    public static ColorPickerController Instance = null;

    public Transform colorPickerPanelPrefab = null;

    private Transform colorPickerPanel = null;
    private ColorPickerPanel colorPickerPanelScript = null;
    private Coroutine askingCorou = null;

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
            Transform newColorPickPanel = Instantiate(colorPickerPanelPrefab, Vector3.zero, Quaternion.identity, null);
            colorPickerPanel = newColorPickPanel;
            colorPickerPanelScript = newColorPickPanel.GetComponent<ColorPickerPanel>();
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
            colorPickerPanel.localScale = Vector3.one;
            colorPickerPanel.parent = anchor;
            colorPickerPanel.gameObject.SetActive(true);
            colorPickerPanelScript.AskForColors(initialColor, callback);
        }
    }
}
