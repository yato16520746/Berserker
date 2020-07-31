using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecPlayerHealth : MonoBehaviour
{
    bool _inRange;
    public bool InRange { get { return _inRange; } }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player Health")
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player Health")
        {
            _inRange = false;
        }
    }
}
