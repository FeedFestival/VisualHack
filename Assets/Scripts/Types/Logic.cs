using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Types
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

    public enum GameViewSizeType
    {
        AspectRatio, FixedResolution
    }

    public enum AndroidViewType
    {
        Landscape, Portrait,
        All
    }

    public class ViewSize
    {
        public string Name;
        public int Width;
        public int Height;
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

        static object _gameViewSizesInstance;
        static MethodInfo _getGroup;

        public static AndroidViewType AndroidViewType;

        static Logic()
        {
            // gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
            var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            _getGroup = sizesType.GetMethod("GetGroup");
            _gameViewSizesInstance = instanceProp.GetValue(null, null);
        }

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


        /*
        *-----------------------------------------
         */

        public static List<ViewSize> ViewSizes
        {
            set { }
            get
            {
                return GetViewSizes(GameViewSizeGroupType.Android);
            }
        }

        public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            var asm = typeof(Editor).Assembly;
            var sizesType = asm.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            var getGroup = sizesType.GetMethod("GetGroup");
            var instance = instanceProp.GetValue(null, null);
            var group = getGroup.Invoke(instance, new object[] { (int)sizeGroupType });
            var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
            var gvsType = asm.GetType("UnityEditor.GameViewSize");
            var ctor = gvsType.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) });
            var newSize = ctor.Invoke(new object[] { (int)viewSizeType, width, height, text });
            addCustomSize.Invoke(group, new object[] { newSize });
        }

        public static List<ViewSize> GetViewSizes(GameViewSizeGroupType sizeGroupType)
        {
            var viewSizes = new List<ViewSize>();

            var group = GetGroup(sizeGroupType);
            var groupType = group.GetType();
            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");
            int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
            var getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var gvsType = getGameViewSize.ReturnType;
            var widthProp = gvsType.GetProperty("width");
            var heightProp = gvsType.GetProperty("height");
            var indexValue = new object[1];

            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];

            var iterations = (sizesCount > displayTexts.Length) ? sizesCount : displayTexts.Length;

            for (var i = 0; i < iterations; i++)
            {
                indexValue[0] = i;

                var size = getGameViewSize.Invoke(group, indexValue);
                int sizeWidth = (int)widthProp.GetValue(size, null);
                int sizeHeight = (int)heightProp.GetValue(size, null);

                if (AndroidViewType == AndroidViewType.Landscape && sizeWidth > sizeHeight && sizeWidth > 440)
                    viewSizes.Add(new ViewSize
                    {
                        Name = displayTexts[i],
                        Width = sizeWidth,
                        Height = sizeHeight
                    });
                else if (AndroidViewType == AndroidViewType.Portrait && sizeWidth < sizeHeight)
                    viewSizes.Add(new ViewSize
                    {
                        Name = displayTexts[i],
                        Width = sizeWidth,
                        Height = sizeHeight
                    });
                else if (AndroidViewType == AndroidViewType.All)
                    viewSizes.Add(new ViewSize
                    {
                        Name = displayTexts[i],
                        Width = sizeWidth,
                        Height = sizeHeight
                    });
            }

            viewSizes = viewSizes.OrderBy(x => x.Width).ToList();

            return viewSizes;
        }

        public static void SetSizeToScreen(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            int idx = FindSize(sizeGroupType, width, height);
            if (idx != -1)
                SetSize(idx);
        }

        public static void SetSize(int index)
        {
            var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var gvWnd = EditorWindow.GetWindow(gvWndType);
            selectedSizeIndexProp.SetValue(gvWnd, index, null);
        }

        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, string text)
        {
            return FindSize(sizeGroupType, text) != -1;
        }
        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            return FindSize(sizeGroupType, width, height) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            // goal:
            // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
            // int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
            // iterate through the sizes via group.GetGameViewSize(int index)

            var group = GetGroup(sizeGroupType);
            var groupType = group.GetType();
            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");
            int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
            var getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var gvsType = getGameViewSize.ReturnType;
            var widthProp = gvsType.GetProperty("width");
            var heightProp = gvsType.GetProperty("height");
            var indexValue = new object[1];
            for (int i = 0; i < sizesCount; i++)
            {
                indexValue[0] = i;
                var size = getGameViewSize.Invoke(group, indexValue);
                int sizeWidth = (int)widthProp.GetValue(size, null);
                int sizeHeight = (int)heightProp.GetValue(size, null);
                if (sizeWidth == width && sizeHeight == height)
                    return i;
            }
            return -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        {
            // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
            // string[] texts = group.GetDisplayTexts();
            // for loop...

            var group = GetGroup(sizeGroupType);
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
            for (int i = 0; i < displayTexts.Length; i++)
            {
                string display = displayTexts[i];
                // the text we get is "Name (W:H)" if the size has a name, or just "W:H" e.g. 16:9
                // so if we're querying a custom size text we substring to only get the name
                // You could see the outputs by just logging
                // Debug.Log(display);
                int pren = display.IndexOf('(');
                if (pren != -1)
                    display = display.Substring(0, pren - 1); // -1 to remove the space that's before the prens. This is very implementation-depdenent
                if (display == text)
                    return i;
            }
            return -1;
        }

        static object GetGroup(GameViewSizeGroupType type)
        {
            return _getGroup.Invoke(_gameViewSizesInstance, new object[] { (int)type });
        }
    }
}

