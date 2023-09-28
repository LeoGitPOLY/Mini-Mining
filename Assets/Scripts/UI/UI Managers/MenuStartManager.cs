using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartManager : MonoBehaviour
{
    private Animator animator;

    private const string OPEN_ANIMATION = "MenuStartOpen"; 

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PLAY()
    {
        if(animator != null)
        {
            animator.Play(OPEN_ANIMATION);

            float timeAnim = AnimationManager.getTimeStateByName(animator, OPEN_ANIMATION);
            Invoke("PlayAfterAnimation", timeAnim);
        }
    }

    private void PlayAfterAnimation()
    {
        UIManager.instance.setVisibleStart();
    }
}
