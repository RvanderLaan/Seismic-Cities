using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Container for buildings in the GUI
/// </summary>
[System.Serializable]
public class BuildingItem {

    public string name;
    public GameObject prefab;
    public int amount;
    public Sprite image;
    public string tooltipText;

	public BuildingItem() {

    }
}
