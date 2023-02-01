using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUIContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    public UISeedSO so;

    public void Setup(UISeedSO data)
    {
        so = data;
        titleText.text = so.title;
        priceText.text = so.price.ToString();
    }

    public void ButtonPressed()
    {
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (pc != null)
            pc.SwapCrop(so.cropData);
    }
}
