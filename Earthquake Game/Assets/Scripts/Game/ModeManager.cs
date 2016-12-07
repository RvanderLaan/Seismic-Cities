using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour {

    public List<GameObject> buildingObjects, destructionObjects;

    public bool startInDestructionMode = false;
    private bool isDestructionMode;
    

    // Use this for initialization
    void Start () {
        isDestructionMode = !startInDestructionMode;
        switchModes();
	}

    public void switchModes() {
        isDestructionMode = !isDestructionMode;
        if (isDestructionMode) {
            // Custom switching
            GetComponent<BuildingPlacer>().stopPreview();

            // General disabling
            foreach (GameObject go in destructionObjects) 
                go.SetActive(true);
            foreach (GameObject go in buildingObjects) 
                go.SetActive(false);

            GetComponent<EarthquakeSimulator>().enabled = false;
        } else {
            // Custom switching


            // General disabling
            foreach (GameObject go in buildingObjects)
                go.SetActive(true);
            foreach (GameObject go in destructionObjects)
                go.SetActive(false);

            GetComponent<EarthquakeSimulator>().enabled = true;
        }


        // Deselect button, else pressing space will press it again
        EventSystem.current.SetSelectedGameObject(null, null);
    }
	
	// Update is called once per frame
	void Update () {

	}
}
