using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongealedSlime : MonoBehaviour
{
    private PlayerData pData;
    public int value = 1;
    
    void Start()
    {
        pData = GameObject.Find("PlayerController").GetComponent<PlayerData>();
    }

    public void Collect()
    {
        pData.GainMoney(value);
        Destroy(this.gameObject);
    }
}
