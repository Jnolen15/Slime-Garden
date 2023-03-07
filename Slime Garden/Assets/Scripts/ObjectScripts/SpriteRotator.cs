using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private bool camRotation;

    void Update()
    {
        if (!camRotation)
            RotateToCam();
        else
            FaceCamDir();
    }

    // Rotates sprite twords camera, but does not rotate on the y axis
    private void RotateToCam()
    {
        var target = Camera.main.transform;
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPos, Vector3.up);
    }

    // Rotates sprite to locked position facing cam (N,E,S,W)
    private void FaceCamDir()
    {
        this.transform.rotation = Camera.main.transform.rotation;
    }
}
