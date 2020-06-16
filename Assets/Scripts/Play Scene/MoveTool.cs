using System.Collections;
using System.Collections.Generic;
using BoardItems;
using UnityEngine;
using UnityEngine.UI;

public class MoveTool : MonoBehaviour
{
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

    IMovable item = null;

    public void OnPointerDown()
    {
        // If selection tool have stuff, move, rotate, scale... that stuff
        // If not, select this then move, rotate, scale...

        if (SelectionTool.SelectedItems.Count > 0)
        {
            foreach (var item in SelectionTool.SelectedItems)
            {
                IMovable moveable = item as IMovable;
                if (moveable != null)
                    moveable.Move(true);
            }
        }
        else
        {
            item = SearchMovableUnderMouse();
            if (item != null)
            {
                item.Move(true);
            }
        }
    }

    public void OnPointerUp()
    {
        if (SelectionTool.SelectedItems.Count > 0)
        {
            foreach (var item in SelectionTool.SelectedItems)
            {
                IMovable moveable = item as IMovable;
                if (moveable != null)
                    moveable.Move(false);
            }
        }
        else
        {
            if (item != null)
            {
                item.Move(false);
                item = null;
            }
        }
    }

    private IMovable SearchMovableUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, InputManager.SelectablesLayer))
        {
            IMovable draggable = hit.collider.GetComponent<IMovable>();
            if (draggable != null)
                return draggable;
        }
        return null;
    }

    # endregion

}
