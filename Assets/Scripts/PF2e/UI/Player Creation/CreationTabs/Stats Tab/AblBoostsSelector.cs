﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinder2e;
using Pathfinder2e.Player;
using Pathfinder2e.Containers;
using static TMPro.TMP_Dropdown;

namespace Pathfinder2e.GameData
{

    public class AblBoostsSelector : MonoBehaviour
    {
        [SerializeField] private CharacterCreation creation = null;
        [SerializeField] private Transform dropdownPrefab = null;

        [Header("Initial Abilities Boosts")]
        [SerializeField] private CanvasGroup initialAblBoostsPanel = null;
        [SerializeField] private Transform ancestryContainer = null;
        [SerializeField] private Transform backgroundContainer = null;
        [SerializeField] private Transform classContainer = null;
        [SerializeField] private List<Toggle> level1Toggles = null;

        // [Header("Every Other Abilities Boosts")]
        // [SerializeField] private CanvasGroup ablBoostsPanel = null;
        // [SerializeField] private Button ablBoostsAcceptButotn = null;
        // [SerializeField] private Toggle[] toggles = null;

        private List<AblBoostData> currentData = null;

        [HideInInspector] public bool isOpen;

        void Start()
        {
            AssignLvl1BoostsFunctionality();

            StartCoroutine(PanelFader.RescaleAndFade(initialAblBoostsPanel.transform, initialAblBoostsPanel, 0.85f, 0f, 0f));
            // StartCoroutine(PanelFader.RescaleAndFade(ablBoostsPanel.transform, ablBoostsPanel, 0.85f, 0f, 0f));
        }

        #region --------------------------------INITIAL BOOSTS--------------------------------

        public void OpenPlayerInitialAblBoostsPanel()
        {
            isOpen = true;
            StartCoroutine(PanelFader.RescaleAndFade(initialAblBoostsPanel.transform, initialAblBoostsPanel, 1f, 1f, 0.1f));

            currentData = new List<AblBoostData>(creation.currentPlayer.abl_boostList);

            AssignInitialAblBoosts();
        }

        public void ClosePlayerInitialAblBoostsPanel()
        {
            isOpen = false;
            StartCoroutine(PanelFader.RescaleAndFade(initialAblBoostsPanel.transform, initialAblBoostsPanel, 0.85f, 0f, 0.1f));

            currentData = null;
        }

        private void AssignInitialAblBoosts()
        {
            GenerateAncestryDrops();
            RefreshAncestryOptions();

            GenerateBackgroundDrops();
            RefreshBackgroundOptions();

            GenerateClassDrops();

            AssignLvl1Boosts();
        }

        //--------------------------------------------ANCESTRIES ASSIGMENT STUFF--------------------------------------------
        private Ancestry ancestry = null;
        private List<TMP_Dropdown.OptionData> ancestryOptionList = new List<OptionData>();
        private List<TMP_Dropdown> allAncestryDrops = new List<TMP_Dropdown>();
        private List<TMP_Dropdown> ancestryBoostDrops = new List<TMP_Dropdown>();
        private List<TMP_Dropdown> ancestryFlawDrops = new List<TMP_Dropdown>();
        private List<TMP_Dropdown> ancestryFreeDrops = new List<TMP_Dropdown>();

