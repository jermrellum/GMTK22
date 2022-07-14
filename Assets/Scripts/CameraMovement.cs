using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float scrollSpeed = 3.0f;
    private float startScrollSpeed = 3.0f;

    [SerializeField] private float borderBottom = 0.0f;
    [SerializeField] private float borderLeft = 0.0f;
    [SerializeField] private float borderRight = 60.0f;
    [SerializeField] private float borderTop = 60.0f;
    [SerializeField] private float zoomMax = 40.0f;
    [SerializeField] private float zoomMin = 3.0f;

    private float minMax(float val, float min, float max)
    {
        //int umin = std::min(min, max);
        //int umax = std::max(min, max);

        return System.Math.Min(System.Math.Max(min, val), max);
    }

    private void Start()
    {
        startScrollSpeed = scrollSpeed;
    }

    void Update()
    {
        float horIn = Input.GetAxis("Horizontal");
        float verIn = Input.GetAxis("Vertical");
        float mscroll = Input.mouseScrollDelta.y;
        float sIn = 0;

        if (mscroll > 0 && transform.position.y > zoomMin)    //zoom in
        {
            scrollSpeed -= 1.0f;
            float percScroll = scrollSpeed / startScrollSpeed;

            sIn = -1.0f;
            verIn = 20.0f * percScroll;
            borderBottom += 0.2f * percScroll;
            moveSpeed -= 0.1f * percScroll;
            

        }
        else if(mscroll < 0 && transform.position.y < zoomMax)  //zoom out
        {
            float percScroll = scrollSpeed / startScrollSpeed;

            sIn = 1.0f;
            verIn = -20.0f * percScroll;
            borderBottom -= 0.2f * percScroll;
            moveSpeed += 0.1f * percScroll;

            scrollSpeed += 1.0f;
            
        }

        bool shiftHeld = Input.GetKey("left shift") || Input.GetKey("right shift");

        float useSpeed = moveSpeed;
        if(shiftHeld)
        {
            useSpeed *= 2;
        }

        this.transform.position = new Vector3(minMax(transform.position.x + horIn * useSpeed * Time.deltaTime, borderLeft, borderRight), 
            transform.position.y + sIn * scrollSpeed * Time.deltaTime, 
            minMax(transform.position.z + verIn * useSpeed * Time.deltaTime, borderBottom, borderTop));
    }
}
