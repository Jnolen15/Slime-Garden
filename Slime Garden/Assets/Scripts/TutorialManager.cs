using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TutorialManager : SerializedMonoBehaviour, IDataPersistence
{
    [SerializeField] private DialogueManager dlogManager;
    [SerializeField] private HabitatControl habitat;
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerData pData;
    [SerializeField] private int introTutProgress;
    [SerializeField] private List<string> introTutKeys = new List<string>();
    public List<TutorialEntry> tutList = new List<TutorialEntry>();
    private TutorialEntry currentTutEntry;

    private bool inspectedSlime;

    [System.Serializable]
    public class TutorialEntry
    {
        public string key;
        [TextArea()]
        public List<string> dialogue;
        public enum NPC
        {
            tutorial,
            farmer,
            upgrader
        }
        public NPC npc;
        public bool taskCompleted;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();

        if(introTutProgress < introTutKeys.Count)
        {
            currentTutEntry = FindTutEntry(introTutKeys[introTutProgress]);
            dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
        }
    }

    // Check for tutorial task completion
    private void Update()
    {
        if (introTutProgress >= introTutKeys.Count)
            return;

        if (currentTutEntry.taskCompleted)
            return;

        if(currentTutEntry.key == "SlimeBasics2")
        {
            if (!inspectedSlime && player.state == PlayerController.State.Inspect)
                inspectedSlime = true;
            else if (inspectedSlime && player.state == PlayerController.State.Default)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        } else if (currentTutEntry.key == "BuildMode")
        {
            if (!currentTutEntry.taskCompleted && player.state == PlayerController.State.Build)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "BuildMode2")
        {
            if (!currentTutEntry.taskCompleted && player.state == PlayerController.State.Default)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "PostSplice")
        {
            if (!currentTutEntry.taskCompleted && habitat.activeSlimes.Count >= 3)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "Planting1")
        {
            if (!currentTutEntry.taskCompleted && pData.GetLevel() >= 2)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "Planting2")
        {
            if (!currentTutEntry.taskCompleted && player.state == PlayerController.State.Plant)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "Planting3")
        {
            if (!currentTutEntry.taskCompleted && player.state == PlayerController.State.Default)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "Planting4")
        {
            if (!currentTutEntry.taskCompleted && pData.GetLevel() >= 3)
            {
                currentTutEntry.taskCompleted = true;
                ProgressIntro(false);
            }
        }
        else if (currentTutEntry.key == "WildZone")
        {
            if (!currentTutEntry.taskCompleted && pData.GetLevel() >= 4)
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
            return;

        // USed to trigger specific events before the next entry
        switch (currentTutEntry.key)
        {
            case "SlimeBasics":
                HabitatControl.SlimeDataEntry slimeOne = new HabitatControl.SlimeDataEntry(new Vector3(2, 0, 0), "Null", "Sapphire", "Sapphire", "Sapphire Slime");
                HabitatControl.SlimeDataEntry slimeTwo = new HabitatControl.SlimeDataEntry(new Vector3(-2, 0, 0), "Null", "Ruby", "Ruby", "Ruby Slime");
                habitat.ConstructSlime(slimeOne);
                habitat.ConstructSlime(slimeTwo);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "FirstLevelUp":
                pData.GainExperience(5);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "BuildMode":
                pData.GainMoney(50);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "BuildMode2":
                pData.GainMoney(4);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "PostSplice":
                pData.GainMoney(10);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "Planting1":
                pData.GainMoney(20);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "Planting2":
                pData.GainMoney(20);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "Planting4":
                pData.GainMoney(40);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            case "WildZoneReturn":
                pData.GainMoney(100);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
            default:
                Debug.Log("Default case, progressing");
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
                return;
        }
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