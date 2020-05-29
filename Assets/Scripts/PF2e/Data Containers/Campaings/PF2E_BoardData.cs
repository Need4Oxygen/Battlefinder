using System.Collections.Generic;
using UnityEngine;

public class PF2E_BoardData
{
    public string guid = "";
    public string boardName = "";

    public TerrainData terrainData;

    public List<PositionableActor> players = new List<PositionableActor>();
    public List<PositionableActor> enemies = new List<PositionableActor>();
    public List<PositionableActor> npcs = new List<PositionableActor>();
}
