using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Paladin_Attack1 : StateMachineBehaviour
{
    Paladin_Delegate _delegate;

    NavMeshAgent _agent;
    PlayerHealth _player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Paladin_Delegate>();

            _agent = _delegate.Agent;
            _player = PlayerHealth.Instance;
        }

        _delegate.Event_setStateSpeed(_delegate.Attack1_MoveSpeed);
        _delegate.StopMoving_Attack1 = false;

        //_delegate.Event_TurnOffAgentRotate();

        _agent.stoppingDistance = 2f;

        _delegate.State = Paladin_State.Attack1;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != Paladin_State.Attack1)
        {
            return;
        }

        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

        _agent.speed = _delegate.StateSpeed;

        if (!_delegate.StopMoving_Attack1)
        {
            _agent.SetDestination(_player.transform.position);

            // looking player
            Vector3 vector = PlayerHealth.Instance.transform.position - animator.transform.position;
            vector.y = 0f;
            if (vector.magnitude > 0.0001f)
            {
                Quaternion rotation = Quaternion.LookRotation(vector);
                _agent.transform.rotation = Quaternion.Lerp(_agent.transform.rotation, rotation, rotateLerp);
            }
        }
    }
}
