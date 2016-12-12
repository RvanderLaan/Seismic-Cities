using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.left * 60, ForceMode2D.Impulse);
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * 60, ForceMode2D.Impulse);
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
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 30 * Random.Range(0.8f, 1), ForceMode2D.Impulse);
            if (i == 0)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * 40 * Random.Range(0.8f, 1), ForceMode2D.Impulse);
                i = 1;
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * 40 * Random.Range(.8f, 1), ForceMode2D.Impulse);
                i = 0;
            }
            yield return new WaitForSeconds(.2f);
        }
    }
}
