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

    private void OnMouseDown()
    {
        shop.SlimePurchased();
    }
}
