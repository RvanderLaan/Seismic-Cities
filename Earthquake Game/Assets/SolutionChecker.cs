using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static string getFeedback(DamageParticleController wave, BuildingPlatformController bpc) {
        Building building = bpc.building;
        Upgrade upgrade = bpc.upgrade;
        

        Building.BuildingType bType = (building != null) ? building.type : Building.BuildingType.None; 
        Upgrade.UpgradeType uType = (upgrade != null) ? upgrade.type : Upgrade.UpgradeType.None;


        BuildingPlatformController.SoilType sType = bpc.soilType;

        // Power of wave: Distance, initial magnitude, 


        if (bType == Building.BuildingType.StoneHouse && sType == BuildingPlatformController.SoilType.Clay)
            return "That clay seems to increase the vibrations of the earthqake. Maybe stone houses are too rigid, they can't bend and will break!";
        else if (bType == Building.BuildingType.Skyscraper && sType == BuildingPlatformController.SoilType.Clay)
            return "Tall buildings are very heavy, so they will collapse when the shaking gets too intense. The clay seems to increase the strength of the vibrations.";

        return "You're building too close to the epicenter, try to place the buildings further away.";
    }
}
