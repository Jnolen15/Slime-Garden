using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongealedSlime : MonoBehaviour
{
    public Color color;
    public int value = 1;

    private SpriteRenderer sr;
    private PlayerController pc;
    
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    void Update()
    {
        sr.color = color;
    }

    private void OnMouseEnter()
    {
        pc.cs += value;
        Destroy(this.gameObject);
    }
}