        private void GenerateAncestryDrops()
        {
            ancestry = DB.Ancestries.Find(ctx => ctx.name == creation.currentPlayer.ancestry);
            ancestryOptionList = GenerateOptionList();
            int abl_free = ancestry.abl_boosts.FindAll(ctx => ctx == "free").Count;

            // Boosts
            if (ancestry.abl_boosts != null)
                foreach (var item in ancestry.abl_boosts)
                    if (item != "free")
                    {
                        TMP_Dropdown drop = GenerateDropdown(ancestryContainer);
                        allAncestryDrops.Add(drop);
                        ancestryBoostDrops.Add(drop);
                        drop.interactable = false;
                        drop.AddOptions(GenerateOptionList(new string[] { item }, false));

                        // Discard obligatory boosts from possible options
                        if (ancestry.abl_boosts.Count > 0)
                            ancestryOptionList.RemoveAll(ctx => ancestry.abl_boosts.Contains(AbilityToAbbr(ctx.text)));
                    }

            // Flaws
            if (ancestry.abl_flaws != null)
                foreach (var item in ancestry.abl_flaws)
                {
                    TMP_Dropdown drop = GenerateDropdown(ancestryContainer);
                    allAncestryDrops.Add(drop);
                    ancestryFlawDrops.Add(drop);
                    drop.interactable = false;
                    drop.AddOptions(GenerateOptionList(new string[] { item }, false));
                    drop.captionText.color = Globals.Theme["untrained"];
                }

            List<AblBoostData> previous = currentData.FindAll(ctx => ctx.from == "ancestry free");
            for (int i = 0; i < abl_free; i++)
            {
                TMP_Dropdown drop = GenerateDropdown(ancestryContainer);
                allAncestryDrops.Add(drop);
                ancestryFreeDrops.Add(drop);

                drop.options = ancestryOptionList;
                drop.onValueChanged.AddListener(v => OnValueChangedAncestryDropdown());

                // Select old boosts if applicable
                if (previous != null)
                    if (previous.Count > i)
                    {
                        string previousAbl = DB.Abl_Abbr2Full(previous[i].abl);
                        if (ancestryOptionList.Find(ctx => ctx.text == previousAbl) != null)
                        {
                            int shouldSelectIndex = 0;
                            shouldSelectIndex = ancestryFreeDrops[i].options.FindIndex(0, ancestryOptionList.Count, ctx => ctx.text == previousAbl);
                            drop.SetValueWithoutNotify(shouldSelectIndex);
                        }
                    }
            }
        }

        private void RefreshAncestryOptions()
        {
            for (int i = 0; i < ancestryFreeDrops.Count; i++)
            {
                // Get drop
                TMP_Dropdown drop = ancestryFreeDrops[i];
                string dropAbl = drop.captionText.text;

                // Discard every other free drop option
                List<OptionData> options = new List<OptionData>(ancestryOptionList);
                foreach (var item in ancestryFreeDrops)
                    if (item.captionText.text != "None" && item.captionText.text != "" && item.captionText.text != dropAbl)
                        options.Remove(options.Find(ctx => ctx.text == item.captionText.text));
                drop.options = options;

                // Reset value in case options change, so value follows
                int shouldSelectIndex = 0;
                shouldSelectIndex = drop.options.FindIndex(0, drop.options.Count, ctx => ctx.text == dropAbl);
                drop.SetValueWithoutNotify(shouldSelectIndex);
            }
        }

        private void OnValueChangedAncestryDropdown()
        {
            RefreshAncestryOptions();
        }

        private void SaveAncestryOptions()
        {
            currentData.RemoveAll(ctx => ctx.from == "ancestry free");

            foreach (var item in ancestryFreeDrops)
            {
                string abl = AbilityToAbbr(item.captionText.text);
                if (abl != "")
                    currentData.Add(new AblBoostData("ancestry free", abl, 1));
            }

            ClearAncestryData();
        }

        private void ClearAncestryData()
        {
            foreach (var item in allAncestryDrops)
            {
                item.gameObject.SetActive(false);
                Destroy(item.gameObject, 0.001f);
            }

            ancestry = null;
            allAncestryDrops.Clear();
            ancestryBoostDrops.Clear();
            ancestryFlawDrops.Clear();
            ancestryFreeDrops.Clear();
            ancestryOptionList.Clear();
        }


        //--------------------------------------------BACKGROUND ASSIGMENT STUFF--------------------------------------------
        private Background background = null;
        private List<TMP_Dropdown> allBackgroundDrops = new List<TMP_Dropdown>();
        private TMP_Dropdown backgroundChoiceDrop = null;
        private TMP_Dropdown backgroundFreeDrop = null;

