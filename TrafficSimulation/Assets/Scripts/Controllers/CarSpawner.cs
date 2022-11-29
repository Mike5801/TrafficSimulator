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
        Car[] carsData = WebClient.res.data;

        for (int i = 0; i < carsData.Length; i++) {
            int id = carsData[i].id;
            int posx = carsData[i].posx;
            int posy = carsData[i].posy;
            bool car_malfunction = carsData[i].car_malfunction;
            if (car_malfunction == true) {
                if (!visitedIDs.Contains(id)) {
                    GameObject car = Instantiate(BrokenCar, new Vector3(15, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                    car.GetComponent<MoveForward>().ID = id;
                    car.GetComponent<MoveForward>().previousPosy = posy + 1;
                    car.GetComponent<MoveForward>().posy = posy;
                    visitedIDs.Add(id);
                } else if (visitedIDs.Contains(id)) {
                    GameObject[] cars;
                    cars = GameObject.FindGameObjectsWithTag("MalFunction");
                    foreach (GameObject car in cars) {
                        if (car.GetComponent<MoveForward>().ID == id) {
                            car.GetComponent<MoveForward>().previousPosy = car.GetComponent<MoveForward>().posy;
                            car.GetComponent<MoveForward>().posy = posy;
                        }
                    }
                }
            } else if (posx == 0) {
                if (!visitedIDs.Contains(id)) {
                    GameObject car = Instantiate(rightCar, new Vector3(25, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                    car.GetComponent<MoveForward>().ID = id;
                    car.GetComponent<MoveForward>().previousPosy = posy + 1;
                    car.GetComponent<MoveForward>().posy = posy;
                    visitedIDs.Add(id);
                } else if (visitedIDs.Contains(id)) {
                    GameObject[] cars;
                    cars = GameObject.FindGameObjectsWithTag("Right");
                    foreach (GameObject car in cars) {
                        if (car.GetComponent<MoveForward>().ID == id) {
                            car.GetComponent<MoveForward>().previousPosy = car.GetComponent<MoveForward>().posy;
                            car.GetComponent<MoveForward>().posy = posy;
                        }
                    }
                }
            } else if (posx == 1) {
                if (!visitedIDs.Contains(id)) {
                    GameObject car = Instantiate(CenterCar, new Vector3(15, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                    car.GetComponent<MoveForward>().ID = id;
                    car.GetComponent<MoveForward>().previousPosy = posy + 1;
                    car.GetComponent<MoveForward>().posy = posy;
                    car.GetComponent<MoveForward>().previousPosx = posx;
                    car.GetComponent<MoveForward>().posx = posx;

                    Debug.Log(car.GetComponent<MoveForward>().previousPosx);
                    Debug.Log(car.GetComponent<MoveForward>().posx);
                    visitedIDs.Add(id);
                } else if (visitedIDs.Contains(id)) {
                    GameObject[] cars;
                    cars = GameObject.FindGameObjectsWithTag("Center");
                    foreach (GameObject car in cars) {
                        if (car.GetComponent<MoveForward>().ID == id) {
                            car.GetComponent<MoveForward>().previousPosy = car.GetComponent<MoveForward>().posy;
                            car.GetComponent<MoveForward>().posy = posy;

                            car.GetComponent<MoveForward>().previousPosx = car.GetComponent<MoveForward>().posx;
                            car.GetComponent<MoveForward>().posx = posx;

                            Debug.Log(car.GetComponent<MoveForward>().previousPosx);
                            Debug.Log(car.GetComponent<MoveForward>().posx);
                        }
                    }
                }
            } else if (posx == 2) {
                if (!visitedIDs.Contains(id)) {
                    GameObject car = Instantiate(leftCar, new Vector3(5, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                    car.GetComponent<MoveForward>().previousPosy = posy + 1;
                    car.GetComponent<MoveForward>().posy = posy;
                    car.GetComponent<MoveForward>().ID = id;
                    visitedIDs.Add(id);
                } else if (visitedIDs.Contains(id)) {
                    GameObject[] cars;
                    cars = GameObject.FindGameObjectsWithTag("Left");
                    foreach (GameObject car in cars) {
                        if (car.GetComponent<MoveForward>().ID == id) {
                            car.GetComponent<MoveForward>().previousPosy = car.GetComponent<MoveForward>().posy;
                            car.GetComponent<MoveForward>().posy = posy;
                        }
                    }
                }
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