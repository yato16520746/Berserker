using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWizard_Death : StateMachineBehaviour
{
    SWizard_Delegate _delegate;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SWizard_Delegate>();
        }

        _delegate.State = SWizard_State.Death;
    }
}
