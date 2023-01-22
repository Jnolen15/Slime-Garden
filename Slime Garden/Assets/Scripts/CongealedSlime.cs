using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongealedSlime : MonoBehaviour
{
    private PlayerController pc;
    public int value = 1;
    
    void Start()
    {
        pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    public void Collect()
    {
        pc.Money += value;
        Destroy(this.gameObject);
    }
}
