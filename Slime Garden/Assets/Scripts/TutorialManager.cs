using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class TutorialManager : SerializedMonoBehaviour, IDataPersistence
{
    [SerializeField] private DialogueManager dlogManager;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private HabitatControl habitat;
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerData pData;
    [SerializeField] private List<string> introTutKeys = new List<string>();
    public List<TutorialEntry> tutList = new List<TutorialEntry>();
    [SerializeField] private List<TaskSO> startingTasks = new List<TaskSO>();
    private TutorialEntry currentTutEntry;
    private int introTutProgress;

    private bool inspectedSlime;

    [System.Serializable]
    public class TutorialEntry
    {
        public string key;
        [TextArea()]
        public List<string> dialogue;
        public bool taskCompleted;
    }


    // This system is more complicated than it needs to be ATM
    // but thats because this was how the entire tutorial operated at first
    // Now it is mostly seperated into Task board missions
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();

        if(introTutProgress < introTutKeys.Count)
        {
            currentTutEntry = FindTutEntry(introTutKeys[introTutProgress]);
            if(currentTutEntry.key == "Intro")
                dlogManager.TypeDialogue(currentTutEntry.dialogue, "Tutorial", () => ProgressIntro(true));
        }
    }

    // Check for tutorial task completion
    private void Update()
    {
        if (introTutProgress >= introTutKeys.Count)
            return;

        if (currentTutEntry.taskCompleted)
            return;

        if(currentTutEntry.key == "Finish")
        {
            if (!inspectedSlime && player.state == PlayerController.State.Inspect)
                inspectedSlime = true;
            else if (inspectedSlime && player.state == PlayerController.State.Default)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
    }

    public TutorialEntry FindTutEntry(string key)
    {
        foreach (TutorialEntry entry in tutList)
        {
            if(entry.key == key)
                return entry;
        }

        Debug.Log("Unable to find tutorial entry of key " + key);
        return null;
    }

    // Progress tutorial / trigger events at certain stages
    public void ProgressIntro(bool increment)
    {
        if (increment)
        {
            introTutProgress++;

            if (introTutProgress >= introTutKeys.Count)
                return;

            currentTutEntry = FindTutEntry(introTutKeys[introTutProgress]);
        }

        // Don't progress if there is an uncomplete task
        if (!currentTutEntry.taskCompleted)
        {
            // Set tutorial quest
            switch (currentTutEntry.key)
            {
                case "Finish":
                    SetQuestText(true, "Inspect a slime with [Right Click].");
                    return;
            }
        } else
            SetQuestText(false);

        // Used to trigger specific events before the next entry
        switch (currentTutEntry.key)
        {
            case "Slimes":
                HabitatControl.SlimeDataEntry slimeOne = new HabitatControl.SlimeDataEntry(new Vector3(2, 0, 0), "Null", "Sapphire", "Sapphire", 0, 100, "Sapphire Slime");
                HabitatControl.SlimeDataEntry slimeTwo = new HabitatControl.SlimeDataEntry(new Vector3(-2, 0, 0), "Null", "Ruby", "Ruby", 0, 100, "Ruby Slime");
                habitat.ConstructSlime(slimeOne);
                habitat.ConstructSlime(slimeTwo);
                break;
            case "Finish":
                pData.GainExperience(10);
                pData.GainMoney(50);
                taskManager.AddTasks(startingTasks);
                break;
        }

        dlogManager.TypeDialogue(currentTutEntry.dialogue, "Tutorial", () => ProgressIntro(true));
    }

    private void SetQuestText(bool toggle, string txt = "")
    {
        questText.gameObject.SetActive(toggle);
        questText.text = txt;
    }

    // ==================== SAVE AND LOAD
    public void LoadData(GameData data)
    {
        introTutProgress = data.tutorialProgress;
    }

    public void SaveData(GameData data)
    {
        data.tutorialProgress = introTutProgress;
    }
}