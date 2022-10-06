using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplicerButton : MonoBehaviour
{
    public bool canBePressed = false;
    public bool isPressed = false;

    public Sprite buttonSprite;
    public Sprite buttonSpriteDown;
    public Sprite lightSprite;
    public Sprite lightSpriteDown;

    private SpriteRenderer sr;
    private SpriteRenderer srLights;

    public TextMeshPro priceText;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        srLights = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        priceText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPressed && sr.sprite != buttonSprite)
        {
            sr.sprite = buttonSprite;
            srLights.sprite = lightSprite;
        } else if (isPressed && sr.sprite != buttonSpriteDown)
        {
            sr.sprite = buttonSpriteDown;
            srLights.sprite = lightSpriteDown;
        }
    }

    private void OnMouseDown()
    {
        if(canBePressed) isPressed = true;
    }
}
