using UnityEngine;
using System.Collections;

public class Soil : MonoBehaviour {

    public float speedCoefficient = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider) {
        Wave wave = collider.gameObject.GetComponent<Wave>();
        if (wave != null) {
            wave.speed *= speedCoefficient;
        }
        
    }

    void OnTriggerExit2D(Collider2D collider) {
        Wave wave = collider.gameObject.GetComponent<Wave>();
        if (wave != null) {
            wave.speed /= speedCoefficient;

            if (!collider.gameObject.tag.Equals("DamageParticle") && (gameObject.tag.Equals("Bottom") || gameObject.tag.Equals("Surface"))) {
                wave.gameObject.SetActive(false);
            }
        }        
    }
}
