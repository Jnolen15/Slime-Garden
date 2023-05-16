using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongealedSlime : MonoBehaviour
{
    private PlayerData pData;
    private StatTracker stats;
    private MenuManager menuManager;
    public int value = 1;
    [SerializeField] private Color color;
    [SerializeField] private GameObject sparkleFX;
    
    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        stats = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();
        menuManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
    }

    public void SetValue(int newVal)
    {
        value = newVal;
    }

    public void Collect()
    {
        stats.IncrementStat("csCollected", 1);
        pData.GainMoney(value);
        Instantiate(sparkleFX, transform.position, Quaternion.identity);
        menuManager.AnimateToken(color);
        Destroy(this.gameObject);
    }

    public void Store()
    {
        Instantiate(sparkleFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
