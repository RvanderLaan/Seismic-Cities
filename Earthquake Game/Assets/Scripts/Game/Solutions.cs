using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solutions : MonoBehaviour {

    public List<SolutionItem> solutions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool correctPlacement(BuildingZone bpc) {
        foreach (SolutionItem si in solutions) {
            if (si.platform.Equals(bpc)) {
                // If it is found, the correct type of building and upgrade should have been placed
                Upgrade.UpgradeType uType = Upgrade.UpgradeType.None;
                if (si.platform.upgrade != null)
                    uType = si.platform.upgrade.type;
                Building.BuildingType bType = Building.BuildingType.None;
                if (si.platform.building != null)
                    bType = si.platform.building.type;

                if (si.upgrade == uType && si.building == bType)
                    return true;

                return false;
            }
        }
        // If it isn't defined as solution, it should be empty
        if ((bpc.building == null || bpc.building.type == Building.BuildingType.None )
            && (bpc.upgrade == null || bpc.upgrade.type == Upgrade.UpgradeType.None))
            return true;
        return false;
    }

    public bool hasPassed() {
        foreach (SolutionItem si in solutions) {

            Upgrade.UpgradeType uType = Upgrade.UpgradeType.None;
            if (si.platform.upgrade != null)
                uType = si.platform.upgrade.type;
            Building.BuildingType bType = Building.BuildingType.None;
            if (si.platform.building != null)
                bType = si.platform.building.type;

            if (!(si.upgrade == uType && si.building == bType))
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class SolutionItem {

    public BuildingZone platform;
    public Building.BuildingType building;
    public Upgrade.UpgradeType upgrade;

}
