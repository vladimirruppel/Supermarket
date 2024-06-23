using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethods : MonoBehaviour
{
    public static Vector3 GetMouseWorldPosition(LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public static Vector3 GetMouseGridPosition(LayerMask layerMask, float yPosition = 2) {
        Vector3 mousePos = GetMouseWorldPosition(layerMask);

        mousePos.x = (int)Math.Round(mousePos.x, MidpointRounding.AwayFromZero);
        mousePos.y = yPosition;
        mousePos.z = (int)Math.Round(mousePos.z, MidpointRounding.AwayFromZero);

        return mousePos;
    }
}
