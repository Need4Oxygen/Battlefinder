using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class PF2E_Globals : MonoBehaviour
{
    public static Dictionary<string, string> CampaignIDs = new Dictionary<string, string>();
    public static string CurrentCampaignID = null;
    public static PF2E_CampaignData CurrentCampaign = null;
    public static PF2E_BoardData CurrentBoard = null;

    public static void CreateCampaign(string name)
    {
        string newCampaignID = name + ".json";

        if (CampaignIDs.ContainsKey(newCampaignID))
            return;

        PF2E_CampaignData newCampaignData = new PF2E_CampaignData(newCampaignID, name);
        Json.SerializeFile(newCampaignData, newCampaignID, Globals.SystemData.PF2ECampaignsPathSep);

        CampaignIDs.Add(newCampaignID, Globals.SystemData.PF2ECampaignsPathSep + newCampaignID);
    }

    public static void DeleteCampaign()
    {
        CampaignIDs.Remove(CurrentCampaignID);
        File.Delete(Globals.SystemData.PF2ECampaignsPathSep + CurrentCampaignID);
    }

    public static void LoadCampaign(string campaignID)
    {
        CurrentCampaignID = campaignID;
        CurrentCampaign = Json.DeserializeFile<PF2E_CampaignData>(Globals.SystemData.PF2ECampaignsPathSep + campaignID);
    }

    public static void SaveCampaign()
    {
        Json.SerializeFile(CurrentCampaign, CurrentCampaignID, Globals.SystemData.PF2ECampaignsPathSep);
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
