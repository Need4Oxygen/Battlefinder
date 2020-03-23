using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] DraggablesMaster draggablesMaster = null;
    [SerializeField] WallConstructor wallConstructor = null;
    [SerializeField] FloorConstructor floorConstructor = null;

    enum ETools { None, Walls, Floors };

    private ETools currentTool = ETools.None;

    void Update()
    {
        switch (currentTool)
        {
            case ETools.Walls: // If there is any tool selected, check if it have been released
                if (Input.GetKeyUp(KeyCode.W))
                { wallConstructor.StopWalling(); currentTool = ETools.None; }
                break;
            case ETools.Floors:
                if (Input.GetKeyUp(KeyCode.F))
                { floorConstructor.StopFlooring(); currentTool = ETools.None; }
                break;
            default: // If no tool is selected, check for tool triggers
                if (Input.GetKeyDown(KeyCode.W))
                { wallConstructor.StartWalling(); currentTool = ETools.Walls; }
                if (Input.GetKeyDown(KeyCode.F))
                { floorConstructor.StartFlooring(); currentTool = ETools.Floors; }
                break;
        }

        if (currentTool == ETools.Walls)
        {
            if (Input.GetMouseButtonDown(0))
                wallConstructor.OnPointerDown();
            if (Input.GetMouseButtonUp(0))
                wallConstructor.OnPointerUp();
            return;
        }

        if (currentTool == ETools.Floors)
        {
            if (Input.GetMouseButtonDown(0))
                floorConstructor.OnPointerDown();
            if (Input.GetMouseButtonUp(0))
                floorConstructor.OnPointerUp();
            return;
        }

        // Left Click
        if (Input.GetMouseButtonDown(0))
            draggablesMaster.OnPointerDown();
        if (Input.GetMouseButtonUp(0))
            draggablesMaster.OnPointerUp();

        // Left Alt
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            draggablesMaster.isLeftAltPressed = true;
        if (Input.GetKeyUp(KeyCode.LeftAlt))
            draggablesMaster.isLeftAltPressed = false;

        // Supr
        if (Input.GetKeyDown(KeyCode.Delete))
            draggablesMaster.isSuprPressed = true;
        if (Input.GetKeyUp(KeyCode.Delete))
            draggablesMaster.isSuprPressed = false;
    }
}
