using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskElement : MonoBehaviour
{
    public TaskSO task;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI goal;

    public void Setup(TaskSO newTask)
    {
        task = newTask;
        description.text = task.description;
        goal.text = ($"0/{task.goal}");
    }

    public void UpdateGoal(int progress, int offset = 0)
    {
        // offset task
        if (task.offsetTask)
            progress -= offset;

        if (progress >= task.goal)
        {
            goal.text = ($"{task.goal}/{task.goal}");
        } else
        {
            goal.text = ($"{progress}/{task.goal}");
        }
    }

    public void OnPress()
    {
        this.GetComponentInParent<TaskUI>().CompleteTask(gameObject, task);
    }
}
