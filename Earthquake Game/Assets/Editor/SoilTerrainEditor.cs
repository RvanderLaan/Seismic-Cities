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

    void OnEnable()
    {
        grid = (Grid) target;
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid width");
        EditorGUILayout.Slider(1, 1f, 100f, null);
        GUILayout.EndHorizontal();


        GUIContent[] content = new GUIContent[8];
        for (int i = 0; i < content.Length; i++)
        {
            content[i] = new GUIContent("Shape " + i);
        }

        shape = GUILayout.SelectionGrid(shape, content, 4);

        GUIContent[] content2 = new GUIContent[8];
        for (int i = 0; i < content.Length; i++)
        {
            content2[i] = new GUIContent("Soil " + i);
        }

        soil = GUILayout.SelectionGrid(soil, content2, 4);


        mat = (Material) EditorGUILayout.ObjectField(mat, typeof(Material));


    }

}
