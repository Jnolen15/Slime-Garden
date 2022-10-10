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

    void Update()
    {
        /*if (!isPressed && sr.sprite != buttonSprite)
        {
            sr.sprite = buttonSprite;
            srLights.sprite = lightSprite;
        } else if (isPressed && sr.sprite != buttonSpriteDown)
        {
            sr.sprite = buttonSpriteDown;
            srLights.sprite = lightSpriteDown;
        }*/
    }

    private void OnMouseDown()
    {
        if(canBePressed) isPressed = true;
    }
}
