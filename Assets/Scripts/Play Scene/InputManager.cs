using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static LayerMask SelectablesLayer;
    public static LayerMask BoardLayer;
    public static E_Tools currentTool = E_Tools.None;

    void Awake()
    {
        SelectablesLayer = LayerMask.GetMask("Selectables");
        BoardLayer = LayerMask.GetMask("Board");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetCurrentTool(E_Tools.None);
            return;
        }

        if (Input.GetButtonDown("Move Tool"))
        {
            SetCurrentTool(E_Tools.Move);
            return;
        }

        if (Input.GetButtonDown("Selection Tool"))
        {
            SetCurrentTool(E_Tools.Selection);
            return;
        }

        if (Input.GetButtonDown("Walls Tool"))
        {
            SetCurrentTool(E_Tools.Walls);
            return;
        }

        if (Input.GetButtonDown("Floors Tool"))
        {
            SetCurrentTool(E_Tools.Floors);
            return;
        }
    }

    void LateUpdate()
    {
        ResetTablePoint();
    }

    private void SetCurrentTool(E_Tools tools)
    {
        currentTool = tools;
        CustomEvents.OnToolChange(tools);
    }

    /// <summary>Called by clicking the move tool button.abstract </summary>
    public void SetMoveTool()
    {
        SetCurrentTool(E_Tools.Move);
    }

    /// <summary>Called by clicking the selection tool button. </summary>
    public void SetSelectionTool()
    {
        SetCurrentTool(E_Tools.Selection);
    }

    /// <summary>Called by clicking the wall tool button. </summary>
    public void SetWallTool()
    {
        SetCurrentTool(E_Tools.Walls);
    }

    /// <summary>Called by clicking the floor tool button. </summary>
    public void SetFloorTool()
    {
        SetCurrentTool(E_Tools.Floors);
    }

    // --------------------------------------TABLE POINT--------------------------------------

    private static Vector3 tablePoint;
    private static bool tablePointSet;

    private void ResetTablePoint()
    {
        tablePoint = Vector3.zero;
        tablePointSet = false;
    }

    public static Vector3 TablePoint(bool rounded)
    { return TablePoint(rounded, 0f); }
    public static Vector3 TablePoint(bool rounded, float hover)
    {
        if (!tablePointSet)
        {
            tablePointSet = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 tp = new Vector3(0, hover, 0);

            if (Physics.Raycast(ray, out hit, 30f, BoardLayer))
            {
                if (rounded)
                    tp = new Vector3(Mathf.Round(hit.point.x), hover, Mathf.Round(hit.point.z));
                else
                    tp = new Vector3(hit.point.x, hover, hit.point.z);
            }

            tablePoint = tp;
            return tp;
        }
        else
        {
            return tablePoint;
        }
    }

}
