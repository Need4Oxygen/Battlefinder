using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static List<Window> OpenWindows = new List<Window>();
    public static IEnumerable<string> OpenWindowNames = from a in OpenWindows select a.windowName;

    public static DWindow OnWindowOpens = null;
    public static DWindow OnWindowCloses = null;

    public static void WindowOpened(Window window)
    {
        // Open new window and push it to stack
        if (!OpenWindows.Contains(window) && !window.isSubpanel)
            OpenWindows.Insert(0, window); ;

        // Set new window as raycaster
        if (window.raycastTarget)
            SetRaycastTarget(window);

        if (OnWindowOpens != null)
            OnWindowOpens(window);

        string type = window.isSubpanel ? "Subpanel" : "Window";
        Debug.Log($"<color=#edbf28>[WindowManager]</color> {type} \"{window.windowName}\" opened, stack count: {OpenWindows.Count} || {string.Join(" ,", OpenWindowNames)}");
    }

    public static void WindowClosed(Window window)
    {
        if (!window.isSubpanel)
        {
            // Delete from stack
            OpenWindows.Remove(window);

            // Set previous window as raycast target
            if (OpenWindows.Count > 0)
                for (int i = 0; i < OpenWindows.Count; i++)
                    if (OpenWindows[i].raycastTarget)
                    {
                        SetRaycastTarget(OpenWindows[i]);
                        break;
                    }
        }

        if (OnWindowCloses != null)
            OnWindowCloses(window);

        string type = window.isSubpanel ? "Subpanel" : "Window";
        Debug.Log($"<color=#edbf28>[WindowManager]</color> {type} \"{window.windowName}\" closed, stack count: {OpenWindows.Count} || {string.Join(" ,", OpenWindowNames)}");
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

        // Activate window and children raycast
        window.raycaster.enabled = true;
        if (window.children.Count > 0)
            foreach (var child in window.children)
                if (child.syncRaycastWithParent)
                    child.raycaster.enabled = true;

        Debug.Log($"<color=#edbf28>[WindowManager]</color> Raycast target: {window.windowName} with {window.children.Count} childs!");
    }

}
