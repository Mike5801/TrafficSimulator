using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject rightCar;
    public GameObject leftCar;
    public GameObject CenterCar;
    // Start is called before the first frame update
    public void OnEnable(){
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    public void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    private void TimeCheck()
{
    if(TimeManager.Minute % 1 == 0)
    {
        StartCoroutine(SpawnCarRandom());
    }
    
}

    // Update is called once per frame
    private IEnumerator SpawnCarRandom()
    {
        int random = Random.Range(0, 3);
        if (random == 0) {
            Instantiate(rightCar, new Vector3(25, 1.5f, -95), Quaternion.Euler(0, 270, 0));
        }
        else if (random == 1) {
            Instantiate(CenterCar, new Vector3(15, 1.5f, -95), Quaternion.Euler(0, 270, 0));
        }
        else if (random == 2) {
            Instantiate(leftCar, new Vector3(5, 1.5f, -95), Quaternion.Euler(0, 270, 0));
        }
        yield return null;      
    }
}