using UnityEngine;

// Callbacks
public delegate void PickColorCallback(Color color);

// Delegates
public delegate void DVoid();
public delegate void DBool(bool value);
public delegate void DString(string str);
public delegate void DTool(E_Tools tools);
public delegate void DWindow(Window window);

public static class CustomEvents
{
    public static DTool OnToolChange = null;
}
