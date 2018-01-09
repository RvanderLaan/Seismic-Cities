using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartTimer : MonoBehaviour {

    public int inactivityTimeOut = 120;
    public int countdown = 30;

    private float inactiveTime= 0;

    public Text timerText;

    private Image background;

	// Use this for initialization
	void Start () {
        background = GetComponent<Image>();
        Reset();
	}
	
	void Update () {
        inactiveTime += Time.deltaTime;

        // If timer reaches zero, start screensaver
        if (inactiveTime > inactivityTimeOut)
        {
            if (!background.enabled)
                Activate();
            int timeLeft = (int) (countdown + inactivityTimeOut - inactiveTime );
            timerText.text = timeLeft + "";

            if (inactiveTime > inactiveTime + countdown)
            {
                Reset();
                GoToMenu();
            }
        }

        if (Input.GetMouseButtonDown(0))
            Invoke("Reset", 0.2f);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Level Selection Menu");
    }
   
    public void Reset()
    {
        inactiveTime = 0.0f;
        background.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Activate()
    {
        background.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }


}
