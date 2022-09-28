using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int movSpeed = 5;
    public int boundsX = 9;
    public int boundsY = 9;

    private int curmovSpeed = 5;
    private float originalSize = 0f;
    private float zoomFactor = 1f;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
        originalSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Directional Camera Controll
        if (Input.GetKey(KeyCode.W) && this.transform.position.y < boundsY)
            this.transform.position += new Vector3(0, 1, 0) * curmovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) && this.transform.position.y > -boundsY)
            this.transform.position += new Vector3(0, -1, 0) * curmovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && this.transform.position.x > -boundsX)
            this.transform.position += new Vector3(-1, 0, 0) * curmovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) && this.transform.position.x < boundsX)
            this.transform.position += new Vector3(1, 0, 0) * curmovSpeed * Time.deltaTime;

        // Camera Speed Up
        if (Input.GetKey(KeyCode.LeftShift))
            curmovSpeed = movSpeed * 2;
        else curmovSpeed = movSpeed;

        // Camera Zoom
        cam.orthographicSize = Mathf.Round(originalSize * zoomFactor);

        if(Input.mouseScrollDelta.y > 0)
        {
            if (zoomFactor > 0.5f)
                zoomFactor -= 0.25f;
            else zoomFactor = 0.5f;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (zoomFactor < 2f)
                zoomFactor += 0.25f;
            else zoomFactor = 2f;
        }
    }
}
