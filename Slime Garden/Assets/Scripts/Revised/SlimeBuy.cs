using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlimeBuy : MonoBehaviour, IInteractable
{
    private Shop shop;

    public TextMeshPro priceText;

    void Start()
    {
        shop = this.transform.GetComponentInParent<Shop>();
    }


    public void Interact()
    {
        shop.SlimePurchased();
    }
}
