using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

    public readonly float size = 8f;

    public static Rect dimensions = new Rect(-32, -48, 128, 64);

    public SoilBlock[,] blocks = new SoilBlock[(int) dimensions.width, (int) dimensions.height];

	// Use this for initialization
	void Start () {
		// Convert soil blocks to appropriate gameobjects

	}

    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != this.gameObject) return;
        
        Vector3 pos = Camera.current.transform.position;
        Gizmos.color = Color.green;

        Rect viewport = Camera.current.pixelRect;

        
        
        for (float i = dimensions.yMin; i <= dimensions.yMax; i += 1)
        {
            float y = i;
            Gizmos.DrawLine(new Vector3(dimensions.xMin, y, 0) * size, new Vector3(dimensions.xMax, y, 0) * size); 
        }

        for (float i = dimensions.xMin; i <= dimensions.xMax; i += 1)
        {
            float y = i;
            Gizmos.DrawLine(new Vector3(y, dimensions.yMin, 0) * size, new Vector3(y, dimensions.yMax, 0) * size);
        }

    }

    public float getHeight(int gridPositionX)
    {
        int index = gridPositionX - (int) dimensions.xMin;
        for (int i = blocks.GetLength(1) - 1; i >= 0; i--)
        {
            Debug.Log(index + ", "+  i);
            if (blocks[index, i] != null)
            {
                Debug.Log("yeh");
                return i - dimensions.yMin;
            }
        }

        return 0;
    }
}
