using UnityEngine;

// Callbacks
public delegate void PickColorCallback(Color color);

// Delegates
public delegate void DVoid();
public delegate void DTool(ETools tools);

public static class CustomEvents
{
    public static DTool OnToolChange = null;
}
