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
        // Open new window and push it to stack
        if (!OpenWindows.Contains(window) && !window.isSubpanel)
            OpenWindows.Push(window); ;

        // Set new window as raycaster
        if (window.raycastTarget)
            SetRaycastTarget(window);

        if (OnWindowOpens != null)
            OnWindowOpens(window);

        string type = window.isSubpanel ? "Subpanel" : "Window";
        Debug.Log($"[WindowManager] {type} \"{window.windowName}\" opened, stack count: {OpenWindows.Count}");
    }

    public static void WindowClosed(Window window)
    {
        if (!window.isSubpanel)
        {
            // Delete from stack
            OpenWindows.Pop();

            // Set previous window as raycast target
            if (OpenWindows.Count > 0)
                if (OpenWindows.Peek().raycastTarget)
                    SetRaycastTarget(OpenWindows.Peek());
        }

        if (OnWindowCloses != null)
            OnWindowCloses(window);

        string type = window.isSubpanel ? "Subpanel" : "Window";
        Debug.Log($"[WindowManager] {type} \"{window.windowName}\" closed, stack count: {OpenWindows.Count}");
    }

    private static void SetRaycastTarget(Window window)
    {
        // Deactivate raycasting of every other window and their children
        foreach (var win in OpenWindows)
        {
            if (win.raycaster != null)
                win.raycaster.enabled = false;

            if (win.children.Count > 0)
                foreach (var child in win.children)
                    if (child.syncRaycastWithParent)
                        child.raycaster.enabled = false;
        }

        // Activate window raycast
        window.raycaster.enabled = true;

        // Activate window's children raycast
        if (window.children.Count > 0)
            foreach (var child in window.children)
                if (child.syncRaycastWithParent)
                    child.raycaster.enabled = true;

        Debug.Log($"[WindowManager] Raycast target: {window.windowName} with {window.children.Count} childs!");
    }

}
