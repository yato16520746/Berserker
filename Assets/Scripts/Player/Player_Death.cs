using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : StateMachineBehaviour
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

        _delegate.State = PlayerState.Death;

        LevelLoader.Instance.LoadCurrentScene();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check state
        if (_delegate.State != PlayerState.Death)
        {
            return;
        }

        // get value
        float speedLerp = _delegate.SpeedLerp * Time.deltaTime;

        // velocity
        Vector3 velocity = new Vector3(0f, _rb.velocity.y, 0f);
        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, speedLerp);
    }
}
