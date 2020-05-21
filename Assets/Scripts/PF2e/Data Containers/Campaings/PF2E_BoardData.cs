using System.Collections.Generic;

public class PF2E_BoardData
{
    public string guid = "";
    public string boardName = "";

    public List<PositionableActor> players = new List<PositionableActor>();
    public List<PositionableActor> enemies = new List<PositionableActor>();
    public List<PositionableActor> npcs = new List<PositionableActor>();
}
