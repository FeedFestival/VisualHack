using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Controller : MonoBehaviour
{
    private Main _main;

    public Move GoDirection;

    public void Initialize(Main main)
    {
        _main = main;
    }

    public void SelectControllerType(int controllerType)
    {
        _main.ControllerType = (ControllerType)controllerType;
        _main.DataService.UpdateUserControllerType(controllerType);

        _main.ButtonClicked((int)ButtonClick.SettingsBackButton);
    }

    public void MoveDirection(int moveIndex)
    {
        GoDirection = (Move)moveIndex;

        _main.DebugText = GoDirection.ToString();

        switch (GoDirection)
        {
            case Move.Up:

                break;

            case Move.Right:
                break;
            case Move.Down:
                break;
            case Move.Left:
                break;
            default:
                throw new ArgumentOutOfRangeException("moveDirection", GoDirection, null);
        }
    }

    public void StopDirection(int moveIndex)
    {
        Move moveDirection = (Move)moveIndex;

        if (moveDirection == GoDirection)
        {
            _main.DebugText = "s- " + moveDirection;

            switch (moveDirection)
            {
                case Move.Up:

                    break;

                case Move.Right:
                    break;
                case Move.Down:
                    break;
                case Move.Left:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("stopDirection", moveDirection, null);
            }
        }
    }
}