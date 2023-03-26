using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SeedUIContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image preview;
    public CropSO so;
    private MenuManager menuManager;

    public void Setup(CropSO data, MenuManager manager)
    {
        menuManager = manager;

        so = data;
        titleText.text = so.cropName;
        priceText.text = so.price.ToString();
        preview.sprite = so.previewImage;
    }

    public void ButtonPressed()
    {
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (pc != null)
            pc.SwapCrop(so);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        menuManager.SetInfoBox(so.cropName, so.price, so.seedDescription);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        menuManager.CloseInfoBox();
    }
}
