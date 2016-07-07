using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGenerator myScript = (MapGenerator)target;

        if (GUILayout.Button("Create Map"))
            myScript.GenerateSql(false);

        if (myScript.MapId != 0)
            if (GUILayout.Button("Update Map"))
                myScript.GenerateSql(true);

    }
}