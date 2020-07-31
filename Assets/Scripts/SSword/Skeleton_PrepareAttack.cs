using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_PrepareAttack : StateMachineBehaviour
{
    Skeleton_Delegate _delegate;
    Transform _parent;
    float _rotateSpeed = 20f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Skeleton_Delegate>();
            _parent = _delegate.Parent.transform;
        }

        // turn on warning
        animator.SetTrigger("Warning");
            

        _delegate.State = SkeletonState.PreapareAttak;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != SkeletonState.PreapareAttak)
        {
            return;
        }

        // quay người về phía player
        Vector3 vector = PlayerHealth.Instance.transform.position - _parent.position;
        vector.y = 0f;
        if (vector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(vector);
            _parent.rotation = Quaternion.Lerp(_parent.rotation, rotation, _rotateSpeed * Time.deltaTime);
        }
    }
}
