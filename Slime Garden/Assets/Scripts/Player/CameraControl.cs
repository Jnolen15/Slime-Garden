using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    // Code based on www.youtube.com/watch?v=pJQndtJ2rk0

    [SerializeField]
    private CinemachineVirtualCamera cineCam;
    [SerializeField] private bool useEdgeScrolling;
    [SerializeField] private bool changeZoom;
    [SerializeField] private int movSpeed = 5;
    [SerializeField] private int rotSpeed = 20;
    [SerializeField] private int zoomSpeed = 5;
    [SerializeField] private int zoomFOVSpeed = 5;
    private int curMovSpeed;
    private int curRotSpeed;
    private int curZoomSpeed;
    private int curZoomFOVSpeed;
    [SerializeField] private int boundsX = 10;
    [SerializeField] private int boundsZ = 10;
    [SerializeField] private int boundsY = 10;
    [SerializeField] private int edgeScrollSize = 20;
    [SerializeField] private Vector3 zoomOffset;
    private Vector3 moveVector;
    private float rotateDir;
    private int snapCount;

    [Header("FOV and move down zoom")]
    [SerializeField] private float zoomMax = 120f;
    [SerializeField] private float zoomMin = 20f;

    [Header("Move forawrd zoom")]
    [SerializeField] private float zoomYMax = 15f;
    [SerializeField] private float zoomYMin = 5f;
    [SerializeField] private float fovMax = 80f;
    [SerializeField] private float fovMin = 40f;
    [SerializeField] private float targetFOV = 60f;


    private void Awake()
    {
        zoomOffset = cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        curMovSpeed = movSpeed;
        curRotSpeed = rotSpeed;
        curZoomSpeed = zoomSpeed;
        curZoomFOVSpeed = zoomFOVSpeed;
    }

    private PlayerController pc;
    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        // Movement
        camMovement(moveVector);
        if (useEdgeScrolling)
            camEdgeScrollMovement();

        // Zoom


        if (changeZoom)
            camZoom();
        else
            camZoomLower();
    }

    private void camMovement(Vector3 inputDir)
    {
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        this.transform.position += moveDir * curMovSpeed * Time.deltaTime;
    }

    private void camEdgeScrollMovement()
    {
        Debug.LogError("Edge scroll not implemented");
        /*Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.mousePosition.x<edgeScrollSize) inputDir.x = -1;
        if (Input.mousePosition.y<edgeScrollSize) inputDir.z = -1;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        this.transform.position += moveDir* curMovSpeed * Time.deltaTime;*/
    }

    private void camRotation()
    {
        if (rotateDir > 0)
            snapCount++;
        else if (rotateDir < 0)
            snapCount--;

        if (snapCount > 3)
            snapCount = 0;
        else if (snapCount < 0)
            snapCount = 3;

        if (snapCount == 0)
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (snapCount == 1)
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        else if (snapCount == 2)
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        else if (snapCount == 3)
            this.transform.eulerAngles = new Vector3(0, -90, 0);

        // Old hold to rotate unsnapped method.
        // To return to this, take method call out of OnRotate and add it to Update
        //this.transform.eulerAngles += new Vector3(0, rotateDir * curRotSpeed * Time.deltaTime, 0);
    }

    private void camZoom()
    {
        Debug.LogError("Alt zoom not implemented");
        /*Vector3 zoomDir = zoomOffset.normalized;

        if (Input.mouseScrollDelta.y < 0)
            zoomOffset += zoomDir;
        if (Input.mouseScrollDelta.y > 0)
            zoomOffset -= zoomDir;

        if (zoomOffset.magnitude < zoomMin)
            zoomOffset = zoomDir * zoomMin;
        if (zoomOffset.magnitude > zoomMax)
            zoomOffset = zoomDir * zoomMax;

        float zoomSpeed = 10f;
        cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
            Vector3.Lerp(cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, zoomOffset, Time.deltaTime * zoomSpeed);*/
    }

    private void camZoomLower()
    {
        zoomOffset.y = Mathf.Clamp(zoomOffset.y, zoomYMin, zoomYMax);
        targetFOV = Mathf.Clamp(targetFOV, fovMin, fovMax);

        cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, zoomOffset, Time.deltaTime * 10);
        cineCam.m_Lens.FieldOfView = Mathf.Lerp(cineCam.m_Lens.FieldOfView, targetFOV, Time.deltaTime * 10);
    }

    // ========== CONTROLS ==========
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVal = context.ReadValue<Vector2>();
        moveVector = new Vector3(inputVal.x, 0, inputVal.y);
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (pc.MouseOverUI())
            return;

        float z = context.ReadValue<float>();

        if (z < 0)
        {
            zoomOffset.y += curZoomSpeed;
            targetFOV += curZoomFOVSpeed;
        }
        if (z > 0)
        {
            zoomOffset.y -= curZoomSpeed;
            targetFOV -= curZoomFOVSpeed;
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        var inputVal = context.ReadValue<float>();
        rotateDir = inputVal;
        camRotation();
    }

    // Not really used anymore since makine Q/E snap. Remove this if it stays that way
    public void OnCamSnap(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        snapCount++;

        if (snapCount > 3)
            snapCount = 0;

        if (snapCount == 0)
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (snapCount == 1)
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        else if (snapCount == 2)
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        else if (snapCount == 3)
            this.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    public void OnCamSpeedUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            curMovSpeed = movSpeed * 2;
            curRotSpeed = rotSpeed * 2;
            curZoomSpeed = zoomSpeed * 2;
            curZoomFOVSpeed = zoomFOVSpeed * 2;
        }
        else if (context.canceled)
        {
            curMovSpeed = movSpeed;
            curRotSpeed = rotSpeed;
            curZoomSpeed = zoomSpeed;
            curZoomFOVSpeed = zoomFOVSpeed;
        }
    }
}
