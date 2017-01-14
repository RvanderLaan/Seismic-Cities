using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public bool showTutorialAfterDialog = false;

    public GameObject background;
    public GameObject skipButton;
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
        
    }

    public void showDialog()
    {
        if (currentDialogItem < currentList.Count)
        {
            currentList[currentDialogItem].enterEvent.Invoke();
            background.SetActive(true);
            skipButton.SetActive(true);
            dialogPanel.SetActive(true);
            fillDialog(currentList[currentDialogItem]);
        } else
        {
            deactivateUI();
            
            //if (showTutorialAfterDialog)
            //{
            //    GetComponent<TutorialManager>().startTutorial();
            //}
        }
        if (currentDialogItem - 1 >= 0)
            currentList[currentDialogItem - 1].leaveEvent.Invoke();

        currentDialogItem++;
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

    public void skipDialog()
    {
        currentDialogItem = currentList.Count;
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

    private void deactivateUI()
    {
        background.SetActive(false);
        skipButton.SetActive(false);
        dialogPanel.SetActive(false);
        poseidonImage.SetActive(false);
        aphroditeImage.SetActive(false);
    }
}
