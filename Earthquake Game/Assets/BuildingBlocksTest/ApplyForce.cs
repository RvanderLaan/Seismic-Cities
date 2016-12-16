using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour {

    public float upForce;
    public float sideForce;

    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.left * 60, ForceMode2D.Impulse);
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * 60, ForceMode2D.Impulse);
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine("shake");
    }
	
	// Update is called once per frame
	void Update () {
    }

    IEnumerator shake()
    {
        int i = 0;
        while(true)
        {
            rigidBody.AddForce(Vector2.up * upForce * Random.Range(0.8f, 1), ForceMode2D.Impulse);
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
    }
}
