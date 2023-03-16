using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour, IDataPersistence
{

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
        Money = data.money;
    }

    public void SaveData(GameData data)
    {
        data.money = Money;
    }
}
