using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplicerButton : MonoBehaviour, IInteractable
{
    public bool canBePressed = false;
    public bool isPressed = false;

    public TextMeshPro priceText;

    [SerializeField] private Material mat;

    void Start()
    {
        mat = this.GetComponentInChildren<MeshRenderer>().material;

        priceText.enabled = false;
    }

    public void Interact()
    {
        if (canBePressed) isPressed = true;
    }

    public void UpdateColor(Color newColor)
    {
        if(mat != null)
            mat.SetColor("_HighlightColor", Color.Lerp(mat.GetColor("_HighlightColor"), newColor, 0.01f));
    }
}
