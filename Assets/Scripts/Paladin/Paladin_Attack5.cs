using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Paladin_Attack5 : StateMachineBehaviour
{
    Paladin_Delegate _delegate;

    NavMeshAgent _agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Paladin_Delegate>();

            _agent = _delegate.Agent;
        }

        _agent.speed = 0;

        _delegate.Event_PlayerSummonClip();

        _delegate.State = Paladin_State.Attack5;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != Paladin_State.Attack5)
        {
            return;
        }
    }
}
