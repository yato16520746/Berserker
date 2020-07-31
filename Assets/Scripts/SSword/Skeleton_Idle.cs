using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Idle : StateMachineBehaviour
{
    Skeleton_Delegate _delegate;

    [SerializeField] float _minTime = 1f;
    [SerializeField] float _maxTime = 2f;
    float _count;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Skeleton_Delegate>();
        }

        _count = Random.Range(_minTime, _maxTime);

        _delegate.State = SkeletonState.Idle;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State !=  SkeletonState.Idle)
        {
            return;
        }

        _count -= Time.deltaTime;
        if (_count < 0)
        {
            animator.SetTrigger("Escape Idle");
        }
    }
}
