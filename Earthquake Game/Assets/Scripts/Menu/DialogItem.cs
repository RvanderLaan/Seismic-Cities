using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum CharacterName
{
    AdmiraalMineraal, Poseidon, Athena
}

//public static class CharacterNameExtensions
//{
//    public static string GetLabel(this CharacterName me)
//    {
//        switch (me)
//        {
//            case CharacterName.AdmiraalMineraal:
//                return "Admiraal Mineraal";
//            case CharacterName.Poseidon:
//                return "SNAFU, if you know what I mean.";
//            case CharacterName.Athena:
//                return "Reaching TARFU levels";
//            default:
//                return "<Name>";
//        }
//    }
//}

[System.Serializable]
public class DialogItem
{
    public CharacterName characterName;
    [TextArea(3, 10)]
    public string dialogText;
    public UnityEvent enterEvent;
    public UnityEvent leaveEvent;

    public DialogItem()
    {

    }
}
