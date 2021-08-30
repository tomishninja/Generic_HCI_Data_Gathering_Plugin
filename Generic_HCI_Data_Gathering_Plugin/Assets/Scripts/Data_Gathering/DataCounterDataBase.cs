using System.Collections.Generic;
using UnityEngine;

public class DataCounterDataBase : MonoBehaviour
{
    public string fileToOutput;

    public string NameOfDataBase;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    string[] dbKeys;

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string, int> database = new Dictionary<string, int>();

    private void Start()
    {
        // add all of the new keys to the database so they can be added later
        for(int index = 0; index < dbKeys.Length; index++)
        {
            database.Add(dbKeys[index], 0);
        }
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

    public void Flush()
    {
        //TODO
    }
}
