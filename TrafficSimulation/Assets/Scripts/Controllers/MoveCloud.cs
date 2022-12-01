using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (transform.position.y >= 30) {
            Destroy(gameObject);
        }
    }

    void Move(){
        transform.Translate(Vector3.up * 10 * Time.deltaTime * 1);
    }

    void EraseCloud()
    {
        Destroy(gameObject);
    }
}
