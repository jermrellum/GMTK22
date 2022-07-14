using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    [SerializeField] int maxHP;
    public int currentHP;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject healthEmptyPrefab;
    private bool spawnedHealth = false;
    [SerializeField] private float maxBarSize = 0.55f;
    [SerializeField] private float healthYDisplacement = 0.5f;
    [SerializeField] private float healthZDisplacement = -0.5f;
    private GameObject healthbar;
    private GameObject healthbarE;

    [SerializeField] private float turnYellow = 0.5f;
    [SerializeField] private float turnRed = 0.2f;

    //[SerializeField] private Material greenMat;
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material redMat;

    Tile tile;

    private void Start()
    {
        tile = GetComponentInParent<Tile>();
        currentHP = maxHP;
    }

    public void destroyThisConstruct()
    {
        Debug.Log(tile.name);
        tile.SetTileTypeToZero();
        Destroy(this.gameObject);
    }

    public void calcHealth()
    {
        float percHealth = (float)currentHP / (float)maxHP;

        if (percHealth < 0.999f)
        {
            if (!spawnedHealth)
            {
                spawnedHealth = true;
                healthbar = Instantiate(healthPrefab, 
                    new Vector3(transform.position.x - percHealth * maxBarSize / 2, transform.position.y + healthYDisplacement, transform.position.z + healthZDisplacement), 
                    healthPrefab.transform.rotation, transform);
                healthbarE = Instantiate(healthEmptyPrefab,
                    new Vector3(transform.position.x, transform.position.y + healthYDisplacement, transform.position.z + healthZDisplacement),
                    healthPrefab.transform.rotation, transform);
            }

            healthbar.transform.localScale = new Vector3(maxBarSize * percHealth / transform.localScale.x, 
                healthPrefab.transform.localScale.y / transform.localScale.y, healthPrefab.transform.localScale.z / transform.localScale.z);

            healthbarE.transform.localScale = new Vector3(healthEmptyPrefab.transform.localScale.x / transform.localScale.x,
                healthEmptyPrefab.transform.localScale.y / transform.localScale.y, healthEmptyPrefab.transform.localScale.z / transform.localScale.z);

            if (percHealth < turnRed)
            {
                healthbar.GetComponent<MeshRenderer>().material = redMat;
            }
            else if (percHealth < turnYellow)
            {
                healthbar.GetComponent<MeshRenderer>().material = yellowMat;
            }
        }
    }
}
