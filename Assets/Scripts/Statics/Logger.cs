using System;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{

    public static List<string> blacklists = new List<string>()
    {
        "WindowManager",
        "TYaml",
        "TJson",
    };

    public static List<string> blacklistsWarn = new List<string>()
    {
    };

    public static List<string> blacklistsError = new List<string>()
    {
    };

    public static Dictionary<string, string> owner_color = new Dictionary<string, string>
    {
        {"Default", "FFFFFF"},
        {"TYaml", "FFFFFF"},
        {"TJson", "FFFFFF"},
        {"ObjPooler", "FFFFFF"},
        {"Audio", "FFFFFF"},
        {"ExternalLinks", "FFFFFF"},
        {"EditorTools", "FFFFFF"},
        {"BookScript", "FFFFFF"},
        {"DB", "ED8793"}, // rojizo
        {"WindowManager", "EDB787"}, // naranja
        {"PanelFader", "87ED96"}, // lima
        {"PrereqChecker", "AE87ED"}, // violeta
        {"CharacterData", "87ABED"}, // celeste
    };

    // 87EDE1 turquesa // 879CED azul // EAED87 amarillo // ED87DE pinku

    public static void Log(string owner, string message)
    {
        if (blacklists.Contains(owner)) return;
        string color = SearchOwnerColor(owner);

        Debug.Log($"<color=#{color}>[{owner}]</color> {message}");
    }

    public static void LogWarning(string owner, string message)
    {
        if (blacklistsWarn.Contains(owner)) return;
        string color = SearchOwnerColor(owner);

        Debug.LogWarning($"<color=#{color}>[{owner}]</color> {message}");
    }

    public static void LogError(string owner, string message) { LogError(owner, message, null); }
    public static void LogError(string owner, string message, UnityEngine.Object obj)
    {
        if (blacklistsError.Contains(owner)) return;
        string color = SearchOwnerColor(owner);

        if (obj == null)
            Debug.LogError($"<color=#{color}>[{owner}]</color> {message}", obj);
        else
            Debug.LogError($"<color=#{color}>[{owner}]</color> {message}");
    }

    private static string SearchOwnerColor(string owner)
    {
        string color;
        owner_color.TryGetValue(owner, out color);
        return string.IsNullOrEmpty(color) ? "ffffff" : color;
    }

}
