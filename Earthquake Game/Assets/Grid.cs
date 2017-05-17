using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

    public float size = 32f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {

        
        Vector3 pos = Camera.current.transform.position;
        Gizmos.color = Color.green;

        Rect viewport = Camera.current.pixelRect;

        Rect rect = new Rect(-8 * size, -16 * size, 32 * size, 18 * size);
        
        for (float i = rect.yMin; i <= rect.yMax; i += size)
        {
            float y = Mathf.Floor(i / size) * size;
            Gizmos.DrawLine(new Vector3(rect.xMin, y, 0), new Vector3(rect.xMax, y, 0)); 
        }

        for (float i = rect.xMin; i <= rect.xMax; i += size)
        {
            float y = Mathf.Floor(i / size) * size;
            Gizmos.DrawLine(new Vector3(y, rect.yMin, 0), new Vector3(y, rect.yMax, 0));
        }

    }
}
