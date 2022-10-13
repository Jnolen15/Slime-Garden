using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteractions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CS")
        {
            other.gameObject.GetComponent<CongealedSlime>().Collect();
        }
    }
}
