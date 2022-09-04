using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDistanceOfObject : MonoBehaviour
{
    public TransformToUse positionToUse = TransformToUse.local;

    // Time 
    Vector3 PositionOfTrackedObject;

    // The Veribles that are used to be tracked from this
    public float SpeedThisFrame { get; private set; }
    public float DistanceTraveled { get; private set; }
    public float TimeStartedInSeconds { get; private set; }
    public float TimeSinceStarted { get => (Time.time - TimeStartedInSeconds); }
    public float Speed { get => (TimeSinceStarted / DistanceTraveled); }

    // the position at the last time of recording
    Vector3 LastRecordedPosition;

    // Start is called before the first frame update
    void Start()
    {
        LastRecordedPosition = this.GetPosition();

        // SetupTheObject
        this.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(LastRecordedPosition, this.GetPosition());
        SpeedThisFrame = Time.deltaTime / d;
        DistanceTraveled += d;
    }

    private Vector3 GetPosition() 
    {
        switch (positionToUse)
        {
            case (TransformToUse.local):
                return this.transform.localPosition;
            case (TransformToUse.world):
                return this.transform.position;
            default:
                return this.transform.position;
        }
    }

    public void Reset()
    {
        // initialize speed verible
        SpeedThisFrame = 0;
        DistanceTraveled = 0;
        TimeStartedInSeconds = Time.time;
    }
}