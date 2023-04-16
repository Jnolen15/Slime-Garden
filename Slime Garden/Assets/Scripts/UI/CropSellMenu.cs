using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSellMenu : MonoBehaviour
{
    private InventoryManager invManager;
    private PlayerData pData;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject feedMenu;
    [SerializeField] private GameObject cropUIPrefab;

    private void Start()
    {
        invManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        UpdateCropMenu();
    }

    public void UpdateCropMenu()
    {
        var contentMenu = feedMenu.transform.GetChild(0);
        foreach (InventoryManager.CropInventroyEntry crop in invManager.inventoryList)
        {
            var element = Instantiate(cropUIPrefab, contentMenu);
            element.GetComponent<CropUIContent>().Setup(crop, menuManager);
        }

        feedMenu.GetComponent<ContentMenu>().UpdateContent();
    }

    public void UpdateCropCount()
    {
        var contentMenu = feedMenu.transform.GetChild(1);
        foreach (Transform child in contentMenu)
        {
            child.GetComponent<CropUIContent>().UpdateValues();
        }
    }

    public void SellCrop(CropSO crop)
    {
        if (0 < invManager.GetNumHeld(crop))
        {
            // Create Crop
            //var slimePos = curSlimeData.transform.position;
            //Vector3 spawnPos = new Vector3(slimePos.x - 1.5f, slimePos.y, slimePos.z - 3);
            //var instCrop = Instantiate(crop.cropObj, spawnPos, Quaternion.identity);
            //var cropObj = instCrop.GetComponent<CropObj>();
            //cropObj.Setup(crop);

            // Take from inventory and add money
            Debug.Log("Sold " + crop.cropName);
            invManager.AddCrop(crop, -1);
            pData.GainMoney(crop.sellValue);
            UpdateCropCount();
        }
    }
}
