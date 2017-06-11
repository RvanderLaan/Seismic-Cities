using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Range(0, 100)]
    public int levelIndex;

    public List<LevelData> levels;

    public Transform buildingZones, terrain, buildingList, upgradeList;

	// Use this for initialization
	void Start () {
        constructLevel(levels[levelIndex]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void constructLevel(LevelData levelData)
    {
        // Set random seed
        Random.InitState(levelData.seed != 0 ? levelData.seed : levelData.levelName.GetHashCode());

        Grid grid = levelData.blocks;

        // Set level name

        // Set terrain blocks

        // Create building zones
        GameObject bz = Resources.Load("Prefabs/BuildingZone") as GameObject;
        foreach (BuildingZoneData data in levelData.buildingZones)
        {
            GameObject instance = GameObject.Instantiate(bz, buildingZones);
            Vector3 pos = new Vector3(data.gridPosition, grid.getHeight(data.gridPosition), 1);
            instance.transform.position = pos * grid.size;
        }

        // Set trees
        

        // Set lists
       

    }
}
