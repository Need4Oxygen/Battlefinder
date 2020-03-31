using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public string title = "";
    public EGames game = EGames.None;

    public GameData(string title, EGames game)
    {
        this.title = title;
        this.game = game;
    }
}
