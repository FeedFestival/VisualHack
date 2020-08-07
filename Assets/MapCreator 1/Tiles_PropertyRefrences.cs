using UnityEngine;
using System.Collections;
using Assets.MapCreator_1;
using System.Collections.Generic;

public class Tiles_PropertyRefrences : MonoBehaviour {

    public Sprite Box;
    public Sprite PortalTrigger;
    public Sprite Portal;
    public Sprite StartPlacement;

    public List<TileProperty> TileProperty;
    private Populate Pop;

    void Awake()
    {
        Pop = this.GetComponent<Populate>();
        TileProperty = new List<TileProperty>();
        Populate();
    }

    public void Populate()
    {
        var temp = new TileProperty();

        temp.Index = 0;
        temp.Name = "None";
        temp.Sprite = null;

        TileProperty.Add(temp);

        temp = new TileProperty();

        temp.Index = 1;
        temp.Name = "Box";
        temp.Sprite = Box;

        TileProperty.Add(temp);

        temp = new TileProperty();

        temp.Index = 2;
        temp.Name = "PortalTrigger";
        temp.Sprite = PortalTrigger;

        TileProperty.Add(temp);

        temp = new TileProperty();

        temp.Index = 3;
        temp.Name = "Portal";
        temp.Sprite = Portal;

        TileProperty.Add(temp);

        temp = new TileProperty();

        temp.Index = 4;
        temp.Name = "StartPlacement";
        temp.Sprite = StartPlacement;

        TileProperty.Add(temp);
    }

    // Return the texture and its atribute to the caller.
    public TileProperty getPropertyById(int i)
    {
        return TileProperty[i];
    }

    public List<TileProperty> getAllTileProperty()
    {
        return TileProperty;
    }
}