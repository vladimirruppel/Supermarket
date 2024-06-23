using System.Collections.Generic;
using UnityEngine;

public class AddingDoorsState : IUserInteractionState
{
    private GameObject visualDoorPrefab;
    private GameObject doorPrefab;
    private Transform doorsParent;

    private GameObject visualDoorObject;
    private bool isBeingPlacedVertically = false;

    public AddingDoorsState(GameObject visualDoorPrefab, GameObject doorPrefab, Transform doorsParent) {
        this.visualDoorPrefab = visualDoorPrefab;
        this.doorPrefab = doorPrefab;
        this.doorsParent = doorsParent;
    }

    public void OnEnter(UserInteractionHandler handler)
    {
        visualDoorObject = GameObject.Instantiate(visualDoorPrefab, new Vector3(0, -2, 0), Quaternion.Euler(-90, 0, 0));
    }

    public void OnUpdate(UserInteractionHandler handler)
    {
        Vector3 mouseGridPos = StaticMethods.GetMouseGridPosition(handler.floorLayerMask, 2);
        
        float mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollValue != 0) {
            isBeingPlacedVertically = !isBeingPlacedVertically;
        }

        Vector3 doorGridPos;
        Quaternion doorQuaternion;
        Vector2 leftPartGridPos, rightPartGridPos;
        if (isBeingPlacedVertically) {
            doorGridPos = new Vector3(mouseGridPos.x, mouseGridPos.y, mouseGridPos.z + 0.5f);
            doorQuaternion = Quaternion.Euler(-90, 0, 90);
            leftPartGridPos = new Vector3(doorGridPos.x, doorGridPos.z - 0.5f);
            rightPartGridPos = new Vector3(doorGridPos.x, doorGridPos.z + 0.5f);
        }
        else {
            doorGridPos = new Vector3(mouseGridPos.x + 0.5f, mouseGridPos.y, mouseGridPos.z);
            doorQuaternion = Quaternion.Euler(-90, 0, 0);
            leftPartGridPos = new Vector3(doorGridPos.x - 0.5f, doorGridPos.z);
            rightPartGridPos = new Vector3(doorGridPos.x + 0.5f, doorGridPos.z);
        }
        visualDoorObject.transform.position = doorGridPos;
        visualDoorObject.transform.rotation = doorQuaternion;

        List<Vector2> takenPositions = new List<Vector2>() { leftPartGridPos, rightPartGridPos };
        bool canBePlaced = !handler.IsObjectAtAnyPositionFromList(takenPositions);

        if (Input.GetMouseButtonDown(0) && canBePlaced) {

            if (!handler.IsObjectAtAnyPositionFromList(takenPositions)) {
                handler.AddObjectWithMultiplePositions(takenPositions, doorPrefab, doorGridPos, doorQuaternion, doorsParent);
            }
        }
    }

    public void OnExit(UserInteractionHandler handler)
    {
        if (visualDoorObject != null)
        {
            GameObject.Destroy(visualDoorObject);
        }
    }
}
