using System;
using System.Collections.Generic;
using Pathfinder2e.Player;
using Pathfinder2e.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Pathfinder2e.GameData;

namespace Pathfinder2e.Player
{

    public class CharacterCreation : MonoBehaviour
    {
        public DVoid OnCharacterCreationClose = null;

        [SerializeField] private ABCSelector ABCSelector = null;
        [SerializeField] private AblBoostsSelector ablBoostsSelector = null;
        [SerializeField] private CanvasGroup playerPanel = null;

        [Header("Name & Level")]
        [SerializeField] private TMP_InputField levelInput = null;
        [SerializeField] private TMP_InputField playerNameInput = null;

        [Header("Tabs")]
        [SerializeField] private CharCreationStats stats = null;
        [SerializeField] private Image[] statsTabImages = null;
        [SerializeField] private Image[] inventoryTabImages = null;
        [SerializeField] private Image[] companionsTabImages = null;
        [SerializeField] private Image[] spellsTabImages = null;

        [Header("ABC Buttons")]
        [SerializeField] private BuildButton ancestryButton = null;
        [SerializeField] private BuildButton backgroundButton = null;
        [SerializeField] private BuildButton classButton = null;

        [Header("Build")]
        [SerializeField] private Transform buildContainer = null;
        [SerializeField] private Transform buildLevelSeparator = null;
        [SerializeField] private Transform buildButton = null;

        private List<GameObject> buildButtonList = new List<GameObject>();
        public PlayerData currentPlayer = null;

        void Start()
        {
            StartCoroutine(PanelFader.RescaleAndFade(playerPanel.transform, playerPanel, 0.85f, 0f, 0f));

            tabOn = Globals.Theme["background_1"];
            tabOff = Globals.Theme["background_2"];
        }

        #region --------------------------------------------GENERAL--------------------------------------------

        private void OpenPlayerCreationPanel()
        {
            StartCoroutine(PanelFader.RescaleAndFade(playerPanel.transform, playerPanel, 1f, 1f, 0.1f));
            OnClickTabStats();
        }

        private void ClosePlayerCreationPanel()
        {
            StartCoroutine(PanelFader.RescaleAndFade(playerPanel.transform, playerPanel, 0.85f, 0f, 0.1f));
            currentPlayer = null;

            CloseAllTabs();

            if (OnCharacterCreationClose != null)
                OnCharacterCreationClose();
        }

        public void OnClickAcceptButton()
        {
            SavePlayer();

            if (ablBoostsSelector.isOpen)
                ablBoostsSelector.ClosePlayerInitialAblBoostsPanel();

            ClosePlayerCreationPanel();
        }

        #endregion

        #region --------------------------------------------PLAYERS--------------------------------------------

        /// <summary> Generates new player and open the player panel. </summary>
        public void NewPlayer()
        {
            string newGuid = Guid.NewGuid().ToString();
            currentPlayer = new PlayerData();
            currentPlayer.guid = newGuid;


            ABCSelector.OpenSelectorPanel();
            ABCSelector.Display("ancestry");
            ABCSelector.acceptButton.onClick.AddListener(() => NewPlayerProcessAccept());
            ABCSelector.backButton.onClick.AddListener(() => NewPlayerProcessBack());
        }

        private void NewPlayerProcessAccept()
        {
            if (ABCSelector.currentlyDisplaying == "ancestry")
            {
                currentPlayer.ancestry = ABCSelector.selectedAncestry;
                ABCSelector.Display("background");
            }
            else if (ABCSelector.currentlyDisplaying == "background")
            {
                currentPlayer.background = ABCSelector.selectedBackground;
                ABCSelector.Display("class");
            }
            else if (ABCSelector.currentlyDisplaying == "class")
            {
                currentPlayer.class_name = ABCSelector.selectedClass;
                CloseABCPanel(true);
                OpenPlayerCreationPanel();
            }
        }

