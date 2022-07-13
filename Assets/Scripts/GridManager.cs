using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    [SerializeField] private GameObject tilePrefab;

    [HideInInspector] public int tileSize = 2;

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
                var nTileCont = Instantiate(tilePrefab, new Vector3(i*tileSize, 0, j*tileSize), Quaternion.identity);
                nTileCont.transform.parent = gameObject.transform;
                nTileCont.name = $"Tile {i} {j}";
                Tile nTile = nTileCont.GetComponentInChildren<Tile>();
                nTile.tileX = i;
                nTile.tileY = j;
            }
        }
    }

}
