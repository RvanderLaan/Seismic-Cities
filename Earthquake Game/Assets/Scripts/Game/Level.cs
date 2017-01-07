using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Specifications for a level
/// </summary>
public class Level : MonoBehaviour {

    public GameObject terrain; // Contains geometry, colliders and scenery? Building zones?
    public Vector4 cameraLimits = new Vector4(0, 20, -6, 6);
    public List<BuildingItem> buildingItems;
    public GameObject earthquakeZone;
    // Building zones?
    // Tutorial + triggers?

    public Level() {
        
    }
}
