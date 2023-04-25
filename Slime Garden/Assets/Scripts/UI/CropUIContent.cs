using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CropUIContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI numText;
    [SerializeField] private Image preview;
    private InventoryManager iManager;
    public InventoryManager.CropInventroyEntry cropData;
    public CropSO so;
    public int numStored;
    private MenuManager menuManager;

    public void Setup(InventoryManager.CropInventroyEntry crop, MenuManager manager)
    {
        iManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();
        menuManager = manager;

        cropData = crop;
        so = iManager.GetCropData(cropData.cropName);
        titleText.text = so.cropName;
        numText.text = cropData.numHeld.ToString();
        numStored = cropData.numHeld;
        preview.sprite = so.previewImage;
    }

    public void UpdateValues()
    {
        numText.text = cropData.numHeld.ToString();
        numStored = cropData.numHeld;
    }

    public void ButtonPressed()
    {
        var sIP = gameObject.GetComponentInParent<SlimeInfoPannel>();
        var cSM = gameObject.GetComponentInParent<CropSellMenu>();

        if (sIP != null)
            sIP.FeedSlime(so);
        else if (cSM != null)
            cSM.SellCrop(so);

        menuManager.SetInfoBox(so.cropName, numStored, so.CropDescription, so.previewImage);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        menuManager.SetInfoBox(so.cropName, numStored, so.CropDescription, so.previewImage);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        menuManager.CloseInfoBox();
    }
}
