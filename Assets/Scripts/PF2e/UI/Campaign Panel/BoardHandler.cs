using System;
using System.Collections.Generic;
using Pathfinder2e.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Pathfinder2e.GameData
{

    public class BoardHandler : MonoBehaviour
    {
        [SerializeField] private WindowRIP window = null;
        [SerializeField] private CampaignHandler campaignHandler = null;
        [SerializeField] private TMP_InputField boardNameInput = null;

        [Space(15)]
        [SerializeField] private Transform actorsButtonPrefab = null;
        [SerializeField] private Transform actorsButtonContainer = null;

        private List<ButtonText> actorButtonList = new List<ButtonText>();
        private BoardData currentBoard = null;

        private void OpenBoardPanel()
        {
            window.OpenWindow();
        }

        private void CloseBoardPanel()
        {
            window.CloseWindow();

            currentBoard = null;
        }

        /// <summary> Called by CampaignHandler when the + button in the boards section of the campaign panel is pressed </summary>
        public void NewBoard()
        {
            string newGuid = Guid.NewGuid().ToString();
            currentBoard = new BoardData();
            currentBoard.guid = newGuid;

            RefreshBoardPanel();
            OpenBoardPanel();
            Globals_PF2E.SaveBoard(currentBoard);
        }

        /// <summary> Called by CampaignHandler when the play button on a board button is pressed </summary>
        public void LoadBoard(BoardData board)
        {
            currentBoard = board;

            RefreshBoardPanel();
            OpenBoardPanel();
        }

        private void RefreshBoardPanel()
        {
            boardNameInput.SetTextWithoutNotify(currentBoard.boardName);

            // Actor lists
            for (int i = actorButtonList.Count - 1; i > 0; i--)
                Destroy(actorButtonList[i].gameObject);
            actorButtonList.Clear();

            List<PActor> actorList = new List<PActor>();
            actorList.AddRange(currentBoard.players);
            actorList.AddRange(currentBoard.npcs);
            actorList.AddRange(currentBoard.enemies);

            foreach (var item in actorList)
            {
                Transform newActorButton = Instantiate(actorsButtonPrefab, Vector3.zero, Quaternion.identity, actorsButtonContainer).transform;
                ButtonText newActorButtonScript = newActorButton.GetComponent<ButtonText>();
                newActorButtonScript.text.text = item.actorName;

                actorButtonList.Add(newActorButtonScript);
            }
        }

        /// <summary> Called by the name inputfiedld in board panel </summary>
        public void OnEndEditName()
        {
            currentBoard.boardName = boardNameInput.text;
            Globals_PF2E.SaveBoard(currentBoard);
            RefreshBoardPanel();
        }

        /// <summary> Called by the play button in board panel </summary>
        public void OnClickPlayButton()
        {
            Globals_PF2E.CurrentBoard = currentBoard;
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayPF2e", LoadSceneMode.Single);

            CloseBoardPanel();
        }

        /// <summary> Called by the back button in board panel </summary>
        public void OnClickBackButton()
        {
            CloseBoardPanel();
            campaignHandler.RefreshCampaignContainers();
        }

        /// <summary> Called by the create button in board panel </summary>
        public void OnClickCreateButton() { }
        /// <summary> Called by the join button in board panel </summary>
        public void OnClickJoinButton() { }

    }

}