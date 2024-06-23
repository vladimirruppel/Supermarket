using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInteractionHandler : MonoBehaviour
{
    public LayerMask floorLayerMask;
    public LayerMask interriorLayerMask;

    public GameObject wallsParent;
    public GameObject enterDoorsParent;
    public GameObject warehouseDoorsParent;

    public GameObject visualWallPrefab;
    public GameObject wallPrefab;
    public GameObject visualEnterDoorPrefab;
    public GameObject enterDoorPrefab;
    public GameObject visualWarehouseDoorPrefab;
    public GameObject warehouseDoorPrefab;

    private IUserInteractionState currentState;
    private Dictionary<Vector2, GameObject> placedObjects = new Dictionary<Vector2, GameObject>();

    void Update()
    {
        currentState?.OnUpdate(this);
    }

    public void SetState(InteractionState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        switch (newState)
        {
            case InteractionState.AddingWalls:
                currentState = new AddingWallsState();
                break;
            case InteractionState.RemovingWalls:
                currentState = new RemovingWallsState();
                break;
            case InteractionState.AddingEnterDoors:
                currentState = new AddingDoorsState(visualEnterDoorPrefab, enterDoorPrefab, enterDoorsParent.transform);
                break;
            case InteractionState.AddingWarehouseDoors:
                currentState = new AddingDoorsState(visualWarehouseDoorPrefab, warehouseDoorPrefab, warehouseDoorsParent.transform);
                break;
            case InteractionState.RemovingDoors:
                currentState = new RemovingDoorsState();
                break;
            default:
                currentState = null;
                break;
        }

        currentState?.OnEnter(this);
    }

    public bool AddObject(Vector2 position, GameObject obj)
    {
        if (!placedObjects.ContainsKey(position))
        {
            placedObjects.Add(position, obj);
            return true;
        }
        return false;
    }

    public GameObject AddObject(GameObject prefab, Vector3 objectPosition, Quaternion quaternion, Transform parent = null)
    {
        Vector2 dictionaryPosition = new Vector2(objectPosition.x, objectPosition.z);

        if (!placedObjects.ContainsKey(dictionaryPosition))
        {
            GameObject obj = Instantiate(prefab, objectPosition, quaternion, parent);
            placedObjects.Add(dictionaryPosition, obj);
            return obj;
        }
        return null;
    }

    public GameObject AddObjectWithMultiplePositions(List<Vector2> positions, GameObject prefab, Vector3 objectPosition, Quaternion quaternion, Transform parent = null)
    {
        if (IsObjectAtAnyPositionFromList(positions))
            return null;

        GameObject obj = Instantiate(prefab, objectPosition, quaternion, parent);

        foreach (Vector2 pos in positions) {
            placedObjects.Add(pos, obj);
        }

        return obj;
    }

    public bool IsObjectAtPosition(Vector2 position)
    {
        return placedObjects.ContainsKey(position);
    }

    public bool IsObjectAtAnyPositionFromList(List<Vector2> positions) {
        foreach (Vector2 pos in positions) {
            if (placedObjects.ContainsKey(pos))
                return true;
        }
        return false;
    }

    public GameObject GetObjectByPosition(Vector2 pos) {
        return IsObjectAtPosition(pos) ? placedObjects[pos] : null;
    }

    public void RemoveObjectByPosition(Vector2 pos) {
        GameObject obj = placedObjects[pos];

        foreach (var item in placedObjects.Where(el => el.Value == obj).ToList())
        {
            placedObjects.Remove(item.Key);
        }

        Destroy(obj);
    }
}
