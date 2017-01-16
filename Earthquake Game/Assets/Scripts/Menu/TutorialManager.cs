using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public Transform cameraContainer;
    public GameObject background;
    public GameObject skipButton;
    public float cameraMovementSpeed = 2;
    public List<GameObject> tutorialInstructions;

    private int currentInstruction = 0;
    private CamMovement camMovementScript;

    private Vector3 initialCameraPosition;
    private Vector3 newCameraPosition;

    private bool cameraIsMoving = false;

    public void Start()
    {
        camMovementScript = cameraContainer.GetComponent<CamMovement>();
        initialCameraPosition = cameraContainer.position;
    }

    public void Update()
    {
        if (cameraIsMoving && newCameraPosition != null)
        {
            cameraContainer.position = Vector3.Lerp(cameraContainer.position, newCameraPosition, Time.deltaTime * cameraMovementSpeed);
        }
    }
	
    public void startTutorial()
    {
        camMovementScript.enabled = false;
        background.SetActive(true);
        skipButton.SetActive(true);
        currentInstruction = 0;
        showInstruction(tutorialInstructions[currentInstruction++]);
    }

    public void nextInstruction()
    {
        if (currentInstruction < tutorialInstructions.Count && currentInstruction > 0)
        {
            tutorialInstructions[currentInstruction - 1].SetActive(false);
            showInstruction(tutorialInstructions[currentInstruction++]);
        }
        else
        {
            tutorialInstructions[currentInstruction - 1].SetActive(false);
            camMovementScript.enabled = true;
            cameraIsMoving = false;
            deactivateUI();
        }
    }

    public void skipTutorial()
    {
        tutorialInstructions[currentInstruction - 1].SetActive(false);
        currentInstruction = tutorialInstructions.Count;
        nextInstruction();
    }

    private void deactivateUI()
    {
        background.SetActive(false);
        skipButton.SetActive(false);
    }

    private void showInstruction(GameObject instruction)
    {
        instruction.SetActive(true);
        WorldToScreenMap script = instruction.GetComponent<WorldToScreenMap>();

        if (currentInstruction == tutorialInstructions.Count)
        {
            cameraIsMoving = true;
            newCameraPosition = initialCameraPosition;
        }

        if (script != null)
        {
            //Debug.Log("script != null");
            cameraIsMoving = true;
            newCameraPosition = new Vector3(script.follow.transform.position.x,
                                            script.follow.transform.position.y,
                                            cameraContainer.position.z);
        }
    }
}
