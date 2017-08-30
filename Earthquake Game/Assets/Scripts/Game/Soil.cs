using UnityEngine;
using System.Collections;

public class Soil : MonoBehaviour {

    public enum SoilType { Bedrock, Rock, SoftRock, Clay, Sand };

    public SoilType type = SoilType.Bedrock;

    private float speedCoefficient = 1f;

	// Use this for initialization
	void Awake () {
        switch (type) {
            case SoilType.Bedrock:
                speedCoefficient = 1.5f;
                break;
            case SoilType.Rock:
                speedCoefficient = 1.25f;
                break;
            case SoilType.SoftRock:
                speedCoefficient = 1f;
                break;
            case SoilType.Clay:
                speedCoefficient = 0.75f;
                break;
            case SoilType.Sand:
                speedCoefficient = 0.5f;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Wave wave = collider.gameObject.GetComponent<Wave>();
        if (wave != null) {
            wave.speed *= speedCoefficient;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Wave wave = collider.gameObject.GetComponent<Wave>();
        if (wave != null) {
            wave.speed /= speedCoefficient;
        }
    }
}
