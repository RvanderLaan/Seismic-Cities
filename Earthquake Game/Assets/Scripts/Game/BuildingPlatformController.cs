using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlatformController : MonoBehaviour
{

    public SoilType soilType;
    public bool isBuilt, isUpgraded;

    public LayerMask terrainLayer;

    // List<Upgrade>
    public Building building;
    public Upgrade upgrade;

    private float epicenterDistance;

    private Solutions solutions;

    private BuildingList buildingList;
    private UpgradeList upgradeList;

    private SpriteRenderer sprite;

    public enum SoilType {
        Marl, Limestone, Sand, Sandstone, Clay, Bedrock, Quicksand,
    }

    public GameObject undoButton;

    // Use this for initialization
    void Start()
    {
        GameObject _GM = GameObject.Find("_GM");
        solutions = _GM.GetComponent<Solutions>();
        buildingList = _GM.GetComponent<BuildingList>();
        upgradeList = _GM.GetComponent<UpgradeList>();

        sprite = GetComponentInChildren<SpriteRenderer>();

        // Raycast downwards to terrain and assign joint anchor
        RaycastHit2D terrainHit = Physics2D.Raycast(transform.position + Vector3.up * 10, -Vector2.up, float.MaxValue, terrainLayer);
        if (terrainHit.collider != null) {
            Vector2 hitPoint = terrainHit.point;
            string terrainName = terrainHit.collider.gameObject.name;
            
            // Try to automatically set soil type based on terrain name

            string[] typeNames = Enum.GetNames(typeof (SoilType));
            Array typeVals     = Enum.GetValues(typeof(SoilType));
            for (int i = 0; i < typeNames.Length; i++) {
                if (terrainName.Equals(typeNames[i])) {
                    soilType = (SoilType) typeVals.GetValue(i);
                }
            }

            gameObject.name = gameObject.name + " of " + soilType + " on " + terrainHit.collider.gameObject.name;

            if (!soilType.ToString().Equals(terrainName)) {
                Debug.Log("WARNING: Soil type property name of building zone do not equal terrain name: " + soilType + " != " + terrainHit.collider.gameObject.name);
            }
        } else {
            Debug.Log("WARNING: Terrain not found of building platform");
        }
    }

    public void startShaking() {
        if (!solutions.correctPlacement(this)) {
            if (building != null)
                if (soilType == SoilType.Quicksand)
                    building.Sink();
                else
                    building.Collapse();
        }
    }

    public void place(Building b) {
        building = b;
        undoButton.SetActive(true);

        isBuilt = true;                                         // Set the platform as unavailable
        sprite.gameObject.SetActive(false); // Hide green sprite
        building.transform.position = transform.position + Vector3.forward;       // Snap building to position of this platform
    }

    public void placeUpgrade(Upgrade u) {
        upgrade = u;
        undoButton.SetActive(true);

        isUpgraded = true;
        upgrade.transform.position = transform.position + Vector3.back * 2;       // Snap building to position of this platform
    }

    public void undoPlacement() {
        // Remove building and upgrade
        buildingList.undo(building.type);
        if (isUpgraded)
            upgradeList.undo(upgrade.type);

        GameObject.Destroy(building.gameObject);
        if (isUpgraded) GameObject.Destroy(upgrade.gameObject);

        isBuilt = false;
        isUpgraded = false;
        sprite.gameObject.SetActive(true);
        undoButton.SetActive(false);
    }
}
