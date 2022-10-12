using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private int curMovSpeed;
    private int curRotSpeed;
    private int curZoomSpeed;
    [SerializeField] private int boundsX = 10;
    [SerializeField] private int boundsZ = 10;
    [SerializeField] private int boundsY = 10;
    [SerializeField] private int edgeScrollSize = 20;
    private Vector3 zoomOffset;

    [Header("FOV and move down zoom")]
    [SerializeField] private float zoomMax = 120f;
    [SerializeField] private float zoomMin = 20f;

    [Header("Move forawrd zoom")]
    [SerializeField] private float zoomYMax = 15f;
    [SerializeField] private float zoomYMin = 5f;
    [SerializeField] private float fovMax = 80f;
    [SerializeField] private float fovMin = 40f;
    private float targetFOV = 60f;

    private void Awake()
    {
        zoomOffset = cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    void Update()
    {
        camMovement();
        if (useEdgeScrolling)
            camEdgeScrollMovement();
        camRotation();
        camSpeedUp();
        if (changeZoom)
            camZoom();
        else
            camZoomLower();
    }

    private void camMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W) && this.transform.position.z < boundsZ)
            inputDir.z = +1;
        if (Input.GetKey(KeyCode.S) && this.transform.position.z > -boundsZ)
            inputDir.z = -1;
        if (Input.GetKey(KeyCode.A) && this.transform.position.x > -boundsX)
            inputDir.x = -1;
        if (Input.GetKey(KeyCode.D) && this.transform.position.x < boundsX)
            inputDir.x = +1;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        this.transform.position += moveDir * curMovSpeed * Time.deltaTime;
    }

    private void camEdgeScrollMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.mousePosition.x<edgeScrollSize) inputDir.x = -1;
        if (Input.mousePosition.y<edgeScrollSize) inputDir.z = -1;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        this.transform.position += moveDir* curMovSpeed * Time.deltaTime;
    }

    private void camRotation()
    {
        // Rotation Control
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotateDir = +1;
        if (Input.GetKey(KeyCode.E))
            rotateDir = -1;

        this.transform.eulerAngles += new Vector3(0, rotateDir * curRotSpeed * Time.deltaTime, 0);

        // Snap to speciic angles Control
        if (Input.GetKey(KeyCode.Alpha1))
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Alpha2))
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        if (Input.GetKey(KeyCode.Alpha3))
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        if (Input.GetKey(KeyCode.Alpha4))
            this.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    private void camSpeedUp()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curMovSpeed = movSpeed * 2;
            curZoomSpeed = zoomSpeed * 2;
            curRotSpeed = rotSpeed * 2;
        }
        else
        {
            curMovSpeed = movSpeed;
            curZoomSpeed = zoomSpeed;
            curRotSpeed = rotSpeed;
        }
    }

    private void camZoom()
    {
        Vector3 zoomDir = zoomOffset.normalized;

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
            Vector3.Lerp(cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, zoomOffset, Time.deltaTime * zoomSpeed);
    }

    private void camZoomLower()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomOffset.y += curZoomSpeed;
            targetFOV += 5;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            zoomOffset.y -= curZoomSpeed;
            targetFOV -= 5;
        }

        zoomOffset.y = Mathf.Clamp(zoomOffset.y, zoomYMin, zoomYMax);
        targetFOV = Mathf.Clamp(targetFOV, fovMin, fovMax);

        cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cineCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, zoomOffset, Time.deltaTime * 10);
        cineCam.m_Lens.FieldOfView = Mathf.Lerp(cineCam.m_Lens.FieldOfView, targetFOV, Time.deltaTime * 10);
    }
}
