using UnityEngine;
using System.Collections;

public class WaveGeneratorGenerator : MonoBehaviour {

    public WaveGenerator instance;
    public StrengthController sc;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            WaveGenerator wg = (WaveGenerator) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            wg.startSpeed = sc.getTimingScore() * wg.startSpeed * .8f + wg.startSpeed * .2f;
            // wg.transform.SetParent(transform);
         }
    }
}
