using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    [SerializeField] float _time = 1f;

    void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0f)
        {
            Destroy(gameObject);    
        }
    }
}
