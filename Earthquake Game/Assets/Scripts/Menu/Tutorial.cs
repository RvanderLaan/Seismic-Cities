using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Transform cameraContainer;
    public TextProvider textProvider;
    public GameObject background;
    public float cameraMovementSpeed = 2;
    public Instruction[] instructions;

    public GameObject instructionPrefab;
    private GameObject instructionObj;
    private TextInserterPro instrText;
    private Button skipButton, nextButton;
    private GameObject arrows;

    private int currentInstruction = 0;
    private CamMovement camMovementScript;

    private bool cameraIsMoving = false;

    public bool startAtStart = true;

    public void Start()
    {
        camMovementScript = cameraContainer.GetComponent<CamMovement>();

        instructionObj = Instantiate(instructionPrefab, transform);
        instrText = instructionObj.GetComponentInChildren<TextInserterPro>();
        Debug.Log("instrText");
        Debug.Log(instrText);

        Button[] buttons = instructionObj.GetComponentsInChildren<Button>();
        skipButton = buttons[0];
        nextButton = buttons[1];

        skipButton.onClick.AddListener(skipTutorial);
        nextButton.onClick.AddListener(nextInstruction);

        Debug.Log(skipButton.name + ", " + nextButton.name);

        arrows = instructionObj.transform.Find("Arrows").gameObject;

        instructionObj.SetActive(false);

        if (startAtStart)
            startTutorial();
    }
	
    public void startTutorial()
    {
        EventManager.TriggerEvent("TutorialStart");
        instructionObj.SetActive(true);
        camMovementScript.enabled = false;
        // background.SetActive(true);
        currentInstruction = 0;
        showInstruction(instructions[currentInstruction]);
    }

    public void nextInstruction()
    {
        if (instructions[currentInstruction].nextEventTrigger != null && instructions[currentInstruction].nextEventTrigger != "")
            EventManager.StopListening(instructions[currentInstruction].nextEventTrigger, nextInstruction);

        if (currentInstruction >= 0 && instructions[currentInstruction].emitOnExit != null && instructions[currentInstruction].emitOnExit != "")
            EventManager.TriggerEvent(instructions[currentInstruction].emitOnExit);

        if (currentInstruction < instructions.Length - 1 && currentInstruction >= 0) {
            currentInstruction++;
            showInstruction(instructions[currentInstruction]);
        }
        else {
            camMovementScript.enabled = true;
            deactivateUI();
            EventManager.TriggerEvent("TutorialSkip");
        }
    }

    public void skipTutorial() {
        EventManager.TriggerEvent("TutorialSkip");
        currentInstruction = instructions.Length-1;
        nextInstruction();
    }

    private void deactivateUI() {
        background.SetActive(false);
        instructionObj.SetActive(false);
    }

    private void showInstruction(Instruction instruction)
    {
        nextButton.interactable = true;
        if (instruction.nextEventTrigger != null && instruction.nextEventTrigger != "") {
            nextButton.interactable = false;

            EventManager.StartListening(instruction.nextEventTrigger, nextInstruction);
        }
        nextButton.GetComponentInChildren<Text>().text = "Next " + (currentInstruction + 1) + "/" + instructions.Length;
        instrText.reset(instruction.text);
        for (int i = 0; i < 4; i++)
            arrows.transform.GetChild(i).gameObject.SetActive(false);
        if (instruction.arrow != -1)
            arrows.transform.GetChild(instruction.arrow).gameObject.SetActive(true);


        // Move instruction
        if (instruction.screenPosition != null && instruction.screenPosition != Vector2.zero) {
            RectTransform rect = instructionObj.GetComponent<RectTransform>();
            Vector2 newPos = new Vector2(Screen.width * instruction.screenPosition.x - rect.rect.width / 2, Screen.height * instruction.screenPosition.y - rect.rect.height / 2);
            if (instruction.arrow != -1)
                newPos += new Vector2(((instruction.arrow - 2) % 2) * rect.rect.width / 2, ((instruction.arrow - 1) % 2) * rect.rect.height / 2);
            rect.position = newPos;
        }

        // Move camera
        if (instruction.focusGameObjectName != null && instruction.focusGameObjectName != "")
        {
            GameObject focusGo = GameObject.Find(instruction.focusGameObjectName);

            if (focusGo == null)
            {
                GameObject[] rootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject go in rootObjs)
                    if (go.name == instruction.focusGameObjectName)
                    {
                        focusGo = go;
                        break;
                    }
            }
            if (focusGo == null)
                Debug.LogWarning("Focus GameObject from instruction is null. (" + instruction.focusGameObjectName + ")");
            else
            {
                RectTransform rect = focusGo.GetComponent<RectTransform>();
                if (rect != null)
                {
                    
                } else
                {
                    camMovementScript.moveTo(new Vector2(focusGo.transform.position.x, focusGo.transform.position.y));
                    camMovementScript.zoomTo(instruction.zoom, true);

                    RectTransform instRect = instructionObj.GetComponent<RectTransform>();
                    Vector2 newPos = new Vector2(Screen.width / 2 - instRect.rect.width / 2, Screen.height / 2 - instRect.rect.height / 2);
                    if (instruction.arrow != -1)
                        newPos += new Vector2(((instruction.arrow - 2) % 2) * instRect.rect.width / 2, ((instruction.arrow - 1) % 2) * instRect.rect.height / 2);
                    instRect.position = newPos;
                }
            }
        }
    }
}
