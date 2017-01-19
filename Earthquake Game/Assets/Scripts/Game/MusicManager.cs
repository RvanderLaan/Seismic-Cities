using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;


    public void Start()
    {
        
    }

	void Awake () {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;

        }

        SceneManager.sceneLoaded += NewSceneLoaded;
        instance = this;
        DontDestroyOnLoad(this.gameObject);
	}

    private void NewSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name.Equals("Level Selection Menu"))
        {
            if (this != null)
                Destroy(this.gameObject);
            if (instance != null)
                Destroy(instance.gameObject);
            return;
        }
    }
}
