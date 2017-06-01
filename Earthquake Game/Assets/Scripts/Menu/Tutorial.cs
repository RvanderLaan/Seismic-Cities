using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Transform cameraContainer;
    public GameObject background;
    public float cameraMovementSpeed = 2;
    public List<Instruction> instructions;

    public GameObject instructionPrefab;
    private GameObject instructionObj;
    private Text instrText;
    private Button skipButton, nextButton;
    private GameObject arrows;

    private int currentInstruction = 0;
    private CamMovement camMovementScript;

    private Vector3 initialCameraPosition;
    private Vector3 newCameraPosition;

    private bool cameraIsMoving = false;

    public void Start()
    {
        camMovementScript = cameraContainer.GetComponent<CamMovement>();
        initialCameraPosition = cameraContainer.position;

        instructionObj = GameObject.Instantiate(instructionPrefab, transform);
        instrText = instructionObj.GetComponentInChildren<Text>();

        Button[] buttons = instructionObj.GetComponentsInChildren<Button>();
        skipButton = buttons[0];
        nextButton = buttons[1];

        skipButton.onClick.AddListener(skipTutorial);
        nextButton.onClick.AddListener(nextInstruction);

        Debug.Log(skipButton.name + ", " + nextButton.name);

        arrows = instructionObj.transform.Find("Arrows").gameObject;

        instructionObj.SetActive(false);
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
        {
            EventManager.StopListening(instructions[currentInstruction].nextEventTrigger, nextInstruction);
        }

        if (currentInstruction < instructions.Count - 1 && currentInstruction >= 0) {
            currentInstruction++;
            showInstruction(instructions[currentInstruction]);
        }
        else {
            camMovementScript.enabled = true;
            cameraIsMoving = false;
            deactivateUI();
            EventManager.TriggerEvent("TutorialSkip");
        }
    }

    public void skipTutorial() {
        EventManager.TriggerEvent("TutorialSkip");
        currentInstruction = instructions.Count-1;
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
        nextButton.GetComponentInChildren<Text>().text = "Next " + (currentInstruction + 1) + "/" + instructions.Count ;
        instrText.text = instruction.text;
        for (int i = 0; i < 4; i++)
            arrows.transform.GetChild(i).gameObject.SetActive(false);
        arrows.transform.GetChild(instruction.arrow).gameObject.SetActive(true);

        

        //instructionObj.GetComponent<RectTransform>()

        if (instruction.focus != null)
        {
            RectTransform focus = instruction.focus.GetComponent<RectTransform>();
            if (focus != null)
            {
                cameraIsMoving = true;
                newCameraPosition = new Vector3(instruction.focus.position.x,
                                                instruction.focus.position.y,
                                                cameraContainer.position.z);
            }
        }


        if (currentInstruction == instructions.Count) {
            cameraIsMoving = true;
            newCameraPosition = initialCameraPosition;
        }
    }
}
