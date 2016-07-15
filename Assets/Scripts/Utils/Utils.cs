using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Utils
{
    public enum Move
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    public enum Direction
    {
        Up = 8,
        Right = 6,
        Down = 2,
        Left = 4
    }

    public enum Obstacle
    {
        Sphere,
        Box,
        Solid,
        Nothing
    }

    public enum ButtonClick
    {
        PlayOfflineButton,
        SettingsButton,
        RedBallButton,
        Map,
        SettingsBackButton,
        MapsBackButton,
        GameSettingsButton,
        ReloadButton,
        LoginButton
    }

    public enum ControllerType
    {
        DefaultPacked,
        Default,
        Zas,
        ClassicPacked,
        Classic
    }

    public enum LoadThenExecute
    {
        Button,
        MapLoad
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
        Deactivated,
        Activated,
        NoPower
    }

    public enum ZoneType
    {
        DeathZone,
        Solid,
        Walkable
    }

    public enum TileType
    {
        Empty,
        Misc,
        DeathZone,
        PuzzleObject,
        Solid
    }

    public enum Misc
    {
        None,
        PipeConnector,
        Tutorial1,
        PipeHorizontal,

        Pit00,
        Pit01,
        Pit02,
        Pit10,
        Pit11,
        Pit12,
        Pit20,
        Pit21,
        Pit22,
        PitA00,
        PitA22,
        PitS00,
        PitS01,
        PitS02,
        PitS20,
        PitS21,
        PitS22,
        Hill00,
        Hill01,
        Hill02,
        Hill10,
        Hill11,
        Hill12,
        Hill20,
        Hill21,
        Hill22,
        HillS00,
        HillS02,
        HillS20,
        HillS22,
        Hill
    }

    public enum PuzzleObject
    {
        Player,
        Box,
        Bridge,
        Trigger,
        Finish
    }

    public static class Utils
    {
        public static readonly float LerpRatio = 0.02f;
        public static readonly float LerpSpeed = 3f;
        
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;

            return value.ToString();
        }

        public static string GetProfilePictureName(string username, int id)
        {
            return string.Format("{0}_{1}", username.Replace(" ", "_"), id);
        }

        public static string GetProfilePictureName(string username, long id)
        {
            return string.Format("{0}_{1}", username.Replace(" ", "_"), id);
        }

        public static string SavePic(Texture2D pic, int width, int height, string picName)
        {
            string path = Application.persistentDataPath + string.Format("/{0}.png", picName);
            Debug.Log(path);
            try
            {
                byte[] bytes = pic.EncodeToPNG();

                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return path;
        }

        public static Texture2D ReadPic(string picName)
        {
            string path = Application.persistentDataPath + string.Format("/{0}.png", picName);
            Debug.Log(path);

            try
            {
                var bytes = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(128, 128);
                tex.LoadImage(bytes);
                return tex;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return null;
        }

        // Take a shot immediately
        static IEnumerator Start()
        {
            yield return UploadPNG();
        }

        static IEnumerator UploadPNG(Texture2D pic = null, int width = 0, int height = 0)
        {
            // We should only read the screen buffer after rendering is complete
            yield return new WaitForEndOfFrame();

            if (pic == null)
            {
                // Create a texture the size of the screen, RGB24 format
                if (width < 1)
                    width = Screen.width;
                if (height < 1)
                    height = Screen.height;
                pic = new Texture2D(width, height, TextureFormat.RGB24, false);
            }

            // Read screen contents into the texture
            pic.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            pic.Apply();

            // Encode texture into PNG
            byte[] bytes = pic.EncodeToPNG();
            Object.Destroy(pic);

            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);


            //// Create a Web Form
            //WWWForm form = new WWWForm();
            //form.AddField("frameCount", Time.frameCount.ToString());
            //form.AddBinaryData("fileUpload", bytes);

            //// Upload to a cgi script
            //WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
            //yield return w;

            //if (w.error != null)
            //{
            //    Debug.Log(w.error);
            //}
            //else
            //{
            //    Debug.Log("Finished Uploading Screenshot");
            //}
        }
    }
}