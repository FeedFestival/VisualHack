﻿using System;
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
            AddScreenResolution(_myScript.InspectorScreenName, _myScript.InspectorScreenWidth, _myScript.InspectorScreenHeight);
        }

        _myScript.InspectorControllerType = (ControllerType)EditorGUILayout.EnumPopup("Controller Type:", _myScript.InspectorControllerType);

        GUILayout.Space(10);
        if (GUILayout.Button("Populate"))
        {
            if (AndroidViewType == AndroidViewType.Landscape)
            {
                AddScreenResolution("4K UHD (Landscape)", 3840, 2160);
                AddScreenResolution("WQXGA + (Landscape)", 3200, 1800);
                AddScreenResolution("iPad Pro (Landscape)", 2732, 2048);
                AddScreenResolution("Samsung S6 (Landscape)", 2560, 1440);
                AddScreenResolution("Full HD (Landscape)", 1920, 1080);

                AddScreenResolution("HD (222:125) (Landscape)", 1776, 1000);

                AddScreenResolution("900p HD+ (Landscape)", 1600, 900);

                AddScreenResolution("HD ready (683:384) (Landscape)", 1366, 768);
                AddScreenResolution("iPhone 6 (667:375) (Landscape)", 1334, 750);

                AddScreenResolution("WXGA-H (Landscape)", 1280, 720);
                
                AddScreenResolution("iPhone 5 (71:40) (Landscape)", 1136, 640);
                AddScreenResolution("WSVGA (128:75) (Landscape)", 1024, 600);
                AddScreenResolution("PV (30:17) (Landscape)", 960, 544);
                AddScreenResolution("PV (53:30) (Landscape)", 848, 480);

                AddScreenResolution("IPhone 6 s(Landscape)", 667, 375);

                AddScreenResolution("nHD (Landscape)", 640, 360);

                AddScreenResolution("iPhone 5 s (71:40)(Landscape)", 568, 320);

                AddScreenResolution("Low (80:39) (Landscape)", 480, 234);
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

    private void AddScreenResolution(string rezName, int xSize, int ySize)
    {
        if (string.IsNullOrEmpty(rezName))
            Error = "Name is null or incorect. " + Environment.NewLine;
        else if (rezName.Contains(xSize.ToString()) ||
            rezName.Contains(xSize.ToString()))
            Error = "Not allowed to contain Width and Height in the name." + Environment.NewLine;
        else if (rezName.Contains(AndroidViewType.ToString()) == false)
            Error = "You must have the ViewType(ex: Landscape)," + Environment.NewLine;
        else
            Error = null;

        if (string.IsNullOrEmpty(Error) == false)
            return;

        if (AndroidViewType == AndroidViewType.Landscape &&
            xSize > ySize && xSize > 280)
        {
            foreach (var vs in EditorUtils.ViewSizes)
            {
                if (vs.Width == xSize && vs.Height == ySize)
                    Error = vs.Width + ":" + vs.Height + "ViewSize allready exists.";
            }
            if (string.IsNullOrEmpty(Error))
                EditorUtils.AddCustomSize(GameViewSizeType.FixedResolution, GameViewSizeGroupType.Android,
                    xSize, ySize, rezName);
        }
    }

}