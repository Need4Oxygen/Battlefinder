﻿using System;
using System.Collections.Generic;
using System.IO;
using Pathfinder2e.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinder2e.GameData
{

    public class CampaingHandler : MonoBehaviour
    {
        // Controls the PF2e campaing retrieval and send thems to load and display
        // Controls the PF2e board, player, enemies and npcs buttons

        [SerializeField] private CharacterCreation characterCreation = null;
        [SerializeField] private BoardHandler boardHandler = null;
        [SerializeField] private ConfirmationController confirmation = null;

        [Header("Campaign Stuff")]
        [SerializeField] private CanvasGroup campaignPanel = null;
        [SerializeField] private GameObject campaignContainersPanel = null;
        [SerializeField] private TMP_Text campaignName = null;

        [Header("Boards")]
        [SerializeField] private Transform boardsContainer = null;
        [SerializeField] private GameObject boardsButtonPrefab = null;
        private List<GameObject> boardsButtonList = new List<GameObject>();

        [Header("Players")]
        [SerializeField] private Transform playersContainer = null;
        [SerializeField] private GameObject playersButtonPrefab = null;
        private List<GameObject> playersButtonList = new List<GameObject>();

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

        void Start()
        {
            StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0f));

            characterCreation.OnCharacterCreationClose += RefreshCampaignContainers;
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
            StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 1f, 1f, 0.1f));
            campaignContainersPanel.SetActive(true);
        }

        private void CloseCampaingPanel()
        {
            StartCoroutine(PanelFader.RescaleAndFade(campaignPanel.transform, campaignPanel, 0.85f, 0f, 0.1f));
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

            // Set current campaing and refresh boards, players, enemies and npcs
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

        #region --------PLAYERS--------

        /// <summary> Called by the + button in the players section of the campaign panel </summary>
        public void OnClickNewPlayerButton()
        {
            characterCreation.NewPlayer();
        }

        // Edition
        private void OnClickPayerButtonEdit(string player)
        {
            characterCreation.LoadPlayer(Globals_PF2E.CurrentCampaign.players[player]);
        }

        // Deletion
        private string playerToDelete = "";
        private void OnClickPayerButtonDelete(string player)
        {
            playerToDelete = player;
            confirmation.AskForConfirmation("Are you sure you want to delete this character?", OnClickPayerButtonDeleteCallback);
        }
        private void OnClickPayerButtonDeleteCallback(bool value)
        {
            if (value)
            {
                Globals_PF2E.CurrentCampaign.players.Remove(playerToDelete);
                RefreshCampaignContainers();
                Globals_PF2E.SaveCampaign();
            }

            playerToDelete = "";
        }

        #endregion

        public void RefreshCampaignContainers()
        {
            // Delete all buttons
            foreach (var button in boardsButtonList)
                Destroy(button, 0.001f);
            foreach (var button in playersButtonList)
                Destroy(button, 0.001f);
            foreach (var button in enemiesButtonList)
                Destroy(button, 0.001f);
            foreach (var button in npcsButtonList)
                Destroy(button, 0.001f);

            boardsButtonList.Clear();
            playersButtonList.Clear();
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
            if (Globals_PF2E.CurrentCampaign.players != null)
                foreach (var player in Globals_PF2E.CurrentCampaign.players)
                {
                    Transform newPlayerButton = Instantiate(playersButtonPrefab, Vector3.zero, Quaternion.identity, playersContainer).transform;
                    PlayerButton newPlayerButtonScript = newPlayerButton.GetComponent<PlayerButton>();
                    newPlayerButtonScript.level.text = player.Value.level.ToString();
                    newPlayerButtonScript.playerName.text = player.Value.playerName;

                    newPlayerButtonScript.editButton.onClick.AddListener(() => OnClickPayerButtonEdit(player.Key));
                    newPlayerButtonScript.deleteButton.onClick.AddListener(() => OnClickPayerButtonDelete(player.Key));

                    playersButtonList.Add(newPlayerButton.gameObject);
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
