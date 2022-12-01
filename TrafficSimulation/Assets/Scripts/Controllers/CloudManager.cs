using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>
/// This class is responsible for managing the cloud spawning ontop the broken car.
///</Summary>
public class CloudManager : MonoBehaviour
{
    public GameObject cloud;
    public Vector3 limit = new Vector3(0, 88, 0);
    public int speed;
    private bool smoke = false;
///<Summary>
/// This method adds to the time.
    public void OnEnable(){
        Debug.Log("CloudManager OnEnable");
        TimeManager.OnMinuteChanged += TimeCheck;
    }

///<Summary>
/// This method removes from the time.
///</Summary>
    public void OnDisable()
    {
        Debug.Log("CloudManager OnDisable");
        TimeManager.OnMinuteChanged -= TimeCheck;
    }
    ///<Summary>
    /// This method checks the time and once it hits 3:23 it will begin to spawn clouds ontop of the broken car.
    ///</Summary>
     private void TimeCheck()
    {
        if(TimeManager.Hour == 3 && TimeManager.Minute == 23)
        {
            smoke = true;
        }
        if (smoke) {
            StartCoroutine(SpawnCloud());
        }
    }

    //Enumerator to spawn clouds
    ///<Summary>
    /// This coroutine spawns the clouds.
    ///</Summary>
    private IEnumerator SpawnCloud()
    {
        Debug.Log("Cloud Spawned");
        GameObject Cloud = Instantiate(cloud, new Vector3(15, 0, 668), Quaternion.Euler(0, 0, 0));
        yield return null;
    }
    ///<Summary>
    /// This method deletes the clouds.
    ///</Summary>
    void EraseCloud()
    {
        Destroy(gameObject);
    }
    
}
