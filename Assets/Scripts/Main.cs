using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public DataService DataService;

    public GameObject DebugContainer;
    public InputField DebugTextGameObject;

    private Transform _cameraTransform;

    private string _debugText;
    public string DebugText
    {
        get { return _debugText; }
        set
        {
            _debugText = value + Environment.NewLine + _debugText;
            if (DebugContainer.transform.parent.gameObject.activeSelf == false)
                DebugContainer.transform.parent.gameObject.SetActive(true);
            DebugTextGameObject.text = _debugText;
        }
    }

    public Image LoadingImage;

    [Header("Start")]
    public GameObject StartPanel;

    public InputField UserInputField;

    [Header("TobBar")]
    public GameObject TopBar;

    [Header("MainMenu")]
    public GameObject MainMenuPanel;
    public Text ProfileName;

    [Header("Settings")]
    public GameObject SettingsPanel;
    public ControllerType ControllerType;

    [Header("Maps")]
    public GameObject MapsPanel;

    public int CurrentMapId;

    [Header("GameView")]
    private Sphere _sphere;
    public Sphere Sphere
    {
        get { return _sphere; }
        set
        {
            _sphere = value;
            Sphere.Initialize(this);

            // Right
            EventTrigger trigger = RightButton.gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { Sphere.MoveDirection(Move.Right); });

            trigger.triggers.Clear();
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { Sphere.StopDirection(Move.Right); });

            trigger.triggers.Add(entry);

            // Down
            trigger = DownButton.gameObject.GetComponent<EventTrigger>();
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { Sphere.MoveDirection(Move.Down); });

            trigger.triggers.Clear();
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { Sphere.StopDirection(Move.Down); });

            trigger.triggers.Add(entry);

            // Left
            trigger = LeftButton.gameObject.GetComponent<EventTrigger>();
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { Sphere.MoveDirection(Move.Left); });

            trigger.triggers.Clear();
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { Sphere.StopDirection(Move.Left); });

            trigger.triggers.Add(entry);

            // Up
            trigger = UpButton.gameObject.GetComponent<EventTrigger>();
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { Sphere.MoveDirection(Move.Up); });

            trigger.triggers.Clear();
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { Sphere.StopDirection(Move.Up); });

            trigger.triggers.Add(entry);
        }
    }

    public GameObject GameViewPanel;
    public MapGenerator Game;

    public Button RightButton;
    public Button LeftButton;
    public Button DownButton;
    public Button UpButton;

    private float _width;
    private float _height;

    private float _rightPoint;
    private float _leftPoint;
    private float _topPoint;
    private float _bottomPoint;

    // Use this for initialization
    void Start()
    {
        DataService = new DataService("Database.db");
        
        // Attempt to get user
        var user = DataService.GetUser();

        if (user != null)
            Login(user);
        else
        {
            StartPanel.SetActive(true);
            TopBar.SetActive(false);
            MainMenuPanel.SetActive(false);
        }
        
        // panels

        SettingsPanel.SetActive(false);
        MapsPanel.SetActive(false);

        GameViewPanel.SetActive(false);

        Game.Initialize(this, DataService);
        Game.gameObject.SetActive(false);

        // Resolutions.

        _width = Screen.width;
        _height = Screen.height;

        _rightPoint = _width / 2.0f;
        _leftPoint = -_rightPoint;
        _topPoint = _height / 2.0f;
        _bottomPoint = -_topPoint;

        _cameraTransform = transform;

        SetupGameUi();
    }

    public void ButtonClicked(int button)
    {
        ButtonClick buttonClick = (ButtonClick)button;

        //--
        StartLoading(true);

        switch (buttonClick)
        {
            case ButtonClick.NextButton:

                if (string.IsNullOrEmpty(UserInputField.text.TrimEnd()))
                    return;
                Login(null, UserInputField.text);
                break;

            case ButtonClick.SettingsButton:
                MainMenuPanel.SetActive(false);
                MapsPanel.SetActive(false);

                SettingsPanel.SetActive(true);
                break;

            case ButtonClick.RedBallButton:

                MainMenuPanel.SetActive(false);

                var i = 0;
                var xPos = -285f;
                var yPos = 100f;
                
                IEnumerable<Map> maps = DataService.GetMaps();
                foreach (var map in maps)
                {
                    i++;
                    if (i > 7)
                    {
                        xPos = -285f;
                        yPos = yPos - 80f;
                    }

                    CreateMapButton(xPos, yPos, map.Id);

                    xPos = xPos + 85f;
                }
                
                MapsPanel.SetActive(true);
                break;

            case ButtonClick.ReloadButton:

                InitGame();
                break;

            case ButtonClick.SettingsBackButton:
                SettingsPanel.SetActive(false);

                ShowMainMenu();
                break;

            case ButtonClick.MapsBackButton:
                MapsPanel.SetActive(false);

                ShowMainMenu();
                break;

            case ButtonClick.GameSettingsButton:

                GameViewPanel.SetActive(false);
                Game.gameObject.SetActive(false);

                // for now
                ShowMainMenu();

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        //--
        StartLoading(false);
    }

    private void Login(User loggedUser, string name = null)
    {
        StartPanel.SetActive(false);

        if (loggedUser == null)
        {
            DataService.CreateUser(name);
            loggedUser = DataService.GetUser();
        }

        ProfileName.text = loggedUser.Name;
        ControllerType = (ControllerType)loggedUser.ControllerType;

        ShowMainMenu();
    }

    private void CreateMapButton(float x, float y, int mapId)
    {
        var go = Instantiate(Resources.Load("Prefabs/UI/Map")) as GameObject;

        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetParent(MapsPanel.transform);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        rectTransform.sizeDelta = new Vector2(50, 50);

        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.localPosition = new Vector3(x, y, 0);

        var but = go.GetComponent<Button>();
        but.onClick = new Button.ButtonClickedEvent();
        but.onClick.AddListener(OpenMap(mapId));
    }

    private UnityAction OpenMap(int mapId)
    {
        return () =>
        {
            InitGame(mapId);
        };
    }

    public void InitGame(int mapId = 0)
    {
        //--
        StartLoading(true);

        if (mapId == 0)
            mapId = CurrentMapId;

        MapsPanel.SetActive(false);

        Game.CreateMap(mapId);

        TopBar.SetActive(false);

        SetupGameControlls();
        GameViewPanel.SetActive(true);

        Game.gameObject.SetActive(true);

        //--
        StartLoading(false);
    }
    
    public void ShowMainMenu()
    {
        if (Game.CurrentGame)
            Game.CurrentGame.gameObject.SetActive(false);

        MainMenuPanel.SetActive(true);
        TopBar.SetActive(true);
    }

    public void ShowDebugLog()
    {
        _debugText = string.Empty;
        DebugContainer.SetActive(!DebugContainer.activeSelf);
    }

    public void SelectControllerType(int controllerType)
    {
        ControllerType = (ControllerType)controllerType;
        
        DataService.UpdateUserControllerType(controllerType);
        
        ButtonClicked((int)ButtonClick.SettingsBackButton);
    }

    public void SetupGameUi()
    {
        Image image;
        Text text;

        LoadingImage.rectTransform.sizeDelta = new Vector2(_width, _height);

        Transform[] allChildren = _cameraTransform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "TopBarBackground":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.sizeDelta = new Vector3(_width, Logic.GetPercent(_height, 20));
                    image.rectTransform.localPosition = new Vector3(_leftPoint, _topPoint, 0f);
                    break;

                case "ProfileName":
                    text = child.gameObject.GetComponent<Text>();
                    var h = Logic.GetPercent(_height, 20);
                    text.rectTransform.sizeDelta = new Vector3(Logic.GetPercent(_width, 30), Logic.GetPercent(h, 75));
                    text.rectTransform.localPosition = new Vector3(Logic.GetPercent(_width, 3), -h, 0f);
                    break;

                case "SettingsButton":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.localPosition = new Vector3(_rightPoint - Logic.GetPercent(_width, 6), _bottomPoint - (-Logic.GetPercent(_height, 90)), 0f);
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
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
                case "LeftController":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
                case "UpController":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
                case "DownController":
                    image = child.gameObject.GetComponent<Image>();
                    image.rectTransform.sizeDelta = new Vector2(Logic.GetPercent(_width, 10.30444f), Logic.GetPercent(_height, 18.33333f));

                    break;
            }
        }
    }

    private void SetupGameControlls()
    {
        Image image;

        var xPos = 0f;
        var yPos = 4.07f;
        var orthographicSize = 4.77f;

        if (_width == 854)
        {
            yPos = 4.07f;
            if (ControllerType == ControllerType.Default || ControllerType == ControllerType.DefaultPacked ||
                ControllerType == ControllerType.Zas)
            {
                xPos = 5.59f;
            }
            else if (ControllerType == ControllerType.Classic)
            {
                xPos = 7.6f;
            }
            else
            {
                xPos = 7f;
            }
        }

        GetComponent<Camera>().orthographicSize = orthographicSize;
        _cameraTransform.position = new Vector3(xPos, yPos, 0);

        Transform[] allChildren = _cameraTransform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "LeftBackground":
                    //image = child.gameObject.GetComponent<Image>();

                    //if (ControllerType == ControllerType.Default || ControllerType == ControllerType.DefaultPacked ||
                    //    ControllerType == ControllerType.Zas)
                    //{
                    //    _cameraTransform.position = new Vector3(0, 0, 0);
                    //    image.gameObject.SetActive(true);
                    //    image.rectTransform.localPosition = new Vector3(_leftPoint, _bottomPoint, 0f);
                    //}
                    //else
                    //{
                    //    // Camera
                    //    _cameraTransform.position = new Vector3(2.08f, 0, 0);
                    //    image.gameObject.SetActive(false);
                    //}

                    break;

                case "RightBackground":
                    image = child.gameObject.GetComponent<Image>();

                    if (ControllerType == ControllerType.Default || ControllerType == ControllerType.DefaultPacked || ControllerType == ControllerType.Zas)
                    {
                        image.rectTransform.sizeDelta = new Vector3(Logic.GetPercent(_width, 12.4f), _height);
                        image.rectTransform.localPosition = new Vector3(_rightPoint, _bottomPoint, 0f);
                    }
                    //else if (ControllerType == ControllerType.ClassicPacked)
                    //{
                    //    image.rectTransform.sizeDelta = new Vector3(Logic.GetPercent(_width, 20.6f), _height);
                    //}
                    //else
                    //{
                    //    image.rectTransform.sizeDelta = new Vector3(Logic.GetPercent(_width, 25.82f), _height);
                    //}

                    break;

                case "RightController":
                    image = child.gameObject.GetComponent<Image>();
                    switch (ControllerType)
                    {
                        case ControllerType.DefaultPacked:
                            image.rectTransform.localPosition = new Vector3(_rightPoint, _bottomPoint, 0f);
                            break;
                        case ControllerType.Default:
                            image.rectTransform.localPosition = new Vector3(_rightPoint,
                                _bottomPoint - (-Logic.GetPercent(_height, 18.1f)), 0f);
                            break;
                        case ControllerType.Zas:
                            image.rectTransform.localPosition = new Vector3(_rightPoint, _bottomPoint, 0f);
                            break;
                        case ControllerType.ClassicPacked:
                            image.rectTransform.localPosition = new Vector3(_rightPoint,
                                _bottomPoint - (-Logic.GetPercent(_height, 12.5f)), 0f);
                            break;
                        case ControllerType.Classic:
                            image.rectTransform.localPosition = new Vector3(_rightPoint,
                                _bottomPoint - (-Logic.GetPercent(_height, 15.6f)), 0f);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                case "LeftController":
                    image = child.gameObject.GetComponent<Image>();
                    switch (ControllerType)
                    {
                        case ControllerType.DefaultPacked:
                            image.rectTransform.localPosition = new Vector3(_leftPoint,
                                _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);
                            break;
                        case ControllerType.Default:
                            image.rectTransform.localPosition = new Vector3(_leftPoint,
                                _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);
                            break;
                        case ControllerType.Zas:
                            image.rectTransform.localPosition = new Vector3(_leftPoint, _bottomPoint, 0f);
                            break;
                        case ControllerType.ClassicPacked:
                            image.rectTransform.localPosition = new Vector3(
                                _rightPoint - Logic.GetPercent(_width, 21f),
                                _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f); // 248
                            break;
                        case ControllerType.Classic:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 25.82f),
                                    _bottomPoint - (-Logic.GetPercent(_height, 15.6f)), 0f); // 215
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                case "UpController":
                    image = child.gameObject.GetComponent<Image>();
                    switch (ControllerType)
                    {
                        case ControllerType.DefaultPacked:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                                    _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);
                            break;
                        case ControllerType.Default:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                                    _bottomPoint - (-Logic.GetPercent(_height, 36.5f)), 0f);
                            break;
                        case ControllerType.Zas:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                                    _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);
                            break;
                        case ControllerType.ClassicPacked:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 2.22f),
                                    _bottomPoint - (-Logic.GetPercent(_height, 31.2f)), 0f);
                            break;
                        case ControllerType.Classic:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 7.75f),
                                    _bottomPoint - (-Logic.GetPercent(_height, 33.33f)), 0f);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case "DownController":
                    image = child.gameObject.GetComponent<Image>();
                    switch (ControllerType)
                    {
                        case ControllerType.DefaultPacked:
                            image.rectTransform.localPosition = new Vector3(
                                _leftPoint + Logic.GetPercent(_width, 2.22f), _bottomPoint, 0f);
                            break;
                        case ControllerType.Default:
                            image.rectTransform.localPosition = new Vector3(
                                _leftPoint + Logic.GetPercent(_width, 2.22f), _bottomPoint, 0f);
                            break;
                        case ControllerType.Zas:
                            image.rectTransform.localPosition = new Vector3(
                                _leftPoint + Logic.GetPercent(_width, 2.22f),
                                _bottomPoint - (-Logic.GetPercent(_height, 18.75f)), 0f);
                            break;
                        case ControllerType.ClassicPacked:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 18.73f), _bottomPoint, 0f); // 267
                            break;
                        case ControllerType.Classic:
                            image.rectTransform.localPosition =
                                new Vector3(_rightPoint - Logic.GetPercent(_width, 18.06f), _bottomPoint, 0f); // 277
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
            }
        }
    }
    
    public void StartLoading(bool value)
    {
        if (LoadingImage.gameObject.activeSelf == value) return;

        Debug.Log(" - " + LoadingImage.gameObject.activeSelf);
        
        LoadingImage.gameObject.SetActive(value);
        Debug.Log(LoadingImage.gameObject.activeSelf);
    }
}