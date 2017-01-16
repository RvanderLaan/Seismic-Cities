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
        Debug.Log("aaaa");
        go.SetActive(true);
    }
}
