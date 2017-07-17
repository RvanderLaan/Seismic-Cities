using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [Range(0, 100)]
    public int levelIndex;

    public List<LevelData> levels;

    public Transform buildingZones, terrain, buildingList, upgradeList, seismographButton, sceneryContainer, targetController;

    public Transform[] objectContainers;

    public int levelScale = 8;

    // Use this for initialization
    void Start () {
        ConstructLevel(levels[levelIndex]);
	}

    public LevelData getLevelData()
    {
        return levels[levelIndex];
    }

    public void NextLevel()
    {
        // Todo: Loading screen/fade

        // Destruct previous level
        DestroyChildren(objectContainers);

        // Disable nextlevel button etc.


        // Construct new level
        levelIndex++;
        ConstructLevel(levels[levelIndex]);
    }

    public void Restart()
    {
        levelIndex--;
        ConstructLevel(levels[levelIndex]);
    }

    void DestroyChildren(params Transform[] transforms)
    {
        foreach (Transform transform in transforms)
        {
            foreach (Transform child in transform)
                GameObject.Destroy(child.gameObject);
        }
    }

    void ConstructLevel(LevelData levelData)
    {
        // Set random seed
        Random.InitState(levelData.seed != 0 ? levelData.seed : levelData.levelName.GetHashCode());

        // Set earthquake
        targetController.position = levelData.earthquake.position;


        // Set level name, done internally

        // Set terrain blocks


        // Create building zones
        CreateBuildingZones(levelData.buildingZones);

        // Set scenery (trees, rocks, plants, etc.)
        CreateScenery(levelData.cameraLimits, levelData.scenery);

        // Set lists
        BuildingList bList = buildingList.GetComponent<BuildingList>();
        bList.buildingItems = levelData.buildingItems;
        Debug.Log(levelData.buildingItems.Count);

        UpgradeList uList = upgradeList.GetComponent<UpgradeList>();
        uList.upgradeItems = levelData.upgradeItems;

        // Seismograph, done internally
    }

    void CreateBuildingZones(List<BuildingZoneData> list)
    {
        // Raycast down onto terrain
        GameObject bz = Resources.Load("Prefabs/BuildingZone") as GameObject;
        BuildingZone bzComp = bz.GetComponent<BuildingZone>();
        bzComp.buildingList = buildingList.GetComponent<BuildingList>();
        bzComp.upgradeList = upgradeList.GetComponent<UpgradeList>();
        foreach (BuildingZoneData data in list)
        {
            GameObject instance = GameObject.Instantiate(bz, buildingZones);
            BuildingZone bzInstance = instance.GetComponent<BuildingZone>();
            bzInstance.allowedPlacements = data.allowedPlacements;
           
            Vector2 origin = new Vector3(levelScale * data.gridPosition, levelScale * Grid.dimensions.yMax);

            RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, Grid.dimensions.height * levelScale);
            if (hit.collider != null)
            {
                instance.transform.position = hit.point;
            }
            else
            {
                Debug.LogError("Building zone cannot be placed (Gridposition: " + data.gridPosition + ")");
            }
        }
    }

    void CreateScenery(Vector4 limits, Scenery scenery)
    {
        Debug.Log(limits);
        // Raycast down onto terrain
        for (float i = limits.x; i < limits.y; i += levelScale)
        {
            bool spawn = Random.value < scenery.spawnChance;
 
            if (spawn)
            {
                Vector2 origin = new Vector3(i, levelScale * Grid.dimensions.yMax);

                RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, Grid.dimensions.height * levelScale);
                if (hit.collider != null)
                {
                    // Todo: Check for building zone or other collider

                    int randomIndex = Random.Range(0, scenery.prefabs.Length);
                    GameObject prefab = scenery.prefabs[randomIndex];
                    GameObject instance = GameObject.Instantiate(prefab, sceneryContainer);

                    Vector3 pos = hit.point;
                    pos.z -= 1;
                    instance.transform.position = pos;
                    instance.transform.localScale = instance.transform.localScale * Random.Range(1 - scenery.randomScale, 1 + scenery.randomScale);
                }
            }            
        }
    }
}
