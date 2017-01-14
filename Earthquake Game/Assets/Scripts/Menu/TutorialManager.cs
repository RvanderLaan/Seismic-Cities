using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public GameObject background;
    public GameObject skipButton;
    public List<GameObject> tutorialInstructions;

    private int currentInstruction = 0;

	
    public void startTutorial()
    {
        background.SetActive(true);
        skipButton.SetActive(true);
        currentInstruction = 0;
        tutorialInstructions[currentInstruction++].SetActive(true);
    }

    public void nextInstruction()
    {
        if (currentInstruction < tutorialInstructions.Count && currentInstruction > 0)
        {
            tutorialInstructions[currentInstruction - 1].SetActive(false);
            tutorialInstructions[currentInstruction++].SetActive(true);
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
}
