using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffMember : MonoBehaviour
{
    [HideInInspector] public int difficulty = 0;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
