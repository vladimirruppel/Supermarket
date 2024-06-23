using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIToolsMenuScript : MonoBehaviour
{
    private UIDocument document;

    private List<Button> actionButtons = new List<Button>();

    private const string addWallsButtonName = "AddWallsButton";
    private const string removeWallsButtonName = "RemoveWallsButton";
    private const string addEnterDoorsButtonName = "AddEnterDoorsButton";
    private const string addWarehouseDoorsButtonName = "AddWarehouseDoorsButton";
    private const string removeDoorsButtonName = "RemoveDoorsButton";
    private const string changeBordersConfirmButtonName = "ChangeBordersConfirmButton";
    private const string changeBordersCancelButtonName = "ChangeBordersCancelButton";
    private const string addCashDesksButtonName = "AddCashDesksButton";
    private const string removeCashDesksButtonName = "RemoveCashDesksButton";
    private const string getInformationButtonName = "GetInformationButton";
    
    void Awake()
    {
        document = GetComponent<UIDocument>();

        actionButtons = document.rootVisualElement.Query<Button>(className: "button").ToList();
        int actionButtonsLength = actionButtons.Count;
        for (int i = 0; i < actionButtonsLength; i++) {
            actionButtons[i].RegisterCallback<ClickEvent, Button>(ActionButtonClickHandler, actionButtons[i]);
        }
    }

    void OnDisable()
    {
        int actionButtonsLength = actionButtons.Count;
        for (int i = 0; i < actionButtonsLength; i++)
        {
            actionButtons[i].UnregisterCallback<ClickEvent, Button>(ActionButtonClickHandler);
        }
    }

    private void ActionButtonClickHandler(ClickEvent evt, Button clickedButton) {
        if (clickedButton.ClassListContains("menu-button")) {
            VisualElement menuParent = clickedButton.parent;

            if (menuParent.ClassListContains("active"))
            {
                menuParent.RemoveFromClassList("active");

                List<VisualElement> activeChildMenus = menuParent.Query(className: "menu").Where(menu => menu.ClassListContains("active")).ToList();
                int activeChildMenusCount = activeChildMenus.Count;
                for (int i = 0; i < activeChildMenusCount; i++) {
                    VisualElement currentActiveChildMenu = activeChildMenus[i];
                    currentActiveChildMenu.RemoveFromClassList("active");

                    // если это не корневой элемент-меню
                    if (currentActiveChildMenu.Query(className: "menu").Where(menu => menu.ClassListContains("active")).ToList().Count > 0) {
                        break;
                    }

                    // действия если это корневой элемент-меню
                    // возможно тут должна быть проверка на имя активной кнопки
                    // и в зависимости от имени кнопки отменяется ее действие
                }
            }
            else
            {
                List<VisualElement> activeMenus = document.rootVisualElement.Query(className: "menu").Where(menu => menu.ClassListContains("active")).ToList();
                int activeMenusCount = activeMenus.Count;
                for (int i = 0; i < activeMenusCount; i++) {
                    if (activeMenus[i].name != menuParent.parent.parent.name)
                        activeMenus[i].RemoveFromClassList("active");
                }

                menuParent.AddToClassList("active");
            }
        }

        UserInteractionHandler userInteractionHandler = GameObject.Find("Main Camera").GetComponent<UserInteractionHandler>();

        switch (clickedButton.name) {
            case addWallsButtonName:
                userInteractionHandler.SetState(InteractionState.AddingWalls);
                break;
            case removeWallsButtonName:
                userInteractionHandler.SetState(InteractionState.RemovingWalls);
                break;
            case addEnterDoorsButtonName:
                userInteractionHandler.SetState(InteractionState.AddingEnterDoors);
                break;
            case addWarehouseDoorsButtonName:
                userInteractionHandler.SetState(InteractionState.AddingWarehouseDoors);
                break;
            case removeDoorsButtonName:
                userInteractionHandler.SetState(InteractionState.RemovingDoors);
                break;
            // в зависимости от имени кнопки запустить определенное состояние в скрипте UserInteractionHandler
            default:
                break;
        }
    }
}