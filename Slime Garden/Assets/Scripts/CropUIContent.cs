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
    public InventoryManager.CropInventroyEntry cropData;
    public CropSO so;
    public int numStored;
    private MenuManager menuManager;

    public void Setup(InventoryManager.CropInventroyEntry crop, MenuManager manager)
    {
        menuManager = manager;

        cropData = crop;
        so = crop.data;
        titleText.text = so.cropName;
        numText.text = cropData.numHeld.ToString();
        numStored = cropData.numHeld;
    }

    public void UpdateValues()
    {
        numText.text = cropData.numHeld.ToString();
        numStored = cropData.numHeld;
    }

    public void ButtonPressed()
    {
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (pc != null)
            pc.SwapCrop(so);
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
