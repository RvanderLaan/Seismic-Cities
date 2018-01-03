using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionChecker
{
    public static string getFeedback(DamageParticleController wave, BuildingZone bpc)
    {
        Building building = bpc.building;
        Upgrade upgrade = bpc.upgrade;


        Building.BuildingType bType = (building != null) ? building.type : Building.BuildingType.None;
        Upgrade.UpgradeType uType = (upgrade != null) ? upgrade.type : Upgrade.UpgradeType.None;


        Soil.SoilType sType = bpc.soilType;

        // Power of wave: Distance, initial magnitude, 


        if (bType == Building.BuildingType.StoneHouse && sType == Soil.SoilType.Clay)
            return "failClay";

        return "failDistance";
    }
}