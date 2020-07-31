using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Paladin_Idle : StateMachineBehaviour
{
    Paladin_Delegate _delegate;

    NavMeshAgent _agent;

    [SerializeField] float _minTime = 0.25f;
    [SerializeField] float _maxTime = 0.5f;
    float _count;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Paladin_Delegate>();

            _agent = _delegate.Agent;
        }

        _agent.speed = 0;

        // random attack
        _delegate.Event_Idle_CalculateCombo();

        if (_delegate.KeepingIdle)
        {
            _count = Random.Range(_minTime, _maxTime);
        }
        else
        {
            _count = 100f;
            animator.SetTrigger("Escape Idle");
        }


        _delegate.State = Paladin_State.Idle;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != Paladin_State.Idle)
        {
            return;
        }

        _count -= Time.deltaTime;
        if (_count < 0)
        {
            animator.SetTrigger("Escape Idle");
            _count = 100f;
        }
    }
}
