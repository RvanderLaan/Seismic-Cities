using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public float intensity = 200;

    public BuildingType type;
    public enum BuildingType { None, StoneHouse, WoodenHouse, Flat, Skyscraper, Thematic }

    Rigidbody2D[] children;

    // Use this for initialization
    void Start () {
		children = GetComponentsInChildren<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void Collapse() {
        Debug.Log("Collapsing " + gameObject.name);
        for (int i = 0; i < children.Length; i++) {
            children[i].bodyType = RigidbodyType2D.Dynamic;
            children[i].AddForce(new Vector2((Random.Range(1, 3) * 2 - 3), 1) * intensity);
        }
    }
}
