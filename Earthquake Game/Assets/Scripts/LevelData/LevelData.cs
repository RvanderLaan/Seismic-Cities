using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Level", menuName = "SeismicCities/Level")]
public class LevelData : ScriptableObject
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

    public Scenery scenery;
    public GameObject[] other; // E.g. tsunamis, will be instantiated at start of level
    
    // Menu / UI
    [SerializeField]
    public Vector4 cameraLimits = new Vector4(0, 599, -150, 150);
    [SerializeField]
    public Tutorial tutorial;
    [SerializeField]
    public Dialog start, pass, fail;            // Fail is dynamic?

    // Player items
    [SerializeField]
    public List<BuildingItem> buildingItems;
    [SerializeField]
    public List<UpgradeItem> upgradeItems;
    [SerializeField]
    public int seismographAmount = 2;

    public void OnValidate()
    {
        Debug.Log("Changed level: " + Time.time);
    }

}
