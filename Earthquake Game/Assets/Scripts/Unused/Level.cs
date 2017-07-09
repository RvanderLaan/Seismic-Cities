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

    /*
    // Game elements
    public Terrain2D terrain;                   // Contains soil layers (which are made up of blocks, preferably 1 collider per layer) and info on click/hover, also location of cracks
    public BuildingZone[] buildingZones;        // Type: SurfaceEntity? Contains x-position, allowed placements, (optional) pre-placed building -> scenery zone
    public Earthquake earthquake;               // Magnitude, 2d position (depth + x-pos)
    public SceneryType sceneryType;             // Scenery? Random (based on seed, type contains which trees/plants can be used) or user created 
    // Tsunami?

    // Menu / UI
    public Tutorial tutorial;
    public Dialog start, pass, fail;            // Fail is dynamic?
    
    // Seismograph placement
    public List<BuildingItem> buildingItems;    // Building placement
    public List<UpgradeItem> upgradeItems;      // Upgrade placement?

    public Vector4 cameraLimits = new Vector4(0, 20, -6, 6);

    */

    public Level() {
        
    }
}
