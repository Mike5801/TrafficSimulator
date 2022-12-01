using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for moving the cloud upwards.
/// </summary>
public class MoveCloud : MonoBehaviour
{
    /// <summary>
    ///Moves the cloud upwards on game tick. if it's position is above 30 it will be destroyed.
    /// </summary>
    void Update()
    {
        Move();
        if (transform.position.y >= 30) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Moves the cloud upwards.
    /// </summary>
    void Move(){
        transform.Translate(Vector3.up * 10 * Time.deltaTime * 1);
    }

/// <summary>
/// Deletes the cloud.
/// </summary>
    void EraseCloud()
    {
        Destroy(gameObject);
    }
}
