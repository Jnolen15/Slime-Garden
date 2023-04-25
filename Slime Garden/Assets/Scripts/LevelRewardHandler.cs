using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class LevelRewardHandler : SerializedMonoBehaviour, IDataPersistence
{
    [SerializeField] private PlayerData pData;
    [SerializeField] private Transform rewardZone;
    [SerializeField] private GameObject rewardBox;
    [SerializeField] private List<LevelReward> levelRewards = new List<LevelReward>();

    public void GrantLevelRewards(int level)
    {
        // Get rid of previous level up reward notifs
        foreach (Transform child in rewardZone)
        {
            Destroy(child.gameObject);
        }

        // Grant awards and add them to notif
        foreach (LevelReward reward in levelRewards)
        {
            if (reward.levelEarned == level)
            {
                DisplayReward(reward);
                reward.GiveReward(true);
            }
        }
    }

    private void DisplayReward(LevelReward reward)
    {
        Transform box = Instantiate(rewardBox, rewardZone).transform;
        box.GetComponentInChildren<TextMeshProUGUI>().text = reward.rewardName;
        box.GetChild(0).GetComponent<Image>().sprite = reward.rewardImage;
    }

    // ================ LEVEL REWARDS ================
    [System.Serializable]
    public abstract class LevelReward
    {
        public int levelEarned;
        public string rewardName;
        public Sprite rewardImage;
        public abstract void GiveReward(bool fromLevelUp);
    }

    [System.Serializable]
    public class FurnitureReward : LevelReward
    {
        public PlaceableObjectSO data;

        public override void GiveReward(bool fromLevelUp)
        {
            Debug.Log($"Awarding {data.placeableName}");
            GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>().UnlockFurniture(data, fromLevelUp);
        }
    }

    [System.Serializable]
    public class SeedReward : LevelReward
    {
        public CropSO data;

        public override void GiveReward(bool fromLevelUp)
        {
            Debug.Log($"Awarding {data.cropName}");
            GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>().UnlockCrop(data, fromLevelUp);
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
