using System;
using UnityEngine;
using System.Collections;
using System.Configuration;
using System.Linq;
using Assets.Scripts.Utils;
using NUnit.Framework.Constraints;
using UnityEditor;

[CustomEditor(typeof(GameUi))]
public class GameUiEditor : Editor
{
    private GameUi _myScript;

    public string Error;

    private AndroidViewType _androidViewType;

    public AndroidViewType AndroidViewType
    {
        set
        {
            _androidViewType = value;
            EditorUtils.AndroidViewType = _androidViewType;
        }
        get { return _androidViewType; }
    }

    private GUIStyle _redStyle;

    public override void OnInspectorGUI()
    {
        if (_myScript == null)
            _myScript = (GameUi)target;

        if (_redStyle == null)
            _redStyle = new GUIStyle();
        _redStyle.normal.textColor = Color.red;

        GUILayout.Space(10);
        if (GUILayout.Button("Refresh UI", GUILayout.Width(UiUtils.GetPercent(Screen.width, 90)), GUILayout.Height(50)))
        {
            _myScript.RefreshCameraTransform();
            _myScript.RefreshUi(true);
        }

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Space(5);

        GUILayout.Label("Resolution");
        GUILayout.Space(5);


        _myScript.InspectorScreenWidth = EditorGUILayout.IntField("Width :", _myScript.InspectorScreenWidth);
        _myScript.InspectorScreenHeight = EditorGUILayout.IntField("Height :", _myScript.InspectorScreenHeight);

        if (string.IsNullOrEmpty(Error) == false)
        {
            GUILayout.Space(10);
            GUILayout.Label(Error, _redStyle);
            GUILayout.Space(10);
        }
        _myScript.InspectorScreenName = EditorGUILayout.TextField("Name :", _myScript.InspectorScreenName);

        GUILayout.Space(10);
        if (GUILayout.Button("Add View Size"))
        {
            if (string.IsNullOrEmpty(_myScript.InspectorScreenName))
                Error = "Name is null or incorect. " + Environment.NewLine;
            else if (_myScript.InspectorScreenName.Contains(_myScript.InspectorScreenWidth.ToString()) ||
                _myScript.InspectorScreenName.Contains(_myScript.InspectorScreenWidth.ToString()))
                Error = "Not allowed to contain Width and Height in the name." + Environment.NewLine;
            else if (_myScript.InspectorScreenName.Contains(AndroidViewType.ToString()) == false)
                Error = "You must have the ViewType(ex: Landscape)," + Environment.NewLine;
            else
                Error = null;

            if (string.IsNullOrEmpty(Error) == false)
                return;

            if (AndroidViewType == AndroidViewType.Landscape &&
                _myScript.InspectorScreenWidth > _myScript.InspectorScreenHeight && _myScript.InspectorScreenWidth > 440)
            {
                foreach (var vs in EditorUtils.ViewSizes)
                {
                    if (vs.Width == _myScript.InspectorScreenWidth && vs.Height == _myScript.InspectorScreenHeight)
                        Error = "ViewSize allready exists.";
                }
                if (string.IsNullOrEmpty(Error))
                    EditorUtils.AddCustomSize(GameViewSizeType.FixedResolution, GameViewSizeGroupType.Android,
                        _myScript.InspectorScreenWidth, _myScript.InspectorScreenHeight, _myScript.InspectorScreenName);
            }
        }

        GUILayout.Space(10);

        AndroidViewType = (AndroidViewType)EditorGUILayout.EnumPopup("Android View Type:", AndroidViewType);

        GUILayout.Space(10);

        foreach (var viewSize in EditorUtils.ViewSizes)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(viewSize.Name);
            GUILayout.Label(viewSize.Width.ToString());
            GUILayout.Label(viewSize.Height.ToString());

            if (GUILayout.Button("Set VS"))
            {
                EditorUtils.SetSizeToScreen(GameViewSizeGroupType.Android, viewSize.Width, viewSize.Height);

                _myScript.InspectorScreenWidth = viewSize.Width;
                _myScript.InspectorScreenHeight = viewSize.Height;
                _myScript.InspectorScreenName = viewSize.Name;

                _myScript.RefreshCameraTransform();
                _myScript.RefreshUi(true);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
        }

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }
}