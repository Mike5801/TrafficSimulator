using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject rightCar;
    public GameObject leftCar;
    public GameObject CenterCar;
    public GameObject BrokenCar;
    public int counter = 0;
    public int StartPos = -95;
    public int EndPos = 2085;
    private HashSet<int> visitedIDs = new HashSet<int>(); 
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
        for (int i = 0; i < WebClient.res.data.Length; i++) {
            if (WebClient.res.data[i].car_malfunction == true && !visitedIDs.Contains(WebClient.res.data[i].id)) {
                GameObject car = Instantiate(BrokenCar, new Vector3(15, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                car.GetComponent<MoveForward>().ID = WebClient.res.data[i].id;
                visitedIDs.Add(WebClient.res.data[i].id);
            } else if (WebClient.res.data[i].posx == 0 && !visitedIDs.Contains(WebClient.res.data[i].id)) {
                GameObject car = Instantiate(rightCar, new Vector3(25, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                car.GetComponent<MoveForward>().ID = WebClient.res.data[i].id;
                visitedIDs.Add(WebClient.res.data[i].id);
            } else if (WebClient.res.data[i].posx == 1 && !visitedIDs.Contains(WebClient.res.data[i].id)) {
                GameObject car = Instantiate(CenterCar, new Vector3(15, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                car.GetComponent<MoveForward>().ID = WebClient.res.data[i].id;
                visitedIDs.Add(WebClient.res.data[i].id);
            } else if (WebClient.res.data[i].posx == 2 && !visitedIDs.Contains(WebClient.res.data[i].id)) {
                GameObject car = Instantiate(leftCar, new Vector3(5, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                car.GetComponent<MoveForward>().ID = WebClient.res.data[i].id;
                visitedIDs.Add(WebClient.res.data[i].id);
            }
        }
        // int random = Random.Range(0, 3);
        // if (random == 0) {
        //     GameObject Car = Instantiate(rightCar, new Vector3(25, 1.5f, StartPos), Quaternion.Euler(0, 270, 0)) as GameObject;
        //     // Assign counter as ID in MoveForward
        //     Car.GetComponent<MoveForward>().ID = counter;
        //     // Debug.Log(Car.GetComponent<MoveForward>().ID);
        //     // if(Car.GetComponent<MoveForward>().ID == 15){
        //     //     Car.GetComponent<MoveForward>().speed = 0;
        //     // }
        //     // counter++;
        // }
        // else if (random == 1) {
        //     GameObject Car = Instantiate(CenterCar, new Vector3(15, 1.5f, -95), Quaternion.Euler(0, 270, 0));
        //     Car.GetComponent<MoveForward>().ID = counter;
        //     // Debug.Log(Car.GetComponent<MoveForward>().ID);
        //     // if(Car.GetComponent<MoveForward>().ID == 15){
        //     //     Car.GetComponent<MoveForward>().speed = 0;
        //     // }
        //     // counter++;
        // }
        // else if (random == 2) {
        //     GameObject Car = Instantiate(leftCar, new Vector3(5, 1.5f, -95), Quaternion.Euler(0, 270, 0));
        //     Car.GetComponent<MoveForward>().ID = counter;
        //     // Debug.Log(Car.GetComponent<MoveForward>().ID);
        //     // if(Car.GetComponent<MoveForward>().ID == 15){
        //     //     Car.GetComponent<MoveForward>().speed = 0;
        //     // }
        //     // counter++;
        // }
        yield return null;      
    }
}