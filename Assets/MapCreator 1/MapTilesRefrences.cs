using UnityEngine;
using System.Collections;
using Assets.MapCreator_1;
using System.Collections.Generic;

public class MapTilesRefrences : MonoBehaviour {

    public Sprite TerrainEmpty;

    public Sprite NormalFloor;
    public Sprite NormalFloorRaised;

    public Sprite FloorWall;

    public Sprite WaterTile;

    public List<TileTexture> TileTextures;
    private Populate Pop;

    void Awake() {
        Pop = this.GetComponent<Populate>();
        TileTextures = new List<TileTexture>();
        Populate();
    }

    public void Populate() {
        
        var temp = new TileTexture();

        temp.Index = 0;
        temp.Name = "TerrainEmpty";
        temp.Sprite = TerrainEmpty;
        temp.Passable = true;

        TileTextures.Add(temp);
        
        temp = new TileTexture();

        temp.Index = 1;
        temp.Name = "NormalFloor";
        temp.Sprite = NormalFloor;
        temp.Passable = true;

        TileTextures.Add(temp);

        temp = new TileTexture();

        temp.Index = 2;
        temp.Name = "NormalFloorRaised";
        temp.Sprite = NormalFloorRaised;
        temp.Passable = false;

        TileTextures.Add(temp);
        
        temp = new TileTexture();

        temp.Index = 3;
        temp.Name = "FloorWall";
        temp.Sprite = FloorWall;
        temp.Passable = false;

        TileTextures.Add(temp);

        temp = new TileTexture();

        temp.Index = 4;
        temp.Name = "WaterTile";
        temp.Sprite = WaterTile;
        temp.Passable = false;

        TileTextures.Add(temp);

        //Pop.Start_this();
    }

    // Return the texture and its atribute to the caller.
    public TileTexture getTerrain(int i) {
        return TileTextures[i];
    }

    public List<TileTexture> getAllTileTextures()
    {
        return TileTextures;
    }
}
