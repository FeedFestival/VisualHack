using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Types;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public int MapId;

    public string MapName;
    public int MapNumber;

    private Main _main;
    private DataService _dataService;

    public GameObject CurrentGame;

    private bool _hasError;

    private int _xLength = 11;
    private int _yLength = 8;

    private readonly float _puzzleZIndex = -10f;

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

    public void GenerateSql(bool update)
    {
        _hasError = false;

        var dataService = new DataService("Database.db");

        var map = new Map
        {
            Name = MapName,
            Number = MapNumber
        };
        
        map.MapTiles = new List<MapTile>();

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            switch (child.tag)
            {
                case "Misc":
                    var miscMapTile = GetMapTyle(child, TileType.Misc);

                    if (miscMapTile != null)
                    {
                        miscMapTile.MapId = map.Id;
                        map.MapTiles.Add(miscMapTile);
                    }
                    break;

                case "DeathZone":
                    var deathMapTile = new MapTile
                    {
                        MapId = map.Id,
                        TyleType = TileType.DeathZone,
                        X = child.position.x,
                        Y = child.position.y
                    };

                    map.MapTiles.Add(deathMapTile);
                    break;
            }

            var puzzleMapTile = GetMapTyle(child, TileType.PuzzleObject);

            if (puzzleMapTile == null) continue;

            puzzleMapTile.MapId = map.Id;
            map.MapTiles.Add(puzzleMapTile);
        }
        
        if (_hasError) return;

        map.Id = dataService.CreateMap(map);
        foreach (var tiles in map.MapTiles)
        {
            tiles.MapId = map.Id;
            Debug.Log(tiles.PrintObject(tiles.TyleType));
        }
        dataService.CreateTiles(map.MapTiles);

        Debug.Log("Map created : " + map);
    }

    private MapTile GetMapTyle(Transform objT, TileType tileType)
    {
        MapTile mapTile = null;

        switch (tileType)
        {
            case TileType.Misc:

                var misc = Misc.None;

                switch (objT.gameObject.name)
                {
                    case "PipeConnector":
                        misc = Misc.PipeConnector;
                        break;
                    case "Tutorial1":
                        misc = Misc.Tutorial1;
                        break;
                    case "PitHorizontal2":
                        misc = Misc.PitHorizontal2;
                        break;
                    case "PipeHorizontal":
                        misc = Misc.PipeConnector;
                        break;
                }

                if (misc != Misc.None)
                {
                    mapTile = new MapTile
                    {
                        TyleType = tileType,
                        Misc = misc,
                        Rotation = objT.eulerAngles.z,
                        X = objT.position.x,
                        Y = objT.position.y,
                        Z = objT.position.z
                    };
                }
                break;

            case TileType.PuzzleObject:

                switch (objT.tag)
                {
                    case "Player":

                        mapTile = new MapTile
                        {
                            TyleType = tileType,
                            X = objT.position.x,
                            Y = objT.position.y,
                            PuzzleObject = PuzzleObject.Player
                        };
                        break;

                    case "Box":

                        mapTile = new MapTile
                        {
                            TyleType = tileType,
                            X = objT.position.x,
                            Y = objT.position.y,
                            PuzzleObject = PuzzleObject.Box
                        };
                        break;

                    case "Bridge":

                        mapTile = new MapTile
                        {
                            TyleType = tileType,
                            X = objT.position.x,
                            Y = objT.position.y,
                            PuzzleObject = PuzzleObject.Bridge,
                            BridgeId = objT.GetComponent<Bridge>().Id
                        };

                        if (mapTile.BridgeId == 0) HasError("Bridge has no Id.");

                        break;

                    case "Trigger":

                        mapTile = new MapTile
                        {
                            TyleType = tileType,
                            X = objT.position.x,
                            Y = objT.position.y,
                            PuzzleObject = PuzzleObject.Trigger,
                            BridgeId = objT.GetComponent<Trigger>().Bridge.Id
                        };

                        if (mapTile.BridgeId == 0) HasError("Trigger has no BridgeId.");

                        break;
                }
                break;

            default:
                throw new ArgumentOutOfRangeException("tileType", tileType, null);
        }
        return mapTile;
    }

    public void CreateMap(int mapId)
    {
        IEnumerable<MapTile> tiles = _dataService.GetTiles(mapId);

        if (tiles == null || !tiles.Any()) return;

        _main.CurrentMapId = mapId;
        _bridges = new List<Bridge>();

        Destroy(CurrentGame);

        CurrentGame = Instantiate(Resources.Load("Prefabs/InitGameObj"), new Vector3(),
            Quaternion.identity) as GameObject;

        CurrentGame.transform.localPosition = new Vector3(0f, 0f, 100f);
        CurrentGame.gameObject.name = "CurrentGame";

        var mapMiscT = new GameObject().transform;
        mapMiscT.SetParent(CurrentGame.transform);
        mapMiscT.localPosition = new Vector3(0, 0, 0);
        mapMiscT.gameObject.name = "mapMisc";

        var death2onesT = new GameObject().transform;
        death2onesT.SetParent(CurrentGame.transform);
        death2onesT.localPosition = new Vector3(0, 0, 0);
        death2onesT.gameObject.name = "death2ones";

        foreach (var tile in tiles)
        {
            Debug.Log(tile.PrintObject(tile.TyleType));

            switch (tile.TyleType)
            {
                case TileType.Misc:

                    CreateMisc(tile, mapMiscT);
                    break;

                case TileType.DeathZone:

                    CreateDeath2ones(tile, death2onesT);
                    break;

                case TileType.PuzzleObject:

                    CreatePuzzleObjects(tile);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void HasError(string error)
    {
        _hasError = true;
        Debug.LogError(error);
    }

    private void CreateMisc(MapTile tile, Transform parentT)
    {
        GameObject objT = Instantiate(Resources.Load("Prefabs/Misc/" + tile.Misc), new Vector3(),
            Quaternion.identity) as GameObject;

        objT.transform.SetParent(parentT);
        objT.transform.localPosition = new Vector3(tile.X, tile.Y, 0f);
        objT.gameObject.name = tile.Misc + "[" + tile.X + ", " + tile.Y + "]";
        objT.transform.localEulerAngles = new Vector3(0, 0, tile.Rotation);
    }

    private void CreateDeath2ones(MapTile tile, Transform parentT)
    {
        GameObject objT = Instantiate(Resources.Load("Prefabs/Zone"), new Vector3(),
            Quaternion.identity) as GameObject;

        objT.transform.SetParent(parentT);
        objT.transform.localPosition = new Vector3(tile.X, tile.Y, _puzzleZIndex);
        objT.gameObject.name = "Zone[" + tile.X + ", " + tile.Y + "]";
    }

    private void CreatePuzzleObjects(MapTile tile)
    {
        GameObject objT = null;

        switch (tile.PuzzleObject)
        {
            case PuzzleObject.Player:

                objT = Instantiate(Resources.Load("Prefabs/Sphere"), new Vector3(), Quaternion.identity) as GameObject;

                objT.gameObject.name = "Sphere";
                objT.transform.SetParent(CurrentGame.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, _puzzleZIndex);

                _main.Sphere = objT.GetComponent<Sphere>();

                break;

            case PuzzleObject.Box:

                objT = Instantiate(Resources.Load("Prefabs/Box"), new Vector3(), Quaternion.identity) as GameObject;
                objT.gameObject.name = "Box";
                objT.transform.SetParent(CurrentGame.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, _puzzleZIndex);

                objT.GetComponent<Box>().Initialize();

                break;

            case PuzzleObject.Bridge:

                objT = Instantiate(Resources.Load("Prefabs/Bridge"), new Vector3(), Quaternion.identity) as GameObject;
                objT.gameObject.name = "Bridge";
                objT.transform.SetParent(CurrentGame.transform);
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
                objT.transform.SetParent(CurrentGame.transform);
                objT.transform.localPosition = new Vector3(tile.X, tile.Y, _puzzleZIndex);

                var bridge = _bridges.Single(item => item.Id == tile.BridgeId);

                if (bridge == null || tile.BridgeId == 0)
                {
                    Debug.LogError("Trigger has no bridge");
                    return;
                }

                objT.GetComponent<Trigger>().Initialize(bridge);

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}