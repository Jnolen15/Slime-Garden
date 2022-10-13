using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplicerButton : MonoBehaviour
{
    public bool canBePressed = false;
    public bool isPressed = false;

    public TextMeshPro priceText;

    void Start()
    {
        priceText.enabled = false;
    }

    private void OnMouseDown()
    {
        if(canBePressed) isPressed = true;
    }
}
