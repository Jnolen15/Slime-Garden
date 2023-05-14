using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCollectionRadious : MonoBehaviour
{
    [SerializeField] CSCollector collector;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CS")
        {
            StartCoroutine(collector.StoreCS(other.GetComponent<CongealedSlime>()));
        }
    }
}
