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
                int moneyAmt = 0;
                Vector3 v3 = Vector3.zero; //displacement vector

                switch (buildType)  //0 = building, 1 = vert wall, 2 = horiz wall
                {
                    case 0: 
                        moneyAmt = 100;
                        v3 = new Vector3(0.25f, 0.0f, -0.15f);
                        break;
                    case 1:
                        moneyAmt = 50;
                        v3 = new Vector3(0.0f, 0.4f, 0.0f);
                        break;
                    case 2: 
                        moneyAmt = 50;
                        v3 = new Vector3(0.0f, 0.4f, 0.0f);
                        break;
                }

                if (gc.money >= moneyAmt)
                {
                    var nCon = Instantiate(conFab, new Vector3(tile.transform.position.x + v3.x, v3.y, tile.transform.position.z + v3.z), 
                        conFab.transform.rotation, tile.transform.parent.transform);
                    nCon.transform.localScale = conFab.transform.localScale;
                    nCon.name = "Construct on " + nCon.transform.parent.name;
                    gc.money -= moneyAmt;

                    switch(buildType)
                    {
                        case 1:
                            nCon.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 90.0f);
                            break;
                        case 2:
                            nCon.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
                            break;
                    }
                }
                else
                {
                    Debug.Log("No money");
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
