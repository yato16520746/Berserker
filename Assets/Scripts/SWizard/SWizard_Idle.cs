using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWizard_Idle : StateMachineBehaviour
{
    SWizard_Delegate _delegate;

    [SerializeField] float _minTimeIdle = 0.75f;
    [SerializeField] float _maxTimeIdle = 1.25f;
    float _count;
    Transform _parentTransform;
    Rigidbody _rb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SWizard_Delegate>();

            _parentTransform = _delegate.Parent.gameObject.transform;
            _rb = _delegate.Rb;
        }

        animator.SetBool("Idling", true);
        _count = Random.Range(_minTimeIdle, _maxTimeIdle);

        _delegate.State = SWizard_State.Idle;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_delegate.State != SWizard_State.Idle)
        {
            return;
        }

        // get values
        float rotateSpeed = _delegate.RotateSpeed * Time.deltaTime;

        // count to exit Idle State
        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            animator.SetBool("Idling", false);

            if (_delegate.Mode == SWizard_Mode.RunRandom)
            {
                animator.SetBool("Running Random", true);
            }
            else if (_delegate.Mode == SWizard_Mode.SummonFire)
            {
                animator.SetTrigger("Summon Fire");
            }
        }

        // look at player if you gonna summon
        Berserker player = Berserker.Instance;
        if (player && _delegate.Mode == SWizard_Mode.SummonFire)
        {
            Vector3 lookPlayer = player.transform.position - animator.transform.position;
            lookPlayer.y = 0f;
            if (lookPlayer.magnitude > 0.2f)
            {
                Quaternion rotation = Quaternion.LookRotation(lookPlayer);
                _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, rotateSpeed);
            }
        }
       
    }
}
