using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 0.05f;

    void Update()
    {
        float horIn = Input.GetAxis("Horizontal");
        float verIn = Input.GetAxis("Vertical");

        this.transform.position = new Vector3(transform.position.x + horIn * moveSpeed, transform.position.y, transform.position.z + verIn * moveSpeed);
    }
}
