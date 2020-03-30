using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static LayerMask BoardLayer;

    [SerializeField] private DraggablesMaster draggablesMaster = null;
    [SerializeField] private Camera cam = null;

    public static ETools currentTool = ETools.None;

    void Awake()
    {
        BoardLayer = LayerMask.GetMask("Table");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetCurrentTool(ETools.None);
            return;
        }

        if (Input.GetButtonDown("Walls"))
        {
            SetCurrentTool(ETools.Walls);
            return;
        }

        if (Input.GetButtonDown("Floors"))
        {
            SetCurrentTool(ETools.Floors);
            return;
        }


    }

    private void SetCurrentTool(ETools tools)
    {
        currentTool = tools;
        CustomEvents.OnToolChange(tools);
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
