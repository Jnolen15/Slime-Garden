using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongealedSlime : MonoBehaviour
{
    private PlayerData pData;
    private MenuManager menuManager;
    public int value = 1;
    [SerializeField] private Color color;
    [SerializeField] private GameObject sparkleFX;
    
    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        menuManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
    }

    public void SetValue(int newVal)
    {
        value = newVal;
    }

    public void Collect()
    {
        pData.GainMoney(value);
        Instantiate(sparkleFX, transform.position, Quaternion.identity);
        menuManager.AnimateToken(color);
        Destroy(this.gameObject);
    }
}
