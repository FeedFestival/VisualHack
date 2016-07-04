using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
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

    [Header("GameView")]
    public Sphere Sphere;
    public GameObject GameViewPanel;
    public GameObject Game;

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
        DataService.CreateDB();

        // panels
        StartPanel.SetActive(true);
        TopBar.SetActive(false);
        MainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MapsPanel.SetActive(false);

        GameViewPanel.SetActive(false);
        Game.SetActive(false);

        // Resolutions.

        _width = Screen.width;
        _height = Screen.height;

        _rightPoint = _width / 2.0f;
        _leftPoint = -_rightPoint;
        _topPoint = _height / 2.0f;
        _bottomPoint = -_topPoint;

        _cameraTransform = transform;

        SetupGameUi();

        Sphere.Initialize(this);
    }

    public void ButtonClicked(int button)
    {
        ButtonClick buttonClick = (ButtonClick)button;

        switch (buttonClick)
        {
            case ButtonClick.NextButton:
                if (string.IsNullOrEmpty(UserInputField.text.TrimEnd()))
                    return;

                DataService.CreateUser(UserInputField.text);

                StartPanel.SetActive(false);

                var user = DataService.GetUser();
                ProfileName.text = user.Name;
                ControllerType = (ControllerType)user.ControllerType;

                ShowMainMenu();
                break;

            case ButtonClick.SettingsButton:
                MainMenuPanel.SetActive(false);
                MapsPanel.SetActive(false);

                SettingsPanel.SetActive(true);
                break;

            case ButtonClick.RedBallButton:
                MainMenuPanel.SetActive(false);

                MapsPanel.SetActive(true);
                break;

            case ButtonClick.Map:
                MapsPanel.SetActive(false);

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
                Game.SetActive(false);

                // for now
                ShowMainMenu();

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ShowMainMenu()
    {
        MainMenuPanel.SetActive(true);
        TopBar.SetActive(true);
    }

    private void InitGame()
    {
        TopBar.SetActive(false);

        SetupGameControlls();
        GameViewPanel.SetActive(true);

        Game.SetActive(true);
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

        if (ControllerType == ControllerType.Default || ControllerType == ControllerType.DefaultPacked ||
            ControllerType == ControllerType.Zas)
        {
            if (_width == 854)
            {
                _cameraTransform.position = new Vector3(0.09f, -0.07f, 0);
                GetComponent<Camera>().orthographicSize = 4.7f;
            }
        }
        else if (ControllerType == ControllerType.Classic)
        {
            if (_width == 854)
            {
                _cameraTransform.position = new Vector3(2.25f, -0.07f, 0);
                GetComponent<Camera>().orthographicSize = 4.77f;
            }
        }
        else
        {
            if (_width == 854)
            {
                _cameraTransform.position = new Vector3(1.4f, -0.07f, 0);
                GetComponent<Camera>().orthographicSize = 4.77f;
            }
        }

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
}