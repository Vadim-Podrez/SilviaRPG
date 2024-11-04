using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton enemySkeleton => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemySkeleton.AnimationFinishTrigger();
    }
}
