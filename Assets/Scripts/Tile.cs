using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject contextPanel;
    [SerializeField] private GameObject menuPanel;

    [HideInInspector] public int tileX;
    [HideInInspector] public int tileY;

    private new Renderer renderer;
    private GameController gc;
    private GridManager gm;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        gc = GetComponentInParent<GameController>();
        gm = GetComponentInParent<GridManager>();
    }

    private void OnMouseEnter()
    {
        if (!gc.contextMenuShowing)
        {
            renderer.material.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        if (!gc.contextMenuShowing)
        {
            renderer.material.color = Color.white;
        }
    }
    public void hidePanel()
    {
        contextPanel.SetActive(false);
        gc.contextMenuShowing = false;
        gc.hoveringOnButton = false;
        renderer.material.color = Color.white;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!gc.contextMenuShowing && gm.gridValues[tileX, tileY] == 0)
            {
                contextPanel.SetActive(true);
                gc.contextMenuShowing = true;
                gc.hoveringOnButton = true;
                menuPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }

    }
}