        private void GenerateBackgroundDrops()
        {
            background = DB.Backgrounds.Find(ctx => ctx.name == creation.currentPlayer.background);

            {   // Choice
                AblBoostData previous = currentData.Find(ctx => ctx.from == "background choice");

                TMP_Dropdown drop = GenerateDropdown(backgroundContainer);
                allBackgroundDrops.Add(drop);
                backgroundChoiceDrop = drop;

                List<TMP_Dropdown.OptionData> optionList = GenerateOptionList(background.abl_choices.ToArray(), true);
                drop.options = optionList;

                int shouldSelectIndex = 0;
                if (previous != null)
                {
                    string previousAbl = DB.Abl_Abbr2Full(previous.abl);
                    if (optionList.Find(ctx => ctx.text == previousAbl) != null)
                        shouldSelectIndex = optionList.FindIndex(0, optionList.Count, ctx => ctx.text == previousAbl);
                }
                drop.SetValueWithoutNotify(shouldSelectIndex);

                drop.onValueChanged.AddListener(v => OnValueChangedBackgroundDropdown());
            }

            {   // Free
                AblBoostData previous = currentData.Find(ctx => ctx.from == "background free");

                TMP_Dropdown drop = GenerateDropdown(backgroundContainer);
                allBackgroundDrops.Add(drop);
                backgroundFreeDrop = drop;

                List<TMP_Dropdown.OptionData> optionList = GenerateOptionList();
                drop.options = optionList;

                int shouldSelectIndex = 0;
                if (previous != null)
                {
                    string previousAbl = DB.Abl_Abbr2Full(previous.abl);
                    if (optionList.Find(ctx => ctx.text == previousAbl) != null)
                        shouldSelectIndex = optionList.FindIndex(0, optionList.Count, ctx => ctx.text == previousAbl);
                }
                drop.SetValueWithoutNotify(shouldSelectIndex);

                drop.onValueChanged.AddListener(v => OnValueChangedBackgroundDropdown());
            }
        }

        private void RefreshBackgroundOptions()
        {
            {   // Choice
                string abl = backgroundChoiceDrop.captionText.text;

                // Discard free drop choice
                List<TMP_Dropdown.OptionData> choiceDropOptions = GenerateOptionList(background.abl_choices.ToArray(), true);
                if (backgroundFreeDrop.captionText.text != "None" &&
                backgroundFreeDrop.captionText.text != "" &&
                backgroundFreeDrop.captionText.text != abl)
                    choiceDropOptions.Remove(choiceDropOptions.Find(ctx => ctx.text == backgroundFreeDrop.captionText.text));
                backgroundChoiceDrop.options = choiceDropOptions;

                // Reset value in case options change, so value follows
                int shouldSelectIndex = 0;
                shouldSelectIndex = backgroundChoiceDrop.options.FindIndex(0, backgroundChoiceDrop.options.Count, ctx => ctx.text == abl);
                backgroundChoiceDrop.SetValueWithoutNotify(shouldSelectIndex);
            }

            {   // Free
                string abl = backgroundFreeDrop.captionText.text;

                // Discard free drop choice
                List<TMP_Dropdown.OptionData> freeDropOptions = GenerateOptionList();
                if (backgroundChoiceDrop.captionText.text != "None" &&
                backgroundChoiceDrop.captionText.text != "" &&
                backgroundChoiceDrop.captionText.text != abl)
                    freeDropOptions.Remove(freeDropOptions.Find(ctx => ctx.text == backgroundChoiceDrop.captionText.text));
                backgroundFreeDrop.options = freeDropOptions;

                // Reset value in case options change, so value follows
                int shouldSelectIndex = 0;
                shouldSelectIndex = backgroundFreeDrop.options.FindIndex(0, backgroundFreeDrop.options.Count, ctx => ctx.text == abl);
                backgroundFreeDrop.SetValueWithoutNotify(shouldSelectIndex);
            }
        }

        private void OnValueChangedBackgroundDropdown()
        {
            RefreshBackgroundOptions();
        }

