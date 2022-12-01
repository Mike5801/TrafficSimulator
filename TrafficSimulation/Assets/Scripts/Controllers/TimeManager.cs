using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//<summary>
// Class to Manage the Time of the simulation.
//</summary>
public class TimeManager : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute{get; private set;}
    public static int Hour{get;private set;}

    private float minuteToRealTime = 1f;
    private float timer;

    //<summary>
    // When the simulation starts, resets values of Minute and hours to 0.
    //</summary>
    void Start()
    {
        Minute = 0;
        Hour = 0;
        timer = minuteToRealTime;
        
    }

    //<summary>
    // Every frame the minute changes and calls function onMinuteChanged, and Minute>=60 adds a hour and call function onHourChanged.
    //</summary>
   void Update()
{
    timer -= Time.deltaTime;

    if(timer <= 0)
    {
        Minute++;

        OnMinuteChanged?.Invoke();

        if(Minute >= 60)
        {
            Hour++;
            OnHourChanged?.Invoke();
            Minute = 0;
        }

        timer = minuteToRealTime;
    }
}
}
