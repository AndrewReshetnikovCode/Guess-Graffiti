using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverZoomCursor : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Cursor")]
    [SerializeField] private Texture2D hoverCursor;     // курсор при наведении (лупа)
    [SerializeField] private Texture2D activeCursor;    // курсор при увеличении
    [SerializeField] private Vector2 hotspot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [Header("Zoom")]
    [SerializeField] private float zoomScale = 1.5f;
    [SerializeField] private float zoomSpeed = 10f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private bool isHovered;
    private bool isZoomed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    private void Update()
    {
        Vector3 targetScale = isZoomed ? originalScale * zoomScale : originalScale;
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            targetScale,
            Time.unscaledDeltaTime * zoomSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        if (!isZoomed)
            SetCursor(hoverCursor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        ResetState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isZoomed = !isZoomed;

        if (isZoomed)
            SetCursor(activeCursor);
        else
            ResetState();
    }

    private void ResetState()
    {
        isZoomed = false;
        SetDefaultCursor();
    }

    private void SetCursor(Texture2D cursor)
    {
        if (cursor != null)
            Cursor.SetCursor(cursor, hotspot, cursorMode);
    }

    private void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
