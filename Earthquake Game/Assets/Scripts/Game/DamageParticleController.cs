using UnityEngine;
using System.Collections;

public class DamageParticleController : MonoBehaviour {

    public int targetID;
    //Factor used in the damage computation. The damage is maximum if the direction of the wave is vertical,
    //so orthogonal to the ground where the building is placed
    public float cosineDegreeFactor;
    public float distance;

    public float intensity;

    public GameObject target;

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
        if (collider.gameObject.CompareTag("BuildingPlatform") && collider.gameObject.GetInstanceID() == targetID)
        {
            collider.gameObject.GetComponent<BuildingZone>().startShaking();
            Destroy(gameObject);
        }
        else if (collider.gameObject.CompareTag("Seismograph") && collider.gameObject.GetInstanceID() == targetID)
        {
            collider.gameObject.GetComponentInChildren<Seismograph>().StartMoving();
            Destroy(gameObject);
        }
    }
}
