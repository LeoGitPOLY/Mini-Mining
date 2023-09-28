using System;
using UnityEngine;

public class ArrowDrawer : MonoBehaviour
{
    [SerializeField] public Transform pointDep;
    [SerializeField] public Transform pointArr;

    [SerializeField] public float offSetDistance;
    [SerializeField] public bool isUpdated = false;

    //private field
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        generateArrow2Points();
    }

    private void Update()
    {
        if (isUpdated)
            generateArrow2Points();
    }

    private void generateArrow2Points()
    {
        Vector2 direction = pointArr.position - pointDep.position;

        float distance = (float)Math.Sqrt(Math.Pow((pointDep.localPosition.x - pointArr.localPosition.x), 2) + Math.Pow((pointDep.localPosition.y - pointArr.localPosition.y), 2));
        float angle = Vector2.Angle(direction, Vector2.right);
        angle = pointDep.position.y > pointArr.position.y ? -angle : angle;

        rectTransform.position = new Vector2(pointDep.position.x, pointDep.position.y);
        rectTransform.sizeDelta = new Vector2(distance - offSetDistance, rectTransform.sizeDelta.y);

        rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        rectTransform.Rotate(Vector3.forward, angle);

        rectTransform.localScale = pointDep.position.x > pointArr.position.x ? new Vector2(1, -1) : new Vector2(1, 1);
    }
}

