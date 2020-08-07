using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Assets.MapCreator_1;

public class Tile : MonoBehaviour {

    public Tile (Dictionary<string, string> Dict) {
        Id = Convert.ToInt32(Dict["Id"]);
        TilePos_X = Convert.ToInt32( Dict["TilePos_X"]);
        TilePos_Y = Convert.ToInt32 (Dict["TilePos_Y"]);
        TileTextureId = Convert.ToInt32(Dict["TileTextureId"]);
        TilePropertyId = Convert.ToInt32(Dict["TilePropertyId"]);
        Position = CalculateCoordinates(TilePos_X , TilePos_Y);
    }

    public int Id ;

    public int TilePos_X ;
    public int TilePos_Y ;

    public int TileTextureId;
    public int TilePropertyId;
    public TileTexture TileTexture;
    public TileProperty TileProperty;

    public Vector3 Position;   

    private Vector3 CalculateCoordinates (int x, int y) {
            
        return new Vector3( (float)x, (float)y, 0);
    }

}
