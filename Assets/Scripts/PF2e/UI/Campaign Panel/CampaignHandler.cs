using System;
using System.Collections.Generic;
using System.IO;
using Pathfinder2e.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinder2e.GameData
{

    public class CampaignHandler : MonoBehaviour
    {
        // Controls the PF2e campaing retrieval and send thems to load and display
        // Controls the PF2e board, character, enemies and npcs buttons

        [SerializeField] private CharacterCreation characterCreation = null;
        [SerializeField] private BoardHandler boardHandler = null;
        [SerializeField] private ConfirmationController confirmation = null;

        [Header("Campaign Stuff")]
        [SerializeField] private Window window = null;
        [SerializeField] private TMP_Text campaignName = null;

        [Header("Boards")]
        [SerializeField] private Transform boardsContainer = null;
        [SerializeField] private GameObject boardsButtonPrefab = null;
        private List<GameObject> boardsButtonList = new List<GameObject>();

        [Header("Characters")]
        [SerializeField] private Transform charactersContainer = null;
        [SerializeField] private GameObject charactersButtonPrefab = null;
        private List<GameObject> charactersButtonList = new List<GameObject>();

        [Header("Enemies")]
        [SerializeField] private Transform enemiesContainer = null;
        [SerializeField] private GameObject enemiesButtonPrefab = null;
        private List<GameObject> enemiesButtonList = new List<GameObject>();

        [Header("NPCs")]
        [SerializeField] private Transform npcsContainer = null;
        [SerializeField] private GameObject npcsButtonPrefab = null;
        private List<GameObject> npcsButtonList = new List<GameObject>();

        void Awake()
        {
            characterCreation.OnCharacterCreationClose += RefreshCampaignContainers;

            if (Time.time < 10)
            {
                char sep = Path.DirectorySeparatorChar;

                if (!Directory.Exists(Globals.SystemData.PF2EFolderPath))
                    Directory.CreateDirectory(Globals.SystemData.PF2EFolderPath);
                if (!Directory.Exists(Globals.SystemData.PF2ECampaignsPath))
                    Directory.CreateDirectory(Globals.SystemData.PF2ECampaignsPath);
                if (!Directory.Exists(Globals.SystemData.PF2EAdditionalContentPath))
                    Directory.CreateDirectory(Globals.SystemData.PF2EAdditionalContentPath);

                var campaigns = Directory.GetFiles(Globals.SystemData.PF2ECampaignsPath);
                if (campaigns != null && Globals_PF2E.CampaignIDs.Count == 0)
                    foreach (var item in campaigns)
                        Globals_PF2E.CampaignIDs.Add(Path.GetFileName(item), Globals.SystemData.PF2ECampaignsPath + sep + item);
            }
        }

        public void OnClickBackButton()
        {
            CloseCampaingPanel();
        }

        public void OnClickDeleteButton()
        {
            DeleteCampaign();
        }

        #region --------CAMPAIGNS--------

        private void OpenCampaingPanel()
        {
            window.OpenWindow();
        }

        private void CloseCampaingPanel()
        {
            window.CloseWindow();
        }

        /// <summary> Called by GameSelectorController after asking the name for a new campaign has been prompted and accepted. </summary>
        public void CreateCampaign(string name)
        {
            string newCampaignID = Globals_PF2E.CreateCampaign(name);
            if (newCampaignID != "")
                LoadCampaign(newCampaignID);
        }

        /// <summary> Load given Campaign. Called by campaign buttons that enumerate existing Campaigns. </summary>
        public void LoadCampaign(string campaignID)
        {
            // Open Campaign Panel
            campaignName.text = campaignID.Replace(".json", "");

            // Set current campaing and refresh boards, characters, enemies and npcs
            Globals_PF2E.LoadCampaign(campaignID);
            RefreshCampaignContainers();

            OpenCampaingPanel();
        }

        // Called by Delete Button in PF2E Campaign Panel
        private void DeleteCampaign()
        {
            confirmation.AskForConfirmation("Do you want to delete current campaign?", DeleteCampaignCallback);
        }
        private void DeleteCampaignCallback(bool value)
        {
            if (value)
            {
                CloseCampaingPanel();
                Globals_PF2E.DeleteCampaign();
            }
        }

        #endregion

        #region --------BOARDS--------

        /// <summary> Called by the + button in the boards section of the campaign panel </summary>
        public void OnClickNewBoardButton()
        {
            boardHandler.NewBoard();
        }

        // Edition
        private void OnClickBoardButtonPlay(string board)
        {
            boardHandler.LoadBoard(Globals_PF2E.CurrentCampaign.boards[board]);
        }

        // Deletion
        private string boardToDelete = "";
        private void OnClickBoardButtonDelete(string board)
        {
            boardToDelete = board;
            confirmation.AskForConfirmation("Are you sure you want to delete this board?", OnClickBoardButtonDeleteCallback);
        }
        private void OnClickBoardButtonDeleteCallback(bool value)
        {
            if (value)
            {
                Globals_PF2E.CurrentCampaign.boards.Remove(boardToDelete);
                RefreshCampaignContainers();
                Globals_PF2E.SaveCampaign();
            }

            boardToDelete = "";
        }

        #endregion

        #region --------CHARACTERS--------

        /// <summary> Called by the + button in the characters section of the campaign panel </summary>
        public void OnClickNewPlayerButton()
        {
            characterCreation.NewPlayer();
        }

        // Edition
        private void OnClickPayerButtonEdit(string character)
        {
            characterCreation.LoadPlayer(Globals_PF2E.CurrentCampaign.characters[character]);
        }

        // Deletion
        private string characterToDelete = "";
        private void OnClickPayerButtonDelete(string character)
        {
            characterToDelete = character;
            confirmation.AskForConfirmation("Are you sure you want to delete this character?", OnClickPayerButtonDeleteCallback);
        }
        private void OnClickPayerButtonDeleteCallback(bool value)
        {
            if (value)
            {
                Globals_PF2E.CurrentCampaign.characters.Remove(characterToDelete);
                RefreshCampaignContainers();
                Globals_PF2E.SaveCampaign();
            }

            characterToDelete = "";
        }

        #endregion

        public void RefreshCampaignContainers()
        {
            // Delete all buttons
            foreach (var button in boardsButtonList)
                Destroy(button, 0.001f);
            foreach (var button in charactersButtonList)
                Destroy(button, 0.001f);
            foreach (var button in enemiesButtonList)
                Destroy(button, 0.001f);
            foreach (var button in npcsButtonList)
                Destroy(button, 0.001f);

            boardsButtonList.Clear();
            charactersButtonList.Clear();
            enemiesButtonList.Clear();
            npcsButtonList.Clear();

            if (Globals_PF2E.CurrentCampaign.boards != null)
                foreach (var board in Globals_PF2E.CurrentCampaign.boards)
                {
                    Transform newBoardButton = Instantiate(boardsButtonPrefab, Vector3.zero, Quaternion.identity, boardsContainer).transform;
                    BoardButton newBoardButtonScript = newBoardButton.GetComponent<BoardButton>();
                    newBoardButtonScript.boardName.text = board.Value.boardName;

                    newBoardButtonScript.playButton.onClick.AddListener(() => OnClickBoardButtonPlay(board.Key));
                    newBoardButtonScript.deleteButton.onClick.AddListener(() => OnClickBoardButtonDelete(board.Key));

                    boardsButtonList.Add(newBoardButton.gameObject);
                }
            if (Globals_PF2E.CurrentCampaign.characters != null)
                foreach (var character in Globals_PF2E.CurrentCampaign.characters)
                {
                    Transform newPlayerButton = Instantiate(charactersButtonPrefab, Vector3.zero, Quaternion.identity, charactersContainer).transform;
                    CharacterButton newPlayerButtonScript = newPlayerButton.GetComponent<CharacterButton>();
                    newPlayerButtonScript.level.text = character.Value.level.ToString();
                    newPlayerButtonScript.characterName.text = character.Value.name;

                    newPlayerButtonScript.editButton.onClick.AddListener(() => OnClickPayerButtonEdit(character.Key));
                    newPlayerButtonScript.deleteButton.onClick.AddListener(() => OnClickPayerButtonDelete(character.Key));

                    charactersButtonList.Add(newPlayerButton.gameObject);
                }
            for (int i = 0; i < Globals_PF2E.CurrentCampaign.enemies.Count; i++)
            {
                Transform newEnemyButton = Instantiate(enemiesButtonPrefab, Vector3.zero, Quaternion.identity, enemiesContainer).transform;
                BoardButton newEnemyButtonScript = newEnemyButton.GetComponent<BoardButton>();

                enemiesButtonList.Add(newEnemyButton.gameObject);
            }
            for (int i = 0; i < Globals_PF2E.CurrentCampaign.npcs.Count; i++)
            {
                Transform newNPCButton = Instantiate(npcsButtonPrefab, Vector3.zero, Quaternion.identity, npcsContainer).transform;
                BoardButton newNPCButtonScript = newNPCButton.GetComponent<BoardButton>();

                npcsButtonList.Add(newNPCButton.gameObject);
            }
        }

    }

}
