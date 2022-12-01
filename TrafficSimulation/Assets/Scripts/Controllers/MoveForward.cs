using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
// Class assign to every car that allows him to move and change lanes.
//</summary>
public class MoveForward : MonoBehaviour
{
    public int speed = 10;
    public int horizontalSpeed = 10;
    public int ID = 0;
    public Vector3 limitPosition = new Vector3(0, 0, 1000);
    public Vector3 limitPositionRight = new Vector3(25, 0, 0);
    public Vector3 limitPositionLeft = new Vector3(5, 0, 0);
    public int posy;
    public int previousPosy;
    public int posx = -1;
    public int previousPosx = -1;

    //<summary>
    // Every frame while the car doesn't reach certain position calls function Move, or else destroy itself.
    //</summary>
    void Update()
    {
        
        if (transform.position.z >= limitPosition.z) {
            Destroy(gameObject);
        } else {
            Move();

        }
    }

    //<summary>
    // The car moves forward, right or left, depending on their previous position and actual position
    //</summary>
    void Move(){
        if (previousPosx != posx) {
            if (posx == 0) {
                if (transform.position.x > limitPositionLeft.x) {
                    transform.Translate(Vector3.forward * horizontalSpeed * Time.deltaTime * 1);
                } else {
                    previousPosx = posx;
                    previousPosy += 3; 
                }
            } else if (posx == 2) {
                if (transform.position.x < limitPositionRight.x) {
                    transform.Translate(Vector3.back * horizontalSpeed * Time.deltaTime * 1);
                } else {
                    previousPosx = posx;
                    previousPosy += 3; 
                }
            }
        } 
        if (previousPosy - posy >= 1) {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 1);
        } else if (previousPosy - posy == 0) {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 0);
        }
    }
}
