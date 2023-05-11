using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class TutorialManager : SerializedMonoBehaviour, IDataPersistence
{
    [SerializeField] private DialogueManager dlogManager;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private HabitatControl habitat;
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerData pData;
    [SerializeField] private int introTutProgress;
    [SerializeField] private List<string> introTutKeys = new List<string>();
    public List<TutorialEntry> tutList = new List<TutorialEntry>();
    [SerializeField] private TutorialEntry currentTutEntry;

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
            if(currentTutEntry.key == "Intro")
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
            else if (currentTutEntry.key == "WildZoneReturn")
            {
                pData.GainMoney(100);
                dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
            }
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
        {
            // Set tutorial quest
            switch (currentTutEntry.key)
            {
                case "SlimeBasics2":
                    SetQuestText(true, "Inspect a slime with [Right Click].");
                    return;
                case "BuildMode":
                    SetQuestText(true, "Enter build mode by clicking the hammer icon in the toolbar.");
                    return;
                case "BuildMode2":
                    SetQuestText(true, "Place the splicer, then return to inspect mode. The glove icon in the toolbar.");
                    return;
                case "PostSplice":
                    SetQuestText(true, "Create a new slime using the splicer.");
                    return;
                case "Planting1":
                    SetQuestText(true, "Create a few more new slimes.");
                    return;
                case "Planting2":
                    SetQuestText(true, "Place down a few farm tiles in build mode. Then switch to planting mode.");
                    return;
                case "Planting3":
                    SetQuestText(true, "Plant Snackroot seeds then return to inspect mode.");
                    return;
                case "Planting4":
                    SetQuestText(true, "Water your crops. Then wait for them to grow and harvest!");
                    return;
                case "WildZone":
                    SetQuestText(true, "Plant and harvest at least 4 Mondo Mellons.");
                    return;
                default:
                    Debug.LogWarning("Default case, no quest");
                    return;
            }
        } else
            SetQuestText(false);

        // USed to trigger specific events before the next entry
        switch (currentTutEntry.key)
        {
            case "SlimeBasics":
                HabitatControl.SlimeDataEntry slimeOne = new HabitatControl.SlimeDataEntry(new Vector3(2, 0, 0), "Null", "Sapphire", "Sapphire", 0, 100, "Sapphire Slime");
                HabitatControl.SlimeDataEntry slimeTwo = new HabitatControl.SlimeDataEntry(new Vector3(-2, 0, 0), "Null", "Ruby", "Ruby", 0, 100, "Ruby Slime");
                habitat.ConstructSlime(slimeOne);
                habitat.ConstructSlime(slimeTwo);
                break;
            case "FirstLevelUp":
                pData.GainExperience(5);
                break;
            case "BuildMode":
                pData.GainMoney(50);
                break;
            case "BuildMode2":
                pData.GainMoney(4);
                break;
            case "PostSplice":
                pData.GainMoney(10);
                break;
            case "Planting1":
                pData.GainMoney(20);
                break;
            case "Planting2":
                pData.GainMoney(20);
                break;
            case "Planting4":
                pData.GainMoney(40);
                break;
            default:
                Debug.Log("Default case, progressing");
                break;
        }

        dlogManager.TypeDialogue(currentTutEntry.dialogue, currentTutEntry.npc.ToString(), () => ProgressIntro(true));
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