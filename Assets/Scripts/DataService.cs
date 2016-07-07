﻿using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using Assets.Scripts.Types;

public class DataService
{
    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

        #region DataServiceInit


#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif

        #endregion

        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    public void CleanUpUsers()
    {
        _connection.DropTable<User>();
        _connection.CreateTable<User>();
    }

    public void CreateDB()
    {
        _connection.DropTable<User>();
        _connection.CreateTable<User>();

        _connection.DropTable<Map>();
        _connection.CreateTable<Map>();

        _connection.DropTable<MapTile>();
        _connection.CreateTable<MapTile>();
    }

    //   public IEnumerable<Person> GetPersons(){
    //	return _connection.Table<Person>();
    //}

    //public IEnumerable<Person> GetPersonsNamedRoberto(){
    //	return _connection.Table<Person>().Where(x => x.Name == "Roberto");
    //}

    //public Person GetJohnny(){
    //	return _connection.Table<Person>().Where(x => x.Name == "Johnny").FirstOrDefault();
    //}

    //public Person CreatePerson(){
    //	var p = new Person{
    //			Name = "Johnny",
    //			Surname = "Mnemonic",
    //			Age = 21
    //	};
    //	_connection.Insert (p);
    //	return p;
    //}

    /*
     * User
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */

    public void CreateUser(string name)
    {
        var user = new User
        {
            Name = name,
            ControllerType = (int)ControllerType.Default
        };
        _connection.Insert(user);
    }

    public void UpdateUserControllerType(int controllerType)
    {
        var user = new User
        {
            Id = 1,
            ControllerType = controllerType
        };
        int rowsAffected = _connection.Update(user);
        Debug.Log("(UPDATE User) rowsAffected : " + rowsAffected);
    }

    public User GetUser()
    {
        return _connection.Table<User>().Where(x => x.Id == 1).FirstOrDefault();
    }

    /*
    * User - END
    * * --------------------------------------------------------------------------------------------------------------------------------------
    */

    /*
     * Map
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */

    // X : 0 - 11
    // Y : 0 - 8
    public int CreateMap(Map map)
    {
        _connection.Insert(map);
        return map.Id;
    }

    public void CreateTiles(List<MapTile> mapTiles)
    {
        _connection.InsertAll(mapTiles);
    }

    public IEnumerable<Map> GetMaps()
    {
        return _connection.Table<Map>();
    }

    public IEnumerable<MapTile> GetTiles(int mapId)
    {
        return _connection.Table<MapTile>().Where(x => x.MapId == mapId);
    }

    /*
     * Map - END
     * * --------------------------------------------------------------------------------------------------------------------------------------
     */
}
