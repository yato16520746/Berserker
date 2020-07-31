using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_Walk : StateMachineBehaviour
{
    Skeleton_Delegate _delegate;

    // game AI
    NavMeshAgent _agent;
    [SerializeField] float _timeSetDestination;
    float _count;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Skeleton_Delegate>();
            _agent = _delegate.NavAgent;
        }

        _count = 0f;
        _agent.speed = _delegate.WalkSpeed;
        _agent.SetDestination(PlayerHealth.Instance.transform.position);

        _delegate.State = SkeletonState.Walk;
    }   

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // kiểm tra đã chuyển state hay chưa
        if (_delegate.State != SkeletonState.Walk)
        {


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

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _agent.speed = 0f;
    }
}
