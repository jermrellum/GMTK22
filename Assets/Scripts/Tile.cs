using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject contextPanel;
    [SerializeField] private GameObject menuPanel;
    private Color rc;

    [HideInInspector] public int tileX;
    [HideInInspector] public int tileY;

    private new Renderer renderer;
    private GameController gc;
    private GridManager gm;

    private bool tileIsSelectable = false;

    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);

    void Start()
    {
        renderer = GetComponent<Renderer>();
        rc = renderer.material.color;

        gc = GetComponentInParent<GameController>();
        gm = GetComponentInParent<GridManager>();

        tileIsSelectable = tileX > 0 && tileX < (gm.gridWidth - 1) && tileY > 0 && tileY < (gm.gridHeight - 1);
    }

    public void SetTileTypeToZero()
    {
        SetTileTypeToZero(0);
    }

    public void SetTileTypeToZero(int gv)
    {
        gm.gridValues[tileX, tileY] = gv;
        gc.calculateSurvivors();
    }

    private void OnMouseEnter()
    {
        if (gc.IsInMenu() && tileIsSelectable && gc.isDay)
        {
            SelectTile();
        }
    }

    public void SelectTile()
    {
        renderer.material.color = Color.yellow;
        if(gm.gridValues[tileX, tileY] > 1)
        {
            Construct cons = transform.parent.gameObject.GetComponentInChildren<Construct>();
            if (cons != null && cons.hl != null)
            {
                cons.hl.SetActive(true);
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            }
        }
        
    }

    public void DeselectTile()
    {
        renderer.material.color = rc;
        if (gm.gridValues[tileX, tileY] > 1)
        {
            Construct cons = transform.parent.gameObject.GetComponentInChildren<Construct>();
            if (cons != null && cons.hl != null)
            {
                cons.hl.SetActive(false);
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
            }
        }
        
    }

    private void OnMouseExit()
    {
        if (gc.IsInMenu())
        {
            DeselectTile();
        }
    }
    public void hidePanel()
    {
        contextPanel.SetActive(false);
        gc.contextMenuShowing = false;
        gc.hoveringOnButton = false;
        gc.isTileContext = false;
        DeselectTile();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (gc.IsInMenu() && gc.isDay && gm.gridValues[tileX, tileY] == 0 && tileIsSelectable)
            {
                gc.CallHudClear();
                contextPanel.SetActive(true);
                gc.contextMenuShowing = true;
                gc.hoveringOnButton = true;
                gc.isTileContext = true;
                gc.onlyHpDisp = false;
                menuPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }

    }
}
