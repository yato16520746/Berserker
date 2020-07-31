using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Paladin_Attack4 : StateMachineBehaviour
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

        _delegate.State = Paladin_State.Attack4;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != Paladin_State.Attack4)
        {
            return;
        }

        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

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
