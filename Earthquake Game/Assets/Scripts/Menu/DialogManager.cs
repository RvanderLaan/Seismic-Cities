using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public bool showTutorialAfterDialog = false;

    public GameObject poseidonImage;
    public GameObject aphroditeImage;
    public GameObject dialogPanel;
    public Text characterName, dialogText;
    //public GameObject levelFailedPrefab;
    //public GameObject levelPassedPrefab;

    private int currentDialogItem = 0;

    private List<DialogItem> currentList;
    public List<DialogItem> dialogList;
    public List<DialogItem> levelFailedList;
    public List<DialogItem> levelPassedList;


    // Use this for initialization
    void Start () {
        currentList = dialogList;
        showDialog();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            levelSuccessfull();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            levelFailed();
    }

    public void showDialog()
    {
        if (currentDialogItem < currentList.Count)
        {
            dialogPanel.SetActive(true);
            fillDialog(currentList[currentDialogItem++]);
        } else
        {
            dialogPanel.SetActive(false);
            poseidonImage.SetActive(false);
            aphroditeImage.SetActive(false);
            
            if (showTutorialAfterDialog)
            {
                GetComponent<TutorialManager>().startTutorial();
            }
        }
    }

    public void levelSuccessfull()
    {
        currentList = levelPassedList;
        currentDialogItem = 0;
        showDialog();
    }

    public void levelFailed()
    {
        currentList = levelFailedList;
        currentDialogItem = 0;
        showDialog();
    }


    private void fillDialog(DialogItem item)
    {
        //TODO: make the images come in from the sides with an animation
        if (item.characterName == DialogItem.CharacterName.Poseidon)
        {
            poseidonImage.SetActive(true);
            aphroditeImage.SetActive(false);
        } else
        {
            poseidonImage.SetActive(false);
            aphroditeImage.SetActive(true);
        }
        characterName.text = item.characterName.ToString();
        dialogText.text = item.dialogText;
    }
}
