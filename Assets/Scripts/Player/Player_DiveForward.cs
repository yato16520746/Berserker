using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DiveForward : StateMachineBehaviour
{
    Player_Delegate _delegate;

    Player _parent;
    Transform _parentTransform;
    Rigidbody _rb;
    Vector3 _direction;

    float _count;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Player_Delegate>();

            _parent = _delegate.Parent;
            _parentTransform = _parent.gameObject.transform;
            _rb = _delegate.Rb;
        }

        Vector3 moveDirection = _parent.MoveDirection;
        if (moveDirection != Vector3.zero)
        {
            _direction = moveDirection;
        }
        else
        {
            _direction = _parentTransform.rotation * Vector3.forward;
        }

        _delegate.Stamina.Dive();

        _delegate.SlowDive = false;

        _delegate.State = PlayerState.DiveForward;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check state
        if (_delegate.State != PlayerState.DiveForward)
        {
            //animator.SetBool("Dive Forward", false);
            return;
        }

        // get values
        Vector3 moveDirection = _parent.MoveDirection;
        float diveSpeed = _delegate.DiveSpeed;
        float speedLerp = _delegate.SpeedLerp * Time.deltaTime;
        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

        // velocity
        if (!_delegate.SlowDive)
        {
            Vector3 velocity = new Vector3(_direction.x * diveSpeed, _rb.velocity.y, _direction.z * diveSpeed);
            _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, speedLerp);
        }
        else
        {
            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 3.5f * Time.deltaTime);
        }

    

        // rotate
        Vector3 look = new Vector3(_direction.x, 0f, _direction.z);
        Quaternion rotation = Quaternion.LookRotation(look);
        _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, rotateLerp);
    }
}
