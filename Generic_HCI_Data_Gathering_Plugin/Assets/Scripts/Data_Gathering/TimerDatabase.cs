using System.Collections.Generic;
using UnityEngine;

public class TimerDatabase : MonoBehaviour
{
    /// <summary>
    /// Where to output the file
    /// </summary>
    public string fileToOutput;

    /// <summary>
    /// The name object the place in the file
    /// </summary>
    public string NameOfDataBase;

    /// <summary>
    /// a array of keys
    /// </summary>
    [SerializeField]
    string[] dbKeys;

    /// <summary>
    /// A dictionary that holds the running data
    /// </summary>
    private Dictionary<string, TimerDataRows> database = new Dictionary<string, TimerDataRows>();

    // a queue that holds the used data
    private Queue<TimerDataRows> dataRows = new Queue<TimerDataRows>();

    // Start is called before the first frame update
    void Start()
    {
        // add all of the new keys to the database so they can be added later
        for (int index = 0; index < dbKeys.Length; index++)
        {
            database.Add(dbKeys[index], null);
        }
    }

    public void startTimer(string key)
    {
        // increment the int by one if it is accepted
        if (database.TryGetValue(key, out TimerDataRows value))
        {
            // only start the timer if it hasn't already started
            if (value == null)
                database[key] = new TimerDataRows(key);
        }
    }

    public void StopTimer(string key)
    {
        // increment the int by one if it is accepted
        if (database.TryGetValue(key, out TimerDataRows value))
        {
            // only start the timer if it hasn't already started
            if (value != null)
            {
                // write the final file
                database[key].End();

                // send it to the stack
                dataRows.Enqueue(database[key]);

                // remove the current entry from the timer set
                database[key] = null;
            }
        }
    }
}


public class TimerDataRows
{
    string ValueName;
    float StartTime;
    float EndTime;
    float TotalTime;
    int StartFrame;
    int EndFrame;

    public TimerDataRows(string name)
    {
        this.ValueName = name;
        this.StartTime = Time.time;
        this.StartFrame = Time.frameCount;
    }

    public void End()
    {
        this.EndTime = Time.time;
        this.EndFrame = Time.frameCount;
        this.TotalTime = this.EndTime - this.StartTime;
    }

    public string AsCSVRow()
    {
        return null; //TODO
    }
}
