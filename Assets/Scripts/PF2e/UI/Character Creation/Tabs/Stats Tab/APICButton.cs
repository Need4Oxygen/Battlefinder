using System.Collections;
using System.Collections.Generic;
using Pathfinder2e.Character;
using TMPro;
using UnityEngine;
using Tools;

namespace Pathfinder2e.GameData
{

    public class APICButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text skillName = null;
        [SerializeField] private TMP_Text ablDependency = null;
        [SerializeField] private TMP_Text profLetterText = null;
        [SerializeField] private TMP_Text dcText = null;
        [SerializeField] private TMP_Text ablText = null;
        [SerializeField] private TMP_Text profText = null;
        [SerializeField] private TMP_Text itemText = null;
        [SerializeField] private TMP_Text tempText = null;
        [SerializeField] private TMP_Text scoreText = null;

        public void Refresh(APIC apic)
        {
            if (skillName != null)
                skillName.text = StrExtensions.ToUpperFirst(apic.selector);
            if (ablDependency != null)
                ablDependency.text = apic.abl.ToUpper();
            if (dcText != null)
                dcText.text = apic.dcScore.ToString();

            profLetterText.text = apic.profColored;
            ablText.text = ToEmptyOrString(apic.ablScore);
            profText.text = ToEmptyOrString(apic.profScore);
            itemText.text = ToEmptyOrString(apic.itemScore);
            tempText.text = ToEmptyOrString(apic.tempScore);
            scoreText.text = ProcessScore(apic.score);
        }

        private string ToEmptyOrString(int value)
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

}
