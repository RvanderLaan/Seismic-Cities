using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour {

    public enum GameMode { Measuring, Building, Upgrade, Simulation, Pass, Fail };
    public List<GameObject> measuringObjects, buildingObjects, upgradeObjects, simulationObjects, passObjects, failObjects;

    GameObject buildingContainer;

    public LevelManager levelManager;
    public TargetController targetController;
    public Earthquake earthquakeSimulator;
    public SeismographPlacer seismographPlacer;
    public BuildingList buildingList;
    public UpgradeList upgradeList;
    CamMovement cameraMovement;

    public Button backButton, readyButton;

    public UserFeedback userFeedback;

    public UnityEvent onPass, onFail;

    private Solutions solutions;

    public int finishWaitTime = 10;

    private int modeIndex = 0;
    private GameMode mode;
    public GameMode Mode {
        get { return mode; }
        set {
            setGameMode(value);
            mode = value;
        }
    }

    GameMode[] modeOrder = { GameMode.Measuring, GameMode.Simulation, GameMode.Building, GameMode.Upgrade, GameMode.Simulation };

    private void Awake()
    {
        buildingContainer = GameObject.Find("BuildingContainer");
        solutions = GetComponent<Solutions>();
        levelManager = GetComponent<LevelManager>();
        cameraMovement = Camera.main.transform.parent.GetComponent<CamMovement>();

        EventManager.StartListening("NextMode", () => nextMode());
    }

    void Start() {
        Mode = GameMode.Measuring;
        targetController.gameObject.SetActive(false);
    }

    // The idea is to put behaviours specific to a mode in a separate object, e.g. the wave simulator,
    // so that we don't need exceptions for every mode. The script starts in its Start method
    public bool setGameMode(GameMode newMode) {
        List<GameObject> oldObjs = getModeObjects(mode);
        List<GameObject> newObjs = getModeObjects(newMode);
        if (cameraMovement != null)
            cameraMovement.detectClicks = true;

        Debug.Log(mode + " -> " + newMode);
        // Exceptions
        if (mode == GameMode.Measuring && newMode == GameMode.Simulation)
        {
            if (!seismographPlacer.finishedPlacing())
            {
                userFeedback.setText("placeAllSeismographs");
                return false;
            }
            targetController.gameObject.SetActive(true);

            // Move camera

            cameraMovement.detectClicks = false;
            cameraMovement.moveTo((Vector2)targetController.transform.position + new Vector2(0, 64));
            cameraMovement.zoomTo(cameraMovement.maxScale);

            GameObject[] seismographs = GameObject.FindGameObjectsWithTag("Seismograph");
            earthquakeSimulator.simulateEarthquake(seismographs);

            StartCoroutine(nextMode(finishWaitTime));
        }
        else if (mode == GameMode.Building && newMode == GameMode.Upgrade) {
            if (!buildingList.finishedPlacing()) {
                userFeedback.setText("placeAllCities");
                return false;
            } else if (upgradeList.finishedPlacing())
            {
                // Skip upgrade mode if no upgrades are specified
                foreach (GameObject go in oldObjs)
                    go.SetActive(false);
                nextMode();
                return false;
            }
        } else if ((mode == GameMode.Building || mode == GameMode.Upgrade) && newMode == GameMode.Simulation) {
            // Check if all buildings have been set
            if (!buildingList.finishedPlacing()) {
                userFeedback.setText("placeAllCities");
                return false;
            } else if (!upgradeList.finishedPlacing()) {
                userFeedback.setText("You should place all upgrades before continuing");
                return false;
            }

            // Disable all undo buttons
            GameObject[] platforms = GameObject.FindGameObjectsWithTag("BuildingPlatform");
            foreach (GameObject go in platforms) {
                Button b = go.GetComponentInChildren<Button>();
                if (b != null)
                    b.gameObject.SetActive(false);
            }

            targetController.gameObject.SetActive(true);
            earthquakeSimulator.simulateEarthquake(platforms);

            // Check if passed
            bool passed = true;
            foreach (GameObject go in platforms) {
                BuildingZone bpc = go.GetComponent<BuildingZone>();
                if (bpc.building != null && bpc.building.type == Building.BuildingType.Thematic)
                    continue;
                else if (!bpc.isCorrect()) {
                    Debug.Log("Incorrect placement: " + bpc.building);
                    passed = false;
                    break;
                }
            }

            backButton.interactable = false;
            readyButton.interactable = false;

            if (passed) {
                StartCoroutine(setGameMode(GameMode.Pass, finishWaitTime));
            } else {
                StartCoroutine(setGameMode(GameMode.Fail, finishWaitTime));
            }
        } else if (mode == GameMode.Upgrade && newMode == GameMode.Building) {
            // Todo: Remove/reset upgrades when going back?

        } else if (newMode == GameMode.Pass) {
            onPass.Invoke();
        } else if (newMode == GameMode.Fail) {
            onFail.Invoke();
        }

        // Disable/Enable GUI
        if (newMode == modeOrder[0]) backButton.interactable = false;
        else backButton.interactable = newMode != GameMode.Simulation;
        readyButton.interactable = newMode != GameMode.Simulation;

        // In every case, switch all objects
        foreach (GameObject go in oldObjs)
            go.SetActive(false);
        foreach (GameObject go in newObjs)
            go.SetActive(true);

        // Hide UI if not necessary
        if (newMode == GameMode.Measuring && levelManager != null && levelManager.getLevelData() != null && levelManager.getLevelData().seismographAmount == 0)
        {
            foreach (GameObject go in measuringObjects)
                go.SetActive(false);
        }

        if (mode == GameMode.Simulation)
            earthquakeSimulator.stopShaking();

        mode = newMode;
        EventManager.TriggerEvent("Mode:" + newMode);

        // Stop wave propogation when modes change
        //targetController.stopWaves();

        return true;
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

    IEnumerator nextMode(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        nextMode();
    }
    public void nextMode()
    {
        if (modeIndex + 1 < modeOrder.Length)
        {
            modeIndex++;
            GameMode newMode = modeOrder[modeIndex];
            bool allowed = setGameMode(newMode);
            if (!allowed)
                modeIndex--;
        }

        // Todo Disable previous button?
    }

    public void previousMode()
    {
        if (modeIndex - 1 >= 0) {
            modeIndex--;
            GameMode newMode = modeOrder[modeIndex];
            setGameMode(newMode);
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
        modeIndex = -1;
        nextMode();
    }

    private List<GameObject> getModeObjects(GameMode gm) {
        switch (gm) {
            case GameMode.Measuring:
                return measuringObjects;
            case GameMode.Building:
                return buildingObjects;
            case GameMode.Upgrade:
                return upgradeObjects;
            case GameMode.Simulation:
                return simulationObjects;
            case GameMode.Pass:
                return passObjects;
            case GameMode.Fail:
                return failObjects;
        }
        return null;
    }
}
