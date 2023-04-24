using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelRewardHandler : SerializedMonoBehaviour, IDataPersistence
{
    [SerializeField] private PlayerData pData;
    [SerializeField] private List<LevelReward> levelRewards = new List<LevelReward>();

    [Button]
    public void GrantLevelRewards(int level)
    {
        foreach (LevelReward reward in levelRewards)
        {
            if (reward.levelEarned == level)
                reward.GiveReward(true);
        }
    }

    // ================ LEVEL REWARDS ================
    [System.Serializable]
    public abstract class LevelReward
    {
        public string rewardName;
        public int levelEarned;
        public abstract void GiveReward(bool fromLevelUp);
    }

    [System.Serializable]
    public class FurnitureReward : LevelReward
    {
        public PlaceableObjectSO data;

        public override void GiveReward(bool fromLevelUp)
        {
            Debug.Log($"Awarding {data.placeableName}");
            GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().UnlockFurniture(data, fromLevelUp);
        }
    }

    [System.Serializable]
    public class SeedReward : LevelReward
    {
        public CropSO data;

        public override void GiveReward(bool fromLevelUp)
        {
            Debug.Log($"Awarding {data.cropName}");
            GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().UnlockCrop(data, fromLevelUp);
        }
    }

    [System.Serializable]
    public class SlimeReward : LevelReward
    {
        public string data;

        public override void GiveReward(bool fromLevelUp)
        {
            Debug.Log($"Awarding {data}");
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<HabitatControl>().UnlockSlime(data, fromLevelUp);
        }
    }

    //================ SAVE / LOAD ================
    public void LoadData(GameData data)
    {
        // Add all unlocks to Inventory lists
        foreach (LevelReward reward in levelRewards)
        {
            if (reward.levelEarned <= data.level)
                reward.GiveReward(false);
        }
    }

    public void SaveData(GameData data)
    {
        //Nothing needed
    }
}
