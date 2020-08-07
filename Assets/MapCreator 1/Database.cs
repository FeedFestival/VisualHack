using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System;

public class Database : MonoBehaviour
{

    public Populate thisMap;

    public TextAsset TileAsset;

    public static List<Tile> Tiles = new List<Tile>();

    public List<Tile> InspectorTiles = new List<Tile>();

    private List<Dictionary<string, string>> TileDictionary = new List<Dictionary<string, string>>();

    private Dictionary<string, string> obj;
    string baseFileDirectory;

    void Start()
    {
        baseFileDirectory = Application.dataPath;
        baseFileDirectory = String.Concat(baseFileDirectory, "/Resources/Maps/");
        //baseFileDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        //var splitFiles = baseFileDirectory.Split(new string[] { "\\" }, StringSplitOptions.None);
        //splitFiles[splitFiles.Length - 1] = "Maps";
        //splitFiles[splitFiles.Length - 2] = "Assets";
        //List<string> list = new List<string>(splitFiles);
        //list.RemoveAt(0);
        //splitFiles = list.ToArray();
        //baseFileDirectory = String.Join("\\", splitFiles);
    }

    public void ReadTilesFromPath(string fileName)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("Maps/" + fileName) as TextAsset;

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);

        ReadTiles(xmldoc);
    }

    void ReadTiles(XmlDocument xmlDoc)
    {
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

        thisMap.Start_this(Tiles);
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

    public void WriteTiles(string FileName)
    {

        XDocument xmlDoc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes")

            , new XComment("Creating an XML_map using Linq to xml")

            , new XElement("TilesTake"

                , from tile in thisMap.GetAllTiles()
                  select new XElement("Tile"
                      , new XElement("Id", tile.Id)
                      , new XElement("TilePos_X", tile.TilePos_X)
                      , new XElement("TilePos_Y", tile.TilePos_Y)
                      , new XElement("TileTextureId", tile.TileTextureId)
                      , new XElement("TilePropertyId", tile.TilePropertyId)
                      )
                )
            );
        xmlDoc.Save(baseFileDirectory + FileName + ".xml");
    }
}