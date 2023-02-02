using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    // Rotates sprite twords camera, but does not rotate on the y axis
    void Update()
    {
        var target = Camera.main.transform;
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPos, Vector3.up);
    }
}
