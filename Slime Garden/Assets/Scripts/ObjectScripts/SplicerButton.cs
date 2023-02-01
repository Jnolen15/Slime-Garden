using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplicerButton : MonoBehaviour, IInteractable
{
    public bool canBePressed = false;
    public bool isPressed = false;

    public TextMeshPro priceText;

    void Start()
    {
        priceText.enabled = false;
    }

    public void Interact()
    {
        if (canBePressed) isPressed = true;
    }
}
