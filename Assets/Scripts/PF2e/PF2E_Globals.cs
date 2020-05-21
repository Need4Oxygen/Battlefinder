using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PF2E_Globals : MonoBehaviour
{
    public static List<PF2E_CampaignID> PF2eCampaignIDs = null;
    public static PF2E_CampaignID PF2eCurrentCampaignID = null;
    public static PF2E_CampaignData PF2eCurrentCampaign = null;

    public static PF2E_BoardData PF2eCurrentBoard = null;

    public static void PF2E_CreateCampaign(string name)
    {
        string newGuid = Guid.NewGuid().ToString();

        PF2E_CampaignID newCampaignID = new PF2E_CampaignID();
        newCampaignID.guid = newGuid;
        newCampaignID.name = name;
        newCampaignID.order = 0;

        PF2E_CampaignData newCampaignData = new PF2E_CampaignData(newGuid, name, E_Game.PF2E);

        PF2eCampaignIDs.Add(newCampaignID);
        Json.SaveInPlayerPrefs("PF2e_campaignsIDList", PF2eCampaignIDs);
        Json.SaveInPlayerPrefs(newGuid, newCampaignData);
        PlayerPrefs.Save();
    }

    public static void PF2E_DeleteCampaign()
    {
        string guid = PF2eCurrentCampaign.guid;
        PF2eCampaignIDs.Remove(PF2eCurrentCampaignID);
        Json.SaveInPlayerPrefs("PF2e_campaignsIDList", PF2eCampaignIDs);
        PlayerPrefs.DeleteKey(guid);
        PlayerPrefs.Save();
    }

    public static void PF2E_LoadCampaign(PF2E_CampaignID campaignID)
    {
        PF2eCurrentCampaignID = campaignID;
        PF2eCurrentCampaign = Json.LoadFromPlayerPrefs<PF2E_CampaignData>(campaignID.guid);
    }

    public static void PF2E_SaveCampaign()
    {
        Json.SaveInPlayerPrefs(PF2eCurrentCampaignID.guid, PF2eCurrentCampaign);
        PlayerPrefs.Save();
    }

    public static void PF2E_SetCurrentBoard(PF2E_BoardData newBoard)
    {
        PF2eCurrentBoard = newBoard;
    }
}
