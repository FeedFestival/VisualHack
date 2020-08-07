using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Populate : MonoBehaviour
{

    public Database DB;
    public ONLOAD OnLoad;

    GameObject Tile;

    public BroadcastPropertyToView broadcast;
    public GameObject[,] Tiles;
    public Tile[] TilesById;

    MapTilesRefrences TileTexture;
    Tiles_PropertyRefrences TileProperty;
    public Sprite SelectedToEdit;

    GameObject cube;

    // Use this for initialization
    public void Start_this(List<Tile> tiles)
    {
        Tile = new GameObject();

        TilesById = new Tile[204];
        Tiles = new GameObject[20, 20];
        cube = new GameObject();

        TileTexture = this.GetComponent<MapTilesRefrences>();
        TileProperty = this.GetComponent<Tiles_PropertyRefrences>();

        if (tiles == null)
        {

            PopulateMap();
        }
        else
        {
            PopulateFromPath(tiles);
        }

    }

    public void PopulateFromPath(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            var TileObject = Instantiate(Tile, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
            var TileObjectProperty = Instantiate(cube, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, -0.1f), Quaternion.identity) as GameObject;
            var TileColl = Instantiate(cube, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, -0.1f), Quaternion.identity) as GameObject;

            // The tile that we take from .xml
            TileObject.AddComponent<Tile>();
            TileObject.AddComponent<SpriteRenderer>();

            TileObject.name = "Tile[" + tile.TilePos_X + "," + tile.TilePos_Y + "]";

            TileObject.GetComponent<Tile>().Id = tile.Id;
            TileObject.GetComponent<Tile>().TilePos_X = tile.TilePos_X;
            TileObject.GetComponent<Tile>().TilePos_Y = tile.TilePos_Y;

            TileObject.GetComponent<Tile>().TileTexture = TileTexture.getTerrain(tile.TileTextureId);
            TileObject.GetComponent<Tile>().TileTextureId = tile.TileTextureId;
            TileObject.GetComponent<SpriteRenderer>().sprite = TileObject.GetComponent<Tile>().TileTexture.Sprite;

            TileObject.GetComponent<Tile>().TileProperty = TileProperty.getPropertyById(tile.TilePropertyId);
            TileObject.GetComponent<Tile>().TilePropertyId = tile.TilePropertyId;

            // If object has property, like : Portal , PortalTrigger, Life.
            TileObjectProperty.name = "TileObjectProperty";
            TileObjectProperty.AddComponent<SpriteRenderer>();

            TileObjectProperty.GetComponent<SpriteRenderer>().sprite = TileObject.GetComponent<Tile>().TileProperty.Sprite;

            TileObjectProperty.transform.parent = TileColl.transform;

            // SpirteCollider. For editing it in the MapCreator.
            TileColl.name = "TileColl";
            TileColl.AddComponent<BoxCollider>();
            TileColl.AddComponent<SpriteRenderer>();
            TileColl.GetComponent<SpriteRenderer>().sprite = SelectedToEdit;
            TileColl.GetComponent<SpriteRenderer>().enabled = false;
            TileColl.AddComponent<EditTile>();
            TileColl.GetComponent<EditTile>().broadcast = broadcast;
            TileColl.AddComponent<ExampleSelectable>();
            TileColl.transform.parent = TileObject.transform;

            TileColl.GetComponent<EditTile>().AwakeThis(TileObject.GetComponent<Tile>());

            // Add the tile in the array for easier management.
            Tiles[tile.TilePos_X, tile.TilePos_Y] = TileObject;

            TilesById[tile.Id] = Tiles[tile.TilePos_X, tile.TilePos_Y].GetComponent<Tile>();
        }
    }

    // Pentru cand incepi o harta de la 0
    void PopulateMap()
    {
        int id = 0;
        for (int x = 0; x < 17; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                var TileObject = Instantiate(Tile, new Vector3((float)x, (float)y, 0), Quaternion.identity) as GameObject;
                var TileObjectProperty = Instantiate(cube, new Vector3((float)x, (float)y, -0.1f), Quaternion.identity) as GameObject;
                var TileColl = Instantiate(cube, new Vector3((float)x, (float)y, -0.1f), Quaternion.identity) as GameObject;

                TileObject.AddComponent<Tile>();
                TileObject.AddComponent<SpriteRenderer>();

                TileObject.name = "Tile[" + x + "," + y + "]";

                TileObject.GetComponent<Tile>().Id = id;
                TileObject.GetComponent<Tile>().TilePos_X = x;
                TileObject.GetComponent<Tile>().TilePos_Y = y;

                TileObject.GetComponent<Tile>().TileTexture = TileTexture.getTerrain(0);
                TileObject.GetComponent<Tile>().TileTextureId = TileObject.GetComponent<Tile>().TileTexture.Index;
                TileObject.GetComponent<SpriteRenderer>().sprite = TileObject.GetComponent<Tile>().TileTexture.Sprite;

                TileObject.GetComponent<Tile>().TileProperty = TileProperty.getPropertyById(0);
                TileObject.GetComponent<Tile>().TilePropertyId = TileObject.GetComponent<Tile>().TileProperty.Index;

                
                // If object has property, like : Portal , PortalTrigger, Life.
                TileObjectProperty.name = "TileObjectProperty";
                TileObjectProperty.AddComponent<SpriteRenderer>();
                TileObjectProperty.transform.parent = TileColl.transform;

                // SpirteCollider. For editing it in the MapCreator.
                TileColl.name = "TileColl";
                TileColl.AddComponent<BoxCollider>();
                TileColl.AddComponent<SpriteRenderer>();
                TileColl.GetComponent<SpriteRenderer>().sprite = SelectedToEdit;
                TileColl.GetComponent<SpriteRenderer>().enabled = false;
                TileColl.AddComponent<EditTile>();
                TileColl.GetComponent<EditTile>().broadcast = broadcast;
                TileColl.AddComponent<ExampleSelectable>();
                TileColl.transform.parent = TileObject.transform;

                TileColl.GetComponent<EditTile>().AwakeThis(TileObject.GetComponent<Tile>());
                Tiles[x, y] = TileObject;

                TilesById[id] = Tiles[x, y].GetComponent<Tile>();
                id++;
            }
        }
    }

    public void ChangeTexture(Tile tile)
    {
        Tiles[tile.TilePos_X, tile.TilePos_Y].GetComponent<SpriteRenderer>().sprite = TileTexture.getTerrain(tile.TileTexture.Index).Sprite;
    }

    public Tile[] GetAllTiles()
    {
        return TilesById;
    }

    public void Select_DeSelect(bool n)
    {
        foreach (GameObject tile in Tiles)
        {
            if (tile != null)
            {
                if (n == true)
                {
                    broadcast.AddOrRemove_EditList(tile.GetComponent<Tile>());
                }

                tile.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = n;
            }
        }
    }

    public bool Play_Map(string Ofilename,string Sfilename)
    {

        foreach (Tile tile in TilesById){
            if (tile.TileTextureId == 0)
                return false;
        }
        OnLoad.OpenMap_filename = Ofilename;
        OnLoad.SavedMap_filename = Sfilename;
        Application.LoadLevel(1);

        return true;
    }

    public void DeleteEverything() {
        foreach (Tile tile in TilesById){
            Destroy(tile.gameObject);
        }
    }
}
