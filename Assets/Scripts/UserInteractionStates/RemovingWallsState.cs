using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingWallsState : IUserInteractionState
{
    private GameObject previousWall; // для возвращения прошлой выбранной стене цвета
    private Color defaultWallColor;

    public void OnEnter(UserInteractionHandler handler)
    {
        defaultWallColor = GetMaterialColor(handler.wallPrefab);
    }

    public void OnUpdate(UserInteractionHandler handler)
    {
        if (previousWall != null)
            SetMaterialColor(previousWall, defaultWallColor);

        Vector3 mousePos = StaticMethods.GetMouseGridPosition(handler.floorLayerMask);
        Vector2 gridPosition = new Vector2(mousePos.x, mousePos.z);
        GameObject hoveredWall = handler.GetObjectByPosition(gridPosition);
        if (hoveredWall != null && hoveredWall.GetComponent<Wall>() != null) {
            // сделать темнее
            SetMaterialColor(hoveredWall, Color.red);
            previousWall = hoveredWall;

            if (Input.GetMouseButton(0)) {
                // удалить
                handler.RemoveObjectByPosition(gridPosition);
            }
        }
    }

    public void OnExit(UserInteractionHandler handler)
    {
        if (previousWall != null)
            SetMaterialColor(previousWall, defaultWallColor);
    }

    private Color GetMaterialColor(GameObject obj) {
        return obj.GetComponent<Renderer>().sharedMaterial.color;
    }

    private void SetMaterialColor(GameObject obj, Color color) {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
