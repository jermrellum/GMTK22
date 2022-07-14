using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text moneyText;
    public Text healthText;
    public Text dayText;
    public Button nightbutton;
    public int money = 1000;
    public int worthOfBuilding = 10;
    public int surviving = 100;
    public int day = 1;
    public bool isDay = true;
    [SerializeField] private int ticksBeforeResume = 120;
    [HideInInspector] public int ticksBRCounter = 0;
    [HideInInspector] public bool contextMenuShowing = false;
    [HideInInspector] public bool hoveringOnButton = false;

    private Light daylight;
    private Color dayColor;
    private GridManager gm;

    private void Start()
    {
        daylight = GetComponentInChildren<Light>();
        dayColor = daylight.color;
        gm = GetComponentInChildren<GridManager>();
    }

    //each building generates some amount of money, if it survived
    private void addMoneyGenerated()
    {
        int survivingBuildings = gm.countNumberOfBuildType(2);

        int earnedMoney = survivingBuildings * worthOfBuilding;

        money += earnedMoney;
    }

    private void calculateSurvivors()
    {
        int survivingHouses = gm.countNumberOfBuildType(5);
        int initHouses = gm.getStartingHouses();

        float percSurv = survivingHouses / initHouses;

        int newPerc = (int) (percSurv * 100);

        surviving = newPerc;
    }

    private void Update()
    {
        moneyText.text = "$" + money;
        healthText.text = "Surviving: " + surviving + "%";

        string timeOfDay = "Day";
        if(!isDay) 
        {
            timeOfDay = "Night";
        }
        dayText.text = timeOfDay + " " + day;
    }

    public void proceedToNight()
    {
        isDay = false;
        daylight.color = Color.black;
        ticksBRCounter = ticksBeforeResume;
    }

    public void proceedToDay()
    {
        isDay = true;
        daylight.color = dayColor;
        day++;
        addMoneyGenerated();
        calculateSurvivors();
        nightbutton.gameObject.SetActive(true);
    }
}
