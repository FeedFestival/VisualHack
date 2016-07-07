using System.Collections.Generic;
using Assets.Scripts.Types;
using SQLite4Unity3d;

//public class Person
//{

//    [PrimaryKey, AutoIncrement]
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public string Surname { get; set; }
//    public int Age { get; set; }

//    public override string ToString()
//    {
//        return string.Format("[Person: Id={0}, Name={1},  Surname={2}, Age={3}]", Id, Name, Surname, Age);
//    }
//}

public class User
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Maps { get; set; }
    public bool IsUsingSound { get; set; }
    public int ControllerType { get; set; }

    public override string ToString()
    {
        return string.Format("[User: Id={0}, Name={1}, Maps={2}, IsUsingSound={3}, ControllerType={4}]", Id, Name, Maps, IsUsingSound, ControllerType);
    }
}

public class Map
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    [Unique]
    public int Number { get; set; }

    [Ignore]
    public List<MapTile> MapTiles { get; set; }

    public override string ToString()
    {
        return string.Format("[Map: Id={0}, Name={1}, Number-{2}, ImportantMapTiles={3}]", Id, Name, Number, MapTiles != null ? MapTiles.Count : 0);
    }
}

public class MapTile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int MapId { get; set; }

    [Ignore]
    public TileType TyleType
    {
        get { return (TileType)TyleTypeId; }
        set
        {
            var tileType = value;
            TyleTypeId = (int)tileType;
        }
    }
    public int TyleTypeId
    {
        get; set;
    }

    [Ignore]
    public PuzzleObject PuzzleObject
    {
        get { return (PuzzleObject)PuzzleObjectId; }
        set
        {
            var puzzleObject = value;
            PuzzleObjectId = (int)puzzleObject;
        }
    }
    public int PuzzleObjectId
    {
        get; set;
    }

    public float X { get; set; }
    public float Y { get; set; }

    public int BridgeId { get; set; }

    // Misc
    [Ignore]
    public Misc Misc
    {
        get { return (Misc)MiscId; }
        set
        {
            var misc = value;
            MiscId = (int)misc;
        }
    }
    public int MiscId
    {
        get; set;
    }
    public float Rotation { get; set; }
    public float Z { get; set; }
    // Misc - END

    public string PrintObject(TileType tileType)
    {
        if (tileType == TileType.Misc)
            return string.Format("[Map: TyleType={0}({1}), Rotation={2}, XPos={3}, YPos={4}]", TyleType, TyleTypeId, Rotation, X, Y);
        if (tileType == TileType.DeathZone)
            return string.Format("[Map: TyleType={0}({1}), XPos={2}, YPos={3}]", TyleType, TyleTypeId, X, Y);
        if (tileType == TileType.PuzzleObject)
            return string.Format("[Map: TyleType={0}({1}), XPos={2}, YPos={3}, PuzzleObject={4}({5}) ]", TyleType, TyleTypeId, X, Y, PuzzleObject, PuzzleObjectId);

        return "mapTile";
    }
}