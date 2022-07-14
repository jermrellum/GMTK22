using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    private GameController gc;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private int initialHousesToPlace = 10;
    [SerializeField] private int houseTileBufferFromSides = 1;

    [HideInInspector] public int tileSize = 2;
    [HideInInspector] public int[,] gridValues; // 0 = empty, 1 = water, 2 = building, 3 = vert wall, 4 = horiz wall, 5 = house

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
    public int getStartingHouses()
    {
        return initialHousesToPlace;
    }

    private bool checkForWater()
    {
        return (countNumberOfBuildType(1) > 0);
    }

    public int countNumberOfBuildType(int buildType)
    {
        int oCount = 0;
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (gridValues[i, j] == buildType)
                {
                    oCount++;
                }
            }
        }

        return oCount;
    }

    void Start()
    {
        gc = GetComponentInParent<GameController>();
        gridValues = new int[gridWidth, gridHeight];
        GenerateGrid();
        PlaceHouses();
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

    private void InstantHouse(int hx, int hy)
    {
        GameObject tilego = GameObject.Find("Tile " + hx + " " + hy);
        Instantiate(housePrefab, new Vector3(hx * tileSize, 0.0f, hy * tileSize), housePrefab.transform.rotation, tilego.transform);
        gridValues[hx, hy] = 5;
    }

    private void PlaceHouses()
    {
        for (int i = 0; i < initialHousesToPlace; i++)
        {
            int rx = Random.Range(0 + houseTileBufferFromSides, gridWidth - houseTileBufferFromSides);
            int ry = Random.Range(0 + houseTileBufferFromSides, gridHeight - houseTileBufferFromSides);

            if(gridValues[rx, ry] == 0)
            {
                InstantHouse(rx, ry);
            }
            else
            {
                i--;
            }
        }
    }

}
