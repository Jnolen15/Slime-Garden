using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int movSpeed = 5;
    public int zoomSpeed = 200;
    public int boundsX = 10;
    public int boundsZ = 10;
    public int boundsY = 10;

    private int curmovSpeed;
    private int curzoomSpeed;

    void Update()
    {
        // Directional Camera Controll
        if (Input.GetKey(KeyCode.W) && this.transform.position.z < boundsZ)
            this.transform.position += new Vector3(0, 0, 1) * curmovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) && this.transform.position.z > -boundsZ*2)
            this.transform.position += new Vector3(0, 0, -1) * curmovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && this.transform.position.x > -boundsX)
            this.transform.position += new Vector3(-1, 0, 0) * curmovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) && this.transform.position.x < boundsX)
            this.transform.position += new Vector3(1, 0, 0) * curmovSpeed * Time.deltaTime;
        
        if (Input.mouseScrollDelta.y < 0 && this.transform.position.y < boundsY)
            this.transform.position += new Vector3(0, 1, 0) * curzoomSpeed * Time.deltaTime;
        if (Input.mouseScrollDelta.y > 0 && this.transform.position.y < boundsY)
            this.transform.position += new Vector3(0, -1, 0) * curzoomSpeed * Time.deltaTime;

        // Camera Speed Up
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curmovSpeed = movSpeed * 2;
            curzoomSpeed = zoomSpeed * 2;
        }
        else
        {
            curmovSpeed = movSpeed;
            curzoomSpeed = zoomSpeed;
        }
    }
}
