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
            switch (child.gameObject.name)
            {
                case "Canvas":
                    if (fromInspector == false)
                        _main.Canvas = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    break;

                case "AdBanner":
                    if (fromInspector == false)
                        _main.AdBanner = child.gameObject;
                    UiUtils.SetFixedSize(UiUtils.XPercent(100), 50, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(50, 500, child.GetComponent<RectTransform>());
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
                    UiUtils.SetIconSize(child.GetComponent<Text>());
                    if (fromInspector == false)
                        _main.LoadingController.LoadingIconCircle = child.GetComponent<Text>();
                    break;

                //---------------------------------------------------------------------------------
                case "DebugButton":
                    if (fromInspector == false)
                        _main.DebugButton = child.gameObject;
                    UiUtils.SetSize(5, LayoutType.Square, child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetPosition(5, 30, child.gameObject.GetComponent<RectTransform>());
                    break;

                case "DebugButtonText":
                    UiUtils.SetIconSize(child.GetComponent<Text>());
                    break;
                    
                case "_DebugPanel":
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    break;

                case "DebugContainer":
                    DebugContainer = child.gameObject;

                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(0, 0, child.gameObject.GetComponent<RectTransform>());

                    DebugContainer.transform.parent.gameObject.SetActive(false);
                    break;

                case "DebugText":
                    if (fromInspector == false)
                        _main.DebugTextGameObject = child.GetComponent<Text>();

                    UiUtils.SetSize(98, 98, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(2, 2, child.gameObject.GetComponent<RectTransform>());
                    
                    break;

                case "DebugScrollImage":

                    UiUtils.SetSize(95, 80, child.GetComponent<RectTransform>());
                    UiUtils.SetPosition(3, 18, child.gameObject.GetComponent<RectTransform>());
                    break;

                case "DebugBackButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<Button>());
                    UiUtils.SetSize(27, 15, child.GetComponent<Button>());
                    UiUtils.SetPosition(15, 10, child.GetComponent<Button>());
                    break;

                //---------------------------------------------------------------------------------
                case "StartPanel":
                    StartPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.GetComponent<Image>());
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());
                    break;

                case "PlayOfflineButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(27, 12, child.GetComponent<Button>());
                    UiUtils.SetPosition(30, 50, child.GetComponent<RectTransform>());
                    break;

                case "LoginButton":
                    UiUtils.SetAnchor(AnchorType.LeftCenter, child.GetComponent<RectTransform>());
                    UiUtils.SetSize(27, 12, child.GetComponent<Button>());
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

                    UiUtils.SetPosition(14, 75, child.GetComponent<Button>());
                    //UiUtils.SetFixedPosition(UiUtils.GetPercent(child.parent.GetComponent<RectTransform>().sizeDelta.x, 14),
                    //    (_height - child.GetComponent<RectTransform>().sizeDelta.y) - 125,
                    //    child.GetComponent<RectTransform>());
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

                    UiUtils.SetPosition(14, 75, child.GetComponent<Button>());
                    //UiUtils.SetFixedPosition(UiUtils.GetPercent(child.parent.GetComponent<RectTransform>().sizeDelta.x, 14),
                    //    (_height - child.GetComponent<RectTransform>().sizeDelta.y) - 125,
                    //    child.GetComponent<RectTransform>());
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
                    ProfileName = child.GetComponent<Text>();
                    UiUtils.SetSize(30, 60, ProfileName.GetComponent<RectTransform>());
                    UiUtils.SetPosition(10, 20, ProfileName.GetComponent<RectTransform>());
                    UiUtils.SetTextSize(child.GetComponent<Text>());
                    break;

                case "SettingsButton":
                    UiUtils.SetAnchor(AnchorType.TopRight, child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetSize(10, 60, child.gameObject.GetComponent<Button>());
                    UiUtils.SetPosition(98, 20, child.gameObject.GetComponent<RectTransform>());
                    break;

                case "SettingsButtonCog":
                    UiUtils.SetIconSize(child.GetComponent<Text>());
                    break;

                //---------------------------------------------------------------------------------
                case "GameViewPanel":
                    GameViewPanel = child.gameObject;
                    UiUtils.SetAnchor(AnchorType.TopLeft, child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetSize(LayoutType.Full, child.GetComponent<RectTransform>());

                    break;

                case "GameSettingsButton":
                    UiUtils.SetAnchor(AnchorType.TopRight, child.gameObject.GetComponent<RectTransform>());

                    UiUtils.SetFixedSize(UiUtils.XPercent(10),
                        UiUtils.GetPercent(TopBarPanel.GetComponent<RectTransform>().sizeDelta.y, 60),
                        child.gameObject.GetComponent<RectTransform>());

                    UiUtils.SetFixedPosition(UiUtils.XPercent(98),
                        UiUtils.GetPercent(TopBarPanel.GetComponent<RectTransform>().sizeDelta.y, 20),
                        child.gameObject.GetComponent<RectTransform>());
                    break;

                case "GameSettingsButtonText":
                    UiUtils.SetIconSize(child.GetComponent<Text>());
                    break;

                case "ReloadButton":
                    UiUtils.SetAnchor(AnchorType.TopRight, child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetFixedSize(UiUtils.XPercent(10),
                        UiUtils.GetPercent(TopBarPanel.GetComponent<RectTransform>().sizeDelta.y, 60),
                        child.gameObject.GetComponent<RectTransform>());
                    UiUtils.SetFixedPosition(UiUtils.XPercent(98),
                        UiUtils.GetPercent(TopBarPanel.GetComponent<RectTransform>().sizeDelta.y, 100),
                        child.gameObject.GetComponent<RectTransform>());
                    break;

                case "ReloadButtonText":
                    UiUtils.SetIconSize(child.GetComponent<Text>());
                    break;

                case "RightController":
                    _rightController = child.GetComponent<Button>();
                    UiUtils.SetSize(LayoutType.Square, 18.33333f, child.GetComponent<RectTransform>());
                    break;
                case "LeftController":
                    _leftController = child.GetComponent<Button>();
                    UiUtils.SetSize(LayoutType.Square, 18.33333f, child.GetComponent<RectTransform>());
                    break;
                case "UpController":
                    _upController = child.GetComponent<Button>();
                    UiUtils.SetSize(LayoutType.Square, 18.33333f, child.GetComponent<RectTransform>());
                    break;
                case "DownController":
                    _downController = child.GetComponent<Button>();
                    UiUtils.SetSize(LayoutType.Square, 18.33333f, child.GetComponent<RectTransform>());
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
            SetupController(true);

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
    public ControllerType InspectorControllerType;
    public void RefreshCameraTransform()
    {
        _width = InspectorScreenWidth;
        _height = InspectorScreenHeight;

        var orthographicSize = 4.77f;
        var xPos = 5.5f;
        var yPos = 3.98f;

        switch (UiUtils.GetAspectRatio())
        {
            case AspectRatio.Unregistered:
                break;
            case AspectRatio.Aspect_16_9:

                GetComponent<Camera>().orthographicSize = orthographicSize;

                switch (InspectorControllerType)
                {
                    case ControllerType.DefaultPacked:
                    case ControllerType.Default:
                    case ControllerType.Zas:
                        break;
                    case ControllerType.ClassicPacked:
                        xPos = 7f;
                        break;
                    case ControllerType.Classic:
                        xPos = 7.6f;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
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
        return () => { _main.InitGame(mapId); };
    }

    private const float BottomSpace = 81.25f;
    private const float LeftSpace = 2.22f;
    private const float RightSpace = 100 - 2.22f;

    public void SetupController(bool isFromInspector = false)
    {
        switch (isFromInspector ? InspectorControllerType : _main.GameProperties.ControllerType)
        {
            case ControllerType.DefaultPacked:
                UiUtils.SetPosition(100, 100, _rightController);
                UiUtils.SetPosition(RightSpace, BottomSpace, _upController);

                UiUtils.SetPosition(LeftSpace, 100, _downController);
                UiUtils.SetPosition(0, BottomSpace, _leftController);
                break;
            case ControllerType.Default:
                UiUtils.SetPosition(100, BottomSpace, _rightController);
                UiUtils.SetPosition(RightSpace, 100 - 37.375f, _upController);

                UiUtils.SetPosition(LeftSpace, 100, _downController);
                UiUtils.SetPosition(0, BottomSpace, _leftController);
                break;
            case ControllerType.ClassicPacked:
                UiUtils.SetPosition(100, 100 - 12.5f, _rightController);
                UiUtils.SetPosition(BottomSpace, 100, _downController);
                UiUtils.SetPosition(100 - 21f, BottomSpace, _leftController);
                UiUtils.SetPosition(RightSpace, 100 - 31.2f, _upController);
                break;
            case ControllerType.Classic:
                UiUtils.SetPosition(100, 100 - 14f, _rightController);
                UiUtils.SetPosition(100 - 18.1f, 100, _downController);
                UiUtils.SetPosition(100 - 26f, 100 - 14f, _leftController);
                UiUtils.SetPosition(100 - 7.75f, 100 - 31.2f, _upController);
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
