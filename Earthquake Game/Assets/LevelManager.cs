using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Range(0, 100)]
    public int levelIndex;

    public List<LevelData> levels;

    public Transform buildingZones, terrain, buildingList, upgradeList, seismographButton;

	// Use this for initialization
	void Start () {
        constructLevel(levels[levelIndex]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public LevelData getLevelData()
    {
        return levels[levelIndex];
    }

    void constructLevel(LevelData levelData)
    {
        // Set random seed
        Random.InitState(levelData.seed != 0 ? levelData.seed : levelData.levelName.GetHashCode());


        // Set level name, done internally


        // Set terrain blocks

        int levelScale = 8;

        // Create building zones
        GameObject bz = Resources.Load("Prefabs/BuildingZone") as GameObject;
        BuildingZone bzComp = bz.GetComponent<BuildingZone>();
        bzComp.buildingList = buildingList.GetComponent<BuildingList>();
        bzComp.upgradeList = upgradeList.GetComponent<UpgradeList>();
        foreach (BuildingZoneData data in levelData.buildingZones)
        {
            GameObject instance = GameObject.Instantiate(bz, buildingZones);
            
            // Vector3 pos = new Vector3(data.gridPosition, grid.getHeight(data.gridPosition), 1);
            Vector2 origin = new Vector3(levelScale * data.gridPosition, levelScale * Grid.dimensions.yMax);

            RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, Grid.dimensions.height * levelScale);
            Debug.Log("Origin: " + origin);
            if (hit.collider != null)
            {
                Debug.Log(hit.point);
                instance.transform.position = hit.point;
            }
            else
            {
                Debug.Log("No no no");
            }

                
        }

        // Set trees


        // Set lists
        BuildingList bList = buildingList.GetComponent<BuildingList>();
        bList.buildingItems = levelData.buildingItems;
        Debug.Log(levelData.buildingItems.Count);

        UpgradeList uList = upgradeList.GetComponent<UpgradeList>();
        uList.upgradeItems = levelData.upgradeItems;

        // Seismograph
    }
}
