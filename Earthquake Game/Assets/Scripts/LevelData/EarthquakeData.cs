using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EarthquakeData {

    public float intervalBetweenWaves = 1;
    public int numberOfWaves = 5;
    public float intensityOfWaves = 1;

    public float shakeMagnitude = 0.8f, 
                 shakeRoughness = 5, 
                 shakeFadeIn = 3, 
                 shakeFadeOut = 15;

    public Vector2 position = new Vector2(128, -96);
}
