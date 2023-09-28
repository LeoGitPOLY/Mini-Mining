using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class AnimationManager
{
    private static List<string> animatorsName;
    private static List<string> currentsState;

    private static int index;
    private static string currentState;
    public static void ChangeAnimationState(Animator animator, string newState)
    {
        animatorIsHere(animator, newState);
        animator.enabled = true;
       
        if (animator != null)
        { 
            //stop the same animation from interrupting itself
            if (currentState == newState) return;

            //play the animation
            animator.Play(newState);

            //reassign the current state
            currentsState[index] = newState;
        }
    }
    public static float getTimeStateByName(Animator anim, string name)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        float timer = 0;
        foreach (AnimationClip item in clips)
        {
            if (item.name == name)
            {
                timer = item.length;
            }
        }
        return timer;
    }


    private static void animatorIsHere(Animator animator, string newState)
    {
        if (animatorsName == null)
            animatorsName = new List<string>();
        if (currentsState == null)
            currentsState = new List<string>();

        if (!animatorsName.Contains(animator.name))
        {
            animatorsName.Add(animator.name);
            currentsState.Add(newState);
        }
        else
        {
           index = animatorsName.IndexOf(animator.name);
           currentState = currentsState[index];
        }

    }
}
