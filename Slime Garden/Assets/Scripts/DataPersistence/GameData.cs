using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int money;
    public List<InventoryManager.CropInventroyEntry> inventoryList;
    public List<HabitatControl.SlimeDataEntry> slimeList;
    public List<GridDataPersistence.PlaceableData> placeableList;

    public GameData()
    {
        money = 0;
        inventoryList = new List<InventoryManager.CropInventroyEntry>();
        slimeList = new List<HabitatControl.SlimeDataEntry>();
        placeableList = new List<GridDataPersistence.PlaceableData>();
    }
}
