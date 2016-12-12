using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeSimulator : MonoBehaviour {

    public float intervalBetweenWaves;
    public int numberOfWaves;
    public float intensityOfWaves;

    public TargetController targetController;

    // Use this for initialization
    void Start () {
		
	}

    public void simulateEarthquake()
    {
        GetComponent<BuildingPlacer>().enabled = false;
        StartCoroutine("SpawnWaves");
    }

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < numberOfWaves; i++)
        {
            //spawn a wave
            targetController.createWave(Random.Range(0.5f, 1f));
            yield return new WaitForSeconds(intervalBetweenWaves);
        }
        yield return new WaitForSeconds(3);
        GetComponent<ScoringManager>().displayScore();
        GetComponent<BuildingPlacer>().enabled = true;
    }
}