public static class GameViewUtils
{
    static object gameViewSizesInstance;
    static MethodInfo getGroup;

    static GameViewUtils()
    {
        // gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        getGroup = sizesType.GetMethod("GetGroup");
        gameViewSizesInstance = instanceProp.GetValue(null, null);
    }

    public enum GameViewSizeType
    {
        AspectRatio, FixedResolution
    }

    [MenuItem("Test/AddSize")]
    public static void AddTestSize()
    {
        AddCustomSize(GameViewSizeType.AspectRatio, GameViewSizeGroupType.Standalone, 123, 456, "Test size");
    }

    [MenuItem("Test/SizeTextQuery")]
    public static void SizeTextQueryTest()
    {
        Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, "Test size"));
    }

    [MenuItem("Test/Query16:9Test")]
    public static void WidescreenQueryTest()
    {
        Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, "16:9"));
    }

    [MenuItem("Test/Set16:9")]
    public static void SetWidescreenTest()
    {
        SetSize(FindSize(GameViewSizeGroupType.Standalone, "16:9"));
    }

    [MenuItem("Test/SetTestSize")]
    public static void SetTestSize()
    {
        int idx = FindSize(GameViewSizeGroupType.Standalone, 123, 456);
        if (idx != -1)
            SetSize(idx);
    }

    public static void SetSize(int index)
    {
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        selectedSizeIndexProp.SetValue(gvWnd, index, null);
    }

    [MenuItem("Test/SizeDimensionsQuery")]
    public static void SizeDimensionsQueryTest()
    {
        Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, 123, 456));
    }

    public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
    {
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupTyge);
        // group.AddCustomSize(new GameViewSize(viewSizeType, width, height, text);

        var group = GetGroup(sizeGroupType);
        var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
        var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        var ctor = gvsType.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) });
        var newSize = ctor.Invoke(new object[] { (int)viewSizeType, width, height, text });
        addCustomSize.Invoke(group, new object[] { newSize });
    }

    public static bool SizeExists(GameViewSizeGroupType sizeGroupType, string text)
    {
        return FindSize(sizeGroupType, text) != -1;
    }

    public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
    {
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        // string[] texts = group.GetDisplayTexts();
        // for loop...

        var group = GetGroup(sizeGroupType);
        var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
        var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
        for (int i = 0; i < displayTexts.Length; i++)
        {
            string display = displayTexts[i];
            // the text we get is "Name (W:H)" if the size has a name, or just "W:H" e.g. 16:9
            // so if we're querying a custom size text we substring to only get the name
            // You could see the outputs by just logging
            // Debug.Log(display);
            int pren = display.IndexOf('(');
            if (pren != -1)
                display = display.Substring(0, pren - 1); // -1 to remove the space that's before the prens. This is very implementation-depdenent
            if (display == text)
                return i;
        }
        return -1;
    }

    public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
    {
        return FindSize(sizeGroupType, width, height) != -1;
    }

    public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
    {
        // goal:
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        // int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
        // iterate through the sizes via group.GetGameViewSize(int index)

        var group = GetGroup(sizeGroupType);
        var groupType = group.GetType();
        var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
        var getCustomCount = groupType.GetMethod("GetCustomCount");
        int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
        var getGameViewSize = groupType.GetMethod("GetGameViewSize");
        var gvsType = getGameViewSize.ReturnType;
        var widthProp = gvsType.GetProperty("width");
        var heightProp = gvsType.GetProperty("height");
        var indexValue = new object[1];
        for (int i = 0; i < sizesCount; i++)
        {
            indexValue[0] = i;
            var size = getGameViewSize.Invoke(group, indexValue);
            int sizeWidth = (int)widthProp.GetValue(size, null);
            int sizeHeight = (int)heightProp.GetValue(size, null);
            if (sizeWidth == width && sizeHeight == height)
                return i;
        }
        return -1;
    }

    static object GetGroup(GameViewSizeGroupType type)
    {
        return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
    }
}

//[MenuItem("Test/LogCurrentGroupType")]
//public static void LogCurrentGroupType()
//{
//    Debug.Log(GetCurrentGroupType());
//}
//public static GameViewSizeGroupType GetCurrentGroupType()
//{
//    var getCurrentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
//    return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(gameViewSizesInstance, null);
//}