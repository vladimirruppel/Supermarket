using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject cubePrefab;
    public int startWidth = 30;
    public int startHeight = 40;

    public float panSpeed = 2f;
    public float panBorderThickness = 20f;
    public Vector2 panLimitLeftUpper;
    public Vector2 panLimitRightLower;

    public float scrollSpeed = 2f;
    public float minZoomHeight = 5f;
    public float maxZoomHeight = 20f;

    private Vector3 lastMousePosition;

    void Start()
    {
        int horizontalBorder = startWidth / 2;
        int verticalBorder = startHeight / 2;

        for (int i = -horizontalBorder; i < horizontalBorder; i++)
        {
            for (int j = -verticalBorder; j < verticalBorder; j++)
            {
                Instantiate(cubePrefab, new Vector3(i, 0, j), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Перемещение камеры при наведении мыши на границы экрана
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

        // Перемещение камеры при нажатии и перемещении колесика мыши
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            pos.x -= delta.x * panSpeed * Time.deltaTime;
            pos.z -= delta.y * panSpeed * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }

        // Масштабирование камеры колесиком мыши
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            pos.y -= scroll * scrollSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minZoomHeight, maxZoomHeight);
        }

        transform.position = pos;
    }
}
