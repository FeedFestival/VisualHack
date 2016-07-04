using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Types
{
    public enum Move
    {
        Up, Right, Down, Left, None
    }

    public enum Direction
    {
        Up = 8, Right = 6, Down = 2, Left = 4
    }

    public enum ButtonClick
    {
        NextButton, SettingsButton, RedBallButton, Map, SettingsBackButton, MapsBackButton, GameSettingsButton
    }

    public enum ControllerType
    {
        DefaultPacked, Default, Zas, ClassicPacked, Classic
    }

    public enum Obstacle
    {
        BorderUp, BorderRight, BorderDown, BorderLeft
    }

    public static class Logic
    {
        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }
    }
}
