using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DataCounterDataBase : MonoBehaviour
{
    /// <summary>
    /// File path for the output files if left blank will default to default file path
    /// </summary>
    public string OutputDir;

    /// <summary>
    /// the file name for the file output
    /// </summary>
    public string FileName;

    /// <summary>
    /// the array of items that this object will gather from other scripts
    /// </summary>
    public DataItem[] GenericDataItems;

    /// <summary>
    /// A list of keys that can be set to be triggered by this database.
    /// </summary>
    [SerializeField]
    string[] dbKeys;

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, int> database = new Dictionary<string, int>();

    private void Start()
    {
        InitalizeUpDataBase();
    }

    /// <summary>
    /// Increments the key value by one when selected
    /// </summary>
    /// <param name="key">
    /// The database term you have selected
    /// </param>
    public void Trigger(string key)
    {
        // increment the int by one if it is accepted
        if (database.TryGetValue(key, out int value))
        {
            database[key]++;
        }
    }

    private void InitalizeUpDataBase()
    {
        // add all of the new keys to the database so they can be added later
        for (int index = 0; index < dbKeys.Length; index++)
        {
            database.Add(dbKeys[index], 0);
        }
    }

    /// <summary>
    /// Write the file out to a repository and clean out the database of all of it's values
    /// </summary>
    public void Flush()
    {
        // create the output string
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(this.GetCSVHeader());

        // add the data sourced from a outside file
        string prepend = "";
        for (int index = 0; index < this.GenericDataItems.Length; index++)
        {
            // get the property out of the object
            object value = this.GenericDataItems[index].GetPropValue();

            // Get the data to the output
            prepend += value + ",";
        }

        // add in all of the rows
        for (int index = 0; index < dbKeys.Length; index++)
        {
            sb.AppendLine(prepend + dbKeys[index] + "," + database[dbKeys[index]]);
        }

        // write the data out to the data base
        FileWriterManager.WriteString(sb.ToString(), this.FileName, this.OutputDir);

        // clean out the string buffer
        sb.Clear();

        // Set up a new counting database
        database = new Dictionary<string, int>();
        this.InitalizeUpDataBase();
    }

    private string GetCSVHeader()
    {
        string output = "";

        // get all of the names of the varibles we need to add
        for (int index = 0; index < GenericDataItems.Length; index++)
        {
            output += this.GenericDataItems[index].GetName() + ",";
        }

        output += "Key,Count";

        return output;
    }
}
