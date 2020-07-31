using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Paladin_Walk : StateMachineBehaviour
{
    Paladin_Delegate _delegate;

    NavMeshAgent _agent;
    [SerializeField] float _timeSetDestination = 0.4f;
    float _count;
    PlayerHealth _player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Paladin_Delegate>();

            _agent = _delegate.Agent;
            _player = PlayerHealth.Instance;
        }

        _count = 0f;
        _agent.speed = _delegate.WalkSpeed;
        //_delegate.Event_TurnOnAgentRotate();

        _delegate.CheckAttack1.gameObject.SetActive(true);

        _delegate.State = Paladin_State.Walk;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != Paladin_State.Walk)
        {
            _delegate.CheckAttack1.gameObject.SetActive(false);
            ////_delegate.Event_TurnOffAgentRotate();

            return;
        }

        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            _agent.SetDestination(_player.transform.position);
            _count = _timeSetDestination;
        }
    }
}
