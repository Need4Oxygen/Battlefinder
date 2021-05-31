using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Window))]
public class WindowConfirmation : MonoBehaviour
{

    private VisualElement root;
    private Window window;

    private Label warning;
    private Button buttonAccept;
    private Button buttonCancel;

    public DVoid OnAcceptActions;
    public DVoid OnCancelActions;

    private void Awake()
    {

        window = GetComponent<Window>();
        root = GetComponent<UIDocument>().rootVisualElement;

        warning = root.Q<Label>("advice");
        buttonAccept = root.Q<Button>("button-accept");
        buttonCancel = root.Q<Button>("button-cancel");

        buttonAccept.RegisterCallback<ClickEvent>(a => OnClickAccept());
        buttonCancel.RegisterCallback<ClickEvent>(a => OnClickCancel());

    }


    public void Ask(string warning, DVoid actionAccept = null, DVoid actionCancel = null)
    {

        this.warning.text = warning;

        OnAcceptActions += actionAccept;
        OnCancelActions += actionCancel;

    }

    private void OnClickAccept()
    {

        if (OnAcceptActions != null) { OnAcceptActions.Invoke(); OnAcceptActions = null; }
        window.Close();

    }

    private void OnClickCancel()
    {

        if (OnCancelActions != null) { OnCancelActions.Invoke(); OnCancelActions = null; }
        window.Close();

    }

    public void OnSubmit(InputValue value)
    {

        if (!value.isPressed) return;
        if (OnAcceptActions != null) { OnAcceptActions.Invoke(); OnAcceptActions = null; }
        window.Close();

    }

    public void OnCancel(InputValue value)
    {

        if (!value.isPressed) return;
        if (OnCancelActions != null) { OnCancelActions.Invoke(); OnCancelActions = null; }
        window.Close();

    }

}
