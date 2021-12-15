﻿using System.Collections;
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
        holder.SetActive(false);
    }
}
