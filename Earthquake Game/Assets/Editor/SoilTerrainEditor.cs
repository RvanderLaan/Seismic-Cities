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

    void OnEnable()
    {
        grid = (Grid) target;
        shapeKeys = new List<SoilBlock.BlockShape>(TileSet.Shapes.Keys);
        matKeys = new List<Soil.SoilType>(TileSet.Materials.Keys);
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

            if (idxX < 0 || idxX >= grid.blocks.GetLength(0) || idxY < 0 || idxY >= grid.blocks.GetLength(1))
                return;
            SoilBlock block = grid.blocks[idxX, idxY];

            if (block == null)
            {
                // Create new block
                Undo.IncrementCurrentGroup();
                GUIUtility.hotControl = controlid;
                e.Use();

                GameObject gameObject = (GameObject)PrefabUtility.InstantiatePrefab(shapePrefab);
                block = gameObject.AddComponent<SoilBlock>();
                grid.blocks[idxX, idxY] = block;
                block.shape = shapeKeys[shape];
                block.type = matKeys[soil];

                gameObject.transform.position = aligned;
                gameObject.transform.parent = grid.transform;

                gameObject.GetComponentInChildren<Renderer>().material = mat;

                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            } else if (block.shape != shapeKeys[shape] || block.type != matKeys[soil])
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
                block = gameObject.AddComponent<SoilBlock>();
                grid.blocks[idxX, idxY] = block;
                block.shape = shapeKeys[shape];
                block.type = matKeys[soil];

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
    }

    float alignToGrid(float x)
    {
        return Mathf.Floor(x / grid.size) * grid.size;
    }

}
