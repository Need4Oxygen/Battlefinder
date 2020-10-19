using System.Collections;
using System.Collections.Generic;
using Pathfinder2e.Player;
using TMPro;
using UnityEngine;

public class PF2E_APICButton : MonoBehaviour
{
    [SerializeField] private bool lore = false;

    [SerializeField] private TMP_Text labelText = null;
    [SerializeField] private TMP_Text profLetterText = null;
    [SerializeField] private TMP_Text dcText = null;
    [SerializeField] private TMP_Text ablText = null;
    [SerializeField] private TMP_Text profText = null;
    [SerializeField] private TMP_Text itemText = null;
    [SerializeField] private TMP_Text tempText = null;
    [SerializeField] private TMP_Text scoreText = null;

    public void Refresh(APIC skill)
    {
        if (lore)
            labelText.text = skill.name;

        profLetterText.text = skill.prof;
        ablText.text = Process(skill.ablScore);
        profText.text = Process(skill.profScore);
        itemText.text = Process(skill.itemScore);
        tempText.text = Process(skill.tempScore);
        scoreText.text = ProcessScore(skill.score);

        if (dcText != null)
            dcText.text = skill.dcScore.ToString();
    }

    private string Process(int value)
    {
        if (value == 0)
            return "";
        else
            return value.ToString();
    }

    private string ProcessScore(int value)
    {
        if (value >= 0)
            return "+" + value.ToString();
        else
            return value.ToString();
    }
}
