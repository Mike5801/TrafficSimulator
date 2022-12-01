using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is used to spawn cars in the game.
/// </summary>
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
    private HashSet<int> leftVisitedIDs = new HashSet<int>();
    private HashSet<int> rightVisitedIDs = new HashSet<int>();

    /// <summary>
    /// This method is used to add time to the counter.
    /// </summary>
    public void OnEnable(){
        TimeManager.OnMinuteChanged += TimeCheck;
        Debug.Log("CarSpawner OnEnable");
    }

    /// <summary>
    /// This method is used to remove time from the counter.
    /// </summary>
    public void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
        Debug.Log("CarSpawner OnDisable");
    }
/// <summary>
/// This method is used to call the SpawnCar coroutine every 1 second.
/// </summary>
    private void TimeCheck()
    {
        if(TimeManager.Minute % 1 == 0)
        {
            StartCoroutine(SpawnCarRandom());
        }
        
    }

    ///<summary>
    /// Corotuine that Spawns a car randomly in 3 of the given positions (left, center, right) eventually with a broken car in the center lane.
    ///</summary>
    ///<returns>IEnumerator</returns>
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
            } else if (posx == 2) {
                if (!visitedIDs.Contains(id)) {
                    GameObject car = Instantiate(rightCar, new Vector3(25, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                    car.GetComponent<MoveForward>().ID = id;
                    car.GetComponent<MoveForward>().previousPosy = posy + 1;
                    car.GetComponent<MoveForward>().posy = posy;
                    rightVisitedIDs.Add(id);
                    visitedIDs.Add(id);
                } else if (visitedIDs.Contains(id)) {
                    if (rightVisitedIDs.Contains(id)) {
                        GameObject[] cars;
                        cars = GameObject.FindGameObjectsWithTag("Right");
                        foreach (GameObject car in cars) {
                            if (car.GetComponent<MoveForward>().ID == id) {
                                car.GetComponent<MoveForward>().previousPosy = car.GetComponent<MoveForward>().posy;
                                car.GetComponent<MoveForward>().posy = posy;
                            }
                        }
                    } else if (!rightVisitedIDs.Contains(id)) {
                        GameObject[] carsCenter;
                        carsCenter = GameObject.FindGameObjectsWithTag("Center");
                        foreach (GameObject carCenter in carsCenter) {
                            if (carCenter.GetComponent<MoveForward>().ID == id) {
                                carCenter.GetComponent<MoveForward>().posx = posx;

                                carCenter.GetComponent<MoveForward>().previousPosy = carCenter.GetComponent<MoveForward>().posy;
                                carCenter.GetComponent<MoveForward>().posy = posy;
                            }
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
                        }
                    }
                }
            } else if (posx == 0) {
                if (!visitedIDs.Contains(id)) {
                    GameObject car = Instantiate(leftCar, new Vector3(5, 1.5f, StartPos), Quaternion.Euler(0, 270, 0));
                    car.GetComponent<MoveForward>().previousPosy = posy + 1;
                    car.GetComponent<MoveForward>().posy = posy;
                    car.GetComponent<MoveForward>().ID = id;
                    leftVisitedIDs.Add(id);
                    visitedIDs.Add(id);
                } else if (visitedIDs.Contains(id)) {
                    if (leftVisitedIDs.Contains(id)) {
                        GameObject[] cars;
                        cars = GameObject.FindGameObjectsWithTag("Left");
                        foreach (GameObject car in cars) {
                            if (car.GetComponent<MoveForward>().ID == id) {
                                car.GetComponent<MoveForward>().previousPosy = car.GetComponent<MoveForward>().posy;
                                car.GetComponent<MoveForward>().posy = posy;
                            }
                        }
                    } else if (!leftVisitedIDs.Contains(id)) {
                        GameObject[] carsCenter;
                        carsCenter = GameObject.FindGameObjectsWithTag("Center");
                        foreach (GameObject carCenter in carsCenter) {
                            if (carCenter.GetComponent<MoveForward>().ID == id) {
                                carCenter.GetComponent<MoveForward>().posx = posx;

                                carCenter.GetComponent<MoveForward>().previousPosy = carCenter.GetComponent<MoveForward>().posy;
                                carCenter.GetComponent<MoveForward>().posy = posy;
                            }
                        }
                    }

                }
            }
        }
        yield return null;      
    }
}