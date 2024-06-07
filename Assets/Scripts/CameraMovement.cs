using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject floorElementPrefab;

    public float panSpeed = 2f;
    public float panBorderThickness = 20f;
    public Vector2 panLimitLeftUpper;
    public Vector2 panLimitRightLower;

    public float scrollSpeed = 2f;
    public float minZoomHeight = 5f;
    public float maxZoomHeight = 20f;

    public float borderCheckInterval = 1f;

    private Vector3 lastMousePosition;
    private HashSet<GameObject> floorElements;
    private Camera camera;

    void Start()
    {
        floorElements = new HashSet<GameObject>();
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        HashSet<GameObject> floorElementsOnScreen = new HashSet<GameObject>();

        for (float x = 0; x <= Screen.width; x += borderCheckInterval)
        {
            for (float y = 0; y <= Screen.height; y += borderCheckInterval)
            {
                Vector3 screenPosition = new Vector3(x, y, 0);
                Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
                worldPosition.y = 0;

                GameObject floorElement = IsFloorElementAlreadyInstantiated(worldPosition);
                if (floorElement == null)
                {
                    Vector3 instantiatingPos = worldPosition;

                    instantiatingPos.x = (int)Math.Round(worldPosition.x, MidpointRounding.AwayFromZero);
                    instantiatingPos.z = (int)Math.Round(worldPosition.z, MidpointRounding.AwayFromZero);

                    floorElement = InstantiateFloorElement(instantiatingPos);
                }

                floorElementsOnScreen.Add(floorElement);
            }
        }

        HashSet<GameObject> objectToDelete = new HashSet<GameObject>();

        foreach (GameObject obj in floorElements)
        {
            if (obj.GetComponent<FloorElement>().deleteIfNotVisible && !floorElementsOnScreen.Contains(obj))
            {
                objectToDelete.Add(obj);
            }
        }
        
        foreach (GameObject obj in objectToDelete)
        {
            floorElements.Remove(obj);
            Destroy(obj);
        }

        objectToDelete.Clear();
    }

    void LateUpdate()
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
            pos.x -= delta.x * panSpeed * Time.deltaTime * pos.y;
            pos.z -= delta.y * panSpeed * Time.deltaTime * pos.y;
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

    private GameObject InstantiateFloorElement(Vector3 pos)
    {
        GameObject instantiatedObject = Instantiate(floorElementPrefab, pos, Quaternion.identity);
        floorElements.Add(instantiatedObject);
        return instantiatedObject;
    }

    private GameObject IsFloorElementAlreadyInstantiated(Vector3 position)
    {
        foreach (GameObject floorElement in floorElements)
        {
            Vector3 floorPosition = floorElement.transform.position;
            if (IsPositionInsideFloorElement(floorPosition, position))
            {
                return floorElement;
            }
        }
        return null;
    }

    // Helper method to check if a position is inside a cube at cubePosition
    private bool IsPositionInsideFloorElement(Vector3 objectPosition, Vector3 position)
    {
        float cubeHalfSize = 0.5f; // Since the cube dimensions are 1x1x1, half-size is 0.5
        return position.x >= objectPosition.x - cubeHalfSize && position.x <= objectPosition.x + cubeHalfSize &&
               position.y >= objectPosition.y - cubeHalfSize && position.y <= objectPosition.y + cubeHalfSize &&
               position.z >= objectPosition.z - cubeHalfSize && position.z <= objectPosition.z + cubeHalfSize;
    }
}
