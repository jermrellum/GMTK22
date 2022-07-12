using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 0.05f;

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

        this.transform.position = new Vector3(minMax(transform.position.x + horIn * moveSpeed, borderLeft, borderRight), transform.position.y, 
            minMax(transform.position.z + verIn * moveSpeed, borderBottom, borderTop));
    }
}
