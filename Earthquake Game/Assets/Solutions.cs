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

    public bool hasPassed() {
        foreach (SolutionItem si in solutions) {
            // Todo: if (si....) return true
        }

        return true;
    }
}

[System.Serializable]
public class SolutionItem {

    public BuildingPlatformController platform;
    public BuildingType building;
    public UpgradeType upgrade;

}

public enum BuildingType { None, House, Skyscraper }
public enum UpgradeType { None, Foundation, Counterweight }
