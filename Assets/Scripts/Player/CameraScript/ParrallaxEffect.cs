using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxEffect : MonoBehaviour
{
    [Header("Camera:")]
    [SerializeField] private CameraScript cam;

    [Header("modification:")]
    [Range(0, 1)] [SerializeField] private float parallaxEffect;

    private float lenght, startPos;

    private float length, startpos;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        //Subscibe to the Camera:
        cam.subcribeParralax(this);
    }

    public void moveParrallax(Transform camPosition)
    {
        float temp = (camPosition.position.x);
        float dist = (camPosition.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
