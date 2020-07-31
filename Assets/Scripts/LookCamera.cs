using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = Camera.main.transform.position - transform.position;
        vector.x = 0;

        Quaternion rotation = Quaternion.LookRotation(vector);
        transform.rotation = rotation;

    }
}
