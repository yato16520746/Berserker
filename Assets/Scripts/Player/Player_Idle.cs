using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle : StateMachineBehaviour
{
    Player_Delegate _delegate;

    Rigidbody _rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Player_Delegate>();

            _rb = _delegate.Rb;
        }

        _delegate.State = PlayerState.Idle;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check state
        if (_delegate.State != PlayerState.Idle)
        {
            return;
        }

        // get value
        float speedLerp = _delegate.SpeedLerp * Time.deltaTime;
        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

        // velocity
        Vector3 velocity = new Vector3(0f, _rb.velocity.y, 0f);
        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, speedLerp);

        // xoay người khi mà vận tốc chưa = 0
        //Vector3 vector = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        //if (vector.magnitude > 0.1f)
        //{
        //    Quaternion rotation = Quaternion.LookRotation(vector);
        //    _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, rotateLerp);
        //}
    }
}
