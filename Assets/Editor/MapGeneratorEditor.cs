using System;
using UnityEngine;
using System.Collections;
using System.Configuration;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private MapGenerator _myScript;

    private bool _setupConfirm;

    public enum InspectorButton
    {
        RecreateDataBase, CleanUpUsers, CreateMap, UpdateMap
    }

    private InspectorButton _actionTool;
    private InspectorButton _action
    {
        get { return _actionTool; }
        set
        {
            _actionTool = value;
            _setupConfirm = true;
        }
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        _myScript = (MapGenerator)target;

        GUILayout.Label("Database");

        if (GUILayout.Button("Recreate Database"))
            _action = InspectorButton.RecreateDataBase;

        if (GUILayout.Button("Clean Up Users"))
            _action = InspectorButton.CleanUpUsers;

        //----------------------------------

        GUILayout.Label("Map Creation");

        _myScript.MapName = EditorGUILayout.TextField("Map name :", _myScript.MapName);
        _myScript.MapNumber = EditorGUILayout.IntField("Map Number :", _myScript.MapNumber);

        if (GUILayout.Button("Create Map"))
            _action = InspectorButton.CreateMap;

        //----------------------------------
        GUILayout.Label("Map Update");

        _myScript.MapId = EditorGUILayout.IntField("Map Id :", _myScript.MapId);

        if (_myScript.MapId != 0)
            if (GUILayout.Button("Update Map"))
                _action = InspectorButton.UpdateMap;


        if (_setupConfirm)
        {
            GUILayout.Label("");
            GUILayout.Label("Confirm ?");
            GUILayout.Label("_______________________________________________");

            if (GUILayout.Button("Yes"))
                ConfirmAccepted();

            if (GUILayout.Button("No"))
                _setupConfirm = false;
        }
    }

    private void ConfirmAccepted()
    {
        switch (_action)
        {
            case InspectorButton.RecreateDataBase:

                _myScript.RecreateDataBase();
                break;

            case InspectorButton.CleanUpUsers:

                _myScript.CleanUpUsers();
                break;

            case InspectorButton.CreateMap:

                _myScript.GenerateSql(false);
                break;

            case InspectorButton.UpdateMap:

                _myScript.GenerateSql(true);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        _setupConfirm = false;
    }
}