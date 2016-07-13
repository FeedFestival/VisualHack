using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
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

    private Image _profilePictureContainer, _topBarBackground;

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

        _rightPoint = _width / 2.0f;
        _leftPoint = -_rightPoint;
        _topPoint = _height / 2.0f;
        _bottomPoint = -_topPoint;

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            GameObject go;
            Image image;
            float h, wSize, hSize;
            switch (child.gameObject.name)
            {
                case "Canvas":
                    if (fromInspector)
                    {
                        go = child.gameObject;
                        go.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _height);
                    }
                    break;

                case "LoadingIcon":
                    if (fromInspector)
                    {
                        go = child.gameObject;
                        go.GetComponent<RectTransform>().localPosition = new Vector3(
                                                Logic.GetPercent(_width / 2, 80),
                                                -Logic.GetPercent(_height / 2, 70),
                                                0
                                                );
                        var image2 = go.transform.parent.GetComponent<Image>();
                        image2.rectTransform.sizeDelta = new Vector2(_width, _height);
                    }
                    else
                        _main.LoadingController.LoadingIcon = child.gameObject;
                    break;

                case "LoadingIconText":
                    if (fromInspector == false)
                        _main.LoadingController.LoadingIconText = child.gameObject.GetComponent<Text>();
                    break;

                case "LoadingIconCircle":
                    if (fromInspector == false)
                        _main.LoadingController.LoadingIconCircle = child.gameObject.GetComponent<Text>();
                    break;

                case "DebugContainer":
                    DebugContainer = child.gameObject;
                    break;

                case "DebugText":
                    if (fromInspector == false)
                        _main.DebugTextGameObject = child.GetComponent<InputField>();
                    break;

                //----------------------------------
                case "StartPanel":
                    StartPanel = child.gameObject;
                    break;

                case "FacebookLoginButton":
                    FacebookLoginButton = child.GetComponent<Button>();
                    //wSize = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.x, 30);
                    hSize = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.y, 60);
                    h = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.y, 20);

                    FacebookLoginButton.GetComponent<RectTransform>().sizeDelta = new Vector3(200, hSize);
                    FacebookLoginButton.GetComponent<RectTransform>().localPosition = new Vector3(Logic.GetPercent(_width, 2), -h, 0f);
                    break;

                case "MainMenuPanel":
                    MainMenuPanel = child.gameObject;
                    break;

                case "SettingsPanel":
                    SettingsPanel = child.gameObject;
                    break;

                case "MapsPanel":
                    MapsPanel = child.gameObject;
                    break;

                case "TopBar":
                    TopBarPanel = child.gameObject;
                    break;

                case "TopBarBackground":
                    _topBarBackground = child.GetComponent<Image>();
                    _topBarBackground.rectTransform.sizeDelta = new Vector3(_width, Logic.GetPercent(_height, 20));
                    _topBarBackground.rectTransform.localPosition = new Vector3(_leftPoint, _topPoint, 0f);
                    break;

                case "ProfilePictureContainer":
                    _profilePictureContainer = child.GetComponent<Image>();

                    hSize = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.y, 60);
                    h = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.y, 20);

                    _profilePictureContainer.rectTransform.sizeDelta = new Vector3(hSize, hSize);
                    _profilePictureContainer.rectTransform.localPosition = new Vector3(Logic.GetPercent(_width, 2), -h, 0f);
                    break;

                case "ProfilePicture":
                    ProfilePicture = child.GetComponent<Image>();

                    hSize = Logic.GetPercent(_profilePictureContainer.rectTransform.sizeDelta.y, 90);

                    ProfilePicture.rectTransform.sizeDelta = new Vector3(hSize, hSize);
                    break;

                case "ProfileName":
                    ProfileName = child.gameObject.GetComponent<Text>();

                    wSize = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.x, 30);
                    hSize = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.y, 60);
                    h = Logic.GetPercent(_topBarBackground.rectTransform.sizeDelta.y, 20);

                    ProfileName.rectTransform.sizeDelta = new Vector3(wSize, hSize);
                    ProfileName.rectTransform.localPosition = new Vector3(Logic.GetPercent(_width, 10), -h, 0f);
                    break;
                    
                case "SettingsButton":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.localPosition = new Vector3(_rightPoint - Logic.GetPercent(_width, 6), _bottomPoint - (-Logic.GetPercent(_height, 90)), 0f);
                    break;

                case "GameViewPanel":
                    GameViewPanel = child.gameObject;
                    break;

                case "GameSettingsButton":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.localPosition = new Vector3(_rightPoint - Logic.GetPercent(_width, 6), _bottomPoint - (-Logic.GetPercent(_height, 90)), 0f);
                    break;

                case "ReloadButton":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.localPosition = new Vector3(_rightPoint - Logic.GetPercent(_width, 6), _bottomPoint - (-Logic.GetPercent(_height, 70)), 0f);
                    break;

                case "RightController":
                    _rightController = child.gameObject.GetComponent<Button>();
                    _rightController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
                case "LeftController":
                    _leftController = child.gameObject.GetComponent<Button>();
                    _leftController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
                case "UpController":
                    _upController = child.gameObject.GetComponent<Button>();
                    _upController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
                case "DownController":
                    _downController = child.gameObject.GetComponent<Button>();
                    _downController.gameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
            }
        }
    }

    public int InspectorScreenWidth;
    public int InspectorScreenHeight;
    public string InspectorScreenName;
    public void RefreshCameraTransform()
    {
        _width = (float)InspectorScreenWidth;
        _height = (float)InspectorScreenHeight;

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
        var i = 0;
        var xPos = -285f;
        var yPos = 100f;

        foreach (var map in _main.GameProperties.Maps)
        {
            i++;
            if (i > 7)
            {
                xPos = -285f;
                yPos = yPos - 80f;
            }

            var go = Instantiate(Resources.Load("Prefabs/UI/Map")) as GameObject;
            if (go == null) return;

            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.SetParent(MapsPanel.transform);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            rectTransform.sizeDelta = new Vector2(50, 50);

            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.localPosition = new Vector3(xPos, yPos, 0);

            var but = go.GetComponent<Button>();
            but.onClick = new Button.ButtonClickedEvent();
            but.onClick.AddListener(OpenMap(map.Id));

            xPos = xPos + 85f;
        }
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
                    _leftPoint + Logic.GetPercent(_width, 2.22f), _bottomPoint, 0f);

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_leftPoint,
                    _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                        _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);

                break;
            case ControllerType.Default:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint,
                    _bottomPoint - (-Logic.GetPercent(_height, 18.1f)), 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(
                    _leftPoint + Logic.GetPercent(_width, 2.22f), _bottomPoint, 0f);

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_leftPoint,
                    _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                        _bottomPoint - (-Logic.GetPercent(_height, 36.5f)), 0f);

                break;
            case ControllerType.Zas:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint, _bottomPoint, 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(
                    _leftPoint + Logic.GetPercent(_width, 2.22f),
                    _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_leftPoint, _bottomPoint, 0f);

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                        _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);

                break;
            case ControllerType.ClassicPacked:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint,
                    _bottomPoint - (-Logic.GetPercent(_height, 12.5f)), 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 18.73f), _bottomPoint, 0f); // 267

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(
                    _rightPoint - Logic.GetPercent(_width, 21f),
                    _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f); // 248

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                        _bottomPoint - (-Logic.GetPercent(_height, 31.2f)), 0f);

                break;
            case ControllerType.Classic:
                image = _rightController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition = new Vector3(_rightPoint,
                    _bottomPoint - (-Logic.GetPercent(_height, 15.6f)), 0f);

                image = _downController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 18.06f), _bottomPoint, 0f); // 277

                image = _leftController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 25.82f),
                        _bottomPoint - (-Logic.GetPercent(_height, 15.6f)), 0f); // 215

                image = _upController.gameObject.GetComponent<Image>();
                image.rectTransform.localPosition =
                    new Vector3(_rightPoint - Logic.GetPercent(_width, 7.75f),
                        _bottomPoint - (-Logic.GetPercent(_height, 33.33f)), 0f);

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
