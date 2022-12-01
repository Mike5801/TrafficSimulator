using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject cloud;
    public Vector3 limit = new Vector3(0, 88, 0);
    public int speed;
    public void OnEnable(){
        Debug.Log("CloudManager OnEnable");
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    public void OnDisable()
    {
        Debug.Log("CloudManager OnDisable");
        TimeManager.OnMinuteChanged -= TimeCheck;
    }
     private void TimeCheck()
    {
        if(TimeManager.Hour >= 3 && TimeManager.Minute >= 23)
        {
            StartCoroutine(SpawnCloud());
        }
        
    }

    //Enumerator to spawn clouds
    private IEnumerator SpawnCloud()
    {
        Debug.Log("Cloud Spawned");
        GameObject Cloud = Instantiate(cloud, new Vector3(15, 0, 665), Quaternion.Euler(0, 0, 0));
        Cloud.transform.Translate(Vector3.up * speed * Time.deltaTime * 1);
        if (Cloud.transform.position.y >= limit.y) {
            Destroy(Cloud);
        }
    
        yield return null;
    }

    void EraseCloud()
    {
        Destroy(gameObject);
    }
    
}
