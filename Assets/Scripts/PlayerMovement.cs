using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horIn = Input.GetAxis("Horizontal");
        float verIn = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horIn * moveSpeed, rb.velocity.y, verIn * moveSpeed);
    }
}
