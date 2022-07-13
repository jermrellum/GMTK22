using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [SerializeField] private int waterAmountStart = 50;
    [SerializeField] private float waterMaxScaleHeight = 1.0f;
    [SerializeField] private int framesPerWaterTick = 30;
    [HideInInspector] public int waterAmount;
    public int tileX = 0;
    public int tileY = 10;

    private int tickCounter = 0;

    private int gw;
    private int gh;

    private GridManager gm;

    private void Awake()
    {
        gm = GetComponentInParent<GridManager>();
        gw = gm.gridWidth;
        gh = gm.gridHeight;
        waterAmount = waterAmountStart;
    }

    private void Start()
    {
        setWaterHeight();
    }

    private void Update()
    {
        if(tickCounter == framesPerWaterTick)
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

        int[] xDispl = { -1, 0, 1, 0 };
        int[] yDispl = { -0, -1, 0, 1 };

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
                Destroy(this.gameObject);
            }
            else
            {
                setWaterHeight();
            }
        }
    }

    private void setWaterHeight()
    {
        float percentWater = (float)waterAmount / (float)waterAmountStart;
        transform.localScale = new Vector3(transform.localScale.x, percentWater * waterMaxScaleHeight, transform.localScale.z);
    }

    //checks if water can flow into next tile, returns amount of water that flowed out
    private int waterFlow(int tx, int ty)
    {
        GameObject newWater = Instantiate(this.gameObject, new Vector3(tx * gm.tileSize, 0.2f, ty * gm.tileSize), Quaternion.identity, gm.transform);
        newWater.transform.localScale = new Vector3(newWater.transform.localScale.x, newWater.transform.localScale.y, newWater.transform.localScale.z);

        int half = waterAmount - (waterAmount / 2);
        WaterSource nWS = newWater.GetComponent<WaterSource>();
        nWS.waterAmount = half;
        nWS.tileX = tx;
        nWS.tileY = ty;

        return half;
    }
}
