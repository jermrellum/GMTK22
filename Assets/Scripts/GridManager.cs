using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    private GameController gc;

    private int floodDirection = 0;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private int initialHousesToPlace = 10;
    [SerializeField] private int houseTileBufferFromSides = 2;
    [SerializeField] private int waterTicksToFlood = 10;

    private int floodCounter = 0;
    private int waterTicker = 0;

    public int framesPerWaterTick = 60;
    [HideInInspector] public int tileSize = 2;
    [HideInInspector] public int[,] gridValues; // 0 = empty, 1 = water, 2 = building, 3 = vert wall, 4 = horiz wall, 5 = house

    private void FixedUpdate()
    {
        if(!gc.isDay)
        {
            if (!checkForWater())
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

            if (waterTicker == framesPerWaterTick)
            {
                Flood();
                waterTicker = 0;
            }
            else
            {
                waterTicker++;
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
        if (gc.difficulty > 0)
        {
            FloodStart();
        }
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

    private void InstantWater(int wx, int wy)
    {
        GameObject tilego = GameObject.Find("Tile " + wx + " " + wy);
        GameObject nWater = Instantiate(waterPrefab, new Vector3(wx * tileSize, 0.0f, wy * tileSize), waterPrefab.transform.rotation, tilego.transform);
        WaterSource nWS = nWater.GetComponent<WaterSource>();
        nWS.tileX = wx;
        nWS.tileY = wy;
        nWS.directionOfFlow = (floodDirection + 2) % 4;
        gridValues[wx, wy] = 1;
    }

    public void Flood()
    {
        if (floodCounter > 0)
        {
            floodCounter--;
            switch (floodDirection)  //0 = north, 1 = east, 2 = south, 3 = west
            {
                case 0:
                    for (int i = 0; i < gridWidth; i++)
                    {
                        InstantWater(i, gridHeight - 1);
                    }
                    break;
                case 1:
                    for (int i = 0; i < gridHeight; i++)
                    {
                        InstantWater(gridWidth - 1, i);
                    }
                    break;
                case 2:
                    for (int i = 0; i < gridWidth; i++)
                    {
                        InstantWater(i, 0);
                    }
                    break;
                case 3:
                    for (int i = 0; i < gridHeight; i++)
                    {
                        InstantWater(0, i);
                    }
                    break;
            }
        }
    }

    public void FloodStart()
    {
        waterTicker = 0;
        floodCounter = waterTicksToFlood;
        floodDirection = Random.Range(0, 4);
        Flood();
    }
}
