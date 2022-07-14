using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayRemember : MonoBehaviour
{
    public int dayToMember = 0;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
