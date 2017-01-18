using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        instance = this;
        DontDestroyOnLoad(this.gameObject);
	}
}
