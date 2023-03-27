using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
