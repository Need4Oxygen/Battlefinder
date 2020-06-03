using System.Collections.Generic;

public class PF2E_CampaignData
{
    public string ID = ""; // ID is the file name
    public string name = "";
    public Dictionary<string, PF2E_BoardData> boards = new Dictionary<string, PF2E_BoardData>();
    public Dictionary<string, PF2E_PlayerData> players = new Dictionary<string, PF2E_PlayerData>();
    public Dictionary<string, PF2E_EnemyData> enemies = new Dictionary<string, PF2E_EnemyData>();
    public Dictionary<string, PF2E_NPCData> npcs = new Dictionary<string, PF2E_NPCData>();

    public PF2E_CampaignData() { }

    public PF2E_CampaignData(string ID, string name)
    {
        this.ID = ID;
        this.name = name;
    }

    public PF2E_CampaignData(string ID, string name, Dictionary<string, PF2E_BoardData> boards, Dictionary<string, PF2E_PlayerData> players, Dictionary<string, PF2E_EnemyData> enemies, Dictionary<string, PF2E_NPCData> npcs)
    {
        this.ID = ID;
        this.name = name;
        this.boards = boards;
        this.players = players;
        this.enemies = enemies;
        this.npcs = npcs;
    }
}
