using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopTweenEffect : MonoBehaviour
{
    public void fadeOut(float duration, float delay)
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0.5f, duration).setDelay(delay).setOnComplete(destroyMe);
        LeanTween.scale(gameObject,new Vector3(0,0,0), duration).setDelay(delay).setOnComplete(destroyMe);
    }

    public void popIn(float duration, float delay, Vector3 scale)
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 1, duration).setDelay(delay);
        LeanTween.scale(gameObject, scale, duration).setDelay(delay);
    }


   private void destroyMe()
    {
        Destroy(gameObject);
    }

}
