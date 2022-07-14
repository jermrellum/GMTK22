using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffMember : MonoBehaviour
{
    [HideInInspector] public bool isHard = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
