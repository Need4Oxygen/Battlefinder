using System.Collections.Generic;
using UnityEngine;

public class PF2E_BoardData
{
    public string guid = "";
    public string boardName = "";

    public float[,,] terrainAlphamaps = null;
    public float[,] terrainHeights = null;
    public Dictionary<string, List<PWallElement>> wallElements = new Dictionary<string, List<PWallElement>>();

    public List<PActor> players = new List<PActor>();
    public List<PActor> enemies = new List<PActor>();
    public List<PActor> npcs = new List<PActor>();
}
