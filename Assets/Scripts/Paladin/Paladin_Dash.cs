using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Paladin_Dash : StateMachineBehaviour
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

        _agent.stoppingDistance = 1.5f;

        _delegate.State = Paladin_State.Dash;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != Paladin_State.Dash)
        {
            //_delegate.Event_DisableCheckAttack3();

            return;
        }

        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

        _agent.speed = Mathf.Lerp(_agent.speed, _delegate.Attack3_moveSpeed1, 8f * Time.deltaTime);
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
