using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonB_Idle : StateMachineBehaviour
{
    SArcher_Delegate _delegate;
    
    [SerializeField] float _minTime;
    [SerializeField] float _maxTime;
    float _count;
        
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SArcher_Delegate>();
        }

        animator.SetBool("Idling", true);
        _count = Random.Range(_minTime, _maxTime);

        _delegate.State = SArcherState.Idle;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check state
        if (_delegate.State != SArcherState.Idle)
        {
            return;
        }

        // exit Ilde state after delay time
        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            animator.SetBool("Idling", false);
        }
    }
}
