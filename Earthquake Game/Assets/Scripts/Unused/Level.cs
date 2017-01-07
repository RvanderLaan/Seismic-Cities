using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Specifications for a level
/// 
/// Currently not used, would cost too much time/effort to do everything in the same scene for just a few levels.
/// Multiple scenes is properly better
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
