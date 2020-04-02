using System.Collections.Generic;
using UnityEngine;

public class PF2E_CampaignID
{
    public string guid;
    public string name;
    public int order;
}

public class PF2E_CampaignData : GameData
{
    public string guid;
    public List<PF2E_BoardData> boards = new List<PF2E_BoardData>();
    public List<PF2E_PlayerData> players = new List<PF2E_PlayerData>();
    public List<PF2E_NPCData> npcs = new List<PF2E_NPCData>();
    public List<PF2E_PropData> props = new List<PF2E_PropData>();

    public PF2E_CampaignData(string title, E_Games game, List<PF2E_BoardData> boards, List<PF2E_PlayerData> players, List<PF2E_NPCData> npcs, List<PF2E_PropData> props) : base(title, game)
    {
        this.boards = boards;
        this.players = players;
        this.npcs = npcs;
        this.props = props;
    }
}
