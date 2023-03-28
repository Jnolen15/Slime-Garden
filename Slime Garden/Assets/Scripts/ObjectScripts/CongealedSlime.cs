using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongealedSlime : MonoBehaviour
{
    private PlayerData pData;
    public int value = 1;
    [SerializeField] private GameObject sparkleFX;
    
    void Start()
    {
        pData = GameObject.Find("PlayerController").GetComponent<PlayerData>();
    }

    public void Collect()
    {
        pData.GainMoney(value);
        Instantiate(sparkleFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
