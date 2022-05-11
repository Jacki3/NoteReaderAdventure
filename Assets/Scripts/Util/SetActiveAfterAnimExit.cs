using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterAnimExit : StateMachineBehaviour
{
    public GameObject holder;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo animatorStateInfo,
        int layerIndex
    )
    {
        if (holder) holder.SetActive(false);
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        animator.gameObject.SetActive(false);
    }
}
