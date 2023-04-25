using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level;
    public float experience;
    public int money;
    public int habitatTier;
    public Vector2Int borderFenceStyle;
    public List<string> unlockedSlimes;                             // Slime patterns unlocked
    public List<InventoryManager.CropInventroyEntry> inventoryList; // Cops in inventory
    public List<HabitatControl.SlimeDataEntry> slimeList;           // Slimes in the habitat
    public List<HabitatControl.SlimeDataEntry> tamedSlimeList;      // Tamed slimes not yet in the habitat
    public List<GridDataPersistence.PlaceableData> placeableList;   // Placeables in the habitat

    public GameData()
    {
        level = 0;
        experience = 0;
        money = 0;
        habitatTier = 0;
        borderFenceStyle = new Vector2Int(0, 0);
        unlockedSlimes = new List<string>();
        unlockedSlimes.Add("Null");
        inventoryList = new List<InventoryManager.CropInventroyEntry>();
        slimeList = new List<HabitatControl.SlimeDataEntry>();
        tamedSlimeList = new List<HabitatControl.SlimeDataEntry>();
        placeableList = new List<GridDataPersistence.PlaceableData>();
    }
}
