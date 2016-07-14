using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    private Main _main;

    [HideInInspector]
    public GameObject DebugContainer;

    [HideInInspector]
    public GameObject StartPanel, MainMenuPanel, TopBarPanel, SettingsPanel, MapsPanel, GameViewPanel;

    [HideInInspector]
    public Button FacebookLoginButton;
    [HideInInspector]
    public Text ProfileName;
    [HideInInspector]
    public Image ProfilePicture;

    private Button _rightController, _leftController, _downController, _upController;

    private float _width, _height, _rightPoint, _leftPoint, _topPoint, _bottomPoint;

    public void Initialize(Main main)
    {
        _main = main;

        RefreshUi();
    }

    public void RefreshUi(bool fromInspector = false)
    {
        if (fromInspector == false)
        {
            _width = _main.GameProperties.Width;
            _height = _main.GameProperties.Height;
        }

        UiUtils.ScreenWidth = (int)_width;
        UiUtils.ScreenHeight = (int)_height;

        _rightPoint = _width / 2.0f;
        _leftPoint = -_rightPoint;
        _topPoint = _height / 2.0f;
        _bottomPoint = -_topPoint;

        var x = 2;

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            Image image;
            switch (child.gameObject.name)
            {
                case "Canvas":
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    break;

                //---------------------------------------------------------------------------------
                case "Loading":
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    break;

                case "LoadingIcon":
                    UiUtils.SetSize(12, LayoutType.Square, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(87, 80, child.GetComponent<RectTransform>());
                    if (fromInspector == false)
                        _main.LoadingController.LoadingIcon = child.gameObject;
                    break;

                case "LoadingIconText":
                    if (fromInspector == false)
                        _main.LoadingController.LoadingIconText = child.GetComponent<Text>();
                    break;

                case "LoadingIconCircle":
                    child.GetComponent<Text>().text = UiUtils.circle;
                    if (fromInspector == false)
                        _main.LoadingController.LoadingIconCircle = child.GetComponent<Text>();
                    break;

                //---------------------------------------------------------------------------------
                case "DebugContainer":
                    DebugContainer = child.gameObject;
                    break;

                case "DebugText":
                    if (fromInspector == false)
                        _main.DebugTextGameObject = child.GetComponent<InputField>();
                    break;

                //---------------------------------------------------------------------------------
                case "StartPanel":
                    StartPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<Image>());
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    break;

                case "PlayOfflineButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(27, 12, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(30, 50, child.GetComponent<RectTransform>());
                    break;

                case "LoginButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(27, 12, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(70, 50, child.GetComponent<RectTransform>());
                    break;

                //---------------------------------------------------------------------------------
                case "MainMenuPanel":
                    MainMenuPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(100, 80, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(0, 20, child.GetComponent<RectTransform>());
                    break;

                case "RedBallButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<Button>());
                    UiUtils.SetSize(27, 15, child.GetComponent<Button>());
                    UiUtils.SetPosition(50, 25, child.GetComponent<Button>());
                    break;

                case "RedBall":
                    UiUtils.SetAnchor(AnchorType.Center, child.GetComponent<Image>());
                    UiUtils.SetSize(LayoutType.Square, 69, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(0, 0, child.GetComponent<Image>());
                    break;

                //---------------------------------------------------------------------------------
                case "SettingsPanel":
                    SettingsPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(74, 80, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(13, 20, child.GetComponent<RectTransform>());
                    break;

                case "SettingsBackButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<Button>());
                    UiUtils.SetSize(27, 15, child.GetComponent<Button>());
                    UiUtils.SetPosition(14, 80, child.GetComponent<Button>());
                    break;

                //---------------------------------------------------------------------------------
                case "MapsPanel":
                    MapsPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(74, 80, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(13, 20, child.GetComponent<RectTransform>());
                    break;

                case "MapsBackButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<Button>());
                    UiUtils.SetSize(27, 15, child.GetComponent<Button>());
                    UiUtils.SetPosition(14, 80, child.GetComponent<Button>());
                    break;

                //---------------------------------------------------------------------------------
                case "TopBar":
                    TopBarPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<Image>());
                    UiUtils.SetSize(100, 20, child.GetComponent<Image>());
                    UiUtils.SetPosition(0, 0, child.GetComponent<Image>());
                    break;

                case "FacebookLoginButton":
                    FacebookLoginButton = child.GetComponent<Button>();
                    UiUtils.SetAnchor(AnchorType.TopLeft, FacebookLoginButton);
                    UiUtils.SetSize(20, 60, FacebookLoginButton);
                    UiUtils.SetPosition(2, 20, FacebookLoginButton);
                    break;

                case "ProfilePictureContainer":
                    UiUtils.SetSize(LayoutType.Square, 60, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(2, 20, child.GetComponent<Image>());
                    break;

                case "ProfilePicture":
                    ProfilePicture = child.GetComponent<Image>();
                    UiUtils.SetAnchor(AnchorType.TopLeft, ProfilePicture);
                    UiUtils.SetSize(87, 87, ProfilePicture);
                    UiUtils.SetPosition(7, 7, child.GetComponent<Image>());
                    break;

                case "ProfileName":
                    ProfileName = child.gameObject.GetComponent<Text>();
                    UiUtils.SetSize(30, 60, ProfileName.GetComponent<RectTransform>());
                    UiUtils.SetPosition(10, 20, ProfileName.GetComponent<RectTransform>());
                    break;

                case "SettingsButton":
                    UiUtils.SetAnchor(AnchorType.TopRight, child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetSize(10, 60, child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetPosition(98, 20, child.gameObject.GetComponent<RectTransform>());
                    break;

                case "SettingsButtonCog":
                    UiUtils.SetIconSize(child.GetComponent<Text>());
                    break;

                //---------------------------------------------------------------------------------
                case "GameViewPanel":
                    GameViewPanel = child.gameObject;
                    break;

                case "GameSettingsButton":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.localPosition = new Vector3(_rightPoint - UiUtils.GetPercent(_width, 6), _bottomPoint - (-UiUtils.GetPercent(_height, 90)), 0f);
                    break;

                case "ReloadButton":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.localPosition = new Vector3(_rightPoint - UiUtils.GetPercent(_width, 6), _bottomPoint - (-UiUtils.GetPercent(_height, 70)), 0f);
                    break;

                case "RightController":
                    _rightController = child.gameObject.GetComponent<Button>();
                    _rightController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(UiUtils.GetPercent(_width, 10.30444f), UiUtils.GetPercent(_height, 18.33333f));

                    break;
                case "LeftController":
                    _leftController = child.gameObject.GetComponent<Button>();
                    _leftController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(UiUtils.GetPercent(_width, 10.30444f), UiUtils.GetPercent(_height, 18.33333f));

                    break;
                case "UpController":
                    _upController = child.gameObject.GetComponent<Button>();
                    _upController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(UiUtils.GetPercent(_width, 10.30444f), UiUtils.GetPercent(_height, 18.33333f));

                    break;
                case "DownController":
                    _downController = child.gameObject.GetComponent<Button>();
                    _downController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(UiUtils.GetPercent(_width, 10.30444f), UiUtils.GetPercent(_height, 18.33333f));

                    break;
            }

            if (child.gameObject.name.Contains("ControllerSettings"))
            {
                UiUtils.SetSize(20, 20, child.GetComponent<RectTransform>());
                UiUtils.SetPosition(x, 20, child.GetComponent<RectTransform>());
                x = x + 25;
            }
        }

        if (fromInspector)
        {
            int i;
            allChildren = MapsPanel.GetComponentsInChildren<Transform>();
            List<Transform> children = allChildren.ToList();
            for (i = 0; i < children.Count; i++)
            {
                if (!children[i].gameObject.name.Contains("Clone")) continue;

                DestroyImmediate(children[i].gameObject);
                children.RemoveAt(i);
            }

            var xPos = 8;
            var yPos = 17;

            for (i = 1; i <= 10; i++)
            {
                SetupMap(xPos, yPos, 0);
                xPos = xPos + 21;

                if (i % 5 != 0) continue;

                xPos = 8;
                yPos = yPos + 26;
            }
        }
    }

    public int InspectorScreenWidth;
    public int InspectorScreenHeight;
    public string InspectorScreenName;
    public void RefreshCameraTransform()
    {
        _width = InspectorScreenWidth;
        _height = InspectorScreenHeight;

        var orthographicSize = 4.77f;
        if (Math.Abs(_width - 480) < 5)
        {
            orthographicSize = 5.5f;
        }
        else if (Math.Abs(_width - 854) < 5)
        {
            orthographicSize = 4.7f;
        }
        else if (Math.Abs(_width - 800) < 5)
        {
            orthographicSize = 5;
        }
        else if (Math.Abs(_width - 1024) < 5)
        {
            orthographicSize = 4.8f;
        }
        GetComponent<Camera>().orthographicSize = orthographicSize;

        var xPos = 5.5f;
        var yPos = 4.07f;

        if (Math.Abs(_width - 480) < 5)
        {

        }
        else if (Math.Abs(_width - 854) < 5)
        {
            yPos = 3.98f;
            xPos = 7.6f;
        }
        else if (Math.Abs(_width - 800) < 5)
        {
            xPos = 7.6f;
        }
        else if (Math.Abs(_width - 1024) < 5)
        {
            xPos = 7.6f;
        }
        transform.position = new Vector3(xPos, yPos, -25);
    }

    public void SetupMapsPanel()
    {
        int i;
        var allChildren = MapsPanel.GetComponentsInChildren<Transform>();
        List<Transform> children = allChildren.ToList();
        for (i = 0; i < children.Count; i++)
        {
            if (!children[i].gameObject.name.Contains("Clone")) continue;

            DestroyImmediate(children[i].gameObject);
            children.RemoveAt(i);
        }
        i = 0;

        var xPos = 8;
        var yPos = 17;

        foreach (var map in _main.GameProperties.Maps)
        {
            SetupMap(xPos, yPos, map.Id);
            xPos = xPos + 21;

            i++;
            if (i % 5 != 0) continue;

            xPos = 8;
            yPos = yPos + 26;
        }
    }

    private void SetupMap(float x, float y, int mapId)
    {
        var go = Instantiate(Resources.Load("Prefabs/UI/Map")) as GameObject;
        if (go == null) return;

        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetParent(MapsPanel.transform);

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = new Vector3(1, 1, 1);

        UiUtils.SetSize(LayoutType.Square, 20, rectTransform);
        UiUtils.SetPosition(x, y, rectTransform);

        var but = go.GetComponent<Button>();
        but.onClick = new Button.ButtonClickedEvent();
        but.onClick.AddListener(OpenMap(mapId));
    }

    private UnityAction OpenMap(int mapId)
    {
        return () =>
        {
            _main.InitGame(mapId);
        };
    }

    public void SetupController()
    {
        Image image;
        switch (_main.GameProperties.ControllerType)
        {
            case ControllerType.DefaultPacked:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint, _bottomPoint, 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(
                    _leftPoint + UiUtils.GetPercent(_width, 2.22f), _bottomPoint, 0f);

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_leftPoint,
                    _bottomPoint - (-UiUtils.GetPercent(_height, 18.75f)), 0f);

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 2.22f),
                        _bottomPoint - (-UiUtils.GetPercent(_height, 18.75f)), 0f);

                break;
            case ControllerType.Default:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint,
                    _bottomPoint - (-UiUtils.GetPercent(_height, 18.1f)), 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(
                    _leftPoint + UiUtils.GetPercent(_width, 2.22f), _bottomPoint, 0f);

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_leftPoint,
                    _bottomPoint - (-UiUtils.GetPercent(_height, 18.75f)), 0f);

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 2.22f),
                        _bottomPoint - (-UiUtils.GetPercent(_height, 36.5f)), 0f);

                break;
            //case ControllerType.Zas:
            //    image = _rightController.gameObject.GetComponent<Image>();
            //    image.rectTransform.localPosition = new Vector3(_rightPoint, _bottomPoint, 0f);

            //    image = _downController.gameObject.GetComponent<Image>();
            //    image.rectTransform.localPosition = new Vector3(
            //        _leftPoint + UiUtils.GetPercent(_width, 2.22f),
            //        _bottomPoint - (-UiUtils.GetPercent(_height, 18.75f)), 0f);

            //    image = _leftController.gameObject.GetComponent<Image>();
            //    image.rectTransform.localPosition = new Vector3(_leftPoint, _bottomPoint, 0f);

            //    image = _upController.gameObject.GetComponent<Image>();
            //    image.rectTransform.localPosition =
            //        new Vector3(_rightPoint - UiUtils.GetPercent(_width, 2.22f),
            //            _bottomPoint - (-UiUtils.GetPercent(_height, 18.75f)), 0f);

            //    break;
            case ControllerType.ClassicPacked:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint,
                    _bottomPoint - (-UiUtils.GetPercent(_height, 12.5f)), 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 18.73f), _bottomPoint, 0f); // 267

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(
                    _rightPoint - UiUtils.GetPercent(_width, 21f),
                    _bottomPoint - (-UiUtils.GetPercent(_height, 18.75f)), 0f); // 248

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 2.22f),
                        _bottomPoint - (-UiUtils.GetPercent(_height, 31.2f)), 0f);

                break;
            case ControllerType.Classic:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint,
                    _bottomPoint - (-UiUtils.GetPercent(_height, 15.6f)), 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 18.06f), _bottomPoint, 0f); // 277

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 25.82f),
                        _bottomPoint - (-UiUtils.GetPercent(_height, 15.6f)), 0f); // 215

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - UiUtils.GetPercent(_width, 7.75f),
                        _bottomPoint - (-UiUtils.GetPercent(_height, 33.33f)), 0f);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetupControllerButtons(Sphere sphere)
    {
        // Right
        EventTrigger trigger = _rightController.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { sphere.MoveDirection(Move.Right); });

        trigger.triggers.Clear();
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { sphere.StopDirection(Move.Right); });

        trigger.triggers.Add(entry);

        // Down
        trigger = _downController.gameObject.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { sphere.MoveDirection(Move.Down); });

        trigger.triggers.Clear();
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { sphere.StopDirection(Move.Down); });

        trigger.triggers.Add(entry);

        // Left
        trigger = _leftController.gameObject.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { sphere.MoveDirection(Move.Left); });

        trigger.triggers.Clear();
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { sphere.StopDirection(Move.Left); });

        trigger.triggers.Add(entry);

        // Up
        trigger = _upController.gameObject.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((eventData) => { sphere.MoveDirection(Move.Up); });

        trigger.triggers.Clear();
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((eventData) => { sphere.StopDirection(Move.Up); });

        trigger.triggers.Add(entry);
    }
}
