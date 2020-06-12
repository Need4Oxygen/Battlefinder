﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PF2E_CCStats : MonoBehaviour
{
    [Serializable] class SkillsWrapper { public List<Image> list = null; } // For Abilities serialization

    [SerializeField] private GameObject statsPanel = null;
    [SerializeField] private PF2E_CharacterCreation creation = null;

    [Header("Health")]
    [SerializeField] private TMP_Text HPCurrentText = null;
    [SerializeField] private TMP_Text HPMaxtText = null;
    [SerializeField] private TMP_Text HPTempText = null;
    [SerializeField] private TMP_Text DyingMaxtText = null;
    [SerializeField] private TMP_InputField damageInput = null;
    [SerializeField] private TMP_InputField dyingInput = null;
    [SerializeField] private TMP_InputField woundsInput = null;
    [SerializeField] private TMP_InputField doomInput = null;

    [Header("AC")]
    [SerializeField] private PF2E_APICButton acAPIC = null;
    [SerializeField] private TMP_Text shieldHealth = null;
    [SerializeField] private TMP_Text shieldHard = null;
    [SerializeField] private TMP_InputField shieldDamage = null;
    [SerializeField] private TMP_Text shieldBonusAC = null;

    [Header("Perception & Saves")]
    [SerializeField] private PF2E_APICButton perceptionAPIC = null;
    [SerializeField] private PF2E_APICButton fortitudeAPIC = null;
    [SerializeField] private PF2E_APICButton reflexAPIC = null;
    [SerializeField] private PF2E_APICButton willAPIC = null;

    [Header("Speeds")]
    [SerializeField] private TMP_Text speedBaseText = null;
    [SerializeField] private TMP_Text speedFlyText = null;
    [SerializeField] private TMP_Text speedSwimText = null;
    [SerializeField] private TMP_Text speedClimbText = null;
    [SerializeField] private TMP_Text speedBurrowText = null;

    [Header("Weapon & Armor")]
    [SerializeField] private TMP_Text unarmoredText = null;
    [SerializeField] private TMP_Text lightArmorText = null;
    [SerializeField] private TMP_Text mediumArmorText = null;
    [SerializeField] private TMP_Text heavyArmorText = null;

    [SerializeField] private TMP_Text unarmedText = null;
    [SerializeField] private TMP_Text simpleWeaponText = null;
    [SerializeField] private TMP_Text martialWeaponText = null;
    [SerializeField] private TMP_Text advancedWeaponText = null;

    [Header("Random")]
    [SerializeField] private TMP_Text sizeText = null;
    [SerializeField] private TMP_Text bulkText = null;
    [SerializeField] private TMP_Text classDCText = null;
    [SerializeField] private TMP_Text traitsText = null;
    [SerializeField] private TMP_InputField languagesInput = null;
    [SerializeField] private TMP_InputField heroPointsInput = null;
    [SerializeField] private TMP_InputField wealthInput = null;
    [SerializeField] private List<SkillsWrapper> ablMapImages = null;
    [SerializeField] private List<PF2E_APICButton> skills = null;

    [HideInInspector] public bool isOpen = false;

    #region --------------------------------------------GENERAL--------------------------------------------

    public void OpenStatsPanel()
    {
        isOpen = true;
        statsPanel.SetActive(true);
        RefreshPlayerIntoPanel();
    }

    public void CloseStatsPanel()
    {
        isOpen = false;
        statsPanel.SetActive(false);
    }

    #endregion

    #region --------------------------------------------PLAYERS--------------------------------------------

    /// <summary> Refresh UI with player data. </summary>
    public void RefreshPlayerIntoPanel()
    {
        HPCurrentText.text = creation.currentPlayer.hp_current.ToString();
        HPMaxtText.text = creation.currentPlayer.hp_max.ToString();
        HPTempText.text = creation.currentPlayer.hp_temp.ToString();
        DyingMaxtText.text = creation.currentPlayer.hp_dyingMax.ToString();

        // AC
        acAPIC.Refresh(creation.currentPlayer.AC_Get());
        shieldHealth.text = "0/0";
        shieldHard.text = "Hard 0";
        shieldDamage.SetTextWithoutNotify("0");
        shieldBonusAC.text = "+0";

        // Perception & Savesd
        perceptionAPIC.Refresh(creation.currentPlayer.Perception_Get());
        fortitudeAPIC.Refresh(creation.currentPlayer.Saves_Get("fortitude"));
        reflexAPIC.Refresh(creation.currentPlayer.Saves_Get("reflex"));
        willAPIC.Refresh(creation.currentPlayer.Saves_Get("will"));

        // ClassDC
        classDCText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.classDC);

        // Size
        sizeText.text = creation.currentPlayer.size;

        // Bulk
        bulkText.text = creation.currentPlayer.bulk_current + "/" + creation.currentPlayer.bulk_encThreshold;

        // Hero Points
        heroPointsInput.SetTextWithoutNotify(creation.currentPlayer.heroPoints.ToString());

        // Wealth
        wealthInput.SetTextWithoutNotify(creation.currentPlayer.Wealth_Formated());

        // Abilities
        Color active = Globals.Theme["text_2"]; Color unactive = Globals.Theme["background_1"];
        int[,] abl_map = creation.currentPlayer.Abl_GetMap();
        for (int i = 0; i < abl_map.GetLength(0); i++)
            for (int j = 0; j < abl_map.GetLength(1); j++)
                if (abl_map[i, j] > 0)
                    ablMapImages[j].list[i].color = active;
                else if (abl_map[i, j] < 0)
                    ablMapImages[j].list[i].color = Color.red;
                else
                    ablMapImages[j].list[i].color = unactive;

        // Skills
        var list = creation.currentPlayer.Skills_GetAllAsList();
        for (int i = 0; i < skills.Count; i++)
            skills[i].Refresh(list[i]);

        // Traits
        string traits = "";
        int count = 0; int total = creation.currentPlayer.traits_list.Count;
        foreach (var item in creation.currentPlayer.traits_list)
        {
            if (count < total - 1)
                traits += item.name + ", ";
            else
                traits += item.name;
            count++;
        }
        traitsText.text = traits;

        // Languages
        if (creation.currentPlayer.languages != null)
            if (creation.currentPlayer.languages != "")
                languagesInput.SetTextWithoutNotify(creation.currentPlayer.languages);

        // Speeds
        speedBaseText.text = creation.currentPlayer.speed_base.ToString();
        speedFlyText.text = creation.currentPlayer.speed_fly.ToString();
        speedSwimText.text = creation.currentPlayer.speed_swim.ToString();
        speedClimbText.text = creation.currentPlayer.speed_climp.ToString();
        speedBurrowText.text = creation.currentPlayer.speed_burrow.ToString();

        // Weapon & Armor Profs
        unarmoredText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.unarmored);
        lightArmorText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.lightArmor);
        mediumArmorText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.mediumArmor);
        heavyArmorText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.heavyArmor);

        unarmedText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.unarmed);
        simpleWeaponText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.simpleWeapons);
        martialWeaponText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.martialWeapons);
        advancedWeaponText.text = PF2E_DataBase.Prof_Enum2ColoredAbbr(creation.currentPlayer.advancedWeapons);
    }

    #endregion

    #region --------------------------------BUTTONS & INPUT--------------------------------

    public void OnEndEditDamage()
    {
        int value = 0; int.TryParse(damageInput.text, out value);
        creation.currentPlayer.hp_damage = value;
        RefreshPlayerIntoPanel();
    }
    public void OnEndEditDying()
    {
        int value = 0; int.TryParse(dyingInput.text, out value);
        creation.currentPlayer.hp_dyingCurrent = value;
        RefreshPlayerIntoPanel();
    }
    public void OnEndEditWounds()
    {
        int value = 0; int.TryParse(woundsInput.text, out value);
        creation.currentPlayer.hp_wounds = value;
        RefreshPlayerIntoPanel();
    }
    public void OnEndEditDoom()
    {
        int value = 0; int.TryParse(doomInput.text, out value);
        creation.currentPlayer.hp_doom = value;
        RefreshPlayerIntoPanel();
    }

    public void OnEndEditLanguages()
    {
        string value = languagesInput.text;
        creation.currentPlayer.languages = value;
        RefreshPlayerIntoPanel();
    }

    public void OnEndEditHeroPoints()
    {
        string value = heroPointsInput.text;
        int valueInt = 0; int.TryParse(value, out valueInt);
        creation.currentPlayer.heroPoints = valueInt;
        RefreshPlayerIntoPanel();
    }

    public void OnEndEditWealth()
    {
        string value = wealthInput.text;
        float valueF = 0; float.TryParse(value, out valueF);
        creation.currentPlayer.wealth = valueF;
        RefreshPlayerIntoPanel();
    }

    public void OnEndEditShieldDamage()
    {
        // string value = shieldDamage.text;
        // creation.currentPlayer.shield = value;
        RefreshPlayerIntoPanel();
    }


    // money

    #endregion

}
