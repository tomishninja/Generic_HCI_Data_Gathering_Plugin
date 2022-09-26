using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TimerDatabase : MonoBehaviour
{
    /// <summary>
    /// Where to output the file
    /// </summary>
    public string OutputDir;

    /// <summary>
    /// The name object the place in the file
    /// </summary>
    public string FileName;

    /// <summary>
    /// the array of items that this object will gather from other scripts
    /// </summary>
    public DataItem[] GenericDataItems;

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

    public void StartTimer(string key)
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

    public void StopAll()
    {
        foreach (KeyValuePair<string, TimerDataRows> entry in database)
        {
            // only start the timer if it hasn't already started
            if (entry.Value != null)
            {
                //if this item hadent ended then add it to the queue
                if (entry.Value.End())
                {
                    // send it to the stack
                    dataRows.Enqueue(entry.Value);
                }
            }
        }
    }

    /// <summary>
    /// Write the data to a file and clean the database
    /// </summary>
    public void Flush()
    {
        // Just incase there was something left stop everything
        this.StopAll();

        string prepend = "";
        for (int index = 0; index < this.GenericDataItems.Length; index++)
        {
            // get the property out of the object
            object value = this.GenericDataItems[index].GetName();

            // Get the data to the output
            prepend += value;
            prepend += ",";
        }

        // create the output string
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(prepend + TimerDataRows.GetCSVHeader());
        //Debug.Log("Timer Header: " + TimerDataRows.GetCSVHeader());

        // add the data sourced from a outside file
        prepend = "";
        for (int index = 0; index < this.GenericDataItems.Length; index++)
        {
            // get the property out of the object
            object value = this.GenericDataItems[index].GetPropValue();

            // Get the data to the output
            prepend += value;
            prepend += ",";
        }

        // time this data
        TimerDataRows obj = null;
        if (this.dataRows.Count > 0)
            obj = this.dataRows.Dequeue();
        while (obj != null)
        {
            sb.AppendLine(prepend + obj.AsCSVRow());
            if (this.dataRows.Count > 0)
                obj = this.dataRows.Dequeue();
            else
                obj = null;
        }

        // write the data out to the data base
        FileWriterManager.WriteString(sb.ToString(), this.FileName, this.OutputDir);

        // clean out the string buffer
        sb.Clear();
        // clean out the database for next time
        database = new Dictionary<string, TimerDataRows>();
    }
}


public class TimerDataRows
{
    public string ValueName;
    public float StartTime;
    public float EndTime;
    public float TotalTime;
    public int StartFrame;
    public int EndFrame;
    private bool hasEnded = false;

    public TimerDataRows(string name)
    {
        this.ValueName = name;
        this.StartTime = Time.time;
        this.StartFrame = Time.frameCount;
    }

    public bool End()
    {
        if (!hasEnded)
        {
            this.EndTime = Time.time;
            this.EndFrame = Time.frameCount;
            this.TotalTime = this.EndTime - this.StartTime;
            hasEnded = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets all the feilds in this class and returns them as a csv file syle row
    /// </summary>
    /// <returns> A Row formated in the style of a csv file</returns>
    public string AsCSVRow()
    {
        System.Type t = typeof(TimerDataRows);
        System.Reflection.FieldInfo[] fields = t.GetFields();
        StringBuilder csvdata = new StringBuilder();
        foreach (var f in fields)
        {
            if (csvdata.Length > 0)
                csvdata.Append(",");

            var x = f.GetValue(this);

            if (x != null)
            {
                csvdata.Append(x.ToString());
            }
        }

        // return the csv data
        return csvdata.ToString();
    }

    public static string GetCSVHeader()
    {
        System.Type t = typeof(TimerDataRows);
        System.Reflection.FieldInfo[] fields = t.GetFields();
        StringBuilder csvdata = new StringBuilder();
        foreach (var f in fields)
        {
            if (csvdata.Length > 0)
                csvdata.Append(",");

            csvdata.Append(f.Name);
        }

        // return the csv data
        return csvdata.ToString();
    }
}
