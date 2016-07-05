using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

    public enum Obstacle
    {
        Player, Box, Solid, Nothing
    }

    public enum ButtonClick
    {
        NextButton, SettingsButton, RedBallButton, Map, SettingsBackButton, MapsBackButton, GameSettingsButton
    }

    public enum ControllerType
    {
        DefaultPacked, Default, Zas, ClassicPacked, Classic
    }
    
    public enum Tag
    {
        [Description("SolidUp")]
        SolidUp,
        [Description("SolidRight")]
        SolidRight,
        [Description("SolidDown")]
        SolidDown,
        [Description("SolidLeft")]
        SolidLeft,

        [Description("BoxUp")]
        BoxUp,
        [Description("BoxRight")]
        BoxRight,
        [Description("BoxDown")]
        BoxDown,
        [Description("BoxLeft")]
        BoxLeft,
    }

    public enum ObjectState
    {
        Deactivated, Activated, NoPower
    }

    public enum ZoneType
    {
        Pit, Solid, Walkable
    }

    public static class Logic
    {
        public static readonly float LerpRatio = 0.02f;
        public static readonly float LerpSpeed = 3f;

        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;

            return value.ToString();
        }
    }
}
