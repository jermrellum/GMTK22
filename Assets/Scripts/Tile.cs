using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject contextPanel;
    [SerializeField] private GameObject menuPanel;

    private new Renderer renderer;
    private GameController gc;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        gc = GetComponentInParent<GameController>();
    }

    void Update()
    {
        
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
            if (!gc.contextMenuShowing)
            {
                contextPanel.SetActive(true);
                gc.contextMenuShowing = true;
                gc.hoveringOnButton = true;
                menuPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }

    }
}
