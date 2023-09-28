using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MondeManager : MonoBehaviour
{
    [SerializeField] private List<UnityEngine.Rendering.Universal.ShadowCaster2D> gameShadow;

    public bool shadowActived;
    private void Start()
    {
        if (!shadowActived)
        {
            foreach (UnityEngine.Rendering.Universal.ShadowCaster2D shadow in gameShadow)
            {
                shadow.castsShadows = shadowActived;
            }
        }
    }
}
