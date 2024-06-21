using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteractionHandler : MonoBehaviour
{
    public LayerMask floorLayerMask;
    public GameObject wallsParent;

    public GameObject visualWallPrefab;
    public GameObject wallPrefab;

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
                //currentState = removingWallsState;
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

    public bool IsObjectAtPosition(Vector2 position)
    {
        return placedObjects.ContainsKey(position);
    }
}
