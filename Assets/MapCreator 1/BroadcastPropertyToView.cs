using UnityEngine;
using System.Collections;
using Assets.MapCreator_1;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;

public class BroadcastPropertyToView : MonoBehaviour
{
    public Populate Pop;
    public Database DB;

    public List<Tile> EditList;
    Tile curentTile;
    List<Tile> curentTileList;

    public int toolbarInt = 0;
    public string[] toolbarStrings;

    private Rect dropDownRect;
    public List<TileTexture> listTexture;
    public List<TileProperty> listProperty;

    public MapTilesRefrences tileRef;
    public Tiles_PropertyRefrences tilePropertyRef;

    public void AddOrRemove_EditList(Tile tile)
    {
        if (EditList == null)
        {
            EditList = new List<Tile>();
        }
        if (!EditList.Contains(tile))
        {
            EditList.Add(tile);
        }
        else
        {
            EditList.Remove(tile);
        }

        if (EditList.Count == 1)
        {
            Broadcast(EditList[0]);
        }
        else
        {
            curentTile = null;
        }
    }

    public void Broadcast(Tile tile)
    {
        curentTile = tile;
        indexNumber = curentTile.TileTextureId;
    }

    void Start()
    {
        dropDownRect = new Rect(110, 200, 125, 300);


        listTexture = tileRef.getAllTileTextures();
        listProperty = tilePropertyRef.getAllTileProperty();
    }

    private Vector2 scrollViewVector = Vector2.zero;

    public int indexNumber;
    bool toShowTextureDropDown = false;
    bool toShowPropertyDropDown = false;

    string fileName = "";
    string OpenfileName = "";
    bool fileOptions = true;

    bool userPresed_Ctrl_A;
    bool userPresed_Ctrl_S;

    bool playAlert = true;
    string alert = "";

