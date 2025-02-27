﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public Wave damageParticle;

    public Color bestColor = Color.red, worstColor = Color.blue;

    private Color renderColor;

    private List<DamageParticleController> damageParticles = new List<DamageParticleController>();

    // Use this for initialization
    void Awake() {

        lr = GetComponent<LineRenderer>();
        lr.positionCount = waveAmount + 1;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        renderColor = lr.material.color;

        //generate the wave objects
        waves = new Wave[waveAmount];
        for (int i = 0; i < waveAmount; i++) {
            float angle = Mathf.PI * 2 * i / waveAmount - Mathf.PI / 2; // + 45 deg so that the seam is at the bottom
            instantiateWaveObj(i, angle);
        }

        //get the buildings gameObjects
        // platforms = GameObject.FindGameObjectsWithTag("BuildingPlatform");

        // Attempt at better distribution of wave objects
        // Generate more objects to the top left and top right, and very few to the bottom
        /*
        int quarter = waveAmount / 4;
        // One quarter of the objects on the bottom half (but the seam should be in at the bottom of the circle, so start at the bottom right quarter with 1/8)
        for (int i = 0; i < quarter / 2; i++)
            instantiateWaveObj(i, Mathf.PI / 2 * i / (quarter / 2) - Mathf.PI / 2);
        // One quarter at the top right 1/8th of the circle (0 to PI / 4)
        for (int i = 0; i < quarter; i++)
            instantiateWaveObj(i + quarter / 2, Mathf.PI / 4 * i / quarter);
        // One quarter at the top 1/4th of the circle (PI / 4 to 3/4 PI)
        for (int i = 0; i < quarter; i++)
            instantiateWaveObj(i + quarter + quarter / 2, Mathf.PI / 2 * i / quarter + Mathf.PI / 4);
        // One quarter at the top left 1/8th of the circle (3/4 PI to PI)
        for (int i = 0; i < quarter; i++)
            instantiateWaveObj(i + 2 * quarter + quarter / 2, Mathf.PI / 4 * i / quarter + Mathf.PI * 3/4);
        // And finally the last 1/8th of the circle at the bottom left
        for (int i = 0; i < quarter / 2; i++)
            instantiateWaveObj(i + 3 * quarter + quarter / 2, Mathf.PI / 2 * i / (quarter / 2) + Mathf.PI);
        */

    }

    void instantiateWaveObj(int i, float angle) {
        Wave w = (Wave)GameObject.Instantiate(instance, transform.position, Quaternion.identity);
        w.transform.SetParent(transform);
        w.direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
        w.speed = startSpeed;
        waves[i] = w;
    }

    public void reset() {
        // Reset positions
        for (int i = 0; i < waveAmount; i++) {
            waves[i].transform.position = transform.position;
            waves[i].speed = startSpeed;
            lr.SetPosition(i, transform.position);
        }
        lr.SetPosition(waveAmount, waves[0].transform.position);

        foreach (DamageParticleController dpg in damageParticles)
            if (dpg != null && dpg.gameObject != null)
                Destroy(dpg.gameObject);
        damageParticles.Clear();

    }

    public bool isDone() {
        if (gameObject.activeSelf)
            return false;
        return Time.time > (startTime + lifeTime);
    }

    public void startWave(GameObject[] targets) {
        startTime = Time.time;

        // Change color based on intensity
        Color col = Color.Lerp(worstColor, bestColor, intensity);
        GetComponent<Renderer>().material.color = col;
        renderColor = col;
        gameObject.SetActive(true);

        shootHomingParticles(targets);
    }

    public void shootHomingParticles(GameObject[] targets)
    {
        //generate the damaging particles, one for each target
        for (int i = 0; i < targets.Length; i++)
        {
            Wave particle = (Wave) GameObject.Instantiate(damageParticle, transform.position, Quaternion.identity);
            particle.transform.SetParent(transform);

            Vector3 direction = (targets[i].transform.position - transform.position).normalized;
            direction.z = 0;
            particle.direction = direction;
            particle.speed = startSpeed;

            DamageParticleController dpg = particle.GetComponent<DamageParticleController>();
            dpg.targetID = targets[i].GetInstanceID();
            //the cosineDegreeFactor is 1 if the direction of the damage particle is vertical (orthogonal to the ground)
            //and is 0 if the direction is horizontal
            dpg.cosineDegreeFactor = 1; //  Mathf.Pow(particle.direction.y, 0.5f);
            dpg.distance = (targets[i].transform.position - transform.position).magnitude;
            dpg.intensity = intensity;
            dpg.target = targets[i].gameObject;

            damageParticles.Add(dpg);
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

        renderColor.a = Mathf.SmoothStep(1, 0, (Time.time - startTime) / lifeTime);
        lr.material.color = renderColor;
	}
}
