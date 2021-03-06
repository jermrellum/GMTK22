using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture;
    private GameController gc;
    private GridManager gm;
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
        gm = GetComponentInParent<GridManager>();
    }
    private void Update()
    {
        if (hoveringOnThis)
        {
            if (Input.GetMouseButtonDown(0))
            {
                int moneyAmt = 0;
                Vector3 v3 = Vector3.zero; //displacement vector
                string conName = "";

                switch (buildType)  //2 = building, 3 = vert wall, 4 = horiz wall
                {
                    case 2: 
                        moneyAmt = 100;
                        v3 = new Vector3(0.15f, 0.0f, -0.15f);
                        conName = "Building";
                        break;
                    case 3:
                        moneyAmt = 50;
                        v3 = new Vector3(-1.0f, 0.077f, 0.0f);
                        conName = "Wall";
                        break;
                    case 4: 
                        moneyAmt = 50;
                        v3 = new Vector3(0.0f, 0.077f, 0.0f);
                        conName = "Horiz wall";
                        break;
                }

                if (gc.money >= moneyAmt)
                {
                    var nCon = Instantiate(conFab, new Vector3(tile.transform.position.x + v3.x, v3.y, tile.transform.position.z + v3.z), 
                        conFab.transform.rotation, tile.transform.parent.transform);
                    Construct nConstr = nCon.GetComponent<Construct>();
                    nConstr.buildType = buildType;
                    nCon.name = conName + " on " + nCon.transform.parent.name;
                    gc.money -= moneyAmt;
                    gm.gridValues[tile.tileX, tile.tileY] = buildType;

                    switch(buildType)
                    {
                        case 3:
                            nConstr.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                            break;
                        case 4:
                            nConstr.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            break;
                    }
                }
                else
                {
                    gc.NoMoney();
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
