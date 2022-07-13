using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Texture2D cursorTexture;
    private GameController gc;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);

    private void Start()
    {
        gc = GetComponentInParent<GameController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gc.proceedToNight();
        this.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
