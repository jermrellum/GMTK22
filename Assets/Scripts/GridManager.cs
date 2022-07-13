using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    private GameController gc;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int initialHousesToPlace = 10;

    [HideInInspector] public int tileSize = 2;
    [HideInInspector] public int[,] gridValues; // 0 = empty, 1 = water, 2 = building, 3 = vert wall, 4 = horiz wall

    private void FixedUpdate()
    {
        if(!gc.isDay && !checkForWater())
        {
            if (gc.ticksBRCounter <= 0)
            {
                gc.proceedToDay();
            }
            else
            {
                gc.ticksBRCounter--;
            }
        }
    }

    private bool checkForWater()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if(gridValues[i, j] == 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void Start()
    {
        gc = GetComponentInParent<GameController>();
        gridValues = new int[gridWidth, gridHeight];
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
                gridValues[i, j] = 0;
            }
        }
    }

}
