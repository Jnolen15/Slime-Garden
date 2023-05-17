using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OtherSOs/Task")]
public class TaskSO : ScriptableObject
{
    public enum RewardType
    {
        crop,
        placeable
    }

    [Header("Task Info")]
    public string id;
    public string description;
    public string stat;
    public int goal;
    public bool offsetTask;
    public bool hasDialogue;
    [TextArea()]
    public List<string> dialogue;

    [Header("Reward Info")]
    public int csReward;
    public List<RewardEntry> rewards;
    public List<TaskSO> unlockedTasks;

    // Reward Class
    [System.Serializable]
    public class RewardEntry
    {
        public RewardType rewardType;
        public string rewardName;
        public Sprite rewardImage;
        public CropSO cropReward;
        public PlaceableObjectSO placeableReward;
    }
}
