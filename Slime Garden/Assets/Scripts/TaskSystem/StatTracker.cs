using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatTracker : MonoBehaviour, IDataPersistence
{
    [Header("Game stats")] // CONVERT TO A KEY VALUE LIST
    [SerializeField] private List<StatDataEnrty> gameStats = new List<StatDataEnrty>();

    [Header("Task offsets")]
    [SerializeField] private List<StatDataEnrty> taskOffsets = new List<StatDataEnrty>();

    [System.Serializable]
    public class StatDataEnrty
    {
        public string id;
        public int value;

        public StatDataEnrty(string newID, int newValue)
        {
            id = newID;
            value = newValue;
        }
    }

    public void IncrementStat(string statName, int ammount)
    {
        Debug.Log($"Incrementing stat {statName} by {ammount}");

        foreach (StatDataEnrty statEntry in gameStats)
        {
            if (statEntry.id == statName)
            {
                statEntry.value += ammount;
                return;
            }
        }

        Debug.LogError(statName + " not found in gameStats list!");
    }

    public void SetStat(string statName, int ammount)
    {
        Debug.Log($"Setting stat {statName}");

        foreach (StatDataEnrty statEntry in gameStats)
        {
            if (statEntry.id == statName)
            {
                statEntry.value = ammount;
                return;
            }
        }

        Debug.LogError(statName + " not found in gameStats list!");
    }

    public int GetStat(string statName)
    {
        foreach (StatDataEnrty statEntry in gameStats)
        {
            if (statEntry.id == statName)
                return statEntry.value;
        }

        return -1;
    }

    public void SetTaskOffset(string taskName, int offset)
    {
        taskOffsets.Add(new StatDataEnrty(taskName, offset));
    }

    public int GetTaskOffset(string taskName)
    {
        foreach(StatDataEnrty taskOffset in taskOffsets)
        {
            if (taskOffset.id == taskName)
                return taskOffset.value;
        }

        return -1;
    }

    // ================== FOR TESTING ONLY ==================
    public void IncrementAllStats()
    {
        foreach (StatDataEnrty statEntry in gameStats)
        {
            statEntry.value += 10;
        }
    }

    // ==================== SAVE / LOAD ====================
    public void LoadData(GameData data)
    {
        if(gameStats.Count <= data.gameStats.Count)
            gameStats = data.gameStats;

        taskOffsets = data.taskOffsets;
    }

    public void SaveData(GameData data)
    {
        data.gameStats = gameStats;
        data.taskOffsets = taskOffsets;
    }
}
