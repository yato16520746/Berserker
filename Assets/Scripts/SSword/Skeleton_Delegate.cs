using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SkeletonState
{
    PreapareAttak,
    Attack,
    Idle,
    Walk,
    Death
}

public class Skeleton_Delegate : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent { get { return _navAgent; } }

    [SerializeField] Rigidbody _rb; 
    public Rigidbody Rb { get { return _rb; } }

    [SerializeField] GameObject _parent;
    public GameObject Parent { get { return _parent; } }

    [SerializeField] Collider _myCollider;
    public Collider MyCollider { get { return _myCollider; } }

    // trạng thái của Skeleton
    [HideInInspector] public SkeletonState State;

    [Header("Walk")]
    [SerializeField] float _walkSpeed = 3.5f;
    public float WalkSpeed { get { return _walkSpeed; } }

    [Space]
    [Header("Damage Sphere Cast")]
    [SerializeField] SphereCastDamage _myDamage;
    public SphereCastDamage MyDamage { get { return _myDamage; } }

    private void Start()
    {
        MyDamage.gameObject.SetActive(false);
    }

    // bỏ hàm này vào giây cuối cùng của quái
    public void DestroyMe()
    {
        _parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }
}
