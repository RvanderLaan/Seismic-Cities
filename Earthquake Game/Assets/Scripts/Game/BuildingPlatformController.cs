using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlatformController : MonoBehaviour
{

    public SoilType soilType;
    public bool isBuilt;

    private Rigidbody2D rigidBody;

    public bool isShaking;
    public float startTime = 0;
    private Vector3 startPos;

    public float baseLifeTime = 5;
    public float stopTime = 0;

    private float frequency = 3, amplitude = 0.2f;

    public LayerMask terrainLayer;

    // List<float> startTimes; // for every start time, add base frequency * dT

    // Todo: Frequency build up of multiple waves (using list) and a distance falloff

    // List<Upgrade>

    private float epicenterDistance;

    public enum SoilType {
        Marl, Limestone, Sand, Sandstone, Clay, Bedrock, Quicksand,
    }

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isShaking = false;
        isBuilt = false;
        startPos = transform.position;

        // Get force values for this soil type (defined below)
        float[] parameters = getValues(soilType);
        frequency = parameters[0];
        amplitude = parameters[1];

        // Raycast downwards to terrain and assign joint anchor
        RaycastHit2D terrainHit = Physics2D.Raycast(transform.position + Vector3.up * 10, -Vector2.up, float.MaxValue, terrainLayer);
        if (terrainHit.collider != null) {
            Vector2 hitPoint = terrainHit.point;
            string terrainName = terrainHit.collider.gameObject.name;
            
            // Try to automatically set soil type based on terrain name

            string[] typeNames = Enum.GetNames(typeof (SoilType));
            Array typeVals     = Enum.GetValues(typeof(SoilType));
            for (int i = 0; i < typeNames.Length; i++) {
                if (terrainName.Equals(typeNames[i])) {
                    soilType = (SoilType) typeVals.GetValue(i);
                }
            }

            gameObject.name = gameObject.name + " of " + soilType + " on " + terrainHit.collider.gameObject.name;

            if (!soilType.ToString().Equals(terrainName)) {
                Debug.Log("WARNING: Soil type property name of building zone do not equal terrain name: " + soilType + " != " + terrainHit.collider.gameObject.name);
            }
        } else {
            Debug.Log("WARNING: Terrain not found of building platform");
        }
    }

    void FixedUpdate() {
        if (isShaking) {
            if (Time.time > stopTime) {
                isShaking = false;
                return;
            }

            // Shake manually around start pos
            float dT = (Time.time - startTime);
            float t = (dT / (stopTime - startTime)); // Time factor: 0 at start, 1 at end of shaking
            

            float actualAmplitude = Mathf.SmoothStep(amplitude, 0, t) / (epicenterDistance / 100);

            float offset = Mathf.Cos(dT * Mathf.PI * 2 * frequency) * actualAmplitude * 2;

            rigidBody.MovePosition(transform.position - Vector3.right * offset);
        }
    }

    public void startShaking(float cosineDegreeFactor, float distance, float intensity) {
        stopTime = (Time.time + baseLifeTime);
        if (!isShaking) {
            startTime = Time.time;
            isShaking = true;
            epicenterDistance = distance;
        } 
    }

    float[] getValues(SoilType type) {
        float[] values = new float[2];

        // Values are: Frequency, amplitude
        switch (type) {
            case SoilType.Bedrock: {
                    values[0] = 4;
                    values[1] = .05f;
                    break;
                }
            case SoilType.Marl: {
                    values[0] = 4f;
                    values[1] = .05f;
                    break;
                }
            case SoilType.Limestone: {
                    values[0] = 3;
                    values[1] = .1f;
                    break;
                }
            case SoilType.Sand: {
                    values[0] = 2;
                    values[1] = .15f;
                    break;
                }
            case SoilType.Sandstone: {
                    values[0] = 4;
                    values[1] = .1f;
                    break;
                }
            case SoilType.Clay: {
                    values[0] = 3;
                    values[1] = .3f;
                    break;
                }
            case SoilType.Quicksand: {
                    values[0] = 2;
                    values[1] = .15f;
                    break;
                }
            default: {
                    values[0] = 0;
                    values[1] = 0;
                    break;
                }
        }
        return values;
    }
}
