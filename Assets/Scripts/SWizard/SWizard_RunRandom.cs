using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SWizard_RunRandom : StateMachineBehaviour
{
    SWizard_Delegate _delegate;

    [SerializeField] float _minDistance = 4f;
    [SerializeField] float _maxDistance = 7f;

    // game AI
    NavMeshAgent _agent;
    Vector3 _destination;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SWizard_Delegate>();

            _agent = _delegate.Agent;
        }

        _agent.isStopped = false;

        // random position from NavMesh
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f;
        Vector3 randomPosition = animator.transform.position 
            + randomDirection.normalized * Random.Range(_minDistance, _maxDistance);

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, 10f, 1);
        _destination = hit.position;
        _agent.SetDestination(_destination);


        _delegate.State = SWizard_State.RunningRandom;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // kiểm tra state
        if (_delegate.State != SWizard_State.RunningRandom)
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }

            return;
        }


        Vector3 distance = _destination - animator.transform.position;
        distance.y = 0f;
        if (distance.magnitude < 0.25f)
        {
            animator.SetBool("Running Random", false);
            _delegate.Mode = SWizard_Mode.SummonFire;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }
}
