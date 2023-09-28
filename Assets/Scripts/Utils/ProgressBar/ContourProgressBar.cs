using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode()]
public class ContourProgressBar : MonoBehaviour
{
    public double maximum;
    public double current;
    public double minimum;  
    public Color couleur;
    public float alphaColor = 0.6f;

    protected Image fill;

    private void Start()
    {
        fill = gameObject.GetComponent<Image>();

        setGameObjectVisible(false);
    }
    // Update is called once per frame
    void Update()
    {
        getCurrentFill();
    }

    void getCurrentFill()
    {
        float currentOffSet = (float) (current - minimum);
        float maximumOffset = (float)(maximum - minimum);
        float fillAmount = currentOffSet / maximumOffset;

        fill.fillAmount = fillAmount;
        fill.color = couleur;
    }

    public void setGameObjectVisible(bool visible)
    {
        if (visible)
            couleur= new Color(fill.color.r, fill.color.g, fill.color.b, alphaColor);
        else
            couleur = new Color(fill.color.r, fill.color.g, fill.color.b, 0f);

    }
}
