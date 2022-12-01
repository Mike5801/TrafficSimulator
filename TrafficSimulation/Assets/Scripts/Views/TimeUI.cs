using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//<summary>
// Class that updates the time on the TextMesh.
//</summary>
public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    
    // <summary>
    // Detect changes on the TimeManager and updates time.
    // </summary>
    private void OnEnable()
{
   TimeManager.OnMinuteChanged += UpdateTime;
   TimeManager.OnHourChanged += UpdateTime;
}

    //<summary>
    // When TimeManager is disabled it start reseting values.
    //</summary>
    private void OnDisable()
{
   TimeManager.OnMinuteChanged -= UpdateTime;
   TimeManager.OnHourChanged -= UpdateTime;
}

    //<summary>
    // Changes the text on timetext depending on the time of the environment.
    //</summary>
    private void UpdateTime()
{
    timeText.text = $"{TimeManager.Hour.ToString("00")}:{TimeManager.Minute:00}";
}
}

