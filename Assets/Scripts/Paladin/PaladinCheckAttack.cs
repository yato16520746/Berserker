using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinCheckAttack : MonoBehaviour
{
    enum CasterType
    {
        Ray,
        Box,
        Phere
    }

    [SerializeField] CasterType _type;
    [SerializeField] LayerMask _mask;
    [SerializeField] float _distance;
    [SerializeField] string _playerHealthName;
    RaycastHit _hit;

    [Header("Animator")]
    [SerializeField] Animator _animator;
    [SerializeField] string _triggerName;


    private void Update()
    {
        // casting
        bool isHit = false;

        if (_type == CasterType.Ray)
        {
            isHit = Physics.Raycast(transform.position, transform.forward, out _hit, _distance, _mask);
        }
        else if (_type == CasterType.Box)
        {
            isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2f, transform.forward,
                out _hit, transform.rotation, _distance, _mask);
        }
        else if (_type == CasterType.Phere)
        {
            isHit = Physics.SphereCast(transform.position, transform.localScale.x / 2, transform.forward,
                out _hit, _distance, _mask);
        }

        
        if (isHit)
        {
            if (_hit.collider.gameObject.name == _playerHealthName)
            {
                WhenHitPlayerHealth();
            }
        }
    }

    private void LateUpdate()
    {
        // looking player
        Vector3 vector = PlayerHealth.Instance.transform.position - transform.position;
        vector.y = 0f;
        if (vector.magnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(vector);
        }
    }

    void WhenHitPlayerHealth()
    {
        _animator.SetTrigger(_triggerName);
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    [Space]
    [SerializeField] bool _debug;
    RaycastHit _hitDeubg;

    private void OnDrawGizmos()
    {
        if (!_debug)
        {
            return;
        }

        if (_type == CasterType.Ray)
        {
            bool isHit = Physics.Raycast(transform.position, transform.forward, out _hitDeubg, _distance, _mask);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.forward * _hitDeubg.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * _distance);
            }
        }
        else if (_type == CasterType.Box)
        {
            bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2f, transform.forward,
                out _hitDeubg, transform.rotation, _distance, _mask);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.forward * _hitDeubg.distance);
                Gizmos.DrawWireCube(transform.position + transform.forward * _hitDeubg.distance, transform.lossyScale);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * _distance);
            }
        }
        else if (_type == CasterType.Phere)
        {
            bool isHit = Physics.SphereCast(transform.position, transform.localScale.x / 2, transform.forward,
                out _hitDeubg, _distance, _mask);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.forward * _hitDeubg.distance);
                Gizmos.DrawWireSphere(transform.position + transform.forward * _hitDeubg.distance, transform.lossyScale.x / 2);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * _distance);
            }
        }
    }

#endif
}
