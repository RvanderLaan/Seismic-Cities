using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountryDetailsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void cancel()
    {
        gameObject.SetActive(false);
    }

    public void play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
