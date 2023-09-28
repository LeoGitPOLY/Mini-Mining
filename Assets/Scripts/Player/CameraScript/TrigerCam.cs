using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerCam : MonoBehaviour
{
    [SerializeField] private float size_cam;
    [SerializeField] private bool boolParralax;

    [Header("Position:")]
    [SerializeField] private Vector2 maxPos;
    [SerializeField] private Vector2 minPos;

    [Header("Times:")]
    [SerializeField] private float timeScaling;
    [SerializeField] private float timeMoving;

    [Header("GameObjectPoint")]
    [SerializeField] private Transform YMax;
    [SerializeField] private Transform YMin;
    [SerializeField] private Transform XMax;
    [SerializeField] private Transform XMin;

    private void Start()
    {
        if (YMax != null)
            maxPos.y = YMax.position.y;
        if (YMin != null)
            minPos.y = YMin.position.y;

        if (XMax != null)
            maxPos.x = XMax.position.x;
        if (XMin != null)
            minPos.x = XMin.position.x;

    }

    public Vector2 getPosMax()
    {
        return maxPos;
    }
    public Vector2 getPosMin()
    {
        return minPos;
    }
    public float getSizeCam()
    {
        return size_cam;
    }
    public float getTimeScale()
    {
        return timeScaling;
    }
    public float getTimeMoving()
    {
        return timeMoving;
    }
    public bool isParrallax()
    {
        return boolParralax;
    }


    public void setMax(Vector2 max)
    {
        maxPos = max;
    }
    public void setMin(Vector2 min)
    {
        minPos = min;
    }

}
