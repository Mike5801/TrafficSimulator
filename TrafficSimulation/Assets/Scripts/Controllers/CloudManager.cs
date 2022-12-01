using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject cloud;
    public Vector3 limit = new Vector3(0, 88, 0);
    public int speed;
        public void OnEnable(){
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    public void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }
     private void TimeCheck()
    {
        if(TimeManager.Minute >= 23 && TimeManager.Hour >= 3)
        {
            StartCoroutine(SpawnCloud());
        }
        
    }

    //Enumerator to spawn clouds
    private IEnumerator SpawnCloud()
    {
        GameObject Cloud = Instantiate(cloud, new Vector3(15, 0, 650), Quaternion.Euler(0, 0, 0));
        Cloud.transform.Translate(Vector3.up * speed * Time.deltaTime * 1);
        if (Cloud.transform.position.z >= limit.z) {
            Destroy(Cloud);
        }
    
        yield return null;
    }

    void EraseCloud()
    {
        Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
