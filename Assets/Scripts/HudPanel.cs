using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudPanel : MonoBehaviour
{
    private GameController gc;
    private Construct cons;

    void Start()
    {
        gc = GetComponentInParent<GameController>();
    }

    public void SetCons(Construct inCon)
    {
        cons = inCon;
    }

    public void ConstructButtonFunc(int buttonFunc)
    {
        switch(buttonFunc)
        {
            case 0: cons.DestroyThisConstruct(); break;
            case 1: cons.RepairConstruct(); break;
            case 2: cons.RebuildConstruct(); break;
        }
        ClearHudOfConstructContexts();
    }

    public void ClearHudOfConstructContexts()
    {
        if (cons != null)
        {
            cons.DeactivateHighlight();
        }
        if (!gc.isTileContext)
        {
            gc.contextMenuShowing = false;
        }
        gc.hoveringOnButton = false;
        gc.onlyHpDisp = false;
        int chiCount = transform.childCount;
        for (int i = 0; i < chiCount; i++)
        {
            Transform ctrans = transform.GetChild(i);
            if (ctrans.gameObject.CompareTag("Construct Context"))
            {
                Destroy(ctrans.gameObject);
            }
        }
    }

    void Update()
    {
        if (gc.firstFrameConstructContext)
        {
            gc.firstFrameConstructContext = false;
        }
        else if (gc.contextMenuShowing && !gc.hoveringOnButton)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                ClearHudOfConstructContexts();
            }
        }
    }
}
