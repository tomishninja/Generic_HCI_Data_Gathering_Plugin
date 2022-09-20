using UnityEngine;

public class CounterDemo : MonoBehaviour
{
    [SerializeField] DataCounterDataBase dataBase;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            dataBase.Trigger("LeftClick");

        if (Input.GetMouseButtonDown(1))
            dataBase.Trigger("CenterClick");

        if (Input.GetMouseButtonDown(2))
            dataBase.Trigger("RightClick");
    }

    private void OnApplicationQuit()
    {
        dataBase.Flush();
    }
}
