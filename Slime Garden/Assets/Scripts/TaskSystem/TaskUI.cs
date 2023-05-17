using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskPref;
    [SerializeField] private GameObject rewardPref;
    [SerializeField] private Transform taskGroup;
    [SerializeField] private Transform rewards;
    [SerializeField] private Transform rewardsZone;

    [SerializeField] private TaskManager taskManager;
    private StatTracker statTracker;
    private bool setup = false;

    void OnEnable()
    {
        if (setup)
            return;

        statTracker = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();

        taskGroup.gameObject.SetActive(true);
        rewards.gameObject.SetActive(false);

        setup = true;
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

    public void UpdateTasks()
    {
        if (statTracker == null)
            return;

        foreach (Transform taskUI in taskGroup)
        {
            var taskElement = taskUI.GetComponent<TaskElement>();

            int progress = statTracker.GetStat(taskElement.task.stat);
            if (progress != -1)
            {
                int offset = statTracker.GetTaskOffset(taskElement.task.id);

                if (!taskElement.task.offsetTask)
                    taskElement.UpdateGoal(progress);
                else if (offset != -1)
                    taskElement.UpdateGoal(progress, offset);
                else
                    Debug.LogError("Task Offset not found");
            }
            else
                Debug.LogWarning("Progress returned -1");
        }
    }

    public void CompleteTask(GameObject taskUI, TaskSO taskData)
    {
        if (taskManager.CompleteTask(taskData))
        {
            Destroy(taskUI);
        }
    }

    public void DisplayRewards(TaskSO completedTask)
    {
        taskGroup.gameObject.SetActive(false);
        rewards.gameObject.SetActive(true);

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
}
