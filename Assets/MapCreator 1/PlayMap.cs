using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

public class PlayMap : MonoBehaviour {

    // This is where you take the ONLOAD object that you have in Map_Creator.
    //  - inside test if there is a map to test, or you have to load one of the saved ones.
    GameObject gameObj;
    void OnLevelWasLoaded(int level)
    {
        if (level == 1) { 
            gameObj = GameObject.FindGameObjectWithTag("OnLoad");

            string filename = gameObj.GetComponent<ONLOAD>().OpenMap_filename;
            if (!string.IsNullOrEmpty(filename))
                ReadTilesFromPath(filename);
            else
            {
                filename = gameObj.GetComponent<ONLOAD>().SavedMap_filename;
                ReadTilesFromPath(filename);
            }
        }
    }

    // It takes the filename and opens it up for reading.
    public void ReadTilesFromPath(string fileName)
    {
        var text = File.ReadAllText(baseFileDirectory + fileName + ".xml");

        ReadTiles(text);
    }

    // Read from xml variables. (have no idea what they do but they work.)
    public static List<Tile> Tiles = new List<Tile>();
    public List<Tile> InspectorTiles = new List<Tile>();
    private List<Dictionary<string, string>> TileDictionary = new List<Dictionary<string, string>>();
    private Dictionary<string, string> obj;
    string baseFileDirectory ;

    // Read the xml and put the data in a Tile list.
    void ReadTiles(string text)
    {
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(text);
        XmlNodeList TileList = xmlDoc.GetElementsByTagName("Tile");

        foreach (XmlNode tileInfo in TileList)
        {
            XmlNodeList tileContent = tileInfo.ChildNodes;

            obj = new Dictionary<string, string>();

            foreach (XmlNode content in tileContent)
            {
                switch (content.Name)
                {
                    case "Id":
                        obj.Add("Id", content.InnerText);
                        break;
                    case "TilePos_X":
                        obj.Add("TilePos_X", content.InnerText);
                        break;
                    case "TilePos_Y":
                        obj.Add("TilePos_Y", content.InnerText);
                        break;
                    case "TileTextureId":
                        obj.Add("TileTextureId", content.InnerText);
                        break;
                    case "TilePropertyId":
                        obj.Add("TilePropertyId", content.InnerText);
                        break;
                }
            }

            TileDictionary.Add(obj);
        }
        for (int i = 0; i < TileDictionary.Count; i++)
        {
            Tiles.Add(new Tile(TileDictionary[i]));
        }

        BuildMap();
    }
    public int getTilePos_XFromId(int id)
    {
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].Id == id)
            {
                return Tiles[i].TilePos_X;
            }
        }

        return 100;
    }
    public int getTilePos_YFromId(int id)
    {
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].Id == id)
            {
                return Tiles[i].TilePos_Y;
            }
        }

        return 100;
    }

    string filepath;

    // Temporary - Just open a map for test playing.
    MapTilesRefrences TileTexture;
    Tiles_PropertyRefrences TileProperty;
    GameRules gameRules;
    void StartMap() {
        baseFileDirectory = Application.dataPath + Path.DirectorySeparatorChar + "Maps" + Path.DirectorySeparatorChar;

        TileTexture = this.GetComponent<MapTilesRefrences>();
        TileProperty = this.GetComponent<Tiles_PropertyRefrences>();
        gameRules = this.gameObject.GetComponent<GameRules>();
        ReadTilesFromPath("Test");
        Debug.Log(baseFileDirectory + "Test");
    }

    // Variables fr map building.
    GameObject Tile;
    public GameObject TileWater;
    public GameObject TileBlocked;
    public GameObject Sphere;
    public GameObject Box;
    public GameObject PortalTrigger;
    public GameObject Win;

    // Build the map with the Tile List.
    void BuildMap() {
        Tile = new GameObject();

        int portalIndex = 0;
        int boxIndex = 0;
        foreach (Tile tile in Tiles)
        {
            // Build World.
            if (tile.TileTextureId == 4)
            {
                var TileObject = Instantiate(TileWater, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                TileObject.name = "Tile[" + tile.TilePos_X + "," + tile.TilePos_Y + "]";
            }
            else if (tile.TileTextureId == 1)
            {
                var TileObject = Instantiate(Tile, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                TileObject.name = "Tile[" + tile.TilePos_X + "," + tile.TilePos_Y + "]";

                TileObject.AddComponent<SpriteRenderer>();
                TileObject.GetComponent<SpriteRenderer>().sprite = TileTexture.getTerrain(tile.TileTextureId).Sprite;
            }
            else {
                var TileObject = Instantiate(TileBlocked, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                TileObject.name = "Tile[" + tile.TilePos_X + "," + tile.TilePos_Y + "]";
            }

            // Place Interactive Objects.
            if (tile.TilePropertyId == 1){
                var PropObject = Instantiate(Box, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                boxIndex++;
                PropObject.name = "Box[" + boxIndex + "]";
                gameRules.addBoxes(PropObject);
            }
            else if (tile.TilePropertyId == 4) {
                var PropObject = Instantiate(Sphere, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                PropObject.name = Sphere.name;
            }
            else if (tile.TilePropertyId == 2) {
                var TileObject = Instantiate(PortalTrigger, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                portalIndex++;
                TileObject.GetComponent<Logic_PortalTrigger>().index = portalIndex;
                TileObject.GetComponent<Logic_PortalTrigger>().gameRules = gameRules;
                TileObject.name = "PortalTrigger[" + portalIndex + "]";
                gameRules.addPortalTriggers(TileObject);
            }
            else if (tile.TilePropertyId == 3)
            {
                var TileObject = Instantiate(Win, new Vector3((float)tile.TilePos_X, (float)tile.TilePos_Y, 0), Quaternion.identity) as GameObject;
                TileObject.name = "Portal";
                TileObject.GetComponent<Logic_WinMap>().gameRules = gameRules;
            } else {
                
            }
        }
    }

    // Temporary - Play Button
    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 80, 20), "Play"))
        {
            StartMap();
        }
    }
}
