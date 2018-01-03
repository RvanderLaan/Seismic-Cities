using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class LevelManager : MonoBehaviour {

    [Range(0, 100)]
    public int levelIndex;

    public List<LevelData> levels;

    public Transform buildingZones, terrain, buildingList, upgradeList, seismographButton, sceneryContainer, targetController, menu, levelSpecific, tutorial, soilGlossary;
    public LevelName levelName;
    public ModeManager modeManager;
    public SeismographPlacer seismographPlacer;


    public Transform[] objectContainers;

    public int levelScale = 8;

    public static Rect dimensions = new Rect(-32, -48, 128, 64);

    private Fader fader;

    // Use this for initialization
    void Awake () {
        fader = GetComponent<Fader>();

        LevelData ld = levels[levelIndex];
        ConstructLevel(ld);

        EventManager.StartListening("NextLevel", () => PrepNextLevel());
	}

    public LevelData getLevelData()
    {
        return levels[levelIndex];
    }

    public void PrepNextLevel()
    {
        if (CameraShaker.Instance.ShakeInstances.Count >= 1)
            CameraShaker.Instance.ShakeInstances[0].StartFadeOut(fader.getFadeInTime());
        menu.gameObject.SetActive(false);
        fader.BeginFade();
        Invoke("NextLevel", fader.getFadeInTime());
    }
    public void NextLevel()
    {
        if (levelIndex + 1 >= levels.Count)
        {
            Debug.Log("You won!");
            levelIndex = -1;

            if (levels.Count > 0)
                PrepNextLevel();
            return;
        }

        // Todo: Loading screen/fade

        // Destruct previous level
        DestroyChildren(objectContainers);

        // Disable nextlevel button etc.

        // Stop wave propogation when modes change
        targetController.GetComponent<TargetController>().stopWaves();

        // Construct new level
        levelIndex++;
        ConstructLevel(levels[levelIndex]);
        fader.EndFade();
    }

    public void Restart()
    {
        levelIndex--;
        PrepNextLevel();
    }

    void DestroyChildren(params Transform[] transforms)
    {
        foreach (Transform transform in transforms)
            foreach (Transform child in transform)
                Destroy(child.gameObject);
    }

    void ConstructLevel(LevelData levelData)
    {
        // Set random seed
        Random.InitState(levelData.seed != 0 ? levelData.seed : levelData.levelName.GetHashCode());

        // Set earthquake
        targetController.position = new Vector3(levelData.earthquake.position.x, levelData.earthquake.position.y, -2);
        targetController.GetComponent<Earthquake>().earthquakeData = levelData.earthquake;

        // Set level name, done internally
        levelName.StartFade(levelData.levelName);

        // Add terrain
        //for (int i = 0; i < terrain.transform.childCount; i++)
        //    GameObject.Destroy(terrain.transform.GetChild(i);
        List<SoilGlossaryItem> soilItems = SetupTerrain(levelData.soilLayers);

        // Create building zones
        CreateBuildingZones(levelData.buildingZones);

        // Set scenery (trees, rocks, plants, etc.)
        CreateScenery(levelData.cameraLimits, levelData);
        CreateUndergroundScenery(levelData.cameraLimits, levelData.underground);

        // Level specifics
        foreach (GameObject go in levelData.other)
            Instantiate(go, go.transform.position, Quaternion.identity, levelSpecific);

        // Set lists
        BuildingList bList = buildingList.GetComponent<BuildingList>();
        bList.Reset(levelData.buildingItems);

        UpgradeList uList = upgradeList.GetComponent<UpgradeList>();
        uList.Reset(levelData.upgradeItems);

        // Seismograph
        seismographPlacer.Reset(levelData.seismographAmount);

        // Dialogue/tutorial
        if (levelData.tutorial.Length > 0)
        {
            tutorial.GetComponent<Tutorial>().instructions = levelData.tutorial;
            tutorial.GetComponent<Tutorial>().startTutorial();
        }

        // Soil glossary
        soilGlossary.GetComponent<SoilGlossary>().init(soilItems);

        // Starting mode
        modeManager.Reset();
    }

    List<SoilGlossaryItem> SetupTerrain(GameObject terrainPrefab)
    {
        GameObject separatedPrefab = terrainPrefab.transform.Find("Separated").gameObject;
        // GameObject allPrefab = terrainPrefab.transform.Find("All_applied").gameObject;

        GameObject separated = Instantiate(separatedPrefab, terrain);
        //GameObject all = Instantiate(allPrefab, terrain);

        List<SoilGlossaryItem> soilItems = new List<SoilGlossaryItem>();

        for (int i = 0; i < separated.transform.childCount; i++)
        {
            // Add collision to separated layers
            GameObject layer = separated.transform.GetChild(i).gameObject;
            layer.SetActive(false);
            layer.transform.localScale = new Vector3(-10, 10, -10);
            ColliderCreator cc = layer.AddComponent<ColliderCreator>();
            cc.isTrigger = true;
            Rigidbody2D r2d = layer.AddComponent<Rigidbody2D>();
            r2d.isKinematic = true;

            layer.layer = LayerMask.NameToLayer("Terrain");
            Soil soil = layer.AddComponent<Soil>();
            Soil.SoilType soilType;
            Material material = layer.GetComponent<MeshRenderer>().material;
            string materialName = material.name;
            string[] split = materialName.Split(' ');
            if (split.Length > 1)
                materialName = materialName.Substring(0, materialName.Length - split[split.Length - 1].Length - 1);

            switch (materialName)
            {
                case "Bedrock":
                    soilType = Soil.SoilType.Bedrock;
                    break;
                case "Volcanic":
                case "Marl":
                case "Sandstone":
                    soilType = Soil.SoilType.Rock;
                    break;
                case "Limestone":
                case "Saltstone":
                    soilType = Soil.SoilType.SoftRock;
                    break;
                case "Clay":
                    soilType = Soil.SoilType.Clay;
                    break;
                case "Sand":
                    soilType = Soil.SoilType.Sand;
                    break;
                case "Grass":
                    soilType = Soil.SoilType.Other;
                    break;
                default:
                    Debug.LogWarning("No correct soil type found for material '" + materialName + "' (object: '" + layer.name + "')");
                    soilType = Soil.SoilType.Other;
                    break;
            }
            soil.type = soilType;

            // Add every type only once
            if (soilType != Soil.SoilType.Other && soilItems.Find((x) => x.type == soilType) == null)
            {
                Texture2D tex = (Texture2D) material.mainTexture;
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                soilItems.Add(new SoilGlossaryItem(soilType, false, sprite));
            }

            layer.SetActive(true);
        }
        //ColliderCreator ccAll = all.AddComponent<ColliderCreator>();
        //all.GetComponent<MeshRenderer>().enabled = true;
        //all.transform.localScale = new Vector3(-10, 10, -10);
        //all.layer = LayerMask.NameToLayer("Terrain");

        return soilItems;
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
           
            Vector2 origin = new Vector3(levelScale * data.gridPosition, levelScale * dimensions.yMax);

            RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, dimensions.height * levelScale);
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

    void CreateScenery(Vector4 limits, LevelData levelData)
    {
        Scenery scenery = levelData.scenery;
        if (scenery == null) return;
        // Raycast down onto terrain
        for (float i = dimensions.xMin * levelScale; i < dimensions.xMax * levelScale; i += levelScale)
        {
            bool spawn = Random.value < scenery.spawnChance;
 
            if (spawn)
            {
                Vector2 origin = new Vector3(i, levelScale * dimensions.yMax);

                // Do not spawn on building zones
                foreach (BuildingZoneData bzd in levelData.buildingZones)
                {
                    int bzdX = levelScale * bzd.gridPosition;
                    float padding = 1;
                    if (origin.x < bzdX + levelScale * padding && origin.x > bzdX - levelScale * padding)
                        spawn = false;
                }
                if (!spawn)
                    continue;

                RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, dimensions.height * levelScale);
                if (hit.collider != null)
                {
                    // Todo: Check for building zone or other collider

                    int randomIndex = Random.Range(0, scenery.prefabs.Length);
                    GameObject prefab = scenery.prefabs[randomIndex];
                    GameObject instance = Instantiate(prefab, sceneryContainer);

                    Vector3 pos = hit.point;
                    pos.z += 1;
                    instance.transform.position = pos;
                    instance.transform.localScale = instance.transform.localScale * Random.Range(1 - scenery.randomScale, 1 + scenery.randomScale);
                }
            }            
        }
    }

    void CreateUndergroundScenery(Vector4 limits, Scenery scenery)
    {
        if (scenery == null) return;
        float bottom = levelScale * dimensions.yMin;
        // Raycast down onto terrain
        for (float i = dimensions.xMin * levelScale; i < dimensions.xMax * levelScale; i += levelScale)
        {
            bool spawn = Random.value < scenery.spawnChance;

            if (spawn)
            {
                Vector2 origin = new Vector3(i, levelScale * dimensions.yMax);

                RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, dimensions.height * levelScale);
                if (hit.collider != null)
                {
                    int randomIndex = Random.Range(0, scenery.prefabs.Length);
                    GameObject prefab = scenery.prefabs[randomIndex];
                    GameObject instance = Instantiate(prefab, sceneryContainer);

                    Vector3 pos = hit.point;
                    pos.y = Random.Range(bottom, pos.y);
                    pos.z -= 1;
                    instance.transform.position = pos;
                    instance.transform.localScale = instance.transform.localScale * Random.Range(1 - scenery.randomScale, 1 + scenery.randomScale);
                    if (scenery.randomRotation)
                        instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
                }
            }
        }
    }
}
