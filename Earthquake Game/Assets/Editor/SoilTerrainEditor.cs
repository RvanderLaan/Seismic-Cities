using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class SoilTerrainEditor : Editor {

    Grid grid;
    LevelData levelData;
    SoilBlock[] blocks;
 
    Texture[] shapes;

    int shape = 0;
    int soil = 0;
    Material mat;

    List<SoilBlock.BlockShape> shapeKeys;
    List<Soil.SoilType> matKeys;

    void OnEnable()
    {
        // serializedObject.Update();
        grid = (Grid) target;
        levelData = grid.levelData;

        shapeKeys = new List<SoilBlock.BlockShape>(TileSet.Shapes.Keys);
        matKeys = new List<Soil.SoilType>(TileSet.Materials.Keys);

        if (levelData == null)
        {
            Debug.LogWarning("No level data specified!");
        }
        else
        {
            //if (levelData.blocks == null || levelData.blocks.Length == 0)
            //{
            //    Debug.Log("Creating new block array");
            //    levelData.blocks = new SoilBlock[(int)Grid.dimensions.width * (int)Grid.dimensions.height];

            //    EditorUtility.SetDirty(levelData);
            //    serializedObject.ApplyModifiedProperties();
            //}
            //blocks = levelData.blocks;
        }
    }

    public override void OnInspectorGUI()
    {
        ////GUILayout.BeginHorizontal();
        //GUILayout.Label("Grid width");
        //EditorGUILayout.Slider(1, 1f, 100f, null);
        //GUILayout.EndHorizontal();

        string[] levels = { "1", "2", "3" };
        int levelIdx = 0;
        levelIdx = EditorGUILayout.Popup("Level", levelIdx, levels);

        GUIContent[] shapeContent = new GUIContent[shapeKeys.Count];
        for (int i = 0; i < shapeContent.Length; i++)
        {
            shapeContent[i] = new GUIContent(shapeKeys[i].ToString());
        }

        shape = GUILayout.SelectionGrid(shape, shapeContent, 3);

        GUIContent[] matContent = new GUIContent[matKeys.Count];
        for (int i = 0; i < matContent.Length; i++)
        {
            matContent[i] = new GUIContent(matKeys[i].ToString());
        }

        soil = GUILayout.SelectionGrid(soil, matContent, 3);

        if (GUILayout.Button("Reconstruct"))
        {
            reconstruct();
        }
    }

    private void OnSceneGUI()
    {
        // Disable selection box etc.
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        // Modifying the grid here
        int controlid = GUIUtility.GetControlID(FocusType.Passive);

        Event e = Event.current;

        Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y));
        Vector3 mousePos = ray.origin;
        if (e.isMouse && e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
        {
            GameObject shapePrefab = TileSet.Shapes[shapeKeys[shape]];
            Material mat = TileSet.Materials[matKeys[soil]];

            Vector3 aligned = new Vector3(alignToGrid(mousePos.x), alignToGrid(mousePos.y), 0.0f);
            int idxX = (int)(aligned.x / grid.size) - (int) Grid.dimensions.xMin;
            int idxY = (int)(aligned.y / grid.size) - (int) Grid.dimensions.yMin;

            // Debug.Log(idxX + ", " + idxY);

            if (idxX < 0 || idxX >= Grid.dimensions.width || idxY < 0 || idxY >= Grid.dimensions.height)
                return;

            SoilBlock block = blocks[(int)Grid.dimensions.width * idxX + idxY];
            Debug.Log((int)Grid.dimensions.width * idxX + idxY);

            if (e.modifiers == EventModifiers.Alt)
            {
                if (block.gameObject != null)
                {
                    // Delete block
                    Undo.IncrementCurrentGroup();
                    GUIUtility.hotControl = controlid;
                    e.Use();


                    // Delete old go
                    GameObject gameObject = block.gameObject;

                    Undo.RecordObject(grid, "Delete " + gameObject.name);
                    blocks[(int)Grid.dimensions.width * idxX + idxY] = null;

                    Undo.DestroyObjectImmediate(gameObject);
                }
            }
            else if (block.gameObject == null)
            {
                // Create new block
                Undo.IncrementCurrentGroup();
                GUIUtility.hotControl = controlid;
                e.Use();

                GameObject gameObject = (GameObject)PrefabUtility.InstantiatePrefab(shapePrefab);
                block.shape = shapeKeys[shape];
                block.type = matKeys[soil];
                block.gameObject = gameObject;

                gameObject.transform.position = aligned;
                gameObject.transform.parent = grid.transform;

                gameObject.GetComponentInChildren<Renderer>().material = mat;

                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            }
            else if (block.shape != shapeKeys[shape] || block.type != matKeys[soil])
            {
                // Change block properties
                Undo.IncrementCurrentGroup();
                GUIUtility.hotControl = controlid;
                e.Use();

                // Delete old go
                GameObject oldGameObject = block.gameObject;
                Undo.DestroyObjectImmediate(oldGameObject);

                // Create new GO
                GameObject gameObject = (GameObject)PrefabUtility.InstantiatePrefab(shapePrefab);
                block.shape = shapeKeys[shape];
                block.type = matKeys[soil];
                block.gameObject = gameObject;

                gameObject.transform.position = aligned;
                gameObject.transform.parent = grid.transform;

                gameObject.GetComponentInChildren<Renderer>().material = mat;

                Undo.RegisterCreatedObjectUndo(gameObject, "Modify " + gameObject.name);
            } 
        }

        // Reset focus
        if (e.isMouse && e.type == EventType.MouseUp && e.button == 0)
        {
            GUIUtility.hotControl = 0;
        }


        //serializedObject.ApplyModifiedProperties();
        //serializedObject.Update();
        // EditorUtility.SetDirty(levelData);
    }

    void reconstruct()
    {
        // Reuse children
        int lastChildIndex = 0;

        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                // Debug.Log(grid.blocks[i, j] == null);
                if (blocks[(int)Grid.dimensions.width * i + j] != null)
                {
                    GameObject shapePrefab = TileSet.Shapes[shapeKeys[shape]];
                    // Debug.Log(grid.blocks[i, j].type);
                    GameObject gameObject;
                    if (lastChildIndex < grid.transform.childCount)
                    {
                        lastChildIndex++;
                        gameObject = grid.transform.GetChild(lastChildIndex).gameObject;
                    }
                    else
                    {
                        gameObject = (GameObject)PrefabUtility.InstantiatePrefab(shapePrefab);
                    }

                    
                    Material mat = TileSet.Materials[matKeys[soil]];
                    gameObject.GetComponentInChildren<Renderer>().material = mat;

                    Vector3 worldPos = new Vector3((i + Grid.dimensions.xMin) * grid.size, (j +Grid.dimensions.yMin) * grid.size);
                    gameObject.transform.position = worldPos;
                    // Set shape, material and position
                }
                else
                {
                    
                }
            }
        }

        // Remove leftover gameobjects
        for (int i = lastChildIndex; i < grid.transform.childCount; i++)
        {
            grid.transform.GetChild(lastChildIndex).gameObject.SetActive(false);
        }
    }

    float alignToGrid(float x)
    {
        return Mathf.Floor(x / grid.size) * grid.size;
    }

}
