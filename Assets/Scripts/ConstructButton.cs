using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture;
    private GameController gc;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);
    private bool hoveringOnThis = false;
    public int buttonFunc = 0;

    private void Start()
    {
        gc = GetComponentInParent<GameController>();
    }

    private void Update()
    {
        if (hoveringOnThis)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickedButton();
            }
        }
    }

    private void ClickedButton()
    {
        gc.ConstructButtonFunc(buttonFunc);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gc.hoveringOnButton = true;
        hoveringOnThis = true;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        gc.hoveringOnButton = false;
        hoveringOnThis = false;
    }
}
