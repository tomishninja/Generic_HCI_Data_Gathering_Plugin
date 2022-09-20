using UnityEngine;


/// <summary>
/// all of the data we require to find the objects 
/// within the unity scene
/// </summary>
[System.Serializable]
public class DataItem
{
    /// <summary>
    /// An array of various post ends for the get name method
    /// </summary>
    private static string[] postEnds = { "_x", "_y", "_z", "_w" };

    /// <summary>
    /// Types of data that have are working in this system.
    /// </summary>
    public enum TypeofData
    {
        Feild,
        Property,
        //Method,
    }

    /// <summary>
    /// The game object containing the script that contains this data.
    /// </summary>
    public GameObject container;

    /// <summary>
    /// This allows data to be saved using a overrinding name.
    /// If left blank it will default to the name of the varible
    /// </summary>
    public string name;

    /// <summary>
    /// The name of the script that this value comes from
    /// </summary>
    public string typeOfScript;

    /// <summary>
    /// The name of the variable that this script comes from
    /// </summary>
    public string nameOfVariable;

    /// <summary>
    /// What is the 
    /// </summary>
    public TypeofData DataType = TypeofData.Feild;

    /// <summary>
    /// Returns the correct Name granted to the object
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        // Get the subject
        object subject = null;
        try
        {
            subject = this.GetPropValue();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.LogError(this._GetName());
            return _GetName();
        }
        

        // Apply the correct post ends for the various feilds
        if (subject.GetType() == typeof(Vector2))
        {
            string output = "";
            string name = output += this._GetName();
            for (int i = 0; i < 2; i++)
            {
                output += name + postEnds[i];
                if (i != 1)
                    output += ",";
            }
            return output;
        }
        else if (subject.GetType() == typeof(Vector3))
        {
            string output = "";
            string name = output += this._GetName();
            for (int i = 0; i < 3; i++)
            {
                output += name + postEnds[i];
                if (i != 2)
                    output += ",";
            }
            return output;
        }
        else if (subject.GetType() == typeof(Vector4) || subject.GetType() == typeof(Quaternion))
        {
            string output = "";
            string name = output += this._GetName();
            for (int i = 0; i < 4; i++)
            {
                output += name + postEnds[i];
                if (i != 3)
                    output += ",";
            }
            return output;
        }
        else
        {
            return this._GetName();
        }
    }

    private string _GetName()
    {
        if (name == null || name.Trim().Equals("") || name.Trim().Length < 1)
            return this.container.name + "_" + nameOfVariable;
        else
            return this.name;
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
    public object GetPropValue()
    {
        // this is a current value
        var current = this.container.GetComponent(this.typeOfScript);
        try
        {
            switch (DataType)
            {
                case DataItem.TypeofData.Feild:
                    return current.GetType().GetField(this.nameOfVariable).GetValue(current);
                case DataItem.TypeofData.Property:
                    return current.GetType().GetProperty(this.nameOfVariable).GetValue(current, null);
                default:
                    //TODO
                    return null;
            }
        }
        catch (System.NullReferenceException exception)
        {
            Debug.LogError(exception.Message.ToString());
            Debug.LogError("Name Of Verible" + this.nameOfVariable.ToString());
        }
        return null;
    }
}
