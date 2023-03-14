using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSell : MonoBehaviour
{
    private PlayerData pData;

    private void Start()
    {
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Crop")
            return;

        CropSO data = col.gameObject.GetComponent<CropObj>().cropData;
        pData.GainMoney(data.sellValue);
        col.gameObject.GetComponent<CropObj>().DestroySelf();
        //Destroy(col.gameObject);
    }
}