        private void SaveBackgroundOptions()
        {
            currentData.RemoveAll(ctx => ctx.from == "background choice");
            currentData.RemoveAll(ctx => ctx.from == "background free");

            string abl = "";

            abl = AbilityToAbbr(backgroundChoiceDrop.captionText.text);
            if (abl != "")
                currentData.Add(new AblBoostData("background choice", abl, 1));

            abl = AbilityToAbbr(backgroundFreeDrop.captionText.text);
            if (abl != "")
                currentData.Add(new AblBoostData("background free", abl, 1));

            ClearBackgroundData();
        }

        private void ClearBackgroundData()
        {
            foreach (var item in allBackgroundDrops)
            {
                item.gameObject.SetActive(false);
                Destroy(item.gameObject, 0.001f);
            }

            background = null;
            allBackgroundDrops.Clear();
            backgroundChoiceDrop = null;
            backgroundFreeDrop = null;
        }


        //--------------------------------------------CLASS ASSIGMENT STUFF--------------------------------------------
        private Class classObj = null;
        private TMP_Dropdown classDrop = null;

        private void GenerateClassDrops()
        {
            classObj = DB.Classes.Find(ctx => ctx.name == creation.currentPlayer.class_name);

            AblBoostData previous = currentData.Find(ctx => ctx.from == "class");

            TMP_Dropdown drop = GenerateDropdown(classContainer);
            classDrop = drop;

            List<TMP_Dropdown.OptionData> optionList = GenerateOptionList(classObj.key_ability_choices.ToArray(), true);
            classDrop.options = optionList;

            int shouldSelectIndex = 0;
            if (previous != null)
            {
                string previousAbl = DB.Abl_Abbr2Full(previous.abl);
                if (optionList.Find(ctx => ctx.text == previousAbl) != null)
                    shouldSelectIndex = optionList.FindIndex(0, optionList.Count, ctx => ctx.text == previousAbl);
            }
            drop.SetValueWithoutNotify(shouldSelectIndex);

            drop.onValueChanged.AddListener(v => OnValueChangedClassDropdown());
        }

        private void OnValueChangedClassDropdown()
        {

        }

        private void SaveClassOptions()
        {
            currentData.RemoveAll(ctx => ctx.from == "class");

            string abl = AbilityToAbbr(classDrop.captionText.text);
            if (abl != "")
                currentData.Add(new AblBoostData("class", abl, 1));

            ClearClassData();
        }

        private void ClearClassData()
        {
            classDrop.gameObject.SetActive(false);
            Destroy(classDrop.gameObject, 0.001f);

            classObj = null;
            classDrop = null;
        }


        //--------------------------------------------LVL 1 ASSIGMENT STUFF--------------------------------------------
        // Assign method to each lvl 1 toggle
        private void AssignLvl1BoostsFunctionality()
        {
            for (int i = 0; i < level1Toggles.Count; i++)
                level1Toggles[i].onValueChanged.RemoveAllListeners();

            // Putting this inside the for breaks it, still don't know why
            level1Toggles[0].onValueChanged.AddListener((v) => { OnValueChangedLvl1BoostsToggle(level1Toggles[0], v); });
            level1Toggles[1].onValueChanged.AddListener((v) => { OnValueChangedLvl1BoostsToggle(level1Toggles[1], v); });
            level1Toggles[2].onValueChanged.AddListener((v) => { OnValueChangedLvl1BoostsToggle(level1Toggles[2], v); });
            level1Toggles[3].onValueChanged.AddListener((v) => { OnValueChangedLvl1BoostsToggle(level1Toggles[3], v); });
            level1Toggles[4].onValueChanged.AddListener((v) => { OnValueChangedLvl1BoostsToggle(level1Toggles[4], v); });
            level1Toggles[5].onValueChanged.AddListener((v) => { OnValueChangedLvl1BoostsToggle(level1Toggles[5], v); });
        }

