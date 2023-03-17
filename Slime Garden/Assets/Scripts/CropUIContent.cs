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
        iManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
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
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        var wzc = GameObject.FindGameObjectWithTag("Player").GetComponent<WildZoneControler>();

        if (pc != null)
            pc.SwapCrop(so);
        else if (wzc != null)
            wzc.SwapCrop(so);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        menuManager.SetInfoBox(so.cropName, numStored, so.CropDescription);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        menuManager.CloseInfoBox();
    }
}
