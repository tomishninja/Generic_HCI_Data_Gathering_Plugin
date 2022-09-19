using UnityEngine;


/// <summary>
/// all of the data we require to find the objects 
/// within the unity scene
/// </summary>
[System.Serializable]
public class DataItem
{

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
