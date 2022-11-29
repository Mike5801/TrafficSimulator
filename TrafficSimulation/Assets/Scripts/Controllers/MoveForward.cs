using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public int speed = 10;
    public int horizontalSpeed = 10;
    public int ID = 0;
    public Vector3 limitPosition = new Vector3(0, 0, 1000);
    public int posy;
    public int previousPosy;
    public int posx = -1;
    public int previousPosx = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.z >= limitPosition.z) {
            Destroy(gameObject);
        } else {
            Move();
        }
    }

    void Move(){
        if (previousPosx != posx) {
            if (posx == 0) {
                float time = 1f;
                while (time > 0) {
                    transform.Translate(Vector3.forward * horizontalSpeed * Time.deltaTime * 1);
                    time -= Time.deltaTime;
                }
                previousPosx = posx;
                previousPosy += 3; 
            } else if (posx == 2) {
                float time = 1f;
                while (time > 0) {
                    transform.Translate(Vector3.back * horizontalSpeed * Time.deltaTime * 1);
                    time -= Time.deltaTime;
                }
                previousPosx = posx;
                previousPosy += 3; 
            }
        } 
        
        if (previousPosy - posy >= 1) {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 1);
        } else if (previousPosy - posy == 0) {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 0);
        }
    }
}