        private void NewPlayerProcessBack()
        {
            if (ABCSelector.currentlyDisplaying == "ancestry")
            {
                currentPlayer = null;
                CloseABCPanel(false);
            }
            else if (ABCSelector.currentlyDisplaying == "background")
            {
                currentPlayer.background = "";
                ABCSelector.Display("ancestry");

            }
            else if (ABCSelector.currentlyDisplaying == "class")
            {
                currentPlayer.class_name = "";
                ABCSelector.Display("background");
            }
        }

        /// <summary> Load new player and open the player panel. </summary>
        public void LoadPlayer(PlayerData player)
        {
            currentPlayer = player;
            RefreshPlayerIntoPanel();
            OpenPlayerCreationPanel();
        }

        /// <summary> Save Player into campaing player list. </summary>
        public void SavePlayer()
        {
            if (Globals_PF2E.CurrentCampaign.players.ContainsKey(currentPlayer.guid))
                GameData.Globals_PF2E.CurrentCampaign.players[currentPlayer.guid] = currentPlayer;
            else
                GameData.Globals_PF2E.CurrentCampaign.players.Add(currentPlayer.guid, currentPlayer);

            GameData.Globals_PF2E.SaveCampaign();
        }

        /// <summary> Refresh UI with player data. </summary>
        public void RefreshPlayerIntoPanel()
        {
            if (stats.isOpen)
                stats.RefreshPlayerIntoPanel();

            levelInput.SetTextWithoutNotify(currentPlayer.level.ToString());
            playerNameInput.SetTextWithoutNotify(currentPlayer.playerName);

            ancestryButton.subtitle.text = currentPlayer.ancestry;
            backgroundButton.subtitle.text = currentPlayer.background;
            classButton.subtitle.text = currentPlayer.class_name;

            // Build
            foreach (var item in buildButtonList)
            {
                item.gameObject.SetActive(false);
                Destroy(item.gameObject, 0.001f);
            }
            buildButtonList.Clear();
            foreach (var level in currentPlayer.build)
            {
                Transform separator = Instantiate(buildLevelSeparator, Vector3.zero, Quaternion.identity, buildContainer);
                buildButtonList.Add(separator.gameObject);
                ButtonText separatorScript = separator.GetComponent<ButtonText>();
                separatorScript.text.text = level.Key.ToString();

                if (level.Value != null) // Separo choices de no choices
                {
                    List<BuildBlock> choices = new List<BuildBlock>();
                    List<BuildBlock> noChoices = new List<BuildBlock>();

                    // foreach (var item in level.Value)
                    //     if (false)
                    //         choices.Add(item.Value);
                    //     else
                    //         noChoices.Add(item.Value);

                    foreach (var item in choices)
                        ChoiceButtonsAssigner(item, GenerateBuildButton());
                    foreach (var item in noChoices)
                        NoChoiceButtonsAssigner(item, GenerateBuildButton());
                }
            }
        }

        #endregion

        #region --------------------------------------------BUILD--------------------------------------------

