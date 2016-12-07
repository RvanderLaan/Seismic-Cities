using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeSimulator : MonoBehaviour {

    public float intervalBetweenWaves;
    public int numberOfWaves;
    public float intensityOfWaves;

    public GameObject epicenter;
    public WaveGenerator waveGenerator;

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
            WaveGenerator wg = Instantiate(waveGenerator, epicenter.transform.position, Quaternion.identity);
            wg.startSpeed = 1;
            yield return new WaitForSeconds(intervalBetweenWaves);
        }
        yield return new WaitForSeconds(3);
        GetComponent<ScoringManager>().displayScore();
        GetComponent<BuildingPlacer>().enabled = true;
    }
}
