using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerData : MonoBehaviour, IDataPersistence
{
    [SerializeField] LevelRewardHandler levelRewards;
    [SerializeField] MenuManager menus;

    // ===================== LEVEL / XP STUFF =====================
    [SerializeField] private List<int> levelThresholds = new List<int>();
    public TextMeshProUGUI levelDisplay;
    public GameObject levelUpScreen;
    public Image expBarFill;
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

    [SerializeField] private float experience = 0;
    public float Experience
    {
        get { return experience; }
        set
        {
            experience = value;

            // Set XP bar fill
            SetEXPBar();

            // Check for level up
            var leveledUp = CheckForLevelUp(experience);
            if (leveledUp)
                IncrementLevel();
        }
    }

    private void SetEXPBar()
    {
        if (GetLevel() == 0)
            expBarFill.fillAmount = (Experience / (float)levelThresholds[GetLevel()]);
        else if (GetLevel() < levelThresholds.Count)
            expBarFill.fillAmount = ((Experience - levelThresholds[GetLevel() - 1]) /
                                    (float)(levelThresholds[GetLevel()] - levelThresholds[GetLevel() - 1]));
        else
            expBarFill.fillAmount = 1;
    }

    public void GainExperience(float value)
    {
        Experience += value;
        Debug.Log($"Earned {value} EXP");
    }

    public void IncrementLevel()
    {
        Debug.Log("LEVELED UP");
        Level++;

        levelRewards.GrantLevelRewards(Level);
        levelRewards.DisplayAllRewards(Level);

        levelUpScreen.SetActive(true);

        SetEXPBar();
    }

    public int GetLevel()
    {
        return Level;
    }

    public bool CheckForLevelUp(float curExp)
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

        menus.TextPopup("-" + cost, Color.red);
    }

    public void GainMoney(int value)
    {
        Money += value;

        menus.TextPopup("+" + value, Color.green);
    }

    // Purchases if player can afford
    public bool TryAndPurchase(int cost)
    {
        if (Money >= cost)
        {
            MakePurchase(cost);
            return true;
        }
        else
        {
            menus.TextPopup("Can't afford!", Color.red);
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
