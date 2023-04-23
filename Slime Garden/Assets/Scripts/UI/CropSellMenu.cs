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
    private int sellAmmount = 1;

    private void Start()
    {
        invManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        UpdateCropMenu();
    }

    
    public void UpdateCropMenu()
    {
        Debug.Log("UPDATING SELL MENU");

        if(!invManager)
            invManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

        feedMenu.GetComponent<ContentMenu>().UpdateContent("crop", cropUIPrefab, invManager, menuManager);
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
        if (invManager.GetNumHeld(crop) > 0)
        {
            // Create Crop
            //var slimePos = curSlimeData.transform.position;
            //Vector3 spawnPos = new Vector3(slimePos.x - 1.5f, slimePos.y, slimePos.z - 3);
            //var instCrop = Instantiate(crop.cropObj, spawnPos, Quaternion.identity);
            //var cropObj = instCrop.GetComponent<CropObj>();
            //cropObj.Setup(crop);

            // If less crops held then the current sell ammount, sell the rest
            var numToSell = 0;
            if (invManager.GetNumHeld(crop) < sellAmmount)
                numToSell = (invManager.GetNumHeld(crop));
            else
                numToSell = sellAmmount;

            // Take from inventory and add money
            Debug.Log("Sold " + numToSell + " " + crop.cropName + " for " + (crop.sellValue * numToSell));
            invManager.AddCrop(crop, -numToSell);
            pData.GainMoney(crop.sellValue * numToSell);
            UpdateCropCount();

            menuManager.AnimateToken(Color.white);
        }
    }

    public void ChangeSellammount(int num)
    {
        sellAmmount = num;
    }
}
