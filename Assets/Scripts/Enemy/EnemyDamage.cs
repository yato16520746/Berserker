using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int _damage = 15;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth)
        {
            playerHealth.AddDamage(-_damage, transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
