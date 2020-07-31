using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle_Aiming : StateMachineBehaviour
{
    Player_Delegate _delegate;

    Player _parent;
    Transform _parentTransform;
    Rigidbody _rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate == null)  
        {
            _delegate = animator.GetComponent<Player_Delegate>();

            _parent = _delegate.Parent;
            _parentTransform = _delegate.Parent.gameObject.transform;
            _rb = _delegate.Rb;
        }

        _delegate.State = PlayerState.Idle_Aiming;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check state
        if (_delegate.State != PlayerState.Idle_Aiming)
        {
            return;
        }

        // get values
        Vector3 mousePosition = _parent.MousePosition;
        Rigidbody rb = _delegate.Rb;
        float speedLerp = _delegate.SpeedLerp * Time.deltaTime;
        float rotateLerp = _delegate.RotateLerp * Time.deltaTime;
        Transform gunLook = _delegate.GunLook;

        // velocity
        Vector3 velocity = new Vector3(0f, _rb.velocity.y, 0f);
        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, speedLerp);

        // rotate - with Mouse
        Vector3 lookVector = mousePosition - _parentTransform.position;
        lookVector.y = 0f;

        if (lookVector.magnitude > _delegate.MouseRange)
        {
            Vector3 gunLookVector = mousePosition - gunLook.position;
            gunLookVector.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(gunLookVector);
            _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, rotateLerp);
        }
     
        //if (Target && !Target.IsDead)
        //{
        //    // xoay người về phía về mục tiêu
        //    Vector3 vector = Target.transform.position - rigPistolRight.transform.position;
        //    vector.y = 0f;
        //    if (vector.magnitude > 0.1f)
        //    {
        //        Quaternion rotation = Quaternion.LookRotation(vector);
        //        _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, rotateLerp);
        //    }
        //}
        //else
        //{
        //    // xoay người theo hướng di chuyển, khi mà vận tốc vẫn chưa đạt 0
        //    Vector3 vector = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        //    if (vector.magnitude > 0.1f)
        //    {
        //        Quaternion rotation = Quaternion.LookRotation(vector);
        //        _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, rotateLerp);
        //    }
        //}
    }
}
