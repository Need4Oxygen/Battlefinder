using System;
using System.Collections.Generic;
using Pathfinder2e.Character;
using Pathfinder2e.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Pathfinder2e.GameData;
using UnityEditor;
using System.Linq;
using Tools;

namespace Pathfinder2e.Character
{

    public class CharacterCreation : MonoBehaviour
    {
        [Serializable] private class StrSpritePair { public string label; public Sprite sprite; }
        public DVoid OnCharacterCreationClose = null;

        [SerializeField] private Window window = null;
        [SerializeField] private ABCSelector ABCSelector = null;
        [SerializeField] private AblBoostsSelector ablBoostsSelector = null;
        [SerializeField] private Searcher searcher = null;
        [SerializeField] private SkillPlanner sklPlanner = null;

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
        [SerializeField] private Transform buildMiniButton = null;
        [SerializeField] private Transform buildMiniButtonContainer = null;
        [SerializeField] private List<StrSpritePair> buildButtonIcons = null;

        private List<GameObject> buildButtonList = new List<GameObject>();
        public CharacterData currentPlayer = null;

        void Start()
        {
            tabOnColor = Globals.Theme["background_1"];
            tabOffColor = Globals.Theme["background_2"];
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- GENERAL

        private void OpenPlayerCreationPanel()
        {
            window.OpenWindow();
            OnClickTabStats();
        }

        private void ClosePlayerCreationPanel()
        {
            SavePlayer();

            if (ablBoostsSelector.init_isOpen)
                ablBoostsSelector.Close_InitialAblBoosts();
            if (ablBoostsSelector.other_isOpen)
                ablBoostsSelector.Close_OtherAblBoosts();
            if (searcher.isOpen)
                searcher.CloseSearcher();

            window.CloseWindow();
            currentPlayer = null;

            CloseAllTabs();

            if (OnCharacterCreationClose != null)
                OnCharacterCreationClose();
        }

        public void OnClickAcceptButton()
        {
            ClosePlayerCreationPanel();
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- PLAYERS

        /// <summary> Generates new player and open the player panel. </summary>
        public void NewPlayer()
        {
            string newGuid = Guid.NewGuid().ToString();
            currentPlayer = new CharacterData();
            currentPlayer.guid = newGuid;

            ABCSelector.Display("ancestry");
            ABCSelector.acceptButton.onClick.AddListener(() => NewPlayerProcessAccept());
            ABCSelector.backButton.onClick.AddListener(() => NewPlayerProcessBack());
        }

        private void NewPlayerProcessAccept()
        {
            if (ABCSelector.currentlyDisplaying == "ancestry")
            {
                currentPlayer.Ancestry_Set(ABCSelector.selectedAncestry);
                ABCSelector.Display("background");
            }
            else if (ABCSelector.currentlyDisplaying == "background")
            {
                currentPlayer.Background_Set(ABCSelector.selectedBackground);
                ABCSelector.Display("class");
            }
            else if (ABCSelector.currentlyDisplaying == "class")
            {
                currentPlayer.Class_Set(ABCSelector.selectedClass);
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
                currentPlayer.Background_Set("");
                ABCSelector.Display("ancestry");

            }
            else if (ABCSelector.currentlyDisplaying == "class")
            {
                currentPlayer.Class_Set("");
                ABCSelector.Display("background");
            }
        }

        /// <summary> Load new player and open the player panel. </summary>
        public void LoadPlayer(CharacterData player)
        {
            currentPlayer = player;
            RefreshPlayerIntoPanel();
            OpenPlayerCreationPanel();
        }

        /// <summary> Save Player into campaing player list. </summary>
        public void SavePlayer()
        {
            if (Globals_PF2E.CurrentCampaign.characters.ContainsKey(currentPlayer.guid))
                GameData.Globals_PF2E.CurrentCampaign.characters[currentPlayer.guid] = currentPlayer;
            else
                GameData.Globals_PF2E.CurrentCampaign.characters.Add(currentPlayer.guid, currentPlayer);

            GameData.Globals_PF2E.SaveCampaign();
        }

        /// <summary> Refresh UI with player data. </summary>
        public void RefreshPlayerIntoPanel()
        {
            if (stats.isOpen)
                stats.RefreshPlayerIntoPanel();

            levelInput.SetTextWithoutNotify(currentPlayer.level.ToString());
            playerNameInput.SetTextWithoutNotify(currentPlayer.name);

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
            for (int i = 0; i < progression.progression.Count; i++)
            {
                ClassStage stage = progression.progression[i];

                // Generate Separator as Level 1 , Level 2...
                GenerateSeparator(i + 1);

                // Generate minibuttons as for unspent skills, skill choices, free skills, increases...
                List<BuildButton> miniButtons = new List<BuildButton>();

                foreach (var item in currentPlayer.skill_unspent.Where(x => x.Key.level.ToInt() == i + 1)) // Unspent
                {
                    BuildButton miniButt = GenerateBuildMiniButton($"{item.Key.from.ToUpperFirst()} Unspent", RuleElement.IsNullOrEmpty(item.Value) ? "X" : "O", null);
                    RuleElement listenerRule = item.Key;
                    miniButt.button.onClick.AddListener(() => OnClick_Skill(listenerRule));
                    miniButtons.Add(miniButt);
                }

                foreach (var item in currentPlayer.skill_choice.Where(x => x.Key.level.ToInt() == i + 1)) // Choices
                {
                    BuildButton miniButt = GenerateBuildMiniButton($"{item.Key.from.ToUpperFirst()} Choice", RuleElement.IsNullOrEmpty(item.Value) ? "X" : "O", null);
                    RuleElement listenerRule = item.Key;
                    miniButt.button.onClick.AddListener(() => OnClick_Skill(listenerRule));
                    miniButtons.Add(miniButt);
                }

                foreach (var item in currentPlayer.skill_free.Where(x => x.Key.level.ToInt() == i + 1)) // Free
                {
                    BuildButton miniButt = GenerateBuildMiniButton($"{item.Key.from.ToUpperFirst()} Free", item.Value != null ? item.Value.Count.ToString() + $"/{item.Key.value}" : "0" + $"/{item.Key.value}", null);
                    RuleElement listenerRule = item.Key;
                    miniButt.button.onClick.AddListener(() => OnClick_Skill(listenerRule));
                    miniButtons.Add(miniButt);
                }

                foreach (var item in currentPlayer.skill_increase.Where(x => x.Key.level.ToInt() == i + 1)) // Increase
                {
                    BuildButton miniButt = GenerateBuildMiniButton($"{item.Key.from.ToUpperFirst()} Increase", RuleElement.IsNullOrEmpty(item.Value) ? "X" : "O", null);
                    RuleElement listenerRule = item.Key;
                    miniButt.button.onClick.AddListener(() => OnClick_Skill(listenerRule));
                    miniButtons.Add(miniButt);
                }

                if (miniButtons.Count > 0)
                {
                    Transform miniCont = GenerateBuildMiniButtonContainer();
                    foreach (var but in miniButtons)
                    {
                        but.transform.SetParent(miniCont);
                        but.transform.localScale = Vector3.one;
                    }
                }

                // Generate other buttons
                foreach (string item in stage.items)
                {
                    BuildButton button = null;
                    Sprite icon = null;
                    foreach (var pair in buildButtonIcons)
                        if (pair.label == item) { icon = pair.sprite; break; }

                    switch (item)
                    {
                        case "initial proficiencies":
                            button = GenerateBuildButton("Initial Ability Boosts", "", icon);
                            button.button.onClick.AddListener(() => OnClick_InitialAbilityBoosts());
                            break;
                        case "ability boosts":
                            button = GenerateBuildButton($"LvL {i + 1} Ability Boosts", "", icon);
                            int lvl = i + 1;
                            button.button.onClick.AddListener(() => OnClick_OtherAbilityBoosts(lvl));
                            break;
                        case "ancestry feat":
                            button = GenerateBuildButton("Ancestry Feat", "---", icon);
                            button.button.onClick.AddListener(() => OnClick_AncestryFeat());
                            break;
                        case "class feat":
                            button = GenerateBuildButton("Class Feat", "---", icon);
                            button.button.onClick.AddListener(() => OnClick_ClassFeat());
                            break;
                        case "skill feat":
                            button = GenerateBuildButton("Skill Feat", "---", icon);
                            button.button.onClick.AddListener(() => OnClick_SkillFeat());
                            break;
                        case "general feat":
                            button = GenerateBuildButton("General Feat", "---", icon);
                            button.button.onClick.AddListener(() => OnClick_GeneralFeat());
                            break;
                        case "alchemy":
                            button = GenerateBuildButton("Class Feature", "Alchemy", icon);
                            // button.button.onClick.AddListener(() => OnClick_GeneralFeat());
                            break;

                        default:
                            continue;
                    }
                }
            }
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BUILD
        private GameObject GenerateSeparator(int stage)
        {
            Transform separator = Instantiate(buildLevelSeparator, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(separator.gameObject);
            ButtonText separatorScript = separator.GetComponent<ButtonText>();
            separatorScript.text.text = $"Level {stage}";

            return separator.gameObject;
        }

        private BuildButton GenerateBuildButton() { return GenerateBuildButton("", "", null); }
        private BuildButton GenerateBuildButton(string title, string subtitle) { return GenerateBuildButton(title, subtitle, null); }
        private BuildButton GenerateBuildButton(string title, string subtitle, Sprite icon)
        {
            Transform button = Instantiate(buildButton, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(button.gameObject);

            BuildButton buttonScript = button.GetComponent<BuildButton>();
            buttonScript.title.text = string.IsNullOrEmpty(title) ? "" : title;
            buttonScript.subtitle.text = string.IsNullOrEmpty(subtitle) ? "" : subtitle;
            if (icon != null) buttonScript.icon.sprite = icon;

            return button.GetComponent<BuildButton>();
        }

        private BuildButton GenerateBuildMiniButton(Transform parent) { return GenerateBuildMiniButton("", "", parent); }
        private BuildButton GenerateBuildMiniButton(string title, string subtitle, Transform parent)
        {
            Transform button = Instantiate(buildMiniButton, Vector3.zero, Quaternion.identity, parent);
            buildButtonList.Add(button.gameObject);

            BuildButton buttonScript = button.GetComponent<BuildButton>();
            buttonScript.title.text = string.IsNullOrEmpty(title) ? "" : title;
            buttonScript.subtitle.text = string.IsNullOrEmpty(subtitle) ? "" : subtitle;

            return button.GetComponent<BuildButton>();
        }

        private Transform GenerateBuildMiniButtonContainer()
        {
            Transform button = Instantiate(buildMiniButtonContainer, Vector3.zero, Quaternion.identity, buildContainer);
            buildButtonList.Add(button.gameObject);
            return button;
        }

        private void NoChoiceButtonsAssigner(BuildBlock buildItem, BuildButton button)
        {
            // button.title.text = buildItem.type;
            // button.subtitle.text = buildItem.value;
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- LISTENERS
        private void OnClick_InitialAbilityBoosts()
        {
            ablBoostsSelector.Open_InitialAblBoosts();
        }

        private void OnClick_OtherAbilityBoosts(int lvl)
        {
            ablBoostsSelector.Open_OtherAblBoosts(lvl);
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

        private void OnClick_Skill(RuleElement rule)
        {
            sklPlanner.OpenWithRule(rule);
        }


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- TABS

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


        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- BUTTONS & INPUT

        /// <summary> Called by the level inputfiedld. </summary>
        public void OnEndEditLevel()
        {
            currentPlayer.Level_Set(levelInput.text.ToInt());
            RefreshPlayerIntoPanel();
        }

        /// <summary> Called by the name inputfiedld. </summary>
        public void OnEndEditName()
        {
            currentPlayer.name = playerNameInput.text;
            RefreshPlayerIntoPanel();
        }

        /// <summary> Called by the ancestry selection button. </summary>
        public void OnClickSelectAncestry()
        {
            if (ablBoostsSelector.init_isOpen) return;
            ABCSelector.Display("ancestry");
            ABCSelector.acceptButton.onClick.AddListener(() => SelectAncestryAccept());
            ABCSelector.backButton.onClick.AddListener(() => SelectAncestryCancel());
        }
        private void SelectAncestryAccept()
        {
            currentPlayer.Ancestry_Set(ABCSelector.selectedAncestry);
            CloseABCPanel(true);
        }
        private void SelectAncestryCancel()
        {
            CloseABCPanel(false);
        }

        /// <summary> Called by the backgrond selection button. </summary>
        public void OnClickSelectBackground()
        {
            if (ablBoostsSelector.init_isOpen) return;
            ABCSelector.Display("background");
            ABCSelector.acceptButton.onClick.AddListener(() => SelectBackgroundAccept());
            ABCSelector.backButton.onClick.AddListener(() => SelectBackgroundCancel());
        }
        private void SelectBackgroundAccept()
        {
            currentPlayer.Background_Set(ABCSelector.selectedBackground);
            CloseABCPanel(true);
        }
        private void SelectBackgroundCancel()
        {
            CloseABCPanel(false);
        }

        /// <summary> Called by the class selection button. </summary>
        public void OnClickSelectedClass()
        {
            if (ablBoostsSelector.init_isOpen) return;
            ABCSelector.Display("class");
            ABCSelector.acceptButton.onClick.AddListener(() => SelectClassAccept());
            ABCSelector.backButton.onClick.AddListener(() => SelectClassCancel());
        }
        private void SelectClassAccept()
        {
            currentPlayer.Class_Set(ABCSelector.selectedClass);
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
            {
                RefreshPlayerIntoPanel();
            }
        }

    }

}
