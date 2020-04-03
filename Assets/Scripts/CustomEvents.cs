using UnityEngine;

// Callbacks
public delegate void PickColorCallback(Color color);

// Delegates
public delegate void DVoid();
public delegate void DBool(bool value);
public delegate void DTool(E_Tools tools);

public static class CustomEvents
{
    public static DTool OnToolChange = null;
}
