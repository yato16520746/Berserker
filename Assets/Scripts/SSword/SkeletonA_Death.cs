using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonA_Death : StateMachineBehaviour
{
    Skeleton_Delegate _delegate;

    Rigidbody _rb;
    Transform _parentTransform;
    SphereCastDamage _myDamage;
    Collider _myCollider;
    NavMeshAgent _agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Skeleton_Delegate>();

            _rb = _delegate.Rb;
            _parentTransform = _delegate.Parent.transform;
            _myDamage = _delegate.MyDamage;
            _myCollider = _delegate.MyCollider;
            _agent = _delegate.NavAgent;
        }

        _myDamage.gameObject.SetActive(false);
        _rb.isKinematic = true;
        _agent.speed = 0f;

        _delegate.State = SkeletonState.Death;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State !=  SkeletonState.Death)
        {
            return;
        }
    }
}
