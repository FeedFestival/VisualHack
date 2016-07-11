using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Types;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    // Inspector

    public GameObject NewGameObject;

    public int MapId;

    public string MapName;
    public int MapNumber;

    public IEnumerable<Map> Maps
    {
        set { }
        get
        {
            if (_dataService == null)
                _dataService = new DataService("Database.db");

            return from element in _dataService.GetMaps()
                   orderby element.Number descending
                   select element;
        }
    }

    // Inspector - END

    public int ExecuteMapId;
    public Map CurrentMap;

    private Transform _mapMiscT;
    private Transform _pitMiscT;
    private Transform _hillMiscT;

    private Transform _death2OnesT;
    private Transform _solid2OnesT;

    private Main _main;
    private DataService _dataService;

    private bool _hasError;

    //private int _xLength = 11;
    //private int _yLength = 8;

    private const float PuzzleZIndex = -10f;

    private List<Bridge> _bridges;

    public void Initialize(Main main, DataService dataService)
    {
        _main = main;
        _dataService = dataService;
    }

    public void CleanUpUsers()
    {
        var dataService = new DataService("Database.db");

        dataService.CleanUpUsers();
    }

    public void RecreateDataBase()
    {
        var dataService = new DataService("Database.db");

        dataService.CreateDB();
    }

    public void GenerateMapSql(Map map = null)
    {
        if (map == null)
            map = new Map
            {
                Id = 0,
                Name = MapName,
                Number = MapNumber,
                GameObject = NewGameObject
            };

        if (map.GameObject == null)
        {
            Debug.LogError("No Map to " + (map.Id > 0 ? "update" : "create"));
            return;
        }

        _hasError = false;

        var dataService = new DataService("Database.db");

        map.MapTiles = new List<MapTile>();

        Transform[] allChildren = map.GameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("Untagged")) continue;

            MapTile mapTile;

            switch (child.tag)
            {
                case "Misc":
                    mapTile = new MapTile().GetMapTyle(child, TileType.Misc);                    
                    if (mapTile == null) break;

                    HasError(mapTile.Error);

                    mapTile.MapId = map.Id;
                    map.MapTiles.Add(mapTile);

                    break;

                case "DeathZone":
                    mapTile = new MapTile
                    {
                        MapId = map.Id,
                        TyleType = TileType.DeathZone,
                        X = child.position.x,
                        Y = child.position.y
                    };

                    map.MapTiles.Add(mapTile);
                    break;

                case "Solid":
                    mapTile = new MapTile
                    {
                        MapId = map.Id,
                        TyleType = TileType.Solid,
                        X = child.position.x,
                        Y = child.position.y
                    };

                    map.MapTiles.Add(mapTile);
                    break;
            }

            mapTile = new MapTile().GetMapTyle(child, TileType.PuzzleObject);
            if (mapTile == null) continue;

            HasError(mapTile.Error);

            mapTile.MapId = map.Id;
            map.MapTiles.Add(mapTile);
        }

        if (_hasError) return;

        if (map.Id > 0)
            dataService.DeleteMapTiles(map.Id);

        map.Id = dataService.UpdateMap(map);

        if (map.MapTiles.Count > 0)
        {
            foreach (var tiles in map.MapTiles)
            {
                tiles.MapId = map.Id;
            }
            dataService.CreateTiles(map.MapTiles);
        }

        NewGameObject = null;
        CurrentMap = null;
        MapName = "";
        MapNumber = 0;
        MapId = 0;

        Debug.Log("Map transaction resolved: " + map);
    }

    private void HasError(string error)
    {
        if (string.IsNullOrEmpty(error)) return;
        
        _hasError = true;
        Debug.LogError(error);
    }

    public void GenerateBaseMap(bool newMap = false)
    {
        if (newMap)
        {
            MapName = "New";
            CurrentMap = null;
            MapNumber = 0;
        }
        else
            MapName = CurrentMap.Name;

        var go = GameObject.FindWithTag("GameScene");
        DestroyImmediate(go);

        go = Instantiate(Resources.Load("Prefabs/InitGameObj"), new Vector3(),
            Quaternion.identity) as GameObject;

        if (go == null) return;

        _bridges = new List<Bridge>();

        go.tag = "GameScene";
        go.transform.localPosition = new Vector3(0f, 0f, 100f);
        go.gameObject.name = "Game[ " + MapName + " ]";

        _mapMiscT = new GameObject().transform;
        _mapMiscT.SetParent(go.transform);
        _mapMiscT.localPosition = new Vector3(0, 0, 0);
        _mapMiscT.gameObject.name = "mapMisc";

        _pitMiscT = new GameObject().transform;
        _pitMiscT.SetParent(_mapMiscT);
        _pitMiscT.localPosition = new Vector3(0, 0, 0);
        _pitMiscT.gameObject.name = "_pit";

        _hillMiscT = new GameObject().transform;
        _hillMiscT.SetParent(_mapMiscT);
        _hillMiscT.localPosition = new Vector3(0, 0, 0);
        _hillMiscT.gameObject.name = "_hill";

        _death2OnesT = new GameObject().transform;
        _death2OnesT.SetParent(go.transform);
        _death2OnesT.localPosition = new Vector3(0, 0, 0);
        _death2OnesT.gameObject.name = "death2ones";

        _solid2OnesT = new GameObject().transform;
        _solid2OnesT.SetParent(go.transform);
        _solid2OnesT.localPosition = new Vector3(0, 0, 0);
        _solid2OnesT.gameObject.name = "solid2ones";

        if (newMap)
            NewGameObject = go;
        else if (CurrentMap != null) CurrentMap.GameObject = go;
    }

    public void CreateMap(bool inEditor = false)
    {
        IEnumerable<MapTile> tiles;

        if (inEditor)
        {
            MapId = 0;

            _dataService = new DataService("Database.db");
            _main = Camera.main.gameObject.GetComponent<Main>();
        }

        CurrentMap = _dataService.GetMap(ExecuteMapId);
        tiles = _dataService.GetTiles(ExecuteMapId);
        
        GenerateBaseMap();

        if (tiles == null || !tiles.Any()) return;

        foreach (var tile in tiles)
        {
            switch (tile.TyleType)
            {
                case TileType.Misc:

                    if (tile.Misc.ToString().Contains("Pit"))
                        CreateMisc(tile, _pitMiscT, true);
                    else if (tile.Misc.ToString().Contains("Hill"))
                        CreateMisc(tile, _hillMiscT, true);
                    else
                        CreateMisc(tile, _mapMiscT);
                    
                    break;

                case TileType.DeathZone:

                    CreateDeath2Ones(tile);
                    break;

                case TileType.Solid:

                    CreateSolid2Ones(tile);
                    break;

                case TileType.PuzzleObject:

                    CreatePuzzleObjects(tile);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void CreateMisc(MapTile tile, Transform parentT, bool isPitOrHill = false)
    {
        GameObject objT = Instantiate(Resources.Load("Prefabs/Misc/" + tile.Misc), new Vector3(),
            Quaternion.identity) as GameObject;

        objT.transform.SetParent(parentT);
        objT.transform.localPosition = new Vector3(tile.X, tile.Y, -100f + tile.Z);

        if (isPitOrHill)
            objT.gameObject.name = tile.Misc + " [" + tile.X + ", " + tile.Y + "]";
        else
            objT.gameObject.name = tile.Misc.ToString();

        objT.transform.localEulerAngles = new Vector3(0, 0, tile.Rotation);
    }

    private void CreateDeath2Ones(MapTile tile)
    {
        GameObject objT = Instantiate(Resources.Load("Prefabs/Zone"), new Vector3(),
            Quaternion.identity) as GameObject;

        objT.transform.SetParent(_death2OnesT);
        objT.transform.localPosition = new Vector3(tile.X, tile.Y, PuzzleZIndex);
        objT.gameObject.name = "Zone [" + tile.X + ", " + tile.Y + "]";
    }

    private void CreateSolid2Ones(MapTile tile)
    {
        GameObject objT = Instantiate(Resources.Load("Prefabs/Solid"), new Vector3(),
            Quaternion.identity) as GameObject;

        objT.transform.SetParent(_solid2OnesT);
        objT.transform.localPosition = new Vector3(tile.X, tile.Y, PuzzleZIndex);
        objT.gameObject.name = "Solid [" + tile.X + ", " + tile.Y + "]";
    }

    private void CreatePuzzleObjects(MapTile tile)
    {
        GameObject objT = null;

        switch (tile.PuzzleObject)
        {
            case PuzzleObject.Player:

                objT = Instantiate(Resources.Load("Prefabs/Sphere"), new Vector3(), Quaternion.identity) as GameObject;

                objT.gameObject.name = "Sphere";
                objT.transform.SetParent(CurrentMap.GameObject.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, PuzzleZIndex);

                _main.Sphere = objT.GetComponent<Sphere>();

                break;

            case PuzzleObject.Box:

                objT = Instantiate(Resources.Load("Prefabs/Box"), new Vector3(), Quaternion.identity) as GameObject;
                objT.gameObject.name = "Box";
                objT.transform.SetParent(CurrentMap.GameObject.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, PuzzleZIndex);

                objT.GetComponent<Box>().Initialize();

                break;

            case PuzzleObject.Bridge:

                objT = Instantiate(Resources.Load("Prefabs/Bridge"), new Vector3(), Quaternion.identity) as GameObject;
                objT.gameObject.name = "Bridge";
                objT.transform.SetParent(CurrentMap.GameObject.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, 0f);

                var b = objT.GetComponent<Bridge>();
                b.Id = tile.BridgeId;

                if (tile.BridgeId == 0)
                    Debug.LogError("Bridge has no trigger");

                _bridges.Add(b);

                break;

            case PuzzleObject.Trigger:

                objT = Instantiate(Resources.Load("Prefabs/Trigger"), new Vector3(), Quaternion.identity) as GameObject;
                objT.gameObject.name = "Trigger";
                objT.transform.SetParent(CurrentMap.GameObject.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, PuzzleZIndex);

                var bridge = _bridges.Single(item => item.Id == tile.BridgeId);

                if (bridge == null || tile.BridgeId == 0)
                {
                    Debug.LogError("Trigger has no bridge");
                    return;
                }

                objT.GetComponent<Trigger>().Initialize(bridge);

                break;

            case PuzzleObject.Finish:

                objT = Instantiate(Resources.Load("Prefabs/Finish"), new Vector3(), Quaternion.identity) as GameObject;
                objT.gameObject.name = "Finish " + "[" + tile.X + ", " + tile.Y + "]";
                objT.transform.SetParent(CurrentMap.GameObject.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, PuzzleZIndex);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}