using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Text;
using System.Xml;
using Mono.Data.SqliteClient;

public class dbAccess : MonoBehaviour
{
    private string connection;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;
    private StringBuilder builder;

    // Use this for initialization
    void Start()
    {

    }

    public string OpenDB(string fileName)
    {
        var ret = string.Empty;

        //ret += "Call to OpenDB:" + fileName + Environment.NewLine;
        // check if file exists in Application.persistentDataPath

        string filepath = string.Empty;

        //ret += " platform: " + Application.platform + Environment.NewLine;

        if (Application.platform == RuntimePlatform.Android)
        {
            filepath = Application.persistentDataPath + "/" + fileName;
            ret += " filepath: " + filepath + Environment.NewLine;
            ret += " filepath exists ? : " + File.Exists(filepath) + Environment.NewLine;
            if (!File.Exists(filepath))
            {
                ret += "File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                                 Application.dataPath + "!/assets/" + fileName + Environment.NewLine;
                // if it doesn't ->
                // open StreamingAssets directory and load the db -> 
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + fileName);
                while (!loadDB.isDone) { }
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
        }
        else
        {
            filepath = Application.dataPath + "/StreamingAssets/" + fileName + ".sqlite";
        }

        //open db connection
        connection = "URI=file:" + filepath;

        //ret += "Obtained a connections string: " + connection + Environment.NewLine;

        //Debug.Log("Stablishing connection to: " + connection);
        //dbcon = new SqliteConnection(connection);
        //dbcon.Open();

        return ret;
    }

    public void CloseDB()
    {
        reader.Close(); // clean everything up
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }

    public bool CreateTable(string name, string[] col, string[] colType)
    { // Create a table, name, column array, column type array
        string query;
        query = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
        for (var i = 1; i < col.Length; i++)
        {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ")";
        try
        {
            dbcmd = dbcon.CreateCommand(); // create empty command
            dbcmd.CommandText = query; // fill the command
            reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {

            Debug.Log(e);
            return false;
        }
        return true;
    }

    public int InsertIntoSingle(string tableName, string colName, string value)
    { // single insert
        string query;
        query = "INSERT INTO " + tableName + "(" + colName + ") " + "VALUES (" + value + ")";
        try
        {
            dbcmd = dbcon.CreateCommand(); // create empty command
            dbcmd.CommandText = query; // fill the command
            reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {

            Debug.Log(e);
            return 0;
        }
        return 1;
    }

    public int InsertIntoSpecific(string tableName, string[] col, string[] values)
    { // Specific insert with col and values
        string query;
        query = "INSERT INTO " + tableName + "(" + col[0];
        for (int i = 1; i < col.Length; i++)
        {
            query += ", " + col[i];
        }
        query += ") VALUES (" + values[0];
        for (int i = 1; i < col.Length; i++)
        {
            query += ", " + values[i];
        }
        query += ")";
        Debug.Log(query);
        try
        {
            dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
        }
        catch (Exception e)
        {

            Debug.Log(e);
            return 0;
        }
        return 1;
    }

    public int InsertInto(string tableName, string[] values)
    { // basic Insert with just values
        string query;
        query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++)
        {
            query += ", " + values[i];
        }
        query += ")";
        try
        {
            dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
        }
        catch (Exception e)
        {

            Debug.Log(e);
            return 0;
        }
        return 1;
    }

    public ArrayList SingleSelectWhere(string tableName, string itemToSelect, string wCol, string wPar, string wValue)
    { // Selects a single Item
        string query;
        query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + wValue;
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
        //string[,] readArray = new string[reader, reader.FieldCount];
        string[] row = new string[reader.FieldCount];
        ArrayList readArray = new ArrayList();
        while (reader.Read())
        {
            int j = 0;
            while (j < reader.FieldCount)
            {
                row[j] = reader.GetString(j);
                j++;
            }
            readArray.Add(row);
        }
        return readArray; // return matches
    }


    public string GetUser()
    { // run a basic Sqlite query
      //dbcmd = dbcon.CreateCommand(); // create empty command
      //dbcmd.CommandText = query; // fill the command
      //reader = dbcmd.ExecuteReader(); // execute command which returns a reader
      //return reader; // return the reader

        var ret = string.Empty;
        var sql = " SELECT * FROM USER WHERE USER_ID= 1 ";

        using (dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();
            using (dbcmd = dbcon.CreateCommand())
            {
                dbcmd.CommandText = sql;

                ret += " connectionDatabase ? : " + dbcon.Database + Environment.NewLine;

                reader = dbcmd.ExecuteReader();

                string[] row = new string[reader.FieldCount];
                ArrayList readArray = new ArrayList();
                
                while (reader.Read())
                {
                    ret += " reader.FieldCount : " + reader.FieldCount + Environment.NewLine + " reader : " + reader.IsClosed + Environment.NewLine;
                    ret += Environment.NewLine + reader.GetString((int)USER._USERNAME) + Environment.NewLine;

                    int j = 0;
                    while (j < reader.FieldCount)
                    {
                        row[j] = reader.GetString(j);
                        j++;
                    }
                    readArray.Add(row);
                }

                

                //using (IDataReader reader = dbcmd.ExecuteReader())
                //{
                //    ret += " reader.FieldCount : " + reader.FieldCount + Environment.NewLine + " reader : " + reader.IsClosed + Environment.NewLine;

                //    while (reader.Read())
                //    {
                //        ret += Environment.NewLine + reader.GetString((int)USER._USERNAME) + Environment.NewLine;
                //    }
                //}
            }
        }
        return ret + Environment.NewLine + sql;
    }

    private enum USER
    {
        _ID = 0, _USERNAME = 1, _MAPS = 2, _COMPLETED_MAPS = 3
    }
}