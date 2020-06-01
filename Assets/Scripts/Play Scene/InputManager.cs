using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static LayerMask BoardLayer;

    [SerializeField] private Camera cam = null;

    public static E_Tools currentTool = E_Tools.None;

    void Awake()
    {
        BoardLayer = LayerMask.GetMask("Table");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetCurrentTool(E_Tools.None);
            return;
        }

        if (Input.GetButtonDown("Walls"))
        {
            SetCurrentTool(E_Tools.Walls);
            return;
        }

        if (Input.GetButtonDown("Floors"))
        {
            SetCurrentTool(E_Tools.Floors);
            return;
        }
    }

    private void SetCurrentTool(E_Tools tools)
    {
        currentTool = tools;
        CustomEvents.OnToolChange(tools);
    }

    // <summary> Called by clicking the wall tool button </summary>
    public void SetWallTool()
    {
        SetCurrentTool(E_Tools.Walls);
    }

    // <summary> Called by clicking the floor tool button </summary>
    public void SetFloorTool()
    {
        SetCurrentTool(E_Tools.Floors);
    }

    public Vector3 MousePosInBoard(bool rounded)
    {
        cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, BoardLayer))
        {
            if (rounded)
                return new Vector3(Mathf.Round(hit.point.x), 0f, Mathf.Round(hit.point.z));
            else
                return new Vector3(hit.point.x, 0f, hit.point.z);
        }

        return Vector3.zero;
    }
}
