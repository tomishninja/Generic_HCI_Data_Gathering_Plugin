using UnityEngine;


/// <summary>
/// all of the data we require to find the objects 
/// within the unity scene
/// </summary>
[System.Serializable]
public class DataItem
{
    public enum TypeofData
    {
        Feild,
        Property,
        //Method,
    }


    public GameObject container;
    public string name;
    public string typeOfScript;
    public string nameOfVerible;
    public TypeofData DataType = TypeofData.Feild;
    //public int index = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        if (name == null || name.Equals("") || name.Length > 0)
            return this.container.name + "_" + nameOfVerible;
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
                    return current.GetType().GetField(this.nameOfVerible).GetValue(current, );
                case DataItem.TypeofData.Property:
                    return current.GetType().GetProperty(this.nameOfVerible).GetValue(current, null);
                default:
                    //TODO
                    return null;
            }
        }
        catch (System.NullReferenceException exception)
        {
            Debug.LogError(exception.Message.ToString());
            Debug.LogError("Name Of Verible" + this.nameOfVerible.ToString());
        }
        return null;

    }
}
