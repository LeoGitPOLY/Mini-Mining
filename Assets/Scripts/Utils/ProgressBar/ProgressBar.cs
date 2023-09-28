using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{

    public double maximum;
    public double current;
    public double minimum;
    public Image mask;
    public Image fill;
    public Color couleur;

    // Update is called once per frame
    void Update()
    {
        getCurrentFill();
    }

    void getCurrentFill()
    {
        float currentOffSet = (float)(current - minimum);
        float maximumOffset = (float)(maximum - minimum);
        float fillAmount = currentOffSet / maximumOffset;

        mask.fillAmount = fillAmount;
        fill.color = couleur;
    }
}