        private void AssignLvl1Boosts()
        {
            foreach (var item in level1Toggles)
                item.SetIsOnWithoutNotify(false);

            foreach (var item in currentData.FindAll(ctx => ctx.from == "lvl1"))
                switch (item.abl)
                {
                    case "str": level1Toggles[0].SetIsOnWithoutNotify(true); break;
                    case "dex": level1Toggles[1].SetIsOnWithoutNotify(true); break;
                    case "con": level1Toggles[2].SetIsOnWithoutNotify(true); break;
                    case "int": level1Toggles[3].SetIsOnWithoutNotify(true); break;
                    case "wis": level1Toggles[4].SetIsOnWithoutNotify(true); break;
                    case "cha": level1Toggles[5].SetIsOnWithoutNotify(true); break;
                    default:
                        break;
                }
        }

        public void OnValueChangedLvl1BoostsToggle(Toggle toggle, bool value)
        {
            if (level1Toggles.FindAll(ctx => ctx.isOn == true).Count <= 4 && value)
                toggle.SetIsOnWithoutNotify(true);
            else
                toggle.SetIsOnWithoutNotify(false);
        }

        private void SaveLvl1Boosts()
        {
            currentData.RemoveAll(ctx => ctx.from == "lvl1");

            for (int i = 0; i < level1Toggles.Count; i++)
                if (level1Toggles[i].isOn)
                    currentData.Add(new AblBoostData("lvl1", DB.Abl_Int2Abbr(i), 1));
        }


        //-------------------------------------------- EXIT --------------------------------------------
        public void OnClickAcceptButton()
        {
            SaveAncestryOptions();
            SaveBackgroundOptions();
            SaveClassOptions();
            SaveLvl1Boosts();

            creation.currentPlayer.abl_boostList = new List<AblBoostData>(currentData);
            creation.RefreshPlayerIntoPanel();
            creation.SavePlayer();
            ClosePlayerInitialAblBoostsPanel();
        }

        public void OnClickCancelButton()
        {
            ClosePlayerInitialAblBoostsPanel();
        }

        #endregion

        #region --------------------------------OTHER BOOSTS--------------------------------

        // public void OpenPlayerAblBoostsPanel()
        // {
        //     StartCoroutine(PanelFader.RescaleAndFade(ablBoostsPanel.transform, ablBoostsPanel, 1f, 1f, 0.1f));
        // }

        // public void ClosePlayerAblBoostsPanel()
        // {
        //     StartCoroutine(PanelFader.RescaleAndFade(ablBoostsPanel.transform, ablBoostsPanel, 0.85f, 0f, 0.1f));
        // }

        #endregion

        private TMP_Dropdown GenerateDropdown(Transform parent)
        {
            Transform drop = Instantiate(dropdownPrefab, Vector3.zero, Quaternion.identity, parent);
            TMP_Dropdown dropdown = drop.GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();
            return dropdown;
        }

        private List<TMP_Dropdown.OptionData> GenerateOptionList() { return GenerateOptionList(null, true); }
        private List<TMP_Dropdown.OptionData> GenerateOptionList(string[] options, bool includeNone)
        {
            List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();

            if (options != null)
            {
                if (includeNone)
                    list.Add(new TMP_Dropdown.OptionData("None"));
                foreach (var item in options)
                    list.Add(new TMP_Dropdown.OptionData(DB.Abl_Abbr2Full(item)));
            }
            else
            {
                list = new List<TMP_Dropdown.OptionData> {
                new TMP_Dropdown.OptionData("None"),
                new TMP_Dropdown.OptionData("Strength"),
                new TMP_Dropdown.OptionData("Dexterity"),
                new TMP_Dropdown.OptionData("Constitution"),
                new TMP_Dropdown.OptionData("Intelligence"),
                new TMP_Dropdown.OptionData("Wisdom"),
                new TMP_Dropdown.OptionData("Charisma")};
            }
            return (list);
        }

        private string AbilityToAbbr(string ablFullName)
        {
            if (ablFullName != "" && ablFullName != "None")
                return DB.Abl_Full2Abbr(ablFullName);
            else
                return "";
        }

    }

}
