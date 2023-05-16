using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OtherSOs/Task")]
public class TaskSO : ScriptableObject
{
    public enum RewardType
    {
        slime,
        crop,
        placeable
    }

    [Header("Task Info")]
    public string id;
    public string description;
    public string stat;
    public int goal;
    public bool offsetTask;

    [Header("Reward Info")]
    public List<RewardEntry> rewards;
    public List<TaskSO> unlockedTasks;

    // Reward Class
    [System.Serializable]
    public class RewardEntry
    {
        public RewardType rewardType;
        public string rewardName;
        public Sprite rewardImage;
    }
}
