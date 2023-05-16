using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatTracker : MonoBehaviour
{
    [Header("CS stats")]
    [SerializeField] private int csCollected;   // CS crystals picked up
    [SerializeField] private int csEarned;      // Money earned by the player from all sources

    [Header("Task offsets")]
    [SerializeField]
    private List<KeyValuePair<string, int>> taskOffsets = new List<KeyValuePair<string, int>>();

    public void IncrementStat(string statName, int ammount)
    {
        Debug.Log($"Incrementing stat {statName} by {ammount}");

        switch (statName)
        {
            case "csCollected":
                csCollected += ammount;
                break;
            case "csEarned":
                csEarned += ammount;
                break;
            default:
                Debug.LogError("Stat not found!");
                break;
        }
    }

    public int GetStat(string statName)
    {
        switch (statName)
        {
            case "csCollected":
                return csCollected;
            case "csEarned":
                return csEarned;
            default:
                Debug.LogError("Stat not found!");
                return -1;
        }
    }

    public void SetTaskOffset(string taskName, int offset)
    {
        taskOffsets.Add(new KeyValuePair<string, int>(taskName, offset));
    }

    public int GetTaskOffset(string taskName)
    {
        foreach(KeyValuePair<string, int> taskOffset in taskOffsets)
        {
            if (taskOffset.Key == taskName)
                return taskOffset.Value;
        }

        return -1;
    }
}
