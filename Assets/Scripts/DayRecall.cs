using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayRecall : MonoBehaviour
{
    public Text surviveText;
    void Start()
    {
        GameObject dr = GameObject.Find("DayRememberer");
        DayRemember drc = dr.GetComponent<DayRemember>();
        string theS = "s";
        if(drc.dayToMember == 1)
        {
            theS = "";
        }
        surviveText.text = "Survived " + drc.dayToMember + " day" + theS;
    }
}
