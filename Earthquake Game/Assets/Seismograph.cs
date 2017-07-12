using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seismograph : MonoBehaviour {

    public float transitionTime = 8;
    private bool moving = false;

    public RectTransform mover;
    private float startTime;

    private RectTransform wrapper;

    /*
     * Variables:
     * - Texture (rectengular)
     * - Position of graph marker (moves in world)
     * 
     * Input:
     * - Soil type
     * - Distance to center
     * 
     * Output:
     * - Squiggly sine wave (random)
     */

    private Vector3 earthquakeOrigin;

    private Texture2D graphTexture;
    private float prevX, prevY;

    public Color lineColor = Color.black;
    public Texture2D graphBackground;

    float[] signature = { .02f, .04f, .03f, 1f, .42f, .25f, .15f, .32f, .12f, .22f, .06f, .23f, .15f, .18f, .13f, .23f, .05f, .17f, .29f, 0.05f };

    private float distanceIntensity = 1;

	// Use this for initialization
	void Start () {
        EventManager.StartListening("SimulateEarthquake", StartMoving);
        // Todo: Init texture size, ~2000 x 500?

        graphTexture = new Texture2D(1500, 500);
        GetComponent<RawImage>().texture = graphTexture;
        prevX = 0;
        prevY = graphTexture.height / 2;
    }

    private void DrawLine(Texture2D tex, Vector2 p1, Vector2 p2, Color col)
    {
        Vector2 t = p1;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
        {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            tex.SetPixel((int)t.x, (int)t.y, col);
        }
    }

    void drawSeismograph(float t, float intensity, float dt)
    {
        int sigIndex = (int)(t * (signature.Length - 1));
        float sigLerp = signature[sigIndex];
        // If difference in intensity is > 0.8, jump to new intensity instead of lerping
        if (signature[sigIndex] - signature[sigIndex + 1] < 0.8f)
            Mathf.Lerp(signature[sigIndex], signature[sigIndex + 1], (signature.Length - 1) * (t %  (1 / (float) (signature.Length -1 ))));
        float ampl = sigLerp * intensity * graphTexture.height / 2;

        float x = graphTexture.width * t;
        float y = (Mathf.Sin(t / dt / 2) + Mathf.Sin(t / dt / 1.3f)) * ampl / 2f + graphTexture.height / 2;
        DrawLine(graphTexture, new Vector2(prevX, prevY), new Vector2(x, y), lineColor);
        

        prevX = x;
        prevY = y;
    }

    // Update is called once per frame
    void Update () {
		if (moving)
        {
            float d = (Time.time - startTime) / transitionTime;
            if (d < 1)
            {
                float dT = 1 / (float)graphTexture.width;
                drawSeismograph(d, distanceIntensity, dT);

                graphTexture.Apply();
            } else
            {
                moving = false;
                // wrapper.gameObject.SetActive(false);
            }
        }
	}

    public void StartMoving()
    {
        Debug.Log(Vector3.SqrMagnitude(transform.position - earthquakeOrigin));
        earthquakeOrigin = GameObject.Find("EpicenterTarget").transform.position;
        distanceIntensity = 1 - Mathf.Clamp(((Vector3.SqrMagnitude(transform.position - earthquakeOrigin) - 5000) / 15000f), 0, 0.9f);

        // wrapper.gameObject.SetActive(true);
        moving = true;
        startTime = Time.time;
    }
}
