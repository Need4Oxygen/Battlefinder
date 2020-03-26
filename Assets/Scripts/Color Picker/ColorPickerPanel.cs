using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerPanel : MonoBehaviour
{
    [SerializeField] public Image mainColor = null;
    [SerializeField] public Slider hueSlider = null;
    [SerializeField] public Slider saturationSlider = null;
    [SerializeField] public Slider brightnessSlider = null;

    [HideInInspector] public Color initialColor = Color.grey;
    [HideInInspector] public PickColorCallback callback = null;

    public void AskForColors(Color initialColor, PickColorCallback callback)
    {
        this.initialColor = initialColor;
        this.callback = callback;

        SetColor(initialColor);
    }

    public void OnAcceptButtonPress()
    {
        callback(mainColor.color);
        ColorPickerController.Instance.isAskingForColor = false;
    }

    public void OnCancelButtonPress()
    {
        callback(initialColor);
        ColorPickerController.Instance.isAskingForColor = false;
    }

    public void OnHUEValueChanged(float value)
    { UpdateColor(); }

    public void OnSaturationValueChanged(float value)
    { UpdateColor(); }

    public void OnLightnessValueChanged(float value)
    { UpdateColor(); }

    private void SetColor(Color color)
    {
        mainColor.color = color;

        float h = 0, s = 0, v = 0;
        Color.RGBToHSV(color, out h, out s, out v);

        hueSlider.value = h;
        saturationSlider.value = s;
        brightnessSlider.value = v;
    }

    private void UpdateColor()
    {
        mainColor.color = Color.HSVToRGB(hueSlider.value, saturationSlider.value, brightnessSlider.value);
    }
}
