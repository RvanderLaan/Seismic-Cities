using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public float intensity = 200;

    public BuildingType type;
    public enum BuildingType { None, StoneHouse, WoodenHouse, Flat, Skyscraper, Thematic, Any }

    Rigidbody2D[] children;

    private bool sinking = false, collapsing = false;

    public AudioClip[] collapseSounds, sinkingSounds;

    // Use this for initialization
    void Start () {
		children = GetComponentsInChildren<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void Collapse() {
        Debug.Log("Collapsing " + gameObject.name);
        if (!collapsing) {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = collapseSounds[Random.Range(0, collapseSounds.Length)];
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.Play();
            collapsing = true;
        }        

        for (int i = 0; i < children.Length; i++) {
            children[i].bodyType = RigidbodyType2D.Dynamic;
            children[i].AddForce(new Vector2((Random.Range(1, 3) * 2 - 3), 1) * intensity);
        }
    }

    public void Sink() {
        if (!sinking) {
            sinking = true;
            StartCoroutine(SinkMovement(transform, transform.position + Vector3.down * 2, Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.forward), 10));

            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = sinkingSounds[Random.Range(0, sinkingSounds.Length)];
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.Play();
        }
    }

    public IEnumerator SinkMovement(Transform transform, Vector3 position, Quaternion rotation, float timeToMove) {
        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;
        float t = 0f;
        while (t < 1) {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            transform.rotation = Quaternion.Slerp(currentRot, rotation, t);
            yield return null;
        }
    }

    public static bool shouldCollapse(BuildingType bt, Upgrade.UpgradeType ut, Soil.SoilType st, float distance, float magnitude) {
        if (bt == BuildingType.StoneHouse && st == Soil.SoilType.Clay)
            return true;
        return false;
    }

    public static bool shouldSink(BuildingType bt, Upgrade.UpgradeType ut, Soil.SoilType st, float distance, float magnitude) {
        if (bt == BuildingType.StoneHouse && st == Soil.SoilType.Sand)
            return true;

        return false;
    }
}
