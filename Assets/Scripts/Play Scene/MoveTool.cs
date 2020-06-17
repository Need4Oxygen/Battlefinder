using System.Collections;
using System.Collections.Generic;
using BoardItems;
using UnityEngine;
using UnityEngine.UI;

public class MoveTool : MonoBehaviour
{
    public static List<IMovable> MovingItems = new List<IMovable>();

    [HideInInspector] public bool isSelected = false;

    [SerializeField] private Button toolButton = null;

    void Awake()
    {
        CustomEvents.OnToolChange += OnToolChange;
        PF2E_SceneManager.OnPlaySceneExit += Unsubscribe;
    }

    void Update()
    {
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
                OnPointerDown();
            if (Input.GetMouseButtonUp(0))
                OnPointerUp();
        }
    }

    private void Unsubscribe()
    {
        CustomEvents.OnToolChange -= OnToolChange;
        PF2E_SceneManager.OnPlaySceneExit -= Unsubscribe;
    }

    private void OnToolChange(E_Tools tool)
    {
        if (tool == E_Tools.Move && !isSelected)
        {
            ToolSelected();
        }
        else if (tool != E_Tools.Move && isSelected)
        {
            ToolUnselected();
        }
    }

    private void ToolSelected()
    {
        isSelected = true;
        toolButton.image.color = Globals.Theme["untrained"];
    }

    private void ToolUnselected()
    {
        isSelected = false;
        toolButton.image.color = Globals.Theme["text_1"];
    }

    #region --------------------------------------MOUSE INPUT--------------------------------------

    bool tempSelection = false;

    public void OnPointerDown()
    {
        // Search for moveables
        List<IMovable> foundMoveables = new List<IMovable>();
        IMovable movable = SearchMovableUnderMouse();
        ISelectable selectable = movable as ISelectable;

        void TempSelection()
        {
            SelectionTool.DeselectAll();
            tempSelection = true;
            SelectionTool.Select(selectable);
        }

        if (selectable != null)
            if (SelectionTool.SelectedItems.Count > 0)
                if (SelectionTool.SelectedItems.Contains(selectable))
                    foreach (var item in SelectionTool.SelectedItems)
                    {
                        IMovable m2 = item as IMovable;
                        if (m2 != null)
                            foundMoveables.Add(m2);
                    }
                else
                    TempSelection();
            else
                TempSelection();

        if (movable != null && !foundMoveables.Contains(movable))
            foundMoveables.Add(movable);

        // Start movement
        if (foundMoveables.Count > 0)
            foreach (var item in foundMoveables)
                MovementStart(item);
    }

    public void OnPointerUp()
    {
        if (tempSelection)
        {
            SelectionTool.DeselectAll();
            tempSelection = false;
        }

        if (MovingItems.Count > 0)
            MovementStopAll(true);
    }

    public static void MovementStart(IMovable movable)
    {
        movable.MoveStart();
        MovingItems.Add(movable);
    }

    public static void MovementStop(IMovable movable, bool snap)
    {
        movable.MoveStop(snap);
        MovingItems.Remove(movable);
    }

    public static void MovementStopAll(bool snap)
    {
        foreach (var item in MovingItems)
            item.MoveStop(snap);
        MovingItems.Clear();
    }

    public static IMovable SearchMovableUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, InputManager.SelectablesLayer))
        {
            IMovable movable = hit.collider.GetComponent<IMovable>();
            if (movable != null)
                return movable;
        }
        return null;
    }

    # endregion

}