    void OnGUI()
    {
        // if you select one tile.
        if (curentTile != null)
        {
            // Make a background box
            GUI.Box(new Rect(5, 5, 130, 400), "Tile " + curentTile.Id.ToString());

            toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);

            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y+10, dropDownRect.width, 25), "Texture"))
            {
                if (!toShowTextureDropDown)
                {
                    toShowTextureDropDown = true;
                }
                else
                {
                    toShowTextureDropDown = false;
                }
            }
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 40, dropDownRect.width, 25), "Property"))
            {
                if (!toShowPropertyDropDown)
                {
                    toShowPropertyDropDown = true;
                }
                else
                {
                    toShowPropertyDropDown = false;
                }
            }
        }
        else // If you select more then 1 tile.
            if (EditList.Count > 1)
            {
                GUI.Box(new Rect(5, 5, 130, 400), "Tile List");

                if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y+10, dropDownRect.width, 25), "Texture"))
                {
                    if (!toShowTextureDropDown)
                    {
                        toShowTextureDropDown = true;
                    }
                    else
                    {
                        toShowTextureDropDown = false;
                    }
                }
                if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 40, dropDownRect.width, 25), "Property"))
                {
                    if (!toShowPropertyDropDown)
                    {
                        toShowPropertyDropDown = true;
                    }
                    else
                    {
                        toShowPropertyDropDown = false;
                    }
                }
            }
            else
            {
                GUI.Box(new Rect(5, 5, 130, 400), "Empty List");
            }


        // ClearList   HOTKEY : C + S

        if ((curentTile != null) || (EditList.Count > 1))
        {
            if ((GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y - 20, dropDownRect.width, 25), "ClearList")) || (userPresed_Ctrl_S))
            {
                if (EditList != null)
                {
                    userPresed_Ctrl_S = false;
                    curentTile = null;
                    EditList.Clear();
                    Pop.Select_DeSelect(false);
                }
            }
        }

        // Select All   HOTKEY : C + A

        if (EditList.Count < 204 && !fileOptions)
        {
            if ((GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 70, dropDownRect.width, 25), "Select All")) || (userPresed_Ctrl_A))
            {
                userPresed_Ctrl_A = false;
                curentTile = null;
                EditList.Clear();
                Pop.Select_DeSelect(false);
                Pop.Select_DeSelect(true);
            }
        }

        // toShowTextureDropDown the dropdown with the tile.
        if (toShowTextureDropDown)
        {
            scrollViewVector = GUI.BeginScrollView(
                                    new Rect((dropDownRect.x + 100), (dropDownRect.y + 25), dropDownRect.width, dropDownRect.height)
                                    , scrollViewVector
                                    , new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (listTexture.Count * 25)))
                                    );

            GUI.Box(new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (listTexture.Count * 25))), "");

            for (int index = 0; index < listTexture.Count; index++)
            {

                if (GUI.Button(new Rect(0, (index * 25), dropDownRect.height, 25), "0"))
                {
                    toShowTextureDropDown = false;
                    if (curentTile != null)
                    {
                        if (curentTile.TileTextureId != listTexture[index].Index)
                        {
                            curentTile.TileTexture = listTexture[index];
                            curentTile.TileTextureId = listTexture[index].Index;
                            Pop.ChangeTexture(curentTile);
                        }
                    }
                    else
                    {
                        foreach (Tile CurentTile in EditList)
                        {
                            if (CurentTile.TileTextureId != listTexture[index].Index)
                            {
                                CurentTile.TileTexture = listTexture[index];
                                CurentTile.TileTextureId = listTexture[index].Index;
                                Pop.ChangeTexture(CurentTile);

                            }
                        }
                    }
                    indexNumber = index;
                }

                GUI.Label(new Rect(5, (index * 25), dropDownRect.height, 25), listTexture[index].Name);

            }

            GUI.EndScrollView();
        }

        // toShowPropertyDropDown the dropdown with the tile.
        if (toShowPropertyDropDown)
        {
            scrollViewVector = GUI.BeginScrollView(
                                    new Rect((dropDownRect.x + 250), (dropDownRect.y + 25), dropDownRect.width, dropDownRect.height)
                                    , scrollViewVector
                                    , new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (listProperty.Count * 25)))
                                    );

            GUI.Box(new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (listProperty.Count * 25))), "");

            for (int index = 1; index < listProperty.Count; index++)
            {

                if (GUI.Button(new Rect(0, (index * 25), dropDownRect.height, 25), "0"))
                {
                    toShowPropertyDropDown = false;
                    if (curentTile != null)
                    {
                        if (curentTile.TilePropertyId != listProperty[index].Index)
                        {
                            curentTile.TileProperty = listProperty[index];
                            curentTile.TilePropertyId = listProperty[index].Index;
                            curentTile.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = listProperty[index].Sprite;
                        }
                    }
                    else
                    {
                        foreach (Tile CurentTile in EditList)
                        {
                            if (CurentTile.TilePropertyId != listProperty[index].Index)
                            {
                                CurentTile.TileProperty = listProperty[index];
                                CurentTile.TilePropertyId = listProperty[index].Index;
                                CurentTile.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = listProperty[index].Sprite;

                            }
                        }
                    }
                    indexNumber = index;
                }

                GUI.Label(new Rect(5, (index * 25), dropDownRect.height, 25), listProperty[index].Name);

            }

            GUI.EndScrollView();
        }

        if (fileOptions)
        {
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 150, dropDownRect.width, 25), "New"))
            {
                fileOptions = false;
                Pop.Start_this(null);
            }
            // Open Map
            OpenfileName = GUI.TextField(new Rect((dropDownRect.x - 100), dropDownRect.y + 190, dropDownRect.width, 25), OpenfileName, 25);
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 220, dropDownRect.width, 25), "Open Map"))
            {
                if (!string.IsNullOrEmpty(OpenfileName))
                {
                    fileOptions = false;
                    DB.ReadTilesFromPath(OpenfileName);
                }
            }
        }
        else
        {
            

            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 150, dropDownRect.width, 25), "Play Map"))
            {
                if (!string.IsNullOrEmpty(OpenfileName) || !string.IsNullOrEmpty(fileName))
                    playAlert = Pop.Play_Map(OpenfileName, fileName);

                if (!playAlert)
                    alert = "There are empty fields in your map.";
                else
                    alert = "You can save.";

                GUI.Label(new Rect((dropDownRect.x - 100), dropDownRect.y + 120, dropDownRect.height, 25), alert);
            }

            fileName = GUI.TextField(new Rect((dropDownRect.x - 100), dropDownRect.y + 190, dropDownRect.width, 25), fileName, 25);
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 220, dropDownRect.width, 25), "Save Changes"))
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileOptions = false;
                    DB.WriteTiles(fileName);
                }
            }
            if (GUI.Button(new Rect((dropDownRect.x - 100), dropDownRect.y + 290, dropDownRect.width, 25), "Cancel Creation"))
            {
                fileOptions = true;
                Pop.DeleteEverything();
            }
        }
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.C)) && (Input.GetKeyDown(KeyCode.A)))
        {
            userPresed_Ctrl_A = true;
        }

        if ((Input.GetKey(KeyCode.C)) && (Input.GetKeyDown(KeyCode.S)))
        {
            userPresed_Ctrl_S = true;
        }
    }
}
