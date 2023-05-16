using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private GameObject taskPref;
    [SerializeField] private Transform taskGroup;
    private int maxTasks = 3;
    [SerializeField] private List<TaskSO> allTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> queuedTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> inProgressTasks = new List<TaskSO>();
    [SerializeField] private List<TaskSO> finishedTasks = new List<TaskSO>();

    private StatTracker statTracker;

    void Start()
    {
        statTracker = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();

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
            // TODO -> Only 3 tasks at a time, make a task Queue
            foreach (TaskSO newTask in taskData.unlockedTasks)
                queuedTasks.Add(newTask);

            AddQueuedTasks();
        }
        else
        {
            Debug.Log("Task not complete yet!");
        }
    }
}
