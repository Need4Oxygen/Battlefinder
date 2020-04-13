using System;
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
    public string guid = "";
    public List<PF2E_BoardData> boards = null;
    public Dictionary<string, PF2E_PlayerData> players = null;
    public List<PF2E_EnemyData> enemies = null;
    public List<PF2E_NPCData> npcs = null;
    public List<PF2E_PropData> props = null;

    public PF2E_CampaignData() { }
    public PF2E_CampaignData(string guid, string title, E_Games game) : base(title, game)
    {
        this.guid = guid;
        this.boards = new List<PF2E_BoardData>();
        this.players = new Dictionary<string, PF2E_PlayerData>();
        this.enemies = new List<PF2E_EnemyData>();
        this.npcs = new List<PF2E_NPCData>();
        this.props = new List<PF2E_PropData>();
    }
    public PF2E_CampaignData(string guid, string title, E_Games game, List<PF2E_BoardData> boards, Dictionary<string, PF2E_PlayerData> players, List<PF2E_EnemyData> enemies, List<PF2E_NPCData> npcs, List<PF2E_PropData> props) : base(title, game)
    {
        this.guid = guid;
        this.boards = boards;
        this.players = players;
        this.enemies = enemies;
        this.npcs = npcs;
        this.props = props;
    }
}
