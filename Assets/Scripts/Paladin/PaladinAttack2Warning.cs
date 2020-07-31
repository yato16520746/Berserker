using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAttack2Warning : MonoBehaviour
{
    [SerializeField] Paladin_Delegate _delegate;

    [HideInInspector] public Vector3 Direction;

    void Start()
    {
        Direction = new Vector3(0f, 0f, 1f);

        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        Vector3 playerPos = PlayerHealth.Instance.transform.position;
        float distance = _delegate.Distance_Attack2;

        transform.position = playerPos + Direction * distance + new Vector3(0f, 0.02f, 0f);
    }

    void Event_Disable()
    {
        gameObject.SetActive(false);
    }
}
