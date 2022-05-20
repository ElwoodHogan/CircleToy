using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 mousePosLastFrame;
    Camera cam;
    [SerializeField] float speed;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float xMov = Input.GetAxis("Horizontal") * speed;
        float yMov = Input.GetAxis("Vertical") * speed;
        if (Input.GetMouseButton(1))
        {
            transform.position -= (cam.ScreenToWorldPoint(Input.mousePosition) - mousePosLastFrame);
        } else if (xMov != 0 || yMov != 0)
            transform.position += new Vector3(xMov, yMov);

        if (Input.mouseScrollDelta.y != 0)
        {
            cam.orthographicSize -= Input.mouseScrollDelta.y;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        mousePosLastFrame = cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
