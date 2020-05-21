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
    public Dictionary<string, PF2E_BoardData> boards = new Dictionary<string, PF2E_BoardData>();
    public Dictionary<string, PF2E_PlayerData> players = new Dictionary<string, PF2E_PlayerData>();
    public Dictionary<string, PF2E_EnemyData> enemies = new Dictionary<string, PF2E_EnemyData>();
    public Dictionary<string, PF2E_NPCData> npcs = new Dictionary<string, PF2E_NPCData>();

    public PF2E_CampaignData() { }

    public PF2E_CampaignData(string guid, string name, E_Game game)
    {
        this.guid = guid;
        this.name = name;
        this.game = game;
    }

    public PF2E_CampaignData(string guid, string name, E_Game game, Dictionary<string, PF2E_BoardData> boards, Dictionary<string, PF2E_PlayerData> players, Dictionary<string, PF2E_EnemyData> enemies, Dictionary<string, PF2E_NPCData> npcs)
    {
        this.guid = guid;
        this.name = name;
        this.game = game;
        this.boards = boards;
        this.players = players;
        this.enemies = enemies;
        this.npcs = npcs;
    }
}
