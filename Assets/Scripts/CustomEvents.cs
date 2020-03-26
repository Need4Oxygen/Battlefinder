using UnityEngine;

// Callbacks
public delegate void PickColorCallback(Color color);

// Delegates
public delegate void DVoid();

public static class CustomEvents
{
    public static DVoid OnTest = null;
}
