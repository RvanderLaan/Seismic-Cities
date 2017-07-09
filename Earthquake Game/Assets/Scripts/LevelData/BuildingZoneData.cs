using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Container for buildings in the GUI
/// </summary>
[System.Serializable]
public class BuildingZoneData {

    public List<AllowedPlacement> allowedPlacements;
    public int gridPosition = 0;


}
