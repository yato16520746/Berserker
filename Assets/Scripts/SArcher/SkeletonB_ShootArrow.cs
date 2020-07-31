using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonB_ShootArrow : StateMachineBehaviour
{
    SArcher_Delegate _delegate;
    Transform _shootTransform;
    Transform _parentTransform;
    [SerializeField] float _rotateSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SArcher_Delegate>();
            _shootTransform = _delegate.ShootTransform;
            _parentTransform = _delegate.ParentTransform;
        }

        _delegate.RotatingWhenShootArrow = true;

        // set biến kiểm tra state
        _delegate.State = SArcherState.DrawArrow;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // kiểm tra state
        if (_delegate.State != SArcherState.DrawArrow)
        {
            return;
        }

        if (!_delegate.RotatingWhenShootArrow)
        {
            return;
        }

        Vector3 playerRb = Berserker.Instance.RbVelocity;
        playerRb.y = 0f;
        if (playerRb.magnitude < 0.1f)
        {
            playerRb = Vector3.zero;
        }
        else
        {
            playerRb = playerRb.normalized;
        }
        playerRb = playerRb.normalized;
        Vector3 targetPosition = PlayerHealth.Instance.transform.position + playerRb * 0.8f;

        Vector3 vector_2 = targetPosition - animator.transform.position;
        vector_2.y = 0f;
        if (vector_2.magnitude < 1.3f)
        {
            // player đứng quá gần, xoay người vào Player
            Quaternion rotation = Quaternion.LookRotation(vector_2);
            _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);

            return;
        }

        // quay người (dựa vào _shootTransform) về phía player
        Vector3 vector = targetPosition - _shootTransform.position;
        vector.y = 0f;
        if (vector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(vector);
            _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        }
    }
}
