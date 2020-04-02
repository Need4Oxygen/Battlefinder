using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public string title = "";
    public E_Games game = E_Games.None;

    public GameData(string title, E_Games game)
    {
        this.title = title;
        this.game = game;
    }
}
