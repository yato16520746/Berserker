using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonB_RunningRandom : StateMachineBehaviour
{
    SArcher_Delegate _delegate;

    // game AI
    NavMeshAgent _agent;
    Vector3 _destination;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SArcher_Delegate>();
            _agent = _delegate.NavMeshAgent;
        }

        _agent.isStopped = false;

        Vector3 randomDirection = Random.insideUnitSphere;
        float distance = Random.Range(2.5f, 10f);
        randomDirection.y = 0f;
        while (randomDirection.magnitude < 0.05f)
        {
            randomDirection.z = 1f;
        }
        Vector3 randomPosition = animator.transform.position
            + randomDirection.normalized * distance;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, 10f, 1);
        _destination = hit.position;

        _agent.SetDestination(_destination);

        // set biến kiểm tra state
        _delegate.State = SArcherState.RunningRandom;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // kiểm tra state
        if (_delegate.State != SArcherState.RunningRandom)
        {
            if (!_agent.isStopped)
            {
                
                _agent.isStopped = true;
                _agent.ResetPath();
            }

            return;
        }

        Vector3 vector = _destination - animator.transform.position;
        vector.y = 0f;
        if (vector.magnitude < 0.4f)
        {
            animator.SetBool("Running Random", false);
        }
    }
}
