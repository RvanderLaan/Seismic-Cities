using UnityEngine;
using System.Collections;

public class DamageParticleController : MonoBehaviour {

    public int platformID;
    //Factor used in the damage computation. The damage is maximum if the direction of the wave is vertical,
    //so orthogonal to the ground where the building is placed
    public float cosineDegreeFactor;
    public float distance;

    public float intensity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        /*
        if (collider.gameObject.tag == "Building" && collider.gameObject.GetInstanceID() == buildingID)
        {
            float speed = gameObject.GetComponent<Wave>().speed;
            collider.gameObject.GetComponent<BuildingHealth>().takeDamage(cosineDegreeFactor * 100 / speed / distance);
            Destroy(gameObject);
        }
        */

        Debug.Log("Collliding: " + collider.gameObject.name);

        if (collider.gameObject.CompareTag("BuildingPlatform") && collider.gameObject.GetInstanceID() == platformID)
        {
            collider.gameObject.GetComponent<BuildingPlatformController>().startShaking();
            Destroy(gameObject);
        }
    }
}
