using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

    public readonly float size = 8f;

    public static Rect dimensions = new Rect(-32, -48, 128, 64);

    [SerializeField]
    public LevelData levelData;

    private SoilBlock[] blocks;

#if UNITY_EDITOR

    // Use this for initialization
    void OnEnable() {
        levelData = GameObject.Find("_GM").GetComponent<LevelManager>().getLevelData();
        // Todo: Convert soil blocks to appropriate gameobjects
        //if (levelData.blocks == null || levelData.blocks.Length == 0)
        //{
        //     levelData.blocks = new SoilBlock[(int)dimensions.width * (int)dimensions.height];
        //    // Todo: Loop through blocks and fill array?
        //    Debug.Log("START BLOCKS INIT");
        //}

        // blocks = levelData.blocks;
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
            if (blocks[(int)dimensions.width * index + i] != null)
            {
                Debug.Log("yeh");
                return i - dimensions.yMin;
            }
        }

        return 0;
    }
#endif
}
