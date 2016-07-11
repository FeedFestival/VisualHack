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
        Sphere, Box, Solid, Nothing
    }

    public enum ButtonClick
    {
        NextButton, SettingsButton, RedBallButton, Map, SettingsBackButton, MapsBackButton, GameSettingsButton, ReloadButton
    }

    public enum ControllerType
    {
        DefaultPacked, Default, Zas, ClassicPacked, Classic
    }

    public enum LoadThenExecute
    {
        Button, MapLoad
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
        DeathZone, Solid, Walkable
    }

    public enum TileType
    {
        Empty, Misc, DeathZone, PuzzleObject, Solid
    }

    public enum Misc
    {
        None, PipeConnector, Tutorial1, PipeHorizontal,

        Pit00, Pit01, Pit02, Pit10, Pit11, Pit12, Pit20, Pit21, Pit22,
        PitA00, PitA22,
        PitS00, PitS01, PitS02, PitS20, PitS21, PitS22,
        Hill00, Hill01, Hill02, Hill10, Hill11, Hill12, Hill20, Hill21, Hill22,
        HillS00, HillS02, HillS20, HillS22,
        Hill
    }

    public enum PuzzleObject
    {
        Player, Box, Bridge, Trigger,
        Finish
    }

    public static class Logic
    {
        public static readonly float LerpRatio = 0.02f;
        public static readonly float LerpSpeed = 3f;

        public static readonly Color32 Transparent = new Color32(0, 0, 0, 0);
        public static readonly Color32 Black = new Color32(0, 0, 0, 255);
        public static readonly Color32 White = new Color32(255, 255, 255, 255);
        public static readonly Color32 WhiteTransparent = new Color32(255, 255, 255, 200);

        public static readonly string circle = "";

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
