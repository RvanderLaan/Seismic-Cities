using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlatformController : MonoBehaviour
{

    public float defaultUpForce;
    public float defaultSideForce;

    public bool isBuilt;

    private float upForce, sideForce;

    private Rigidbody2D rigidBody;

    private bool isShaking;
    private int shakeTimes;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isShaking = false;
        isBuilt = false;
        //StartCoroutine("shake");
    }

    public void startShaking(float cosineDegreeFactor, float distance, float intensity)
    {        
        shakeTimes = 0;
        sideForce = defaultSideForce * cosineDegreeFactor * intensity / distance * 10;
        upForce = defaultUpForce * cosineDegreeFactor * intensity / distance * 10;

        if (!isShaking)
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
}
