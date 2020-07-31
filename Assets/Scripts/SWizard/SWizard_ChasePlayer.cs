using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SWizard_ChasePlayer : StateMachineBehaviour
{
    SWizard_Delegate _delegate;

    [SerializeField] float _distance = 2.5f;
    NavMeshAgent _agent;
    Vector3 _destination;
    Berserker _player;
    SWizard _parent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SWizard_Delegate>();

            _agent = _delegate.Agent;
            _player = Berserker.Instance;
            _parent = _delegate.Parent;
        }

        _agent.isStopped = false;

        _destination = _player.RandomPositionNearMe(_distance);
        _agent.SetDestination(_destination);

        _delegate.State = SWizard_State.ChasePlayer;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != SWizard_State.ChasePlayer)
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }

            return;
        }

        Vector3 distance = _destination - _player.transform.position;
        if (distance.magnitude >= _parent.AttackRange)
        {
            _destination = _player.RandomPositionNearMe(_distance);
            _agent.SetDestination(_destination);
        }
    }
}
