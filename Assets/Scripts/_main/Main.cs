﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [HideInInspector]
    public DataService DataService;
    [HideInInspector]
    public GameProperties GameProperties;
    [HideInInspector]
    public LoadingController LoadingController;

    public FacebookController FacebookController;

    private User _loggedUser;

    private GameUi _gameUi;

    private MapGenerator _mapGenerator;

    private ButtonClick _buttonClick;

    private Sphere _sphere;
    public Sphere Sphere
    {
        get { return _sphere; }
        set
        {
            _sphere = value;
            Sphere.Initialize(this);

            if (_gameUi != null)
                _gameUi.SetupControllerButtons(Sphere);
        }
    }

    [HideInInspector]
    public GameObject DebugButton;

    [HideInInspector]
    public Text DebugTextGameObject;
    private string _debugText;
    public string DebugText
    {
        get { return _debugText; }
        set
        {
            var time = "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " ." + DateTime.Now.Second + "]";

            _debugText = time + value + Environment.NewLine + _debugText;

            DebugTextGameObject.text = _debugText;
        }
    }

    public GameObject AdBanner;

    public GameObject Canvas;

    // Map
    public int MaxX = 11;
    public int MaxY = 8;
    public MapTile[,] Tiles;

    public void ClearDebugLog()
    {
        _debugText = string.Empty;
        //DebugContainer.SetActive(!DebugContainer.activeSelf);
    }

    // Use this for initialization
    void Start()
    {
        var go = GameObject.FindWithTag("GameScene");
        DestroyImmediate(go);
        _mapGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>();

        DataService = new DataService("Database.db");

        LoadingController = GetComponent<LoadingController>();
        LoadingController.Initialize(this);

        GameProperties = GetComponent<GameProperties>();
        GameProperties.Initialize(this);

        _gameUi = GetComponent<GameUi>();
        _gameUi.Initialize(this);

        /*
         * Facebook
         */
        FacebookController = GetComponent<FacebookController>();
        FacebookController.Initialize(this);

        _loggedUser = DataService.GetUser();
        FacebookInitializationComplete();

        _gameUi.SettingsPanel.SetActive(false);
        _gameUi.MapsPanel.SetActive(false);
        _gameUi.GameViewPanel.SetActive(false);

        _mapGenerator.Initialize(this, DataService);
        _mapGenerator.gameObject.SetActive(false);
    }

    public void FacebookInitializationComplete(User user = null)
    {
        if (user != null)
        {
            if (_loggedUser == null)
            {
                user.ControllerType = (int)ControllerType.Classic;

                DataService.CreateUser(user);
            }
            else
            {
                user.Id = _loggedUser.Id;
                user.IsUsingSound = _loggedUser.IsUsingSound;

                DataService.UpdateUser(user);
            }
            _loggedUser = DataService.GetLastUser();
        }

        if (_loggedUser == null)
        {
            _gameUi.StartPanel.SetActive(true);
            _gameUi.TopBarPanel.SetActive(false);
            _gameUi.MainMenuPanel.SetActive(false);
        }
        else
        {
            if (_loggedUser.FacebookApp != null && user == null)
                SetProfilePicture();
            else if (user != null)
                FacebookController.GetProfilePicture();

            Login();
        }
    }

    private void Login(bool isNew = false)
    {
        _gameUi.StartPanel.SetActive(false);

        if (isNew)
        {
            DataService.CreateUser(_loggedUser);
            _loggedUser = DataService.GetLastUser();
        }

        if (_loggedUser.FacebookApp == null)
        {
            _gameUi.ProfileName.gameObject.SetActive(false);

            _gameUi.ProfilePicture.gameObject.SetActive(false);
            _gameUi.ProfilePicture.transform.parent.gameObject.SetActive(false);

            _gameUi.FacebookLoginButton.gameObject.SetActive(true);
        }
        else
        {
            _gameUi.FacebookLoginButton.gameObject.SetActive(false);

            _gameUi.ProfileName.text = _loggedUser.Name;

            _gameUi.ProfileName.gameObject.SetActive(true);
            _gameUi.ProfilePicture.gameObject.SetActive(true);
            _gameUi.ProfilePicture.transform.parent.gameObject.SetActive(true);
        }
        GameProperties.ControllerType = (ControllerType)_loggedUser.ControllerType;

        ShowMainMenu();
    }

    public void SetProfilePicture(Texture2D texture = null)
    {
        string picName = Utils.GetProfilePictureName(_loggedUser.Name, _loggedUser.FacebookApp.FacebookId);

        if (texture != null)
            Utils.SavePic(texture, texture.width, texture.height, picName);

        texture = Utils.ReadPic(picName);
        DebugText = picName;

        _gameUi.ProfilePicture.sprite = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2());
    }

    public void ButtonClicked(int button)
    {
        _buttonClick = (ButtonClick)button;

        // for realoadButton we need custom logic so that the loader behaves corectly.
        if (_buttonClick == ButtonClick.ReloadButton)
        {
            InitGame(0, true);
            return;
        }

        StartCoroutine(LoadingController.LoaderWaitThenExecute(LoadThenExecute.Button));
    }

    public void ExecuteButtonAction()
    {
        switch (_buttonClick)
        {
            case ButtonClick.PlayOfflineButton:

                _loggedUser = new User
                {
                    Name = "Anonymouse",
                    ControllerType = (int)ControllerType.Classic
                };

                Login(true);
                break;

            case ButtonClick.SettingsButton:
                _gameUi.MainMenuPanel.SetActive(false);
                _gameUi.MapsPanel.SetActive(false);

                _gameUi.SettingsPanel.SetActive(true);
                if (FacebookController.IsShowingBanner == false)
                    FacebookController.ShowAndroidBanner(true);
                break;

            case ButtonClick.RedBallButton:

                _gameUi.MainMenuPanel.SetActive(false);

                if (GameProperties.Maps == null || GameProperties.Maps.Any())
                {
                    GameProperties.Maps = DataService.GetMaps();

                    _gameUi.SetupMapsPanel();
                }

                _gameUi.MapsPanel.SetActive(true);
                if (FacebookController.IsShowingBanner == false)
                    FacebookController.ShowAndroidBanner(true);
                break;

            case ButtonClick.SettingsBackButton:
                _gameUi.SettingsPanel.SetActive(false);

                ShowMainMenu();
                break;

            case ButtonClick.MapsBackButton:
                _gameUi.MapsPanel.SetActive(false);

                FacebookController.ShowAndroidBanner(true);

                ShowMainMenu();
                break;

            case ButtonClick.GameSettingsButton:

                _gameUi.GameViewPanel.SetActive(false);
                _mapGenerator.gameObject.SetActive(false);

                // for now
                ShowMainMenu();

                break;

            case ButtonClick.LoginButton:

                FacebookController.Login();

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        StartCoroutine(LoadingController.FinishLoaderWait());
    }

    public void InitGame(int mapId, bool reload = false)
    {
        _mapGenerator.SetupCurrentMap(mapId);

        StartCoroutine(reload
            ? LoadingController.LoaderWaitThenExecute(LoadThenExecute.MapLoad)
            : LoadingController.LoadMapWithText(_mapGenerator.CurrentMap.Name));
    }

    public void ExecuteInitGame()
    {
        _mapGenerator.CreateMap();

        _gameUi.MapsPanel.SetActive(false);
        _gameUi.TopBarPanel.SetActive(false);

        if (FacebookController.IsShowingBanner)
            FacebookController.ShowAndroidBanner(false);

        GameProperties.SetupCamera();
        _gameUi.SetupController();

        _gameUi.GameViewPanel.SetActive(true);
        _mapGenerator.gameObject.SetActive(true);

        StartCoroutine(LoadingController.FinishLoaderWait());
    }

    public int GetNextMapId()
    {
        return DataService.GetNextMapId(_mapGenerator.CurrentMap.Number + 1);
    }

    public void ShowMainMenu()
    {
        if (_mapGenerator.CurrentMap != null && _mapGenerator.CurrentMap.GameObject)
            _mapGenerator.CurrentMap.GameObject.SetActive(false);

        _gameUi.MainMenuPanel.SetActive(true);
        _gameUi.TopBarPanel.SetActive(true);

        if (FacebookController.IsShowingBanner == false)
            FacebookController.ShowAndroidBanner(true);
    }

    public void SelectControllerType(int controllerType)
    {
        GameProperties.ControllerType = (ControllerType)controllerType;

        var user = new User
        {
            Id = 1,
            ControllerType = controllerType
        };
        DataService.UpdateUser(user);

        ButtonClicked((int)ButtonClick.SettingsBackButton);
    }

    public void ShowDebugLog(bool value)
    {
        _gameUi.DebugContainer.transform.parent.gameObject.SetActive(value);
        DebugButton.SetActive(!value);

        if (value)
        {
            _gameUi.StartPanel.SetActive(false);
            _gameUi.MainMenuPanel.SetActive(false);
            _gameUi.TopBarPanel.SetActive(false);
            _gameUi.SettingsPanel.SetActive(false);
            _gameUi.MapsPanel.SetActive(false);
            _gameUi.GameViewPanel.SetActive(false);

            return;
        }
        ShowMainMenu();
    }
}