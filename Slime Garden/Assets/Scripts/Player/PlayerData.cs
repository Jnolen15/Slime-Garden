using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour, IDataPersistence
{
    [SerializeField] LevelRewardHandler levelRewards;

    // ===================== LEVEL / XP STUFF =====================
    [SerializeField] private List<int> levelThresholds = new List<int>();
    public TextMeshProUGUI levelDisplay;
    [SerializeField] private int level = 0;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            levelDisplay.text = level.ToString();
        }
    }

    [SerializeField] private int experience = 0;
    public int Experience
    {
        get { return experience; }
        set
        {
            experience = value;
            var leveledUp = CheckForLevelUp(experience);

            if (leveledUp)
                IncrementLevel();
        }
    }

    public void GainExperience(int value)
    {
        Experience += value;
    }

    public void IncrementLevel()
    {
        Debug.Log("LEVELED UP");
        Level++;
        levelRewards.GrantLevelRewards(Level);
    }

    public int GetLevel()
    {
        return Level;
    }

    public bool CheckForLevelUp(int curExp)
    {
        if (GetLevel() >= levelThresholds.Count)
            return false;

        if (curExp >= levelThresholds[GetLevel()])
        {
            return true;
        }

        return false;
    }

    // ===================== MONEY STUFF =====================
    public TextMeshProUGUI csDisplay;
    [SerializeField] private int money = 0;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            csDisplay.text = money.ToString();
        }
    }

    public bool CanAfford(int cost)
    {
        if (Money >= cost)
            return true;

        return false;
    }

    public void MakePurchase(int cost)
    {
        Money -= cost;
    }

    public void GainMoney(int value)
    {
        Money += value;
    }

    // Purchases if player can afford
    public bool TryAndPurchase(int cost)
    {
        if (Money >= cost)
        {
            //Debug.Log("Purchased!");
            Money -= cost;
            return true;
        }
        else
        {
            //Debug.Log("Cannot afford!");
            return false;
        }
    }


    // ===================== SAVE AND LOAD =====================
    public void LoadData(GameData data)
    {
        Level = data.level;
        Experience = data.experience;
        Money = data.money;
    }

    public void SaveData(GameData data)
    {
        data.level = Level;
        data.experience = Experience;
        data.money = Money;
    }
}
