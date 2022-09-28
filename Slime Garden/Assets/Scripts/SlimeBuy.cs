using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlimeBuy : MonoBehaviour
{
    private Shop shop;

    public TextMeshPro priceText;

    void Start()
    {
        shop = this.transform.GetComponentInParent<Shop>();
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        shop.SlimePurchased();
    }
}
