using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] AudioSource DayMusic;
    [SerializeField] AudioSource NightMusic;
    [SerializeField] private int framesBeforeResume = 120;
    [HideInInspector] public int ticksBRCounter = 0;
    [HideInInspector] public bool contextMenuShowing = false;
    [HideInInspector] public bool hoveringOnButton = false;
    [HideInInspector] public bool firstFrameConstructContext = false;
    [HideInInspector] public bool onlyHpDisp = false;
    [HideInInspector] public bool isTileContext = false;
    public int difficulty = 0; // 0 = hard, 1 = medium, 2 = easiest

    private Light daylight;
    private Color dayColor;
    private GridManager gm;
    private HudPanel hudPan;

    private void Awake()
    {
        /*GameObject dr = GameObject.Find("Difficulty Rememberer");
        DiffMember dm = dr.GetComponent<DiffMember>();

        difficulty = dm.difficulty;

        Destroy(dr);*/

        difficulty = 2;
    }

    private void Start()
    {
        daylight = GetComponentInChildren<Light>();
        dayColor = daylight.color;
        gm = GetComponentInChildren<GridManager>();
        hudPan = GetComponentInChildren<HudPanel>();
    }

    public bool IsInMenu()
    {
        return (!contextMenuShowing || onlyHpDisp);
    }

    public void CallHudClear()
    {
        hudPan.ClearHudOfConstructContexts();
    }

    public void ConstructButtonFunc(int buttonFunc)
    {
        hudPan.ConstructButtonFunc(buttonFunc);
    }

    //each building generates some amount of money, if it survived
    private void addMoneyGenerated()
    {
        int survivingBuildings = gm.countNumberOfBuildType(2);

        int earnedMoney = survivingBuildings * worthOfBuilding * surviving;

        money += earnedMoney;

        if(difficulty == 2)
        {
            money += 250;
        }
    }

    public void calculateSurvivors()
    {
        int survivingHouses = gm.countNumberOfBuildType(5);
        int initHouses = gm.getStartingHouses();

        float percSurv = (float) survivingHouses / (float) initHouses;

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

    public void NoMoney()
    {
        Debug.Log("no money");
    }

    public void proceedToNight()
    {
        isDay = false;
        daylight.color = Color.black;
        ticksBRCounter = framesBeforeResume;

        if(difficulty == 0)
        {
            gm.FloodStart();
        }

        DayMusic.Stop();
        NightMusic.Play();
    }

    public void proceedToDay()
    {
        calculateSurvivors();
        
        if(surviving <= 0)
        {
            GameEnd();
        }

        addMoneyGenerated();
        isDay = true;
        daylight.color = dayColor;
        day++;

        nightbutton.gameObject.SetActive(true);
        NightMusic.Stop();
        DayMusic.Play();

        if (difficulty > 0)
        {
            gm.FloodStart();
        }
    }

    private void GameEnd()
    {
        GameObject dr = GameObject.Find("DayRememberer");
        DayRemember drc = dr.GetComponent<DayRemember>();
        drc.dayToMember = day;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
