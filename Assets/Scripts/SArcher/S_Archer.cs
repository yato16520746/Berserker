using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_Archer : MonoBehaviour
{
    [Header("Shoot Arrow")]
    [SerializeField] LayerMask _checkMask;
    Ray _ray;
    RaycastHit _raycastHit;
    [SerializeField] float _range = 10f;
    [SerializeField] bool _canShoot;
    readonly float _timeCheckCanShoot = 0.5f;
    float _count;

    [Space]
    [SerializeField] Animator _animator;

    private void Start()
    {
        _ray = new Ray();
    }

    private void Update()
    {
        // CheckCanShoot() sometime
        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            CheckCanShoot();
            _count = _timeCheckCanShoot;
        }
    }

    // Note: call sometime
    void CheckCanShoot()
    {
        if (!PlayerHealth.Instance)
        {
            return;
        }

        // get distance
        Vector3 vector = PlayerHealth.Instance.transform.position - transform.position;
        vector.y = 0f;
        float distance = vector.magnitude;

        if (distance < _range)
        {
            // raycast
            _ray.origin = transform.position + Vector3.up;
            _ray.direction = vector.normalized;

            RaycastHit hit;
           
            if (Physics.Raycast(_ray, out hit, _range, _checkMask))
            {
                PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                if (playerHealth)
                {
                    _canShoot = true;
                }
                else
                {
                    _canShoot = false;
                }
            }
        }
        else
        {
            _canShoot = false;
        }

        _animator.SetBool("Can Shoot", _canShoot);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!Player.Instance)
        {
            return;
        }

        if (_canShoot)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawLine(transform.position + Vector3.up, Player.Instance.transform.position + Vector3.up);
    }

#endif
}
