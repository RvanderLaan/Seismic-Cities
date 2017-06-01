using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Level", menuName = "SeismicCities/Level")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    public string levelName = "New Level";

    // Terrain / game objects
    [SerializeField]
    public SoilBlock[][] blocks; // Contains soil layers (which are made up of blocks, preferably 1 collider per layer) and info on click/hover, also location of cracks 

    [SerializeField]
    public List<BuildingZone> buildingZones;        // Type: SurfaceEntity? Contains x-position, allowed placements, (optional) pre-placed building -> scenery zone

    [SerializeField]
    public Earthquake earthquake;               // Magnitude, 2d position (depth + x-pos)
    // public List<Plate> plates;

    // public SceneryType sceneryType;             // Scenery? Random (based on seed, type contains which trees/plants can be used) or user created 
    // Tsunami?

    // Menu / UI
    [SerializeField]
    public Vector4 cameraLimits = new Vector4(0, 20, -6, 6);
    [SerializeField]
    public Tutorial tutorial;
    [SerializeField]
    public Dialog start, pass, fail;            // Fail is dynamic?

    // Player items
    [SerializeField]
    public List<BuildingItem> buildingItems;    // Building placement
    [SerializeField]
    public List<UpgradeItem> upgradeItems;      // Upgrade placement?
    [SerializeField]
    public int seismographAmount = 0;

    
}
