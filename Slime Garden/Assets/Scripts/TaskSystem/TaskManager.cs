using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour, IDataPersistence
{
    private int maxTasks = 3;
    [SerializeField] private List<TaskSO> allTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> queuedTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> inProgressTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> finishedTasks = new List<TaskSO>();

    [SerializeField] private TaskUI taskUI;
    [SerializeField] private DialogueManager dlogManager;
    private StatTracker statTracker;
    private PlayerData pData;
    private InventoryManager inventoryManager;

    void Start()
    {
        statTracker = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inventoryManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();

        taskUI.AddTasksToBoard(inProgressTasks);
    }

    public void AddTasks(List<TaskSO> newTasks)
    {
        // Add new Tasks
        foreach (TaskSO newTask in newTasks)
            queuedTasks.Add(newTask);

        AddQueuedTasks();
    }

    public void AddQueuedTasks()
    {
        int numTaks = maxTasks - inProgressTasks.Count;
        List<TaskSO> tasksToAdd = new List<TaskSO>();

        // Add queued tasks to board, as long as there is room (3 max)
        for (int i = 0; i < numTaks; i++)
        {
            if (queuedTasks.Count == 0)
                break;

            var task = queuedTasks[0];

            queuedTasks.Remove(task);
            inProgressTasks.Add(task);

            tasksToAdd.Add(task);

            // if offset task, save offset
            if (task.offsetTask)
                statTracker.SetTaskOffset(task.id, statTracker.GetStat(task.stat));
        }

        taskUI.AddTasksToBoard(tasksToAdd);
        taskUI.UpdateTasks();
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

    public bool CompleteTask(TaskSO taskData)
    {
        if (CheckTaskCompletion(taskData))
        {
            Debug.Log("Task Complete!");

            // Move task to complete
            inProgressTasks.Remove(taskData);
            finishedTasks.Add(taskData);

            // Reward player
            taskUI.DisplayRewards(taskData);
            GrantRewards(taskData, true);

            // Display dialogue if there is any
            if (taskData.hasDialogue)
            {
                var listCopy = new List<string>(taskData.dialogue);
                dlogManager.TypeDialogue(listCopy, "Tutorial");
            }

            // Add new Tasks
            foreach (TaskSO newTask in taskData.unlockedTasks)
                queuedTasks.Add(newTask);

            AddQueuedTasks();

            return true;
        }
        else
        {
            Debug.Log("Task not complete yet!");
            return false;
        }
    }

    public void GrantRewards(TaskSO completedTask, bool newUnlock)
    {
        if(inventoryManager == null)
            inventoryManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();

        // Award CS but only on completion not on re-load
        if (newUnlock && completedTask.csReward > 0)
        {
            pData.GainMoney(completedTask.csReward);
        }

        // Award rewards
        foreach (TaskSO.RewardEntry reward in completedTask.rewards)
        {
            if(reward.rewardType == TaskSO.RewardType.crop)
            {
                inventoryManager.UnlockCrop(reward.cropReward, newUnlock);
            }
            else if (reward.rewardType == TaskSO.RewardType.placeable)
            {
                inventoryManager.UnlockFurniture(reward.placeableReward, newUnlock);
            }
        }
    }

    // ==================== SAVE / LOAD ====================
    public void LoadData(GameData data)
    {
        // Load lists
        foreach (string taskID in data.queuedTasks)
        {
            if (FindTask(taskID))
                queuedTasks.Add(FindTask(taskID));
        }

        foreach (string taskID in data.inProgressTasks)
        {
            if (FindTask(taskID))
                inProgressTasks.Add(FindTask(taskID));
        }

        foreach (string taskID in data.finishedTasks)
        {
            if (FindTask(taskID))
                finishedTasks.Add(FindTask(taskID));
        }

        // Re-Reward for completed tasks
        foreach (TaskSO taskData in finishedTasks)
        {
            GrantRewards(taskData, false);
        }
    }

    private TaskSO FindTask(string id)
    {
        foreach (TaskSO taskData in allTasks)
        {
            if (taskData.id == id)
                return taskData;
        }

        Debug.LogWarning($"{id} not found!");
        return null;
    }

    public void SaveData(GameData data)
    {
        // Clear previous data
        data.queuedTasks.Clear();
        data.inProgressTasks.Clear();
        data.finishedTasks.Clear();

        // Save Lists
        foreach (TaskSO taskData in queuedTasks)
        {
            data.queuedTasks.Add(taskData.id);
        }

        foreach (TaskSO taskData in inProgressTasks)
        {
            data.inProgressTasks.Add(taskData.id);
        }

        foreach (TaskSO taskData in finishedTasks)
        {
            data.finishedTasks.Add(taskData.id);
        }
    }
}
