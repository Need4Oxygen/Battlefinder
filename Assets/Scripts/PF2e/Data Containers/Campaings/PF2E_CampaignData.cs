using System;
using System.Collections.Generic;
using UnityEngine;

public class PF2E_CampaignID
{
    public string guid;
    public string name;
    public int order;
}

public class PF2E_CampaignData
{
    public string guid = "";
    public string name = "";
    public E_Game game = E_Game.None;
    public List<PF2E_BoardData> boards = new List<PF2E_BoardData>();
    public Dictionary<string, PF2E_PlayerData> players = new Dictionary<string, PF2E_PlayerData>();
    public List<PF2E_EnemyData> enemies = new List<PF2E_EnemyData>();
    public List<PF2E_NPCData> npcs = new List<PF2E_NPCData>();
    public List<PF2E_PropData> props = new List<PF2E_PropData>();

    public PF2E_CampaignData() { }

    public PF2E_CampaignData(string guid, string name, E_Game game)
    {
        this.guid = guid;
        this.name = name;
        this.game = game;
    }

    public PF2E_CampaignData(string guid, string name, E_Game game, List<PF2E_BoardData> boards, Dictionary<string, PF2E_PlayerData> players, List<PF2E_EnemyData> enemies, List<PF2E_NPCData> npcs, List<PF2E_PropData> props)
    {
        this.guid = guid;
        this.name = name;
        this.game = game;
        this.boards = boards;
        this.players = players;
        this.enemies = enemies;
        this.npcs = npcs;
        this.props = props;
    }
}
