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

    private void HandleCameraMovement()
    {
        Vector3 pos = transform.position;

        // Camera movement by pressing arrows or WASD keys
        pos.z += Input.GetAxis("Vertical") * panSpeed * Time.deltaTime * pos.y;
        pos.x += Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime * pos.y;

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
