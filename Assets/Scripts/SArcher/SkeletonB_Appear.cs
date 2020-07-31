using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonB_Appear : StateMachineBehaviour
{
    SArcher_Delegate _delegate;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<SArcher_Delegate>();
        }

        _delegate.State = SArcherState.Appear;
    }
}
