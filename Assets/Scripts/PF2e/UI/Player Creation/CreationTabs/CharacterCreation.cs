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

        [SerializeField] private Window window = null;
        [SerializeField] private ABCSelector ABCSelector = null;
        [SerializeField] private AblBoostsSelector ablBoostsSelector = null;
        [SerializeField] private Searcher searcher = null;

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
            tabOnColor = Globals.Theme["background_1"];
            tabOffColor = Globals.Theme["background_2"];
        }

        #region --------------------------------------------GENERAL--------------------------------------------

        private void OpenPlayerCreationPanel()
        {
            window.OpenWindow();
            OnClickTabStats();
        }

        private void ClosePlayerCreationPanel()
        {
            window.CloseWindow();
            currentPlayer = null;

            CloseAllTabs();

            if (OnCharacterCreationClose != null)
                OnCharacterCreationClose();
        }

        public void OnClickAcceptButton()
        {
            SavePlayer();

            if (ablBoostsSelector.isOpen)
                ablBoostsSelector.ClosePlayerInitialAblBoosts();
            if (searcher.isOpen)
                searcher.CloseSearcher();

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
            ClassProgression progression = DB.ClassProgression.Find(ctx => ctx.name == currentPlayer.class_name);
            int stageCounter = 0;

            foreach (var stage in progression.progression)
            {
                stageCounter++;
                GenerateSeparator(stageCounter);

                foreach (var item in stage.items)
                {
                    switch (item)
                    {
                        case "initial proficiencies": // I'm ussing this to initialize abilities boost and later, when developed, also fot unspent lectures
                            BuildButton initAblBoosts = GenerateBuildButton(item, "");
                            initAblBoosts.button.onClick.AddListener(() => OnClick_InitialAbilityBoosts());
                            // Should get into the build, retrieve init abl choices, check if they have errors and display a message
                            // if (currentPlayer.Build_GetFromBlock<>) 
                            break;
                        case "ancestry feat":
                            // search in build for choice
                            BuildButton ancestryFeat = GenerateBuildButton("Ancestry Feat", "");
                            ancestryFeat.button.onClick.AddListener(() => OnClick_AncestryFeat());
                            break;
                        case "class feat":
                            // search in build for choice
                            BuildButton classFeat = GenerateBuildButton("Class Feat", "");
                            classFeat.button.onClick.AddListener(() => OnClick_ClassFeat());
                            break;
                        case "skill feat":
                            // search in build for choice
                            BuildButton skillFeat = GenerateBuildButton("Skill Feat", "");
                            skillFeat.button.onClick.AddListener(() => OnClick_SkillFeat());
                            break;
                        case "general feat":
                            // search in build for choice
                            BuildButton generalFeat = GenerateBuildButton("General Feat", "");
                            generalFeat.button.onClick.AddListener(() => OnClick_GeneralFeat());
                            break;
                        case "alchemy": // Features should be checked last, searching in class feature feats
                            BuildButton alch = GenerateBuildButton("Class Feature", "Alchemy");
                            break;
                        default:
                            // buildButton
                            break;
                    }
                }
            }
        }

        #endregion

        #region --------------------------------------------BUILD--------------------------------------------

        private GameObject GenerateSeparator(int stage)
        {
            Transform separator = Instantiate(buildLevelSeparator, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(separator.gameObject);
            ButtonText separatorScript = separator.GetComponent<ButtonText>();
            separatorScript.text.text = $"Level {stage}";

            return separator.gameObject;
        }

        private BuildButton GenerateBuildButton() { return GenerateBuildButton("", ""); }
        private BuildButton GenerateBuildButton(string title, string subtitle)
        {
            Transform button = Instantiate(buildButton, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(button.gameObject);

            BuildButton buttonScript = button.GetComponent<BuildButton>();
            if (!string.IsNullOrEmpty(title))
                buttonScript.title.text = title;
            if (!string.IsNullOrEmpty(subtitle))
                buttonScript.subtitle.text = subtitle;

            return button.GetComponent<BuildButton>();
        }

        private void NoChoiceButtonsAssigner(BuildBlock buildItem, BuildButton button)
        {
            // button.title.text = buildItem.type;
            // button.subtitle.text = buildItem.value;
        }

        private void OnClick_InitialAbilityBoosts()
        {
            ablBoostsSelector.OpenPlayerInitialAblBoosts();
        }

        private void OnClick_AncestryFeat()
        {
            searcher.Search(E_Searcher_Type.AncestryFeat);
        }

        private void OnClick_ClassFeat()
        {
            searcher.Search(E_Searcher_Type.ClassFeat);
        }

        private void OnClick_SkillFeat()
        {
            searcher.Search(E_Searcher_Type.SkillFeat);
        }

        private void OnClick_GeneralFeat()
        {
            searcher.Search(E_Searcher_Type.GeneralFeat);
        }

        #endregion

        #region --------------------------------------------TABS--------------------------------------------

        private Color tabOnColor;
        private Color tabOffColor;

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
                        item.color = tabOnColor;
                else
                    foreach (var item in array)
                        item.color = tabOffColor;
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
            if (ablBoostsSelector.isOpen) return;
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
            if (ablBoostsSelector.isOpen) return;
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
            if (ablBoostsSelector.isOpen) return;
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
