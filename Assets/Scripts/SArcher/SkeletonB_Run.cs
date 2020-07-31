using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonB_Run : StateMachineBehaviour
{
    SArcher_Delegate _delegate;

    // game AI
    NavMeshAgent _agent;
    [SerializeField] float _timeSetDestination = 0.33f;
    float _count;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SArcher_Delegate>();
            _agent = _delegate.NavMeshAgent;
        }

        _count = 0f;
        _agent.isStopped = false;

        // set biến kiểm tra state
        _delegate.State = SArcherState.ChasingPlayer;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // kiểm tra đã chuyển state hay chưa
        if (_delegate.State != SArcherState.ChasingPlayer)
        {
            // chỉ reset path 1 lần khi đã ko còn ở state này
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
     
            return;
        }

        // tìm đường chạy đến player sau mỗi khoảng tg
        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            _agent.SetDestination(PlayerHealth.Instance.transform.position);
            _count = _timeSetDestination;
        }
    }
}
