using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonderiesManager : MonoBehaviour
{
    [Header("Chose a déplacer:")]
    [SerializeField] private GameObject backGroundSprite;
    [SerializeField] private Transform bunker;
    [SerializeField] private Transform triggercamBunker;

    private GameObject left;
    private GameObject right;
    private GameObject bottom;

    private float size = 0.04f;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void resetTransformInfo(int totalY)
    {
        SpriteRenderer sr = backGroundSprite.GetComponent<SpriteRenderer>();
        TrigerCam tr = triggercamBunker.GetComponent<TrigerCam>();

        this.transform.localScale = new Vector3(50, 2 * totalY, 0);
        
        sr.size = new Vector2(50, totalY);
        backGroundSprite.transform.position = new Vector2(0, -((totalY/2) + 1));
        
        bunker.position = new Vector3(0, -totalY + 1, 0);

        triggercamBunker.position = new Vector3(0, -totalY + 1, 0);
        tr.setMin(new Vector2(tr.getPosMin().x, -(totalY + 11)));
    }
    public void bonderiesSetta()
    {
        SpriteRenderer boxLeft = left.GetComponent<SpriteRenderer>();
        SpriteRenderer boxRight = right.GetComponent<SpriteRenderer>();
        SpriteRenderer boxBottom = bottom.GetComponent<SpriteRenderer>();
        float rap = transform.localScale.x / transform.localScale.y;

        boxLeft.enabled = true;
        boxRight.enabled = true;
        boxBottom.enabled = true;

        boxLeft.size = new Vector2(1, 2 + transform.localScale.y / (transform.localScale.x * size));
        boxRight.size = new Vector2(1, 2 + transform.localScale.y / (transform.localScale.x * size));
        boxBottom.size = new Vector2(transform.localScale.x / (transform.localScale.x * size), 1);




        left.transform.localScale = new Vector2(size, size * rap);
        right.transform.localScale = new Vector2(size, size * rap);
        bottom.transform.localScale = new Vector2(size, size * rap);

        left.transform.localPosition = new Vector2(-(0.5f + size / 2), 0);
        right.transform.localPosition = new Vector2(0.5f + size / 2, 0);
        bottom.transform.localPosition = new Vector2(0, -(0.5f + size * rap / 2));
    }
}
