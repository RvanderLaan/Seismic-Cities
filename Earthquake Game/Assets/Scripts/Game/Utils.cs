using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour {

    public void Restart() {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void activateGameObject(GameObject go)
    {
        go.SetActive(true);
    }

    public void openURL(string url) {
        Application.OpenURL(url);
    }


    public void setLanguage(int lan) {
        PlayerPrefs.SetInt("Language", (int) lan);

        TextInserter[] textInserters = Object.FindObjectsOfType<TextInserter>();
        foreach (TextInserter ti in textInserters)
            ti.updateText();
    }
}
