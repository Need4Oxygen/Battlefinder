using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static Stack<Window> OpenWindows = new Stack<Window>();

    public static DWindow OnWindowOpens = null;
    public static DWindow OnWindowCloses = null;

    public static void WindowOpened(Window window)
    {
        if (OpenWindows.Contains(window))
            OpenWindows.Push(window); ;

        if (OnWindowOpens != null)
            OnWindowOpens(window);
    }

    public static Window WindowClosed(Window window)
    {
        Window closedWindow = OpenWindows.Pop();

        if (OnWindowCloses != null)
            OnWindowCloses(window);

        return closedWindow;
    }


}
