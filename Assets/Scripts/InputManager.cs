using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] DraggablesMaster draggablesMaster = null;
    [SerializeField] WallConstructor wallConstructor = null;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
            wallConstructor.StartWalling();
        if (Input.GetKeyUp(KeyCode.W))
            wallConstructor.StopWalling();

        if (wallConstructor.isWalling)
        {
            if (Input.GetMouseButtonDown(0))
                wallConstructor.OnPointerDown();
            if (Input.GetMouseButtonUp(0))
                wallConstructor.OnPointerUp();

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
