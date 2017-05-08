using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Instruction {

    public float zoom = 1;
    public Transform focus;

    [Range(0, 3)]
    public int arrow = 0;

    [TextArea(3, 10)]
    public string text;

    // If event trigger is defined, do not show NEXT button
    public UnityEvent enterEvent;
    public string nextEventTrigger;
}
