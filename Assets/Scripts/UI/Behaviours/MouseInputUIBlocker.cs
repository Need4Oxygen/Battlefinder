using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Original script by: Sjonsson (Unity Forums: https://answers.unity.com/questions/967170/detect-if-pointer-is-over-any-ui-element.html)
// Modified by: Manuel Segura

[RequireComponent(typeof(EventTrigger))]
public class MouseInputUIBlocker : MonoBehaviour
{
    public static bool BlockedByUI;
    private EventTrigger eventTrigger;

    private void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
        enterUIEntry.eventID = EventTriggerType.PointerEnter;
        enterUIEntry.callback.AddListener((eventData) => { SetBlocked(true); });
        eventTrigger.triggers.Add(enterUIEntry);

        EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
        exitUIEntry.eventID = EventTriggerType.PointerExit;
        exitUIEntry.callback.AddListener((eventData) => { SetBlocked(false); });
        eventTrigger.triggers.Add(exitUIEntry);
    }

    private void SetBlocked(bool value)
    {
        BlockedByUI = value;
    }
}
