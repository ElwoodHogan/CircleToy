using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePoint : MonoBehaviour
{
    public Vector3 mousePosLastFrame;

    private void OnMouseDrag()
    {
        transform.position = transform.position + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePosLastFrame);
    }
    private void Update()
    {
        mousePosLastFrame = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
}
