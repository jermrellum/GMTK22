using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text moneyText;
    public Text healthText;
    public int money = 1000;
    public int surviving = 100;
    [HideInInspector] public bool contextMenuShowing = false;
    [HideInInspector] public bool hoveringOnButton = false;

    private void Update()
    {
        moneyText.text = "$" + money;
        healthText.text = "Surviving: " + surviving + "%";
    }
}