        private BuildButton GenerateBuildButton()
        {
            Transform button = Instantiate(buildButton, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(button.gameObject);
            return button.GetComponent<BuildButton>();
        }

        private void ChoiceButtonsAssigner(BuildBlock buildItem, BuildButton button)
        {
            if (buildItem.name == "initial ability boosts")
            {
                button.title.text = buildItem.name;
                button.button.onClick.AddListener(() => OnClickInitialAbilityBoosts());

                //     PF2E_InitAblBoostData initAblData = currentPlayer.Build_Get<PF2E_InitAblBoostData>("Level 1", "Initial Ability Boosts");
                //     int x = 0;
                //     if (initAblData != null)
                //         x = initAblData.lvl1boosts.Count - 4;
                //     if (x < 0)
                //         button.subtitle.text = "Boosts not assigned: " + (x * -1);
                //     else
                //         button.subtitle.text = "Everything is correct.";
            }
            else
            {
                // Debug.LogWarning("[Creation] Choice button: " + buildItem.type + " miss functionality!");

                // button.title.text = buildItem.type;
                // button.subtitle.text = buildItem.value;
            }
        }

        private void NoChoiceButtonsAssigner(BuildBlock buildItem, BuildButton button)
        {
            // button.title.text = buildItem.type;
            // button.subtitle.text = buildItem.value;
        }

        private void OnClickInitialAbilityBoosts()
        {
            ablBoostsSelector.OpenPlayerInitialAblBoostsPanel();
        }

        #endregion

        #region --------------------------------------------TABS--------------------------------------------

        private Color tabOn;
        private Color tabOff;

        public void OnClickTabStats()
        {
            if (!stats.isOpen)
            {
                CloseAllTabs();

                SetTabColors(statsTabImages, true);
                stats.OpenStatsPanel();
            }
        }

        public void OnClickTabInventory()
        {
            CloseAllTabs();
            SetTabColors(inventoryTabImages, true);
        }

        public void OnClickTabCompanions()
        {
            CloseAllTabs();
            SetTabColors(companionsTabImages, true);
        }

        public void OnClickTabSpells()
        {
            CloseAllTabs();
            SetTabColors(spellsTabImages, true);
        }

        private void CloseAllTabs()
        {
            if (stats.isOpen)
                stats.CloseStatsPanel();

            SetTabColors(statsTabImages, false);
            SetTabColors(inventoryTabImages, false);
            SetTabColors(companionsTabImages, false);
            SetTabColors(spellsTabImages, false);
        }

        private void SetTabColors(Image[] array, bool active)
        {
            if (array != null)
            {
                if (active)
                    foreach (var item in array)
                        item.color = tabOn;
                else
                    foreach (var item in array)
                        item.color = tabOff;
            }
        }

        #endregion

        #region --------------------------------------------BUTTONS & INPUT--------------------------------------------

        /// <summary> Called by the level inputfiedld. </summary>
        public void OnEndEditLevel()
        {
            currentPlayer.level = int.Parse(levelInput.text);
            RefreshPlayerIntoPanel();
        }

        /// <summary> Called by the name inputfiedld. </summary>
        public void OnEndEditName()
        {
            currentPlayer.playerName = playerNameInput.text;
            RefreshPlayerIntoPanel();
        }

        /// <summary> Called by the ancestry selection button. </summary>
        public void OnClickSelectAncestry()
        {
            ABCSelector.OpenSelectorPanel();
            ABCSelector.Display("ancestry");
            ABCSelector.acceptButton.onClick.AddListener(() => SelectAncestryAccept());
            ABCSelector.backButton.onClick.AddListener(() => SelectAncestryCancel());
        }
        private void SelectAncestryAccept()
        {
            currentPlayer.ancestry = ABCSelector.selectedAncestry;
            CloseABCPanel(true);
        }
        private void SelectAncestryCancel()
        {
            CloseABCPanel(false);
        }

        /// <summary> Called by the backgrond selection button. </summary>
        public void OnClickSelectBackground()
        {
            ABCSelector.OpenSelectorPanel();
            ABCSelector.Display("background");
            ABCSelector.acceptButton.onClick.AddListener(() => SelectBackgroundAccept());
            ABCSelector.backButton.onClick.AddListener(() => SelectBackgroundCancel());
        }
        private void SelectBackgroundAccept()
        {
            currentPlayer.background = ABCSelector.selectedBackground;
            CloseABCPanel(true);
        }
        private void SelectBackgroundCancel()
        {
            CloseABCPanel(false);
        }

        /// <summary> Called by the class selection button. </summary>
        public void OnClickSelectedClass()
        {
            ABCSelector.OpenSelectorPanel();
            ABCSelector.Display("class");
            ABCSelector.acceptButton.onClick.AddListener(() => SelectClassAccept());
            ABCSelector.backButton.onClick.AddListener(() => SelectClassCancel());
        }
        private void SelectClassAccept()
        {
            currentPlayer.class_name = ABCSelector.selectedClass;
            CloseABCPanel(true);
        }
        private void SelectClassCancel()
        {
            CloseABCPanel(false);
        }

        private void CloseABCPanel(bool refreshPanels)
        {
            ABCSelector.CloseSelectorPanel();

            ABCSelector.acceptButton.onClick.RemoveAllListeners();
            ABCSelector.backButton.onClick.RemoveAllListeners();

            if (refreshPanels)
                RefreshPlayerIntoPanel();
        }

        #endregion

    }

}
