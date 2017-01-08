using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlatformController : MonoBehaviour
{

    public SoilType soilType;
    public bool isBuilt;
    public LayerMask terrainLayer;

    private float[] defaultForces;
    private float upForce, sideForce;
    private Rigidbody2D rigidBody;

    private bool isShaking;
    private int shakeTimes;

    public enum SoilType {
        Marl, Limestone, Sand, Sandstone, Clay, Bedrock, Quicksand,
    }



    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isShaking = false;
        isBuilt = false;

        // Get force values for this soil type (defined below)
        defaultForces = getValues(soilType);

        // Raycast downwards to terrain and assign joint anchor
        RaycastHit2D terrainHit = Physics2D.Raycast(transform.position + Vector3.up * 10, -Vector2.up, float.MaxValue, terrainLayer);
        if (terrainHit.collider != null) {
            Vector2 hitPoint = terrainHit.point;
            
            Joint2D joint = GetComponent<Joint2D>();
            joint.connectedBody = terrainHit.collider.GetComponent<Rigidbody2D>();

            gameObject.name = gameObject.name + " of " + soilType + " on " + terrainHit.collider.gameObject.name;
            if (!soilType.Equals(terrainHit.collider.gameObject.name)) {
                Debug.Log("WARNING: Soil type property name of building zone do not equal terrain name: " + soilType + " != " + terrainHit.collider.gameObject.name);
            }
        } else {
            Debug.Log("WARNING: NO RIGID BODY FOUND TO ANCHOR THIS BUILDING PLATFORM TO!");
        }
    }

    public void startShaking(float cosineDegreeFactor, float distance, float intensity)
    {        
        shakeTimes = 0;
        sideForce = defaultForces[0] * cosineDegreeFactor * intensity / distance;
        upForce = defaultForces[1] * cosineDegreeFactor * intensity / distance;

        Debug.Log(sideForce + ", " + upForce);

        // if (!isShaking)
        {
            isShaking = true;
            StartCoroutine("shake");
        }
    }

    IEnumerator shake()
    {
        int i = 0;
        for (shakeTimes = 0; shakeTimes < 10; shakeTimes++)
        {
            //rigidBody.AddForce(Vector2.up * upForce * Random.Range(0.8f, 1), ForceMode2D.Impulse);
            if (i == 0)
            {
                rigidBody.AddForce(Vector2.left * sideForce * Random.Range(0.8f, 1), ForceMode2D.Impulse);
                i = 1;
            }
            else
            {
                rigidBody.AddForce(Vector2.right * sideForce * Random.Range(.8f, 1), ForceMode2D.Impulse);
                i = 0;
            }
            yield return new WaitForSeconds(.2f);
        }
        isShaking = false;
    }

    float[] getValues(SoilType type) {
        float[] values = new float[2];

        switch (type) {
            case SoilType.Bedrock: {
                    values[0] = 1;
                    values[1] = 0;
                    break;
                }
            case SoilType.Marl: {
                    values[0] = 1;
                    values[1] = 0;
                    break;
                }
            case SoilType.Limestone: {
                    values[0] = 1;
                    values[1] = 0;
                    break;
                }
            case SoilType.Sand: {
                    values[0] = 1;
                    values[1] = 0;
                    break;
                }
            case SoilType.Sandstone: {
                    values[0] = 1;
                    values[1] = 0;
                    break;
                }
            case SoilType.Clay: {
                    values[0] = 1;
                    values[1] = 0;
                    break;
                }
            case SoilType.Quicksand: {
                    values[0] = 1;
                    values[1] = 0;
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
