using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
   // [SerializeField] private GameObject canvas;
   // [SerializeField] private GameObject button;

    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        renderer.material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
         //   GameObject newCanvas = Instantiate(canvas) as GameObject;
        //    GameObject newButton = Instantiate(button) as GameObject;
         //   newButton.transform.SetParent(newCanvas.transform, false);
        //    newButton.transform.position = Input.mousePosition;
        }

    }
}
