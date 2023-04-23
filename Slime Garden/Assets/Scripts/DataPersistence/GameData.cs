using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level;
    public int money;
    public int habitatTier;
    public Vector2Int borderFenceStyle;
    public List<InventoryManager.CropInventroyEntry> inventoryList;
    public List<HabitatControl.SlimeDataEntry> slimeList;
    public List<HabitatControl.SlimeDataEntry> tamedSlimeList;
    public List<GridDataPersistence.PlaceableData> placeableList;

    public GameData()
    {
        level = 0;
        money = 0;
        habitatTier = 0;
        borderFenceStyle = new Vector2Int(0, 0);
        inventoryList = new List<InventoryManager.CropInventroyEntry>();
        slimeList = new List<HabitatControl.SlimeDataEntry>();
        tamedSlimeList = new List<HabitatControl.SlimeDataEntry>();
        placeableList = new List<GridDataPersistence.PlaceableData>();
    }
}
