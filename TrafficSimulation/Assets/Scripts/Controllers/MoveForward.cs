using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public int speed = 10;
    public int ID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move(){
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

//     public void OnEnable()
// {
//     TimeManager.OnMinuteChanged += TimeCheck;
// }

// public void OnDisable()
// {
//     TimeManager.OnMinuteChanged -= TimeCheck;
// }

// private void TimeCheck()
// {
//     if(TimeManager.Hour == 10 && TimeManager.Minute == 30)
//     {
//         StartCoroutine();
//     }
    
// }
}
