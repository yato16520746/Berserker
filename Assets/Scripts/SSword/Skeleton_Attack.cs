using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Attack : StateMachineBehaviour
{
    Skeleton_Delegate _delegate;
    Rigidbody _rb;
    Transform _parent;
    SphereCastDamage _myDamage;

    readonly float _time = 0.4f;
    float _count;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Skeleton_Delegate>();
            _rb = _delegate.Rb;
            _parent = _delegate.Parent.transform;
            _myDamage = _delegate.MyDamage;
        }

        _count = _time;

        Vector3 direction = _parent.rotation * Vector3.forward;

        _rb.isKinematic = false;
        _rb.velocity = direction * 50f;

        _myDamage.gameObject.SetActive(true);

        _delegate.State = SkeletonState.Attack;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo  stateInfo, int layerIndex)
    {
        if (_delegate.State != SkeletonState.Attack)
        {
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true;

            _myDamage.gameObject.SetActive(false);

            return;
        }

        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 8f * Time.deltaTime);

        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            animator.SetBool("Attack", false);
        }


    }
}
