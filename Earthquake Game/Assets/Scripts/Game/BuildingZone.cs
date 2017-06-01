using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingZone : MonoBehaviour
{

    public Soil.SoilType soilType;
    public bool isBuilt, isUpgraded;

    public LayerMask terrainLayer;

    public Building building;
    public Upgrade upgrade;

    private float epicenterDistance;

    private Solutions solutions;

    private BuildingList buildingList;
    private UpgradeList upgradeList;

    public List<AllowedPlacement> allowedPlacements;

    private SpriteRenderer sprite;

    public GameObject undoButton;

    // Use this for initialization
    void Start()
    {
        GameObject _GM = GameObject.Find("_GM");
        solutions = _GM.GetComponent<Solutions>();
        buildingList = _GM.GetComponent<BuildingList>();
        upgradeList = _GM.GetComponent<UpgradeList>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void startShaking() {
        if (!isCorrect()) {
            if (building != null)
                if (soilType == Soil.SoilType.Sand)
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

    /// <summary>
    /// Whether the building/upgrade placed here are allowed
    /// </summary>
    /// <returns></returns>
    public bool isCorrect() {
        Building.BuildingType thisBType = Building.BuildingType.None;
        if (building != null)
            thisBType = building.type;
        Upgrade.UpgradeType thisUType = Upgrade.UpgradeType.None;
        if (upgrade != null)
            thisUType = upgrade.type;

        foreach (AllowedPlacement ap in allowedPlacements) {
            if ((ap.building == thisBType || ap.building == Building.BuildingType.Any)
                && (ap.upgrade == thisUType || ap.upgrade == Upgrade.UpgradeType.Any))
                return true;
        }
        return thisBType == Building.BuildingType.None && thisUType == Upgrade.UpgradeType.None;
    }
}

[Serializable]
public class AllowedPlacement {
    public Building.BuildingType building;
    public Upgrade.UpgradeType upgrade;
}