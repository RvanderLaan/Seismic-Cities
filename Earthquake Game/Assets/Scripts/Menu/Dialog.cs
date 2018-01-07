using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

    public bool showTutorialAfterDialog = false;

    public GameObject background;
    public GameObject skipButton;
    public GameObject imageLeft, imageRight;
    public GameObject dialogPanel;
    public Text characterName, dialogText;
    //public GameObject levelFailedPrefab;
    //public GameObject levelPassedPrefab;

    private int currentDialogItem = 0;

    public List<DialogItem> dialogList;

    private CamMovement camMovementScript;


    // Use this for initialization
    void Start () {
        camMovementScript = Camera.main.GetComponentInParent<CamMovement>();
    }

    public void showDialog()
    {
        Debug.Log("ShowDialog");
        if (currentDialogItem < dialogList.Count)
        {
            camMovementScript.enabled = false;
            dialogList[currentDialogItem].enterEvent.Invoke();
            background.SetActive(true);
            skipButton.SetActive(true);
            dialogPanel.SetActive(true);
            fillDialog(dialogList[currentDialogItem]);
        } else
        {
            deactivateUI();
            camMovementScript.enabled = true;
            
            //if (showTutorialAfterDialog)
            //    GetComponent<Tutorial>().startTutorial();
        }
        if (currentDialogItem - 1 >= 0)
            dialogList[currentDialogItem - 1].leaveEvent.Invoke();

        currentDialogItem++;
    }

    public void skipDialog()
    {
        currentDialogItem = dialogList.Count;
        showDialog();
    }


    private void fillDialog(DialogItem item)
    {
        if (item.characterName == CharacterName.AdmiraalMineraal)
        {
            imageLeft.SetActive(true);
            imageRight.SetActive(false);
        } else
        {
            imageLeft.SetActive(false);
            imageRight.SetActive(true);
        }
        characterName.text = AddSpacesToSentence(item.characterName.ToString());
        dialogText.GetComponent<TextInserter>().reset(item.dialogText);
    }

    public void deactivateUI()
    {
        background.SetActive(false);
        skipButton.SetActive(false);
        dialogPanel.SetActive(false);
        imageLeft.SetActive(false);
        imageRight.SetActive(false);
        currentDialogItem = 0;
    }

    string AddSpacesToSentence(string text)
    {
        string res = "";
        foreach (char x in text)
            res += char.IsUpper(x) ? " " + x : x.ToString();
        return res.TrimStart(' '); ;
    }
}
