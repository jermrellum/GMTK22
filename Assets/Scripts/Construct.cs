using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Construct : MonoBehaviour
{
    [SerializeField] int maxHP;
    private int useMaxHP;
    public int currentHP;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject healthEmptyPrefab;
    private bool spawnedHealth = false;
    [SerializeField] private float maxBarSize = 0.55f;
    [SerializeField] private float healthXDisplacement = 0.0f;
    [SerializeField] private float healthYDisplacement = 0.5f;
    [SerializeField] private float healthZDisplacement = -0.5f;
    private GameObject healthbar;
    private GameObject healthbarE;
    public GameObject conFab;
    public int buildType;

    [SerializeField] private GameObject conHealthFabText;
    [SerializeField] private GameObject constructButtonFab;

    [HideInInspector] public GameObject hl;

    [SerializeField] private float turnYellow = 0.5f;
    [SerializeField] private float turnRed = 0.2f;

    private GameController gc;
    private GameObject hud;
    private HudPanel hudPan;

    //[SerializeField] private Material greenMat;
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material redMat;

    Tile tile;

    private void Start()
    {
        hud = GameObject.Find("Game HUD");
        hudPan = hud.GetComponent<HudPanel>();
        hl = transform.Find("Highlighter").gameObject;
        tile = transform.parent.GetComponentInChildren<Tile>();
        gc = GetComponentInParent<GameController>();

        useMaxHP = maxHP;
        if (gc.difficulty == 2)
        {
            useMaxHP = maxHP * 2;
        }

        currentHP = useMaxHP;
    }

    private void OnMouseEnter()
    {
        if (gc.IsInMenu() && gc.isDay)
        {
            hl.SetActive(true);
            tile.SelectTile();
        }
        
    }

    private void OnMouseExit()
    {
        DeactivateHighlight();
    }

    public void DeactivateHighlight()
    {
        hl.SetActive(false);
        tile.DeselectTile();
    }

    private void OnMouseOver()
    {
        if (gc.isDay && Input.GetMouseButtonDown(1))
        {
            hudPan.ClearHudOfConstructContexts();

            gc.contextMenuShowing = true;
            gc.firstFrameConstructContext = true;
            gc.onlyHpDisp = true;

            hudPan.SetCons(this);

            GameObject conHealthGo = Instantiate(conHealthFabText, hud.transform, false);
            Text chText = conHealthGo.GetComponentInChildren<Text>();
            chText.text = "HP: " + currentHP + " / " + useMaxHP;
            conHealthGo.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 25, 0);

            if (buildType != 5)
            {
                GameObject desGo = Instantiate(constructButtonFab, hud.transform, false);
                Text desText = desGo.GetComponentInChildren<Text>();
                desText.text = "Destroy";
                desGo.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                gc.onlyHpDisp = false;
                ConstructButton cbco = desGo.GetComponent<ConstructButton>();
                cbco.buttonFunc = 0;

            }

            if(currentHP == 0)
            {
                GameObject rebGo = Instantiate(constructButtonFab, hud.transform, false);
                Text rebText = rebGo.GetComponentInChildren<Text>();
                rebText.text = "Rebuild $" + GetRebuildCost();
                rebGo.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                gc.onlyHpDisp = false;
                ConstructButton cbco = rebGo.GetComponent<ConstructButton>();
                cbco.buttonFunc = 2;
            }
            else if(currentHP < useMaxHP)
            {
                GameObject cbGo = Instantiate(constructButtonFab, hud.transform, false);
                Text cbText = cbGo.GetComponentInChildren<Text>();
                cbText.text = "Repair $" + GetRepairCost();
                cbGo.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 30, 0);
                gc.onlyHpDisp = false;
                ConstructButton cbco = cbGo.GetComponent<ConstructButton>();
                cbco.buttonFunc = 1;
            }
        }
    }

    private int minMax(int val, int min, int max)
    {
        return System.Math.Min(System.Math.Max(min, val), max);
    }

    private int GetRepairCost()
    {
        int mBase = 50;
        switch(buildType)
        {
            case 3: mBase = 50; break;
            case 4: mBase = 100; break;
            case 5: mBase = 500; break;
        }
        return minMax((int)(mBase * ((float)(useMaxHP - currentHP) / (float)useMaxHP)), 1, mBase - 1);
    }

    private int GetRebuildCost()
    {
        return 1000;
    }

    public void DestroyThisConstruct()
    {
        if (buildType != 5)
        {
            tile.SetTileTypeToZero();
            Destroy(this.gameObject);
        }
        else
        {
            currentHP = 0;
            tile.SetTileTypeToZero(6);
            transform.SetPositionAndRotation(new Vector3(transform.position.x - 0.1f, -0.83f, transform.position.z + 0.2f),
                Quaternion.Euler(new Vector3(15, 80, 350)));
            if (healthbar != null)
            {
                Destroy(healthbar);
                Destroy(healthbarE);
            }
            spawnedHealth = false;
        }
    }

    public void RepairConstruct()
    {
        int cost = GetRepairCost();
        if (gc.money - cost >= 0)
        {
            currentHP = useMaxHP;
            gc.money -= cost;
            spawnedHealth = false;
            if (healthbar != null)
            {
                Destroy(healthbar);
                Destroy(healthbarE);
            }
        }
        else
        {
            gc.NoMoney();
        }
    }
    
    public void RebuildConstruct()
    {
        int cost = GetRebuildCost();
        if (gc.money - cost >= 0)
        {
            currentHP = useMaxHP;
            gc.money -= cost;
            tile.SetTileTypeToZero(5);
            transform.SetPositionAndRotation(new Vector3(transform.position.x + 0.1f, 0, transform.position.z - 0.2f),
                Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else
        {
            gc.NoMoney();
        }
    }

    public void CalcHealth()
    {
        float percHealth = (float)currentHP / (float)useMaxHP;

        if (percHealth < 0.999f)
        {
            if (!spawnedHealth)
            {
                spawnedHealth = true;
                healthbar = Instantiate(healthPrefab, 
                    new Vector3(transform.position.x + healthXDisplacement, transform.position.y + healthYDisplacement, transform.position.z + healthZDisplacement), 
                    healthPrefab.transform.rotation, transform);
                healthbarE = Instantiate(healthEmptyPrefab,
                    new Vector3(transform.position.x + healthXDisplacement, transform.position.y + healthYDisplacement, transform.position.z + healthZDisplacement),
                    healthPrefab.transform.rotation, transform);
            }

            healthbar.transform.localScale = new Vector3(maxBarSize * percHealth, healthPrefab.transform.localScale.y, healthPrefab.transform.localScale.z);
            healthbarE.transform.localScale = new Vector3(healthEmptyPrefab.transform.localScale.x, healthEmptyPrefab.transform.localScale.y, healthEmptyPrefab.transform.localScale.z);
            healthbar.transform.position = new Vector3(transform.position.x + healthXDisplacement - (1 - percHealth) * maxBarSize/2, healthbar.transform.position.y, healthbar.transform.position.z);

            if (percHealth <= turnRed)
            {
                healthbar.GetComponent<MeshRenderer>().material = redMat;
            }
            else if (percHealth <= turnYellow)
            {
                healthbar.GetComponent<MeshRenderer>().material = yellowMat;
            }
        }
    }
}
