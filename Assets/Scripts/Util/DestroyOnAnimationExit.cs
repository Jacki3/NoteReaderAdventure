using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimationExit : StateMachineBehaviour
{
    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo animatorStateInfo,
        int layerIndex
    )
    {
        Destroy(animator.gameObject, animatorStateInfo.length);
    }
}
