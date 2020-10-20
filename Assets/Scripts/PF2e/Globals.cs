using System.Collections.Generic;
using System.IO;
using Pathfinder2e.GameData;
using UnityEngine;

namespace Pathfinder2e.GameData
{

    public abstract class Globals_PF2E : MonoBehaviour
    {
        public static Dictionary<string, string> CampaignIDs = new Dictionary<string, string>();
        public static string CurrentCampaignID = null;
        public static CampaignData CurrentCampaign = null;
        public static BoardData CurrentBoard = null;

        /// <summary>Create a new campaing if one of the same ID doesn't exist. </summary>
        /// <returns>Returns campaign ID. </returns>
        public static string CreateCampaign(string name)
        {
            string newCampaignID = name + ".json";

            if (CampaignIDs.ContainsKey(newCampaignID))
                return "";

            CampaignData newCampaignData = new CampaignData(newCampaignID, name);
            Json.SerializeFile(newCampaignData, newCampaignID, Globals.SystemData.PF2ECampaignsPathSep);

            CampaignIDs.Add(newCampaignID, Globals.SystemData.PF2ECampaignsPathSep + newCampaignID);

            return newCampaignID;
        }

        public static void DeleteCampaign()
        {
            CampaignIDs.Remove(CurrentCampaignID);
            File.Delete(Globals.SystemData.PF2ECampaignsPathSep + CurrentCampaignID);
        }

        public static void LoadCampaign(string campaignID)
        {
            CurrentCampaignID = campaignID;
            CurrentCampaign = Json.DeserializeFile<CampaignData>(Globals.SystemData.PF2ECampaignsPathSep + campaignID);
        }

        public static void SaveCampaign()
        {
            Json.SerializeFile(CurrentCampaign, CurrentCampaignID, Globals.SystemData.PF2ECampaignsPathSep);
        }

        public static void SaveBoard(BoardData board)
        {
            if (CurrentCampaign.boards.ContainsKey(board.guid))
                CurrentCampaign.boards[board.guid] = board;
            else
                CurrentCampaign.boards.Add(board.guid, board);

            SaveCampaign();
        }
    }

}
