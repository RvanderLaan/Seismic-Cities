using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public Transform cameraContainer;
    public GameObject background;
    public GameObject skipButton;
    public List<GameObject> tutorialInstructions;

    

    private int currentInstruction = 0;

    public void Start()
    {
    }
	
    public void startTutorial()
    {
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

        if (script != null)
        {
            
            //camera.gameObject.GetComponentInParent<CamMovement>().enabled = false;
            
            cameraContainer.position = new Vector3(script.follow.transform.position.x,
                                                    script.follow.transform.position.y,
                                                    cameraContainer.position.z);
                                                    
            
        }
    }
}
