using UnityEngine;

using System.Collections.Generic;

/// <summary>
/// Contains all data that defines a level. Using this as an asset does not work, an extension fixes the problem (LevelData)
/// </summary>
public class LevelDataScript : ScriptableObject
{
    /// <summary>
    /// Name of level, shown at start
    /// </summary>
    [SerializeField]
    public string levelName = "New Level";

    /// <summary>
    /// Seed for random elements (e.g. tree generation)
    /// </summary>
    [SerializeField]
    public int seed = 0;

    /// <summary>
    /// Contains soil layers (which are made up of blocks, preferably 1 collider per layer) and info on click/hover, also location of cracks
    /// </summary>
    [SerializeField]
    public GameObject soilLayers;

    // Type: SurfaceEntity? Contains x-position, allowed placements, (optional) pre-placed building -> scenery zone
    [SerializeField]
    public List<BuildingZoneData> buildingZones;        

    [SerializeField]
    public EarthquakeData earthquake;
    // public List<Plate> plates;

    [SerializeField]
    public Scenery scenery, underground;
    [SerializeField]
    public GameObject[] other; // E.g. tsunamis, will be instantiated at start of level
    
    // Menu / UI
    [SerializeField]
    public Vector4 cameraLimits = new Vector4(0, 599, -150, 150);
    [SerializeField]
    public Instruction[] tutorial;
    [SerializeField]
    public DialogItem[] start, pass;

    // Player items
    [SerializeField]
    public List<BuildingItem> buildingItems;
    [SerializeField]
    public List<UpgradeItem> upgradeItems;
    [SerializeField]
    public int seismographAmount = 2;
}
