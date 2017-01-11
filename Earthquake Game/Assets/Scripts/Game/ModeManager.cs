using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour {

    public enum GameMode { Building, Upgrade, Simulation, Finish };
    public List<GameObject> buildingObjects, upgradeObjects, simulationObjects, finishObjects;

    GameObject buildingContainer;

    public TargetController targetController;
    public EarthquakeSimulator earthquakeSimulator;
    public BuildingList buildingList;
    public UpgradeList upgradeList;

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
        if (mode == GameMode.Building && newMode == GameMode.Upgrade || newMode == GameMode.Simulation) {
            // Todo: Check if all buildings have been set
            if (!buildingList.finishedPlacing()) {
                Debug.Log("Place all buildings before continuing");
                return;
            }

        } else if (mode == GameMode.Upgrade && newMode == GameMode.Building) {
            // Todo: Remove/reset upgrades

        } else if (mode == GameMode.Upgrade && newMode == GameMode.Simulation) {
            // Check if all foundations have been set
            if (!upgradeList.finishedPlacing()) {
                Debug.Log("Place all upgrades before continuing");
                return;
            }

        } else if (newMode == GameMode.Simulation) {
            earthquakeSimulator.simulateEarthquake();
            StartCoroutine(setGameMode(GameMode.Finish, 15));

            // Todo: Check damage
            GameObject[] platforms = GameObject.FindGameObjectsWithTag("BuildingPlatform");

            /*
                
            */
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
            case GameMode.Upgrade:
                return upgradeObjects;
            case GameMode.Simulation:
                return simulationObjects;
            case GameMode.Finish:
                return finishObjects;
        }
        return null;
    }
}
