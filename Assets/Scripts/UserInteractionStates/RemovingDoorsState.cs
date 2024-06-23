using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingDoorsState : IUserInteractionState
{
    private GameObject previousDoor; // для возвращения прошлой выбранной двери цвета
    private Color defaultEnterDoorColor;
    private Color defaultWarehouseDoorColor;

    public void OnEnter(UserInteractionHandler handler)
    {
        defaultEnterDoorColor = GetMaterialColor(handler.enterDoorPrefab);
        defaultWarehouseDoorColor = GetMaterialColor(handler.warehouseDoorPrefab);
    }

    public void OnUpdate(UserInteractionHandler handler)
    {
        if (previousDoor != null)
            SetDefaultDoorMaterialColor(previousDoor);

        Vector3 mousePos = StaticMethods.GetMouseGridPosition(handler.floorLayerMask);
        Vector2 gridPosition = new Vector2(mousePos.x, mousePos.z);
        GameObject hoveredDoor = handler.GetObjectByPosition(gridPosition);
        if (hoveredDoor != null &&
            (hoveredDoor.GetComponent<EnterDoor>() != null || hoveredDoor.GetComponent<WarehouseDoor>() != null))
        {
            // сделать темнее
            SetMaterialColor(hoveredDoor, Color.red);
            previousDoor = hoveredDoor;

            if (Input.GetMouseButtonDown(0))
            {
                // удалить
                handler.RemoveObjectByPosition(gridPosition);
            }
        }
    }

    public void OnExit(UserInteractionHandler handler)
    {
        if (previousDoor != null)
            SetDefaultDoorMaterialColor(previousDoor);
    }

    private void SetDefaultDoorMaterialColor(GameObject doorObj) {
        if (doorObj.GetComponent<EnterDoor>() != null)
            SetMaterialColor(doorObj, defaultEnterDoorColor);
        else if (doorObj.GetComponent<WarehouseDoor>() != null)
            SetMaterialColor(doorObj, defaultWarehouseDoorColor);
        else
            Debug.LogError("doorObj variable doesn't have any of the doors' script");
    }

    private Color GetMaterialColor(GameObject obj)
    {
        return obj.GetComponent<Renderer>().sharedMaterial.color;
    }

    private void SetMaterialColor(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
