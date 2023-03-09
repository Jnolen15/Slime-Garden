using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<CropInventroyEntry> inventoryList = new List<CropInventroyEntry>();

    [System.Serializable]
    public class CropInventroyEntry 
    {
        public CropSO data;
        public int numHeld = 0;
    }

    public void AddCrop(CropSO crop, int ammount)
    {
        foreach(CropInventroyEntry cropSlot in inventoryList)
        {
            if(crop == cropSlot.data)
            {
                cropSlot.numHeld += ammount;
                Debug.Log("Harvested crop " + cropSlot.data.cropName);
                break;
            }
        }
    }

    public int GetNumHeld(CropSO crop)
    {
        foreach (CropInventroyEntry cropSlot in inventoryList)
        {
            if (crop == cropSlot.data)
                return cropSlot.numHeld;
        }

        return 0;
    }
}
