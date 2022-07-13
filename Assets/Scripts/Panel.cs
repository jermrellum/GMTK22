using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private GameController gc;
    private Tile tile;

    void Start()
    {
        gc = GetComponentInParent<GameController>();
        tile = GetComponentInParent<Tile>();
    }

    void Update()
    {
        if (gc.contextMenuShowing && !gc.hoveringOnButton)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                tile.hidePanel();
            }
        }
    }
}
