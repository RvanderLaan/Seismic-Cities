using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Instruction {

    [Range(0, 1)]
    public float zoom = 1;
    public string focusGameObjectName;
    public Vector2 screenPosition = new Vector2(0.5f, 0.5f);

    [Range(-1, 3)]
    public int arrow = -1;

    public string text;

    // If event trigger is defined, do not show NEXT button
    public string emitOnExit;
    public string nextEventTrigger;
}
