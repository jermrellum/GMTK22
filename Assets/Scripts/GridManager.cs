using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private GameObject tilePrefab;

    private int tileSize = 2;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int i=0; i<gridWidth; i++)
        {
            for(int j=0; j<gridHeight; j++)
            {
                var nTile = Instantiate(tilePrefab, new Vector3(i*tileSize, 0, j*tileSize), Quaternion.identity);
                nTile.transform.parent = gameObject.transform;
                nTile.name = $"Tile {i} {j}";
            }
        }
    }

}
