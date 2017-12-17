using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoilGlossaryItem {
    public Soil.SoilType type;
    public bool isNew;
    public Sprite sprite;

    public SoilGlossaryItem(Soil.SoilType t, bool b, Sprite s)
    {
        type = t;
        isNew = b;
        sprite = s;
    }
}
