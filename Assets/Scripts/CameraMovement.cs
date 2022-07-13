using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;

    [SerializeField] private float borderBottom = 0.0f;
    [SerializeField] private float borderLeft = 0.0f;
    [SerializeField] private float borderRight = 60.0f;
    [SerializeField] private float borderTop = 60.0f;

    private float minMax(float val, float min, float max)
    {
        //int umin = std::min(min, max);
        //int umax = std::max(min, max);

        return System.Math.Min(System.Math.Max(min, val), max);
    }

    void Update()
    {
        float horIn = Input.GetAxis("Horizontal");
        float verIn = Input.GetAxis("Vertical");
        bool shiftHeld = Input.GetKey("left shift") || Input.GetKey("right shift");

        float useSpeed = moveSpeed;
        if(shiftHeld)
        {
            useSpeed *= 2;
        }

        this.transform.position = new Vector3(minMax(transform.position.x + horIn * useSpeed * Time.deltaTime, borderLeft, borderRight), transform.position.y, 
            minMax(transform.position.z + verIn * useSpeed * Time.deltaTime, borderBottom, borderTop));
    }
}
