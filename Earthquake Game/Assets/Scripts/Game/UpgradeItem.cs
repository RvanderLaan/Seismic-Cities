using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Container for upgrades (foundations etc.) in the GUI
/// </summary>
[System.Serializable]
public class UpgradeItem {

    public string name;
    public GameObject prefab;
    public int amount;
    public Sprite image;
    public string tooltipText;

    // Todo: Position: In ground, on top, side?

	public UpgradeItem() {

    }
}
