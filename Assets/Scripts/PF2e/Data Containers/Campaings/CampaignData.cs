using System.Collections.Generic;
using Pathfinder2e.Character;

namespace Pathfinder2e.GameData
{

    public class CampaignData
    {
        public string ID = ""; // ID is the file name
        public string name = "";
        public Dictionary<string, BoardData> boards = new Dictionary<string, BoardData>();
        public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();
        public Dictionary<string, EnemyData> enemies = new Dictionary<string, EnemyData>();
        public Dictionary<string, NPCData> npcs = new Dictionary<string, NPCData>();

        public CampaignData() { }

        public CampaignData(string ID, string name)
        {
            this.ID = ID;
            this.name = name;
        }

        public CampaignData(string ID, string name, Dictionary<string, BoardData> boards, Dictionary<string, CharacterData> characters, Dictionary<string, EnemyData> enemies, Dictionary<string, NPCData> npcs)
        {
            this.ID = ID;
            this.name = name;
            this.boards = boards;
            this.characters = characters;
            this.enemies = enemies;
            this.npcs = npcs;
        }
    }

}
