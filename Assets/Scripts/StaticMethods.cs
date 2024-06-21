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
}
