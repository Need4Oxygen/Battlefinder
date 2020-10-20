using System.Collections.Generic;

namespace Pathfinder2e.GameData
{

    public class BoardData
    {
        public string guid = "";
        public string boardName = "";

        public string boardMaps = "";

        public List<PActor> players = new List<PActor>();
        public List<PActor> enemies = new List<PActor>();
        public List<PActor> npcs = new List<PActor>();
    }

    public class BoardDetails
    {
        public Dictionary<string, float> terrainAlphamaps = new Dictionary<string, float>();
        public Dictionary<string, float> terrainHeightmaps = new Dictionary<string, float>();
        public Dictionary<string, List<PWallElement>> wallElements = new Dictionary<string, List<PWallElement>>();
    }

}