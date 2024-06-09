using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 2f;
    public float panBorderThickness = 20f;
    public Vector2 panLimitLeftUpper;
    public Vector2 panLimitRightLower;

    public float scrollSpeed = 2f;
    public float minZoomHeight = 5f;
    public float maxZoomHeight = 20f;

    private Vector3 lastMousePosition;

    void Update()
    {
        HandleCameraMovement();
    }

    void HandleCameraMovement()
    {
        Vector3 pos = transform.position;

        // Camera movement by hovering the mouse over the screen borders
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime * pos.y;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime * pos.y;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime * pos.y;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime * pos.y;
        }

        // Camera movement by pressing and dragging the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            pos.x -= delta.x * panSpeed * Time.deltaTime * pos.y;
            pos.z -= delta.y * panSpeed * Time.deltaTime * pos.y;
            lastMousePosition = Input.mousePosition;
        }

        // Camera zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            pos.y -= scroll * scrollSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minZoomHeight, maxZoomHeight);
        }

        pos.x = Mathf.Clamp(pos.x, panLimitLeftUpper.x, panLimitRightLower.x);
        pos.z = Mathf.Clamp(pos.z, panLimitLeftUpper.y, panLimitRightLower.y);
        transform.position = pos;
    }
}
