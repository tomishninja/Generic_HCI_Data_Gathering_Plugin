using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTimesObjectWasMoved : MonoBehaviour
{
    [SerializeField] Transform obj;
    [SerializeField] TimerDatabase dataBase;
    Vector3 lastPosition;
    bool wasMoving = false;

    private void Start()
    {
        lastPosition = obj.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (obj.position == lastPosition)
        {
            if (wasMoving)
            {
                dataBase.StopTimer("TimeBetweenMovement");

                wasMoving = false;

                Debug.Log("Started Timer");
            }
        }
        else if(!wasMoving)
        {
            dataBase.StartTimer("TimeBetweenMovement");
            wasMoving = true;
            Debug.Log("Ended Timer");
        }

        lastPosition = obj.position;
    }

    private void OnApplicationQuit()
    {
        dataBase.Flush();
    }
}
