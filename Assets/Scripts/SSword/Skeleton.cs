using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    public static string Attack_Bool = "Attack";

    [SerializeField] Animator _animator;
    [SerializeField] Skeleton_Delegate _myDelegate;

    [Space]
    [SerializeField] float _attackDistance;
    readonly float _timeCheckAttack = 0.7f;
    float _count;
    [SerializeField] LayerMask _rayMask;
    Ray _ray;
    RaycastHit _raycastHit;

    private void Start()
    {
        _ray = new Ray();
    }

    private void Update()   
    {
        // gọi CheckAttack() ở mỗi khoảng tg
        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            CheckAttack();
            _count = _timeCheckAttack;
        }

        // check player Dead
         _animator.SetBool("Player Dead", PlayerHealth.Instance.IsDead);     
    }

    // Note: không được gọi hàm này liên tục
    void CheckAttack()
    {
        if (_myDelegate.State == SkeletonState.Attack ||
            _myDelegate.State == SkeletonState.PreapareAttak ||
            _myDelegate.State == SkeletonState.Death)
        {
            return;
        }

        // check khoảng cách
        Vector3 vector = PlayerHealth.Instance.transform.position - transform.position;
        vector.y = 0f;
        float distance = vector.magnitude;

        float range = _attackDistance + Random.Range(-_attackDistance / 8, _attackDistance / 8);

        if (distance < range)
        {
            // trong tầm, check xem có vật cản hay không
            _ray.origin = transform.position + Vector3.up;
            _ray.direction = vector.normalized;

#if UNITY_EDITOR
            Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * range);
#endif

            if (Physics.Raycast(_ray, out _raycastHit, distance, _rayMask))
            {
                PlayerHealth playerHealth = _raycastHit.collider.GetComponent<PlayerHealth>();
                if (playerHealth)
                {
                    _animator.SetBool("Attack", true);
                }
            }
        }      
    }
}
