using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour {

    public enum GameMode { Building, Test, Simulation, Finish };
    public List<GameObject> buildingObjects, testObjects, simulationObjects, finishObjects;

    // TODO: List components vs list objects? or both

    public bool startInDestructionMode = false;
    private bool isDestructionMode;

    public GameMode mode {
        get { return mode;  }
        set { setGameMode(value); }
    }
    
    private void setGameMode(GameMode gm) {
        // Exceptions
        if (mode == GameMode.Building && gm == GameMode.Test) {
            // Test mode -> copy all buildings so they can be restored
        }


        if (gm == mode)
            return;
        List<GameObject> oldObjs = getModeObjects(mode);
        List<GameObject> newObjs = getModeObjects(gm);
        mode = gm;

        foreach (GameObject go in oldObjs)
            go.SetActive(false);
        foreach (GameObject go in newObjs)
            go.SetActive(true);
    }

    private List<GameObject> getModeObjects(GameMode gm) {
        switch (mode) {
            case GameMode.Building:
                return buildingObjects;
            case GameMode.Test:
                return testObjects;
            case GameMode.Simulation:
                return simulationObjects;
            case GameMode.Finish:
                return finishObjects;
        }
        return null;
    }

    // Start in tutorial mode
    // Switch to building mode
    //      Switch to test mode (and back)
    // Switch to simulation mode (and back?)
    

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
            foreach (GameObject go in testObjects) 
                go.SetActive(true);
            foreach (GameObject go in buildingObjects) 
                go.SetActive(false);

            GetComponent<EarthquakeSimulator>().enabled = false;
        } else {
            // Custom switching


            // General disabling
            foreach (GameObject go in buildingObjects)
                go.SetActive(true);
            foreach (GameObject go in testObjects)
                go.SetActive(false);

            GetComponent<EarthquakeSimulator>().enabled = true;
        }


        // Deselect button, else pressing space will press it again
        EventSystem.current.SetSelectedGameObject(null, null);
    }

    public bool isInDestructionMode() {
        return isDestructionMode;
    }

    public bool isInBuildingMode() {
        return !isDestructionMode;
    }

    // Update is called once per frame
    void Update () {

	}
}
