using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Types;

public class GameProperties : MonoBehaviour
{
    private Transform _cameraTransform;

    [HideInInspector]
    public IEnumerable<Map> Maps;

    [HideInInspector]
    public float OrthographicSize, Width, Height;

    [HideInInspector]
    public ControllerType ControllerType;

    public void Initialize(Main main)
    {
        _cameraTransform = transform;

        // Resolutions.
        Width = Screen.width;
        Height = Screen.height;

        OrthographicSize = 4.77f;
        if (Math.Abs(Width - 480) < 5)
        {
            OrthographicSize = 5.5f;
        }
        else if (Math.Abs(Width - 854) < 5)
        {
            OrthographicSize = 4.7f;
        }
        else if (Math.Abs(Width - 800) < 5)
        {
            OrthographicSize = 5;
        }
        else if (Math.Abs(Width - 1024) < 5)
        {
            OrthographicSize = 4.8f;
        }
        GetComponent<Camera>().orthographicSize = OrthographicSize;
    }

    public void SetupCamera()
    {
        var xPos = 5.5f;
        var yPos = 4.07f;

        if (Math.Abs(Width - 480) < 5)
        {

        }
        else if (Math.Abs(Width - 854) < 5)
        {
            yPos = 3.98f;
            switch (ControllerType)
            {
                case ControllerType.Default:
                case ControllerType.DefaultPacked:
                case ControllerType.Zas:
                    xPos = 5.6f;
                    break;
                case ControllerType.Classic:
                    xPos = 7.6f;
                    break;
                default:
                    xPos = 7f;
                    break;
            }
        }
        else if (Math.Abs(Width - 800) < 5)
        {
            switch (ControllerType)
            {
                case ControllerType.Default:
                case ControllerType.DefaultPacked:
                case ControllerType.Zas:
                    xPos = 5.5f;
                    break;
                case ControllerType.Classic:
                    xPos = 7.6f;
                    break;
                default:
                    xPos = 7f;
                    break;
            }
        }
        else if (Math.Abs(Width - 1024) < 5)
        {
            switch (ControllerType)
            {
                case ControllerType.Default:
                case ControllerType.DefaultPacked:
                case ControllerType.Zas:
                    xPos = 5.5f;
                    break;
                case ControllerType.Classic:
                    xPos = 7.6f;
                    break;
                default:
                    xPos = 7f;
                    break;
            }
        }
        _cameraTransform.position = new Vector3(xPos, yPos, -25);
    }
}