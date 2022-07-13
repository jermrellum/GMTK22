using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture;
    private GameController gc;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);
    private bool hoveringOnThis = false;
    private Tile tile;

    [SerializeField] GameObject conFab;
    [SerializeField] int buildType;

    private void Start()
    {
        gc = GetComponentInParent<GameController>();
        tile = GetComponentInParent<Tile>();
    }
    private void Update()
    {
        if (hoveringOnThis)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var nCon = Instantiate(conFab, new Vector3(tile.transform.position.x + 0.25f, 0, tile.transform.position.z - 0.15f), conFab.transform.rotation, tile.transform.parent.transform);
                nCon.transform.localScale = conFab.transform.localScale;
                nCon.name = "Construct on " + nCon.transform.parent.name;

                switch(buildType)
                {
                    case 0: gc.money -= 100; break;
                    case 1: gc.money -= 50; break;
                }

                tile.hidePanel();
            }
        }
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
