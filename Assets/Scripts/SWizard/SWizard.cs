using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWizard : MonoBehaviour
{
    [SerializeField] float _attackRange = 7f;
    public float AttackRange { get { return _attackRange; } }
    readonly float _timeCheckAttack = 0.4f;
    float _count;

    [Space]
    [SerializeField] Animator _myAnim;

    private void Start()
    {
        // random quaternion    
        Vector3 direction = Random.insideUnitSphere;
        direction.y = 0f;
        if (direction.magnitude < 0.001f)
        {
            direction.z = 1f;
        }

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    private void Update()
    {
        _count -= Time.deltaTime;
        if (_count < 0)
        {
            CheckCanAttack();
            _count = _timeCheckAttack;
        }
    }

    void CheckCanAttack()
    {
        if (!PlayerHealth.Instance)
        {
            _myAnim.SetBool("Can Attack", false);
        }
            
        // get distance
        Vector3 distance = PlayerHealth.Instance.transform.position - transform.position;
        distance.y = 0f;

        if (distance.magnitude < _attackRange)
        {
            _myAnim.SetBool("Can Attack", true);
        }
        else
        {
            _myAnim.SetBool("Can Attack", false);
        }
    }
}
