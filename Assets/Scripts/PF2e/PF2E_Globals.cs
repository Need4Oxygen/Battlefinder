using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PF2E_Globals : MonoBehaviour
{
    public static List<PF2E_CampaignID> CampaignIDS = null;
    public static PF2E_CampaignID CurrentCampaignID = null;
    public static PF2E_CampaignData CurrentCampaign = null;

    public static PF2E_BoardData CurrentBoard = null;

    public static void CreateCampaign(string name)
    {
        string newGuid = Guid.NewGuid().ToString();

        PF2E_CampaignID newCampaignID = new PF2E_CampaignID();
        newCampaignID.guid = newGuid;
        newCampaignID.name = name;
        newCampaignID.order = 0;

        PF2E_CampaignData newCampaignData = new PF2E_CampaignData(newGuid, name, E_Game.PF2E);

        CampaignIDS.Add(newCampaignID);
        Json.SaveInPlayerPrefs("PF2e_campaignsIDList", CampaignIDS);
        Json.SaveInPlayerPrefs(newGuid, newCampaignData);
        PlayerPrefs.Save();
    }

    public static void DeleteCampaign()
    {
        string guid = CurrentCampaign.guid;
        CampaignIDS.Remove(CurrentCampaignID);
        Json.SaveInPlayerPrefs("PF2e_campaignsIDList", CampaignIDS);
        PlayerPrefs.DeleteKey(guid);
        PlayerPrefs.Save();
    }

    public static void LoadCampaign(PF2E_CampaignID campaignID)
    {
        CurrentCampaignID = campaignID;
        CurrentCampaign = Json.LoadFromPlayerPrefs<PF2E_CampaignData>(campaignID.guid);
    }

    public static void SaveCampaign()
    {
        Json.SaveInPlayerPrefs(CurrentCampaignID.guid, CurrentCampaign);
        PlayerPrefs.Save();
    }

    public static void SaveBoard(PF2E_BoardData board)
    {
        if (CurrentCampaign.boards.ContainsKey(board.guid))
            CurrentCampaign.boards[board.guid] = board;
        else
            CurrentCampaign.boards.Add(board.guid, board);

        SaveCampaign();
    }
}
