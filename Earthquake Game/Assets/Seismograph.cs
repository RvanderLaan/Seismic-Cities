using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seismograph : MonoBehaviour {

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

    public float transitionTime = 8;
    private bool moving = false;

    private float startTime;

    private RectTransform pointer;

    private Vector3 earthquakeOrigin;

    private Texture2D graphTexture;
    private float prevX, prevY;

    public Color lineColor = Color.black, fillColor = Color.white, gridColor = Color.blue;
    private RawImage rawImage;
    public Texture2D graphBackground;
    public float xOffset = 0.3f;

    float[] signature = { .33f, 1f, .42f, .25f, .15f, .92f, .12f, .22f, .56f, .23f, .15f, .28f, .13f, .23f, .05f, .17f, .29f, 0.05f, .02f, .04f, };

    private float distanceIntensity = 1;

	// Use this for initialization
	void Start () {
        // EventManager.StartListening("SimulateEarthquake", StartMoving);
        // Todo: Init texture size, ~2000 x 500?

        graphTexture = new Texture2D(600, 150, TextureFormat.RGB24, true);
        rawImage = transform.parent.GetComponentInChildren<RawImage>();
        rawImage.texture = graphTexture;
        prevX = 0;
        prevY = 0.5f;
        pointer = transform.parent.Find("Seismogram").Find("Panel").Find("Pointer").GetComponent<RectTransform>();

        var fillColorArray = graphTexture.GetPixels();
        for (var i = 0; i < fillColorArray.Length; ++i)
            fillColorArray[i] = fillColor;
        for (var i = 0; i < fillColorArray.Length; i += graphTexture.width / 10)
            for (int j = 0; j < graphTexture.height; j++)
                graphTexture.SetPixel(i, j, gridColor);
        graphTexture.Apply();
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
        float ampl = sigLerp * intensity;

        float x = graphTexture.width * t * (1 - xOffset);
        float y = (Mathf.Sin(t / dt / 2) + Mathf.Sin(t / dt / 1.3f)) * ampl;

        float pixHeight = y * graphTexture.height / 2 + graphTexture.height / 2;
        float pixHeightPrev = prevY * graphTexture.height / 2 + graphTexture.height / 2;
        DrawLine(graphTexture, new Vector2(prevX, pixHeightPrev), new Vector2(x, pixHeight), lineColor);

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

                pointer.rotation = Quaternion.Euler(new Vector3(0, 0, -prevY * 30)); //  Quaternion.Slerp(pointer.rotation, Quaternion.Euler(new Vector3(0, 0, prevY * 30)), 0.9f);

                Rect uvRect = rawImage.uvRect;
                uvRect.x = d * (1 - xOffset) + xOffset;
                rawImage.uvRect = uvRect;


            } else
            {
                moving = false;
            }
        }
	}

    public void StartMoving()
    {
        if (!moving)
        {
            // Debug.Log(Vector3.SqrMagnitude(transform.position - earthquakeOrigin));
            earthquakeOrigin = GameObject.Find("EpicenterTarget").transform.position;
            distanceIntensity = 1 - Mathf.Clamp(((Vector3.SqrMagnitude(transform.position - earthquakeOrigin) - 5000) / 15000f), 0, 0.9f);

            // wrapper.gameObject.SetActive(true);
            moving = true;
            startTime = Time.time;
        }
    }
}
