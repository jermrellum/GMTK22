using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text moneyText;
    public Text healthText;
    public Text dayText;
    public int money = 1000;
    public int surviving = 100;
    public int day = 1;
    [HideInInspector] public bool contextMenuShowing = false;
    [HideInInspector] public bool hoveringOnButton = false;

    private void Update()
    {
        moneyText.text = "$" + money;
        healthText.text = "Surviving: " + surviving + "%";
        dayText.text = "Day " + day;
    }
}
