using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private MenuManager menus;

    // List of all crops and placebes in game
    public List<CropSO> inGameCrops = new List<CropSO>();
    public List<PlaceableObjectSO> inGamePlaceables = new List<PlaceableObjectSO>();

    // List of crops and placebes the player has unlocked
    public List<CropSO> availableCrops = new List<CropSO>();
    public List<PlaceableObjectSO> availablePlaceables = new List<PlaceableObjectSO>();

    // Crops the player currently has stored
    public List<CropInventroyEntry> inventoryList = new List<CropInventroyEntry>();

    [System.Serializable]
    public class CropInventroyEntry 
    {
        public string cropName;
        public int numHeld = 0;

        public CropInventroyEntry(string name, int num = 0)
        {
            cropName = name;
            numHeld = num;
        }
    }

    //  ================== ADD NEW CONTENT TO LISTS ==================
    public void UnlockFurniture(PlaceableObjectSO newData, bool fromLevelUp)
    {
        availablePlaceables.Add(newData);

        if (fromLevelUp)
            menus.UpdateBuildMenu();
    }

    public void UnlockCrop(CropSO newData, bool fromLevelUp)
    {
        // add to avaiable list and inventory list
        availableCrops.Add(newData);

        // Only add to inventory list if called as a level up
        // This will cause a crop to be added twice if ran during load since inventory list saves
        if (fromLevelUp && newData.canBeHarvested)
        {
            CropInventroyEntry newEntry = new CropInventroyEntry(newData.cropName);
            inventoryList.Add(newEntry);
        }

        if (fromLevelUp)
        {
            menus.UpdateSeedMenu();
            menus.UpdateCropSell();
            menus.UpdateSlimeFeed();
        }

    }

    // ================== INVENTORY MANAGEMENT ==================
    public void AddCrop(CropSO crop, int ammount)
    {
        foreach(CropInventroyEntry cropSlot in inventoryList)
        {
            if(crop.cropName == cropSlot.cropName)
            {
                cropSlot.numHeld += ammount;
                Debug.Log("Harvested crop " + cropSlot.cropName);
                return;
            }
        }

        // If crop was not in list
        Debug.Log("Crop not in list, adding it");
        CropInventroyEntry newEntry = new CropInventroyEntry(crop.cropName);
        inventoryList.Add(newEntry);
        AddCrop(crop, ammount);
    }

    public int GetNumHeld(CropSO crop)
    {
        if (crop == null)
            return 0;

        foreach (CropInventroyEntry cropSlot in inventoryList)
        {
            if (crop.cropName == cropSlot.cropName)
                return cropSlot.numHeld;
        }

        return 0;
    }

    public CropSO GetCropData(string cName)
    {
        foreach (CropSO crop in availableCrops)
        {
            if (crop.cropName == cName)
                return crop;
        }

        return null;
    }


    // ================== SAVE LOAD ==================
    private void ResetInventoryList()
    {
        foreach(CropSO crop in availableCrops)
        {
            if (crop.canBeHarvested)
            {
                CropInventroyEntry newEntry = new CropInventroyEntry(crop.cropName);

                inventoryList.Add(newEntry);
            }
        }
    }

    public void LoadData(GameData data)
    {
        inventoryList = data.inventoryList;

        // List is empty so fill it
        if (inventoryList.Count <= 0)
            ResetInventoryList();
    }

    public void SaveData(GameData data)
    {
        // Save crop inventory
        data.inventoryList = inventoryList;
    }
}
