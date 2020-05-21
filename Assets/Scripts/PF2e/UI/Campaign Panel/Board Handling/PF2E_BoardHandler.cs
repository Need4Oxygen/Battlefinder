using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PF2E_BoardHandler : MonoBehaviour
{
    [SerializeField] private string pathfinder2ePlayScene = null;

    [Space(15)]
    [SerializeField] private PF2E_CampaingHandler campaignHandler = null;
    [SerializeField] private CanvasGroup boardPanel = null;
    [SerializeField] private TMP_InputField boardNameInput = null;

    [Space(15)]
    [SerializeField] private Transform actorsButtonPrefab = null;
    [SerializeField] private Transform actorsButtonContainer = null;

    private List<ButtonText> actorButtonList = new List<ButtonText>();
    private PF2E_BoardData currentBoard = null;

    void Start()
    {
        StartCoroutine(PanelFader.RescaleAndFade(boardPanel.transform, boardPanel, 0.85f, 0f, 0f));
    }

    private void OpenBoardPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(boardPanel.transform, boardPanel, 1f, 1f, 0.1f));
    }

    private void CloseBoardPanel()
    {
        StartCoroutine(PanelFader.RescaleAndFade(boardPanel.transform, boardPanel, 0.85f, 0f, 0.1f));

        currentBoard = null;
    }

    /// <summary> Called by CampaignHandler when the + button in the boards section of the campaign panel is pressed </summary>
    public void NewBoard()
    {
        string newGuid = Guid.NewGuid().ToString();
        currentBoard = new PF2E_BoardData();
        currentBoard.guid = newGuid;

        RefreshBoardPanel();
        OpenBoardPanel();
        SaveBoard();
    }

    /// <summary> Called by CampaignHandler when the play button on a board button is pressed </summary>
    public void LoadBoard(PF2E_BoardData board)
    {
        currentBoard = board;

        RefreshBoardPanel();
        OpenBoardPanel();
    }

    private void SaveBoard()
    {
        if (PF2E_Globals.PF2eCurrentCampaign.boards.ContainsKey(currentBoard.guid))
            PF2E_Globals.PF2eCurrentCampaign.boards[currentBoard.guid] = currentBoard;
        else
            PF2E_Globals.PF2eCurrentCampaign.boards.Add(currentBoard.guid, currentBoard);

        PF2E_Globals.PF2E_SaveCampaign();
    }

    private void RefreshBoardPanel()
    {
        boardNameInput.SetTextWithoutNotify(currentBoard.boardName);

        // Actor lists
        for (int i = actorButtonList.Count - 1; i > 0; i--)
            Destroy(actorButtonList[i].gameObject);
        actorButtonList.Clear();

        List<PositionableActor> actorList = new List<PositionableActor>();
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
        RefreshBoardPanel();
    }

    /// <summary> Called by the play button in board panel </summary>
    public void OnClickPlayButton()
    {
        PF2E_Globals.PF2E_SetCurrentBoard(currentBoard);
        SceneManager.LoadScene(pathfinder2ePlayScene, LoadSceneMode.Single);

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
