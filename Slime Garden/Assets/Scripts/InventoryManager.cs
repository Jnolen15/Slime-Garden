using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public List<CropSO> cropList = new List<CropSO>();
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

    // ================== INVENTORY MANAGEMENT ==================
    public void AddCrop(CropSO crop, int ammount)
    {
        foreach(CropInventroyEntry cropSlot in inventoryList)
        {
            if(crop.cropName == cropSlot.cropName)
            {
                cropSlot.numHeld += ammount;
                Debug.Log("Harvested crop " + cropSlot.cropName);
                break;
            }
        }
    }

    public int GetNumHeld(CropSO crop)
    {
        foreach (CropInventroyEntry cropSlot in inventoryList)
        {
            if (crop.cropName == cropSlot.cropName)
                return cropSlot.numHeld;
        }

        return 0;
    }

    public CropSO GetCropData(string cName)
    {
        foreach (CropSO crop in cropList)
        {
            if (crop.cropName == cName)
                return crop;
        }

        return null;
    }


    // ================== SAVE LOAD ==================
    private void ResetInventoryList()
    {
        foreach(CropSO crop in cropList)
        {
            CropInventroyEntry newEntry = new CropInventroyEntry(crop.cropName);

            inventoryList.Add(newEntry);
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
        data.inventoryList = inventoryList;
    }
}
