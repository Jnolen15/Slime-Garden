using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private GameObject taskPref;
    [SerializeField] private GameObject rewardPref;
    [SerializeField] private Transform taskGroup;
    [SerializeField] private Transform rewards;
    [SerializeField] private Transform rewardsZone;
    private int maxTasks = 3;
    [SerializeField] private List<TaskSO> allTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> queuedTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> inProgressTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> finishedTasks = new List<TaskSO>();

    private StatTracker statTracker;
    private InventoryManager inventoryManager;

    void Start()
    {
        statTracker = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();
        inventoryManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();

        taskGroup.gameObject.SetActive(true);
        rewards.gameObject.SetActive(false);

        AddTasksToBoard(inProgressTasks);
    }

    public void AddTasksToBoard(List<TaskSO> tasksToAdd)
    {
        // Show current tasks on board
        foreach (TaskSO task in tasksToAdd)
        {
            var taskElement = Instantiate(taskPref, taskGroup);
            taskElement.GetComponent<TaskElement>().Setup(task);
        }

        UpdateTasks();
    }

    public void AddQueuedTasks()
    {
        int numTaks = maxTasks - inProgressTasks.Count;

        // Add queued tasks to board, as long as there is room (3 max)
        for(int i = 0; i < numTaks; i++)
        {
            if (queuedTasks.Count == 0)
                break;

            var task = queuedTasks[0];

            queuedTasks.Remove(task);
            inProgressTasks.Add(task);

            var taskElement = Instantiate(taskPref, taskGroup);
            taskElement.GetComponent<TaskElement>().Setup(task);

            // if offset task, save offset
            if(task.offsetTask)
                statTracker.SetTaskOffset(task.id, statTracker.GetStat(task.stat));
        }

        UpdateTasks();
    }

    public void UpdateTasks()
    {
        foreach (Transform taskUI in taskGroup)
        {
            var taskElement = taskUI.GetComponent<TaskElement>();

            int progress = statTracker.GetStat(taskElement.task.stat);
            if (progress != -1)
            {
                int offset = statTracker.GetTaskOffset(taskElement.task.id);

                if (!taskElement.task.offsetTask)
                    taskElement.UpdateGoal(progress);
                else if(offset != -1)
                    taskElement.UpdateGoal(progress, offset);
                else
                    Debug.LogError("Task Offset not found");
            }
            else
                Debug.LogWarning("Progress returned -1");
        }
    }

    public bool CheckTaskCompletion(TaskSO taskData)
    {
        // offset task
        if (taskData.offsetTask)
        {
            int stat = statTracker.GetStat(taskData.stat);
            int offset = statTracker.GetTaskOffset(taskData.id);

            if (offset == -1)
                Debug.LogError("Task Offset not found");

            if (stat >= (taskData.goal + offset))
                return true;
        }
        // Lifetime goal task
        else
        {
            int stat = statTracker.GetStat(taskData.stat);
            if (stat >= taskData.goal)
                return true;
        }

        return false;
    }

    public void CompleteTask(GameObject taskUI, TaskSO taskData)
    {
        if (CheckTaskCompletion(taskData))
        {
            Debug.Log("Task Complete!");

            // Move task to complete
            Destroy(taskUI);
            inProgressTasks.Remove(taskData);
            finishedTasks.Add(taskData);

            // Reward player
            taskGroup.gameObject.SetActive(false);
            rewards.gameObject.SetActive(true);
            DisplayRewards(taskData);
            GrantRewards(taskData);

            // Add new Tasks
            foreach (TaskSO newTask in taskData.unlockedTasks)
                queuedTasks.Add(newTask);

            AddQueuedTasks();
        }
        else
        {
            Debug.Log("Task not complete yet!");
        }
    }

    public void DisplayRewards(TaskSO completedTask)
    {
        // Get rid of previous level up reward notifs
        foreach (Transform child in rewardsZone)
        {
            Destroy(child.gameObject);
        }

        // Display new award notifs
        foreach (TaskSO.RewardEntry reward in completedTask.rewards)
        {
            Transform box = Instantiate(rewardPref, rewardsZone).transform;
            box.GetComponentInChildren<TextMeshProUGUI>().text = reward.rewardName;
            box.GetChild(0).GetComponent<Image>().sprite = reward.rewardImage;
        }
    }

    public void GrantRewards(TaskSO completedTask)
    {
        foreach (TaskSO.RewardEntry reward in completedTask.rewards)
        {
            if(reward.rewardType == TaskSO.RewardType.crop)
            {
                inventoryManager.UnlockCrop(reward.cropReward, true);
            }
            else if (reward.rewardType == TaskSO.RewardType.placeable)
            {
                inventoryManager.UnlockFurniture(reward.placeableReward, true);
            }
        }
    }
}
