using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SArcher_Death : StateMachineBehaviour
{
    SArcher_Delegate _delegate;

    Collider _collider;
    NavMeshAgent _agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SArcher_Delegate>();

            _collider = _delegate.Collider;
            _agent = _delegate.NavMeshAgent;
        }

        //_collider.gameObject.SetActive(false);
        _agent.isStopped = true;

        _delegate.State = SArcherState.Death;
    }
}
