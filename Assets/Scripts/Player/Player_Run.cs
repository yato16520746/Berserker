using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Run : StateMachineBehaviour
{
    Player_Delegate _delegate;

    Player _parent;
    Transform _parentTransform;
    Rigidbody _rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Player_Delegate>();

            _parent = _delegate.Parent;
            _parentTransform = _parent.gameObject.transform;
            _rb = _delegate.Rb;
        }

        _delegate.State = PlayerState.Run;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check state
        if (_delegate.State != PlayerState.Run)
        {
            return;
        }

        // get values
        Vector3 moveDirection = _parent.MoveDirection;
        float runSpeed = _delegate.RunSpeed;
        float speedLerp = _delegate.SpeedLerp * Time.deltaTime;
        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

        // velocity
        Vector3 velocity = new Vector3(moveDirection.x * runSpeed, _rb.velocity.y, moveDirection.z * runSpeed);
        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, speedLerp);

        // rotate
        Vector3 vector = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        if (vector.magnitude > 0.3f)
        {
            Quaternion rotation = Quaternion.LookRotation(vector);
            _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, rotateLerp);
        }

    }
}
