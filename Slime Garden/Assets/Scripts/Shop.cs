using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int price = 10;
    public int xBounds;
    public int yBounds;

    public GameObject slimeCrate;

    private SlimeBuy SlimeBuy;
    private PlayerController pc;

    void Start()
    {
        SlimeBuy = this.transform.GetChild(0).GetComponent<SlimeBuy>();
        pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    void Update()
    {
        // Update shop price texts
        SlimeBuy.priceText.text = price.ToString();
        if (pc.cs >= price)
            SlimeBuy.priceText.color = Color.green;
        else
            SlimeBuy.priceText.color = Color.red;
    }

    public void SlimePurchased()
    {
        if (pc.cs >= price)
        {
            pc.cs -= price;
            Debug.Log("BOUGHT SLIME");

            // Spawn a slime crate somewhere in the pen
            float randX = Random.Range(-xBounds, xBounds);
            float randY = Random.Range(-yBounds, yBounds);
            Vector3 pos = new Vector3(randX, randY, 0);

            GameObject crate = Instantiate(slimeCrate, pos, transform.rotation);
        }
    }
}
