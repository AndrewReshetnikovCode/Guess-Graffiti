using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIRaycastUtility
{
    public static List<RaycastResult> GetUIElementsUnderCursor(Canvas canvas, Camera camera = null)
    {
        if (canvas == null)
        {
            Debug.LogWarning("UIRaycastUtility: Canvas is null.");
            return new List<RaycastResult>();
        }

        if (camera == null)
            camera = Camera.main;

        // Проверяем наличие Raycaster
        var raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            Debug.LogWarning($"UIRaycastUtility: Canvas '{canvas.name}' does not have a GraphicRaycaster.");
            return new List<RaycastResult>();
        }

        // Создаём EventData для позиции курсора
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Выполняем Raycast
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        return results;
    }
}
