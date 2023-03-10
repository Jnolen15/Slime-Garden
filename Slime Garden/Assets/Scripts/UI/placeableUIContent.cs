using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class placeableUIContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image preview;
    public PlaceableObjectSO so;
    private MenuManager menuManager;

    public void Setup(PlaceableObjectSO data, MenuManager manager)
    {
        menuManager = manager;

        so = data;
        titleText.text = so.placeableName;
        priceText.text = so.price.ToString();
        preview.sprite = so.previewImage;
    }

    public void ButtonPressed()
    {
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (pc != null)
            pc.SwapPlaceable(so);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        menuManager.SetInfoBox(so.placeableName, so.price, so.description);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        menuManager.CloseInfoBox();
    }
}
