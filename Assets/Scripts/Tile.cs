using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

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
}
