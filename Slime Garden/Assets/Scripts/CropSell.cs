using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSell : MonoBehaviour
{
    private PlayerController pc;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Crop")
            return;

        CropSO data = col.gameObject.GetComponent<CropObj>().cropData;
        pc.Money += data.sellValue;
        Destroy(col.gameObject);
    }
}
