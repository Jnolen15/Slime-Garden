using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class placeableUIContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    public UIPlaceableSO so;

    public void Setup(UIPlaceableSO data)
    {
        so = data;
        titleText.text = so.title;
        priceText.text = so.price.ToString();
    }

    public void ButtonPressed()
    {
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (pc != null)
            pc.SwapPlaceable(so.placeableData);
    }
}
