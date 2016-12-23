using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour {

    public enum GameMode { Building, Test, Simulation, Finish };
    public List<GameObject> buildingObjects, testObjects, simulationObjects, finishObjects;

    GameObject buildingContainerBackup = null;
    GameObject buildingContainer;

    public TargetController targetController;
    public EarthquakeSimulator earthquakeSimulator;
    
    private GameMode mode;
    public GameMode Mode {
        get { return mode; }
        set {
            setGameMode(value);
            mode = value;
        }
    }

    void Start() {
        Mode = GameMode.Building;
        buildingContainer = GameObject.Find("BuildingContainer");

        targetController.gameObject.SetActive(false);
    }

    // The idea is to put behaviours specific to a mode in a separate object, e.g. the wave simulator,
    // so that we don't need exceptions for every mode. The script starts in its Start method
    public void setGameMode(GameMode newMode) {
        Debug.Log(mode + " -> " + newMode);
        // Exceptions
        if (mode == GameMode.Building && newMode == GameMode.Test) {
            // Test mode -> copy all buildings so they can be restored after testing
            buildingContainerBackup = GameObject.Instantiate(buildingContainer);
            buildingContainerBackup.SetActive(false);

        } else if (mode == GameMode.Test && newMode == GameMode.Building) {
            // Reset buildings
            foreach (Transform child in buildingContainer.transform) 
                GameObject.DestroyImmediate(child.gameObject);

            foreach (Transform child in buildingContainerBackup.transform) {
                child.transform.SetParent(buildingContainer.transform);
                child.gameObject.name = "YES";
            }
            // Todo: Stop shaking of platforms
        } else if (newMode == GameMode.Simulation) {
            // Set position of epicenter


            earthquakeSimulator.simulateEarthquake();
            StartCoroutine(setGameMode(GameMode.Finish, 15));
        }

        // In every case, switch all objects
        List<GameObject> oldObjs = getModeObjects(mode);
        List<GameObject> newObjs = getModeObjects(newMode);

        foreach (GameObject go in oldObjs) {
            go.SetActive(false);
        }
        foreach (GameObject go in newObjs) {
            go.SetActive(true);
        }

        mode = newMode;

        // Stop wave propogation when modes change
        targetController.stopWaves();
    }

    IEnumerator setGameMode(GameMode newMode, int seconds) {
        yield return new WaitForSeconds(seconds);
        setGameMode(newMode);
    }

    // Function with string so you can do it via button click
    public void setGameMode(string newMode) {
        bool match = false;
        foreach (GameMode val in GameMode.GetValues(typeof(GameMode))) {
            if (newMode.Equals(val.ToString())) {
                setGameMode(val);
                match = true;
                break;
            }
        }
        if (!match)
            Debug.Log("Mode not recognized: " + newMode);
    }

    private List<GameObject> getModeObjects(GameMode gm) {
        switch (gm) {
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
}
