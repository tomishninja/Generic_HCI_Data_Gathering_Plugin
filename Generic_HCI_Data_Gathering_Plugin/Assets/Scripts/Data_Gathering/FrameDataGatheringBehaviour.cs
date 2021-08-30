using UnityEngine;

[System.Serializable]
public class FrameDataGatheringBehaviour : MonoBehaviour
{
    // basic behaviour modifier telling the code when to track inputs
    public enum Frequency
    {
        EveryFrame,
        Never
    }

    /// <summary>
    /// Where to output the file
    /// </summary>
    public string fileToOutput;

    /// <summary>
    /// The name object the place in the file
    /// </summary>
    public string NameOfDataBase;

    /// <summary>
    /// The frequency that this instance of the script will run at.
    /// </summary>
    public Frequency frequency = Frequency.EveryFrame;

    /// <summary>
    /// the array of items that this object will gather from other scripts
    /// </summary>
    public DataItem[] GenericDataItems;

    /// <summary>
    /// Behavioural object manipulaiton from wihtin unity
    /// </summary>
    public UnityData unityDataGatherer;

    /// <summary>
    /// The output builder as a csv.
    /// </summary>
    private System.Text.StringBuilder output;

    
    private void Start()
    {
        // get the string builder ready
        output = new System.Text.StringBuilder();
    }

    // Update is called once per frame
    void Update()
    {
        // Work out if you should update the frame
        switch (frequency)
        {
            case Frequency.EveryFrame:
                WriteFrame();
                break;
            default:
                // Do nothing
                break;
        }
    }

    /// <summary>
    /// Writes a line of data to the output
    /// </summary>
    public void WriteFrame()
    {
        // get the unity data and write it to the frame
        string unity = this.unityDataGatherer.getUnityData();
        if (unity.Length > 0)
        {
            output.Append(unity);
        }
        

        for (int index = 0; index < this.GenericDataItems.Length; index++)
        {
            // this is a current value
            var current = this.GenericDataItems[index].container.GetComponent(this.GenericDataItems[index].typeOfScript);

            // get the property out of the object
            object value = this.GetPropValue(current, this.GenericDataItems[index].nameOfVerible, this.GenericDataItems[index].DataType);

            // Get the data to the output
            WriteValueToOutput(value);
        }

        // this data has been recorded now move on to the next line
        output.AppendLine();
    }

    /// <summary>
    /// writes the value of a object to the string buffer
    /// </summary>
    /// <param name="value">
    /// the base value that was found. Must be a primitive, string, vector or quaternion
    /// </param>
    public void WriteValueToOutput(object value)
    {
        if (value.GetType() == typeof(Vector3))
        {
            Debug.Log("Vector3");
             //TODO
        }
        else if(value.GetType() == typeof(Vector2))
        {
            //TODO
        }
        else if (value.GetType() == typeof(Vector4))
        {
            //TODO
        }
        else if (value.GetType() == typeof(Quaternion))
        {
            //TODO
        }
        else
        {
            // default behaviour (int, float, string )
            output.Append(value.ToString());
            Debug.Log(value.ToString());
        }
        output.Append(",");
    }

    /// <summary>
    /// Get a value out of a script
    /// </summary>
    /// <param name="src">The game object with a parameter that you want to get</param>
    /// <param name="propName">The property name of the object you want to get</param>
    /// <returns>
    /// The value of the object that you want to get out of this object
    /// </returns>
    /// <remarks>
    /// This may want to have some behaviour 
    /// </remarks>
    private object GetPropValue(object src, string propName, DataItem.TypeofData dataType)
    {
        switch (dataType)
        {
            case DataItem.TypeofData.Feild:
                return src.GetType().GetField(propName).GetValue(src);
            case DataItem.TypeofData.Property:
                return src.GetType().GetProperty(propName).GetValue(src);
            default:
                //TODO
                return null;
        }
        
    }

    /// <summary>
    /// Write the data to a file and move on
    /// </summary>
    public void Flush()
    {
        //TODO write string buffer to file

        // clean out the string buffer
        output.Clear();
    }

    /// <summary>
    /// Stops the system from recording data
    /// </summary>
    public void Pause()
    {
        this.frequency = Frequency.Never;
    }

    /// <summary>
    /// Change the rate you collect data from the system
    /// </summary>
    /// <param name="frequency">
    /// The speed you want to get data from the system
    /// </param>
    public void changeFrequency(Frequency frequency)
    {
        this.frequency = frequency;
    }

}

/// <summary>
/// all of the data we require to find the objects 
/// within the unity scene
/// </summary>
[System.Serializable]
public class DataItem {
    public enum TypeofData
    {
        Feild,
        Property,
        //Method,
    }


    public GameObject container;
    public string typeOfScript;
    public string nameOfVerible;
    public TypeofData DataType = TypeofData.Feild;
    //public int index = 0;
}

/// <summary>
/// This class will get the main system to write the data 
/// </summary>
[System.Serializable]
public class UnityData
{
    public bool deltaTime = false;
    public bool systemTime = false;
    public bool frameCount = false;

    public string getUnityData()
    {
        string output = "";

        if (deltaTime)
        {
            output += Time.deltaTime.ToString() + ",";
        }

        if (systemTime)
        {
            output += Time.time.ToString() + ",";
        }

        if (frameCount)
        {
            output += Time.frameCount.ToString() + ",";
        }

        return output;
    }
}


