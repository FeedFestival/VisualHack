using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public int MapId;

    public string MapName;
    public int MapNumber;

    public void GenerateSql(bool update)
    {
        var DataService = new DataService("Database.db");

        var map = new Map
        {
            Name = MapName,
            Number = MapNumber
        };

        map.Id = DataService.CreateMap(map);

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

                        Debug.Log(miscMapTile.PrintObject(miscMapTile.TyleType));
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

                    Debug.Log(deathMapTile.PrintObject(deathMapTile.TyleType));

                    break;
            }

            var puzzleMapTile = GetMapTyle(child, TileType.PuzzleObject);

            if (puzzleMapTile == null) continue;

            puzzleMapTile.MapId = map.Id;
            map.MapTiles.Add(puzzleMapTile);

            Debug.Log(puzzleMapTile.PrintObject(puzzleMapTile.TyleType));
        }

        Debug.Log("Map created : " + map);

        //DataService.CreateTiles(map.MapTiles);
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
                        Y = objT.position.y
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
                            PuzzleObject = PuzzleObject.Bridge
                        };
                        break;

                    case "Trigger":

                        mapTile = new MapTile
                        {
                            TyleType = tileType,
                            X = objT.position.x,
                            Y = objT.position.y,
                            PuzzleObject = PuzzleObject.Trigger
                        };
                        break;
                }
                break;

            default:
                throw new ArgumentOutOfRangeException("tileType", tileType, null);
        }
        return mapTile;
    }
}