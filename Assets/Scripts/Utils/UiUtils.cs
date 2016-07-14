using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public enum AnchorType
    {
        TopRight, TopLeft,
        LeftCenter,
        Center
    }

    public enum LayoutType
    {
        Square,
        Full
    }

    public static class UiUtils
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        public static readonly Color32 Transparent = new Color32(0, 0, 0, 0);
        public static readonly Color32 Black = new Color32(0, 0, 0, 255);
        public static readonly Color32 White = new Color32(255, 255, 255, 255);
        public static readonly Color32 WhiteTransparent = new Color32(255, 255, 255, 200);

        public static readonly string circle = "";

        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static float XPercent(float percent)
        {
            return (ScreenWidth / 100f) * percent;
        }
        public static float YPercent(float percent)
        {
            return (ScreenHeight / 100f) * percent;
        }

        public static void SetSize(float x, float y, Button button)
        {
            SetSize(x, y, button.GetComponent<RectTransform>());
        }

        public static void SetSize(float x, float y, Image image)
        {
            SetSize(x, y, image.GetComponent<RectTransform>());
        }

        public static void SetSize(float x, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = GetPercent(parent.sizeDelta.x, x);
            var h = GetPercent(parent.sizeDelta.y, y);

            rt.GetComponent<RectTransform>().sizeDelta = new Vector3(w, h);
        }

        public static void SetSize(LayoutType layoutType, RectTransform rt)
        {
            if (layoutType == LayoutType.Full)
            {
                rt.sizeDelta = new Vector3(ScreenWidth, ScreenHeight);
            }
            //rt.sizeDelta = new Vector3(ScreenWidth, ScreenHeight);
        }

        public static void SetSize(float x, LayoutType layoutType, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = GetPercent(parent.sizeDelta.x, x);
            float h = w;

            rt.sizeDelta = new Vector3(w, h);
        }

        public static void SetSize(LayoutType layoutType, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            float w;
            var h = GetPercent(parent.sizeDelta.y, y);
            w = h;

            rt.sizeDelta = new Vector3(w, h);
        }

        public static void SetPosition(float x, float y, Button button)
        {
            SetPosition(x, y, button.GetComponent<RectTransform>());
        }

        public static void SetPosition(float x, float y, Image image)
        {
            SetPosition(x, y, image.GetComponent<RectTransform>());
        }

        public static void SetPosition(float x, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = GetPercent(parent.sizeDelta.x, x);
            var h = GetPercent(parent.sizeDelta.y, y);

            rt.GetComponent<RectTransform>().localPosition = new Vector3(w, -h, 0f);
        }

        public static void SetAnchor(AnchorType at, RectTransform rt)
        {
            Vector2 anchor;
            Vector2 pivot;

            if (at == AnchorType.TopLeft)
            {
                anchor = new Vector2(0, 1);
                pivot = new Vector2(0, 1);
            }
            else if (at == AnchorType.LeftCenter)
            {
                anchor = new Vector2(0, 0.5f);
                pivot = new Vector2(0.5f, 0.5f);
            }
            else if (at == AnchorType.Center)
            {
                anchor = new Vector2(0.5f, 0.5f);
                pivot = new Vector2(0.5f, 0.5f);
            }
            else
            {
                anchor = new Vector2(1, 1);
                pivot = new Vector2(1, 1);
            }

            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = pivot;
        }

        public static void SetAnchor(AnchorType at, Button button)
        {
            SetAnchor(at, button.GetComponent<RectTransform>());
        }

        public static void SetAnchor(AnchorType at, Image image)
        {
            SetAnchor(at, image.GetComponent<RectTransform>());
        }

        public static void SetIconSize(Text text)
        {
            var parent = text.transform.parent.GetComponent<RectTransform>();
        }
    }
}