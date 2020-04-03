using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PF2E_CampaignID
{
    public string guid;
    public string name;
    public int order;
}

public class PF2E_CampaignData : GameData
{
    public string guid = "";
    public List<PF2E_BoardData> boards = null;
    public List<PF2E_PlayerData> players = null;
    public List<PF2E_NPCData> npcs = null;
    public List<PF2E_PropData> props = null;

    public PF2E_CampaignData() { }
    public PF2E_CampaignData(string guid, string title, E_Games game) : base(title, game)
    {
        this.guid = guid;
        this.boards = new List<PF2E_BoardData>();
        this.players = new List<PF2E_PlayerData>();
        this.npcs = new List<PF2E_NPCData>();
        this.props = new List<PF2E_PropData>();
    }
    public PF2E_CampaignData(string guid, string title, E_Games game, List<PF2E_BoardData> boards, List<PF2E_PlayerData> players, List<PF2E_NPCData> npcs, List<PF2E_PropData> props) : base(title, game)
    {
        this.guid = guid;
        this.boards = boards;
        this.players = players;
        this.npcs = npcs;
        this.props = props;
    }
}
