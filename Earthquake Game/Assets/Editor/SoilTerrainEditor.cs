using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class SoilTerrainEditor : Editor {

    Grid grid;

    Texture[] shapes;

    int shape = 0;
    int soil = 0;
    Material mat;

    List<SoilBlock.BlockShape> shapeKeys;
    List<Soil.SoilType> matKeys;


    GameObject[] tiles;
    int gridWidth = 32, gridHeight = 32;

    void OnEnable()
    {
        grid = (Grid) target;
        shapeKeys = new List<SoilBlock.BlockShape>(TileSet.Shapes.Keys);
        matKeys = new List<Soil.SoilType>(TileSet.Materials.Keys);

        tiles = new GameObject[gridWidth * gridHeight];
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();

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
    }

    private void OnSceneGUI()
    {
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
            int idx = (int)(-aligned.y / grid.size * gridWidth + aligned.x / grid.size);
            GameObject gameObject = tiles[idx];


            if (gameObject == null)
            {
                Undo.IncrementCurrentGroup();
                GUIUtility.hotControl = controlid;
                e.Use();

                gameObject = (GameObject)PrefabUtility.InstantiatePrefab(shapePrefab);
                
                gameObject.transform.position = aligned;
                gameObject.transform.parent = grid.transform;
                // gameObject.layer = Layer

                gameObject.GetComponent<Renderer>().material = mat;

                tiles[idx] = gameObject;

                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            }
            

            


        }

        // Reset focus
        if (e.isMouse && e.type == EventType.MouseUp && e.button == 0)
        {
            GUIUtility.hotControl = 0;
        }
    }

    float alignToGrid(float x)
    {
        return Mathf.Ceil(x / grid.size) * grid.size;
    }

}
