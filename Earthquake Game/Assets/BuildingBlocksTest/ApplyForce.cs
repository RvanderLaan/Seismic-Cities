using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyForce : MonoBehaviour {

    public float upForce;
    public float sideForce;

    private Rigidbody2D rigidBody;

    public Text sideText, upText;

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.left * 60, ForceMode2D.Impulse);
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * 60, ForceMode2D.Impulse);
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine("shake");
    }

    public void setUpForce(float value) {
        upText.text = value + "";
        upForce = value;
    }
    public void setSideForce(float value) {
        sideText.text = value + "";
        sideForce = value;
    }
    public void restart() {
        Application.LoadLevel(Application.loadedLevel);
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
