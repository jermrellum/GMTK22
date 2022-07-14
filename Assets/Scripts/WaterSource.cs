using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [SerializeField] private int waterAmountStart = 200;
    [SerializeField] private float waterMaxScaleHeight = 1.0f;
    public int directionOfFlow = 1; // 0 = north, 1 = east, 2 = south, 3 = west
    [HideInInspector] public int waterAmount;
    [HideInInspector] public int waterReserve;
    public int tileX = 0;
    public int tileY = 10;

    private int tickCounter = 0;

    private int gw;
    private int gh;

    private GridManager gm;
    private GameController gc;

    private void Awake()
    {
        gm = GetComponentInParent<GridManager>();
        gc = GetComponentInParent<GameController>();
        gw = gm.gridWidth;
        gh = gm.gridHeight;
        waterAmount = waterAmountStart;
        waterReserve = 0;
    }

    private void Start()
    {
        setWaterHeight();
    }

    private void FixedUpdate()
    {
        if(gc.isDay)
        {
            return;
        }

        if(tickCounter == gm.framesPerWaterTick)
        {
            waterTick();
            tickCounter = 0;
        }
        else
        {
            tickCounter++;
        }
    }

    private void waterTick()
    {
        //checking adjacent tiles

        waterAmount--;

        if(waterAmount == 0)
        {
            removeWater();
            return;
        }

        int[] xDispl = { -1, 0, 1, 0 };
        int[] yDispl = { -0, -1, 0, 1 };

        switch (directionOfFlow)
        {
            case 0:
                xDispl = new[] { 0, -1, 1, 0 };
                yDispl = new[] { 1, 0, 0, -1 };
                break;
            case 1:
                xDispl = new[] { 1, 0, 0, -1 };
                yDispl = new[] { 0, -1, 1, 0 };
                break;
            case 2:
                xDispl = new[] { 0, -1, 1, 0 };
                yDispl = new[] { -1, 0, 0, 1 };
                break;
            case 3:
                xDispl = new[] { -1, 0, 0, 1 };
                yDispl = new[] { 0, -1, 1, 0 }; 
                break;
        }

        for(int i=0; i<4; i++)
        {
            int checkX = tileX + xDispl[i];
            int checkY = tileY + yDispl[i];

            if(checkX >= 0 && checkX < gw && checkY >= 0 && checkY < gh)
            {
                int flowed = waterFlow(checkX, checkY);
                waterAmount -= flowed;
            }
            else
            {
                waterAmount /= 2;
            }

            if (waterAmount <= 0)
            {
                if(waterReserve > 0)
                {
                    break;
                }
                removeWater();
                break; //idt it can even reach here, but just in case
            }
            else
            {
                setWaterHeight();
            }
        }

        emptyReserve();
        
    }

    private void removeWater()
    {
        gm.gridValues[tileX, tileY] = 0;
        Destroy(this.gameObject);
    }

    private void emptyReserve()
    {
        waterAmount += waterReserve;
        waterReserve = 0;
    }

    private void setWaterHeight()
    {
        float percentWater = (float)waterAmount / (float)waterAmountStart;
        transform.localScale = new Vector3(transform.localScale.x, percentWater * waterMaxScaleHeight, transform.localScale.z);
    }

    //checks if water can flow into next tile, returns amount of water that flowed out
    private int waterFlow(int tx, int ty)
    {
        int gval = gm.gridValues[tx, ty];
        GameObject tilego = GameObject.Find("Tile " + tx + " " + ty);
        int half = waterAmount - (waterAmount / 2);
        if (gval == 0)
        {
            GameObject newWater = Instantiate(this.gameObject, new Vector3(tx * gm.tileSize, 0.2f, ty * gm.tileSize), Quaternion.identity, tilego.transform);
            newWater.transform.localScale = new Vector3(newWater.transform.localScale.x, newWater.transform.localScale.y, newWater.transform.localScale.z);

            
            WaterSource nWS = newWater.GetComponent<WaterSource>();
            nWS.waterAmount = half;
            nWS.tileX = tx;
            nWS.tileY = ty;
            gm.gridValues[tx, ty] = 1;

            return half;
        }
        else if(gval == 1) //flowing into other water
        {
            WaterSource tileWater = tilego.GetComponentInChildren<WaterSource>();
            tileWater.waterReserve += half;

            return half;
        }
        else //flowing into construct
        {
            Construct constr = tilego.GetComponentInChildren<Construct>();
            constr.currentHP -= half;
            if(constr.currentHP > 0)
            {
                constr.calcHealth();
                return 0;
            }
            else
            {
                constr.destroyThisConstruct();
                return half;
            }
        }
    }
}
