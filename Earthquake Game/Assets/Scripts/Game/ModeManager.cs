using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour {

    public enum GameMode { Building, Test, Simulation, Finish };
    public List<GameObject> buildingObjects, testObjects, simulationObjects, finishObjects;

    
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
    }

    public void setGameMode(GameMode newMode) {
        Debug.Log(mode + " -> " + newMode);
        // Exceptions
        if (mode == GameMode.Building && newMode == GameMode.Test) {
            // Test mode -> copy all buildings so they can be restored after testing

        }
        
        // Generally, switch all objects
        List<GameObject> oldObjs = getModeObjects(mode);
        List<GameObject> newObjs = getModeObjects(newMode);

        foreach (GameObject go in oldObjs) {
            go.SetActive(false);
        }
        foreach (GameObject go in newObjs) {
            go.SetActive(true);
        }

        mode = newMode;
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
