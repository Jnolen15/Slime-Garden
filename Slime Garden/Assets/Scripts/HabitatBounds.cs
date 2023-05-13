using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatBounds : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Slime")
        {
            Debug.Log("Slime out of bounds, reloacating");
            other.transform.position = new Vector3(0, 5, 0);
        }
    }
}
