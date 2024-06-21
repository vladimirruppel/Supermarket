using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingWallsState : IUserInteractionState
{
    private GameObject visualWallObject;
    
    public void OnEnter(UserInteractionHandler handler)
    {
        visualWallObject = GameObject.Instantiate(handler.visualWallPrefab, new Vector3(0, -2, 0), Quaternion.Euler(-90, 0, 0));
    }

    public void OnUpdate(UserInteractionHandler handler)
    {
        Vector3 mousePos = StaticMethods.GetMouseWorldPosition(handler.floorLayerMask);

        mousePos.x = (int)Math.Round(mousePos.x, MidpointRounding.AwayFromZero);
        mousePos.y = 2;
        mousePos.z = (int)Math.Round(mousePos.z, MidpointRounding.AwayFromZero);
        visualWallObject.transform.position = mousePos;

        if (Input.GetMouseButton(0))
        {
            if (!handler.IsObjectAtPosition(new Vector2(mousePos.x, mousePos.z)))
            {
                handler.AddObject(handler.wallPrefab, mousePos, Quaternion.Euler(-90, 0, 0), handler.wallsParent.transform);
            }
        }
    }

    public void OnExit(UserInteractionHandler handler)
    {
        if (visualWallObject != null)
        {
            GameObject.Destroy(visualWallObject);
        }
    }

    private Vector3 GetMouseWorldPosition(UserInteractionHandler handler)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, handler.floorLayerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}