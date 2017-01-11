using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class DialogItem
{
    public CharacterName characterName;
    public string dialogText;
    public UnityEvent enterEvent;
    public UnityEvent leaveEvent;

    public DialogItem()
    {

    }

    public enum CharacterName
    {
        Poseidon, Aphrodite
    }
}
