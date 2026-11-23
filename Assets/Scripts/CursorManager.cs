using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [Header("Cursor Settings")]
    public Texture2D defaultCursor;
    public Texture2D heldDownCursor;
    public Texture2D hoverGrabbableCursor;
    public Texture2D holdingUnitCursor;
    public Texture2D flipCardCursor;
    public Texture2D attackCursor;

    [Header("Cursor Positions")]
    public Vector2 defaultCursorHotspot = Vector2.zero;
    public Vector2 heldDownCursorHotspot = Vector2.zero;
    public Vector2 hoverGrabbableCursorHotspot = Vector2.zero;
    public Vector2 holdingUnitCursorHotspot = Vector2.zero;
    public Vector2 flipCardCursorHotspot = Vector2.zero;
    public Vector2 attackCursorHotspot = Vector2.zero;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetNormalCursor();
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(defaultCursor, defaultCursorHotspot, CursorMode.Auto);
    }

    public void SetHeldDownCursor()
    {
        Cursor.SetCursor(heldDownCursor, heldDownCursorHotspot, CursorMode.Auto);
    }

    public void SetHoverGrabbableCursor()
    {
        Cursor.SetCursor(hoverGrabbableCursor, hoverGrabbableCursorHotspot, CursorMode.Auto);
    }

    public void SetHoldingUnitCursor()
    {
        Cursor.SetCursor(holdingUnitCursor, holdingUnitCursorHotspot, CursorMode.Auto);
    }

    public void SetFlipCardCursor()
    {
        Cursor.SetCursor(flipCardCursor, flipCardCursorHotspot, CursorMode.Auto);
    }

    public void SetAttackCursor()
    {
        if (attackCursor == null)
        {
            SetNormalCursor();
            return;
        }

        Cursor.SetCursor(attackCursor, attackCursorHotspot, CursorMode.Auto);
    }
}
