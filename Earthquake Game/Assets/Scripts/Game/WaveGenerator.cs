using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

    public float lifeTime = 15;
    private float startTime = float.MinValue;

    public int waveAmount = 128;
    public float startSpeed = .1f;
    public Wave instance;
    private Wave[] waves;

    public float intensity = 1;

    LineRenderer lr;
    public float lineWidth = 0.05f;

    private GameObject[] platforms;
    public Wave damageParticle;

    public Color bestColor = Color.red, worstColor = Color.blue;

    // Use this for initialization
    void Awake() {
        

        lr = GetComponent<LineRenderer>();
        lr.numPositions = waveAmount + 1;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        //generate the wave objects
        waves = new Wave[waveAmount];
        for (int i = 0; i < waveAmount; i++) {
            Wave w = (Wave) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            w.transform.SetParent(transform);
            float angle = Mathf.PI * 2 * i / waveAmount - Mathf.PI / 2; // + 45 deg so that the seam is at the bottom
            w.direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
            w.speed = startSpeed;
            waves[i] = w;
        }
	}

    public void reset() {
        // Reset positions
        for (int i = 0; i < waveAmount; i++) {
            waves[i].transform.position = transform.position;
            waves[i].speed = startSpeed;
            lr.SetPosition(i, transform.position);
        }
        lr.SetPosition(waveAmount, waves[0].transform.position);
    }

    public bool isDone() {
        return Time.time > (startTime + lifeTime);
    }

    public void startWave() {
        //get the buildings gameObjects
        platforms = GameObject.FindGameObjectsWithTag("BuildingPlatform");

        startTime = Time.time;

        // Change color based on intensity
        Color col = Color.Lerp(worstColor, bestColor, intensity);
        GetComponent<Renderer>().material.color = col;

        gameObject.SetActive(true);
        //generate the damaging particles, one for each building
        for (int i = 0; i < platforms.Length; i++) {
            Wave particle = (Wave)GameObject.Instantiate(damageParticle, transform.position, Quaternion.identity);
            particle.transform.SetParent(transform);
            particle.direction = (platforms[i].transform.position - transform.position).normalized;
            particle.speed = startSpeed;

            DamageParticleController dpg = particle.GetComponent<DamageParticleController>();
            dpg.platformID = platforms[i].GetInstanceID();
            //the cosineDegreeFactor is 1 if the direction of the damage particle is vertical (orthogonal to the ground)
            //and is 0 if the direction is horizontal
            dpg.cosineDegreeFactor = Mathf.Pow(particle.direction.y, 0.7f);
            dpg.distance = (platforms[i].transform.position - transform.position).magnitude;
            dpg.intensity = intensity;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isDone())
            gameObject.SetActive(false);
        for (int i = 0; i < waveAmount; i++) {
            lr.SetPosition(i, waves[i].transform.position);
        }
        lr.SetPosition(waveAmount, waves[0].transform.position);
	}
}
