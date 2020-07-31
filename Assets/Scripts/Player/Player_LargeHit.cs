using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LargeHit : StateMachineBehaviour
{
    Player_Delegate _delegate;

    Player _parent;
    Transform _parentTransform;
    Rigidbody _rb;

    Vector3 _forceDirection;
    [SerializeField] float _speedLerpToZero;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Player_Delegate>();

            _parent = _delegate.Parent;
            _parentTransform = _parent.gameObject.transform;
            _rb = _delegate.Rb;
        }

        float hitForce = _delegate.HitForce;
        _forceDirection = _delegate.HitForceDirection;

        _rb.velocity = new Vector3(_forceDirection.x * hitForce, _rb.velocity.y, _forceDirection.z * hitForce);

        _delegate.State = PlayerState.LargeHit;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != PlayerState.LargeHit)
        {
            return;
        }

        // get values
        float speedLerp = _delegate.SpeedLerp * Time.deltaTime;
        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;

        // velocity
        Vector3 velocity = new Vector3(0f, _rb.velocity.y, 0f);
        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity,speedLerp);

        // rotate
        Vector3 vector = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        if (vector.magnitude > 0.3f)
        {
            Quaternion rotation = Quaternion.LookRotation(-_forceDirection);
            _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, rotateLerp);
        }
    }
}
