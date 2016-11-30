using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

    public int waveAmount = 128;
    public float startSpeed = .1f;
    public Wave instance;
    private Wave[] waves;

    LineRenderer lr;
    public float lineWidth = 0.05f;

    private GameObject[] buildings;
    public Wave damageParticle;

    // Use this for initialization
    void Start() {
        //get the buildings gameObjects
        buildings = GameObject.FindGameObjectsWithTag("Building");

        lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(waveAmount + 1);

        //generate the wave objects
        waves = new Wave[waveAmount];
        for (int i = 0; i < waveAmount; i++) {
            Wave w = (Wave) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            w.transform.SetParent(transform);
            w.direction = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / waveAmount), Mathf.Sin(Mathf.PI * 2 * i / waveAmount), 0).normalized;
            w.speed = startSpeed;
            waves[i] = w;
        }

        //generate the damaging particles, one for each building
        for (int i = 0; i < buildings.Length; i++)
        {
            Wave particle = (Wave) GameObject.Instantiate(damageParticle, transform.position, Quaternion.identity);
            particle.transform.SetParent(transform);
            particle.direction = (buildings[i].transform.position - transform.position).normalized;
            particle.speed = startSpeed;
            particle.GetComponent<DamageParticleController>().buildingID = buildings[i].GetInstanceID();
            //the cosineDegreeFactor is 1 if the direction of the damage particle is vertical (orthogonal to the ground)
            //and is 0 if the direction is horizontal
            particle.GetComponent<DamageParticleController>().cosineDegreeFactor = Mathf.Pow(particle.direction.y, 2);
        }

	}
	
	// Update is called once per frame
	void Update () {

        lr.SetWidth(lineWidth, lineWidth);
        for (int i = 0; i < waveAmount; i++) {
            lr.SetPosition(i, waves[i].transform.position);
        }
        lr.SetPosition(waveAmount, waves[0].transform.position);
	}
}
