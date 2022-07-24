using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float scrollSpeed = 3.0f;
    private float startScrollSpeed = 3.0f;

    private float mouseX = -1;
    private float mouseY = -1;

    private int sw;
    private int sh;

    [SerializeField] private float borderBottom = 0.0f;
    [SerializeField] private float borderLeft = 0.0f;
    [SerializeField] private float borderRight = 60.0f;
    [SerializeField] private float borderTop = 60.0f;
    [SerializeField] private float zoomMax = 40.0f;
    [SerializeField] private float zoomMin = 3.0f;
    [SerializeField] private float camConst = 10.0f;

    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);

    private float minMax(float val, float min, float max)
    {
        //int umin = std::min(min, max);
        //int umax = std::max(min, max);

        return System.Math.Min(System.Math.Max(min, val), max);
    }

    private void Start()
    {
        startScrollSpeed = scrollSpeed;

        sw = Screen.currentResolution.width;
        sh = Screen.currentResolution.height;
    }

    private void ClickMove()
    {
        if(Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            bool mouseHasData = (mouseX != -1);

            float prevMx = mouseX;
            float prevMy = mouseY;

            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;

            if (mouseHasData)
            {
                float mxDelta = (mouseX - prevMx) / sw;
                float myDelta = (mouseY - prevMy) / sh;

                this.transform.position = new Vector3(minMax(transform.position.x - mxDelta * moveSpeed * camConst, borderLeft, borderRight),
                    transform.position.y,
                    minMax(transform.position.z - myDelta * moveSpeed * camConst, borderBottom, borderTop));
            }

        }
        else
        {
            mouseX = -1;
            mouseY = -1;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    void Update()
    {
        ClickMove();

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
            borderTop += 1.3f * percScroll;
            moveSpeed -= 0.1f * percScroll;
            camConst -= 0.3f * percScroll;
            

        }
        else if(mscroll < 0 && transform.position.y < zoomMax)  //zoom out
        {
            float percScroll = scrollSpeed / startScrollSpeed;

            sIn = 1.0f;
            verIn = -20.0f * percScroll;
            borderBottom -= 0.2f * percScroll;
            borderTop -= 1.3f * percScroll;
            moveSpeed += 0.1f * percScroll;
            camConst += 0.3f * percScroll;

            scrollSpeed += 1.0f;
        }

        borderBottom = minMax(borderBottom, -16.66f, -3.27f);
        borderTop = minMax(borderTop, -3.0f, 63.0f);
        moveSpeed = minMax(moveSpeed, 5.63f, 12.33f);
        scrollSpeed = minMax(scrollSpeed, 71.0f, 136.0f);
        camConst = minMax(camConst, 2.0f, 15.0f);

        bool shiftHeld = Input.GetKey("left shift") || Input.GetKey("right shift");

        float useSpeed = moveSpeed;
        if(shiftHeld)
        {
            useSpeed *= 2;
        }

        this.transform.position = new Vector3(minMax(transform.position.x + horIn * useSpeed * Time.deltaTime, borderLeft, borderRight), 
            minMax(transform.position.y + sIn * scrollSpeed * Time.deltaTime, zoomMin, zoomMax), 
            minMax(transform.position.z + verIn * useSpeed * Time.deltaTime, borderBottom, borderTop));
    }
}
