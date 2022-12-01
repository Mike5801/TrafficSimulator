using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
// Class to manage the camera on the game view
//</summary>
public class DroneCameraMovement : MonoBehaviour
{

    // <summary>
    // Moves the drone camera according to the WASD keys.
    // </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * Time.deltaTime * 25);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * Time.deltaTime * 25);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * 25);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 25);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.back * Time.deltaTime * 25);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 25);
        }  
    }
}
