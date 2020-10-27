using System.Collections.Generic;
using Pathfinder2e;
using Pathfinder2e.Containers;
using UnityEngine;

namespace Pathfinder2e.Player
{

    public class APIC
    {
        public PlayerData playerData = null;

        public string name = "";
        public string abl = "";
        public int initialScore = 0;
        public List<LectureFull> lectures = new List<LectureFull>();

        // private List<PF2E_Effect> itemModifiers = new List<PF2E_Effect>();
        // private List<PF2E_Effect> circModifiers = new List<PF2E_Effect>();

        public APIC(string name, PlayerData playerData, string abl, int initialScore)
        {
            this.name = name;
            this.playerData = playerData;
            this.abl = abl;
            this.initialScore = initialScore;
        }

        public int profScore
        {
            get
            {
                if (playerData != null)
                {
                    if (lectures.Count < 0)
                        return 0;

                    switch (DB.Prof_FindMax(lectures))
                    {
                        case "L": return playerData.level + 8;
                        case "M": return playerData.level + 6;
                        case "E": return playerData.level + 4;
                        case "T": return playerData.level + 2;
                        default: return 0;
                    }
                }
                else
                {
                    Debug.Log($"[APIC] Error: {name} is missing PlayerData!");
                    return 0;
                }
            }
        }

        public int ablScore { get { return playerData.Abl_GetMod(abl); } }

        public int score { get { return initialScore + ablScore + profScore; } }

        public string prof { get { return DB.Prof_FindMax(lectures); } }

        public string profColored { get { return DB.Prof_FindMaxColored(lectures); } }

        public int dcScore { get { return 10 + profScore; } }

        public int itemScore = 0;

        public int tempScore = 0;
    }

}
