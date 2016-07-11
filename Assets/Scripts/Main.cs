using System;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Types;

public class Main : MonoBehaviour
{
    [HideInInspector]
    public DataService DataService;
    [HideInInspector]
    public GameProperties GameProperties;
    [HideInInspector]
    public LoadingController LoadingController;

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

            _gameUi.SetupControllerButtons(Sphere);
        }
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

        // Init all UI variables
        _gameUi = GetComponent<GameUi>();
        _gameUi.Initialize(this);

        // Attempt to get user
        var user = DataService.GetUser();

        if (user != null)
            Login(user);
        else
        {
            _gameUi.StartPanel.SetActive(true);
            _gameUi.TopBarPanel.SetActive(false);
            _gameUi.MainMenuPanel.SetActive(false);
        }
        _gameUi.SettingsPanel.SetActive(false);
        _gameUi.MapsPanel.SetActive(false);
        _gameUi.GameViewPanel.SetActive(false);

        _mapGenerator.Initialize(this, DataService);
        _mapGenerator.gameObject.SetActive(false);
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
            case ButtonClick.NextButton:

                if (string.IsNullOrEmpty(_gameUi.UserInputField.text.TrimEnd()))
                    return;
                Login(null, _gameUi.UserInputField.text);
                break;

            case ButtonClick.SettingsButton:
                _gameUi.MainMenuPanel.SetActive(false);
                _gameUi.MapsPanel.SetActive(false);

                _gameUi.SettingsPanel.SetActive(true);
                break;

            case ButtonClick.RedBallButton:

                _gameUi.MainMenuPanel.SetActive(false);

                if (GameProperties.Maps == null || GameProperties.Maps.Any())
                {
                    GameProperties.Maps = DataService.GetMaps();

                    _gameUi.SetupMapsPanel();
                }

                _gameUi.MapsPanel.SetActive(true);
                break;

            case ButtonClick.SettingsBackButton:
                _gameUi.SettingsPanel.SetActive(false);

                ShowMainMenu();
                break;

            case ButtonClick.MapsBackButton:
                _gameUi.MapsPanel.SetActive(false);

                ShowMainMenu();
                break;

            case ButtonClick.GameSettingsButton:

                _gameUi.GameViewPanel.SetActive(false);
                _mapGenerator.gameObject.SetActive(false);

                // for now
                ShowMainMenu();

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        StartCoroutine(LoadingController.FinishLoaderWait());
    }

    private void Login(User loggedUser, string username = null)
    {
        _gameUi.StartPanel.SetActive(false);

        if (loggedUser == null)
        {
            var user = new User
            {
                Name = username,
                ControllerType = (int)ControllerType.Classic
            };

            DataService.CreateUser(user);
            loggedUser = DataService.GetUser();
        }

        _gameUi.ProfileName.text = loggedUser.Name;
        GameProperties.ControllerType = (ControllerType)loggedUser.ControllerType;

        ShowMainMenu();
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
    }

    public void SelectControllerType(int controllerType)
    {
        GameProperties.ControllerType = (ControllerType)controllerType;

        DataService.UpdateUserControllerType(controllerType);

        ButtonClicked((int)ButtonClick.SettingsBackButton);
    }
}