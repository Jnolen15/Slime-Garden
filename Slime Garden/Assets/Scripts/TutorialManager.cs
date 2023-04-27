using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TutorialManager : SerializedMonoBehaviour
{
    [SerializeField] private DialogueManager dlogManager;
    [SerializeField] private string tutChapter;

    public List<TutorialEntry> tutList = new List<TutorialEntry>();

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
    }

    void Start()
    {
        FindAndPlay("intro");
    }

    private void Update()
    {
        // Custom behavior based on tutorial stage
        switch (tutChapter)
        {
            case "":
                return;
        }
    }

    // Find tutorial with given key and activate the dialogue manager
    public void FindAndPlay(string key)
    {
        foreach (TutorialEntry entry in tutList)
        {
            if(entry.key == key)
            {
                dlogManager.TypeDialogue(entry.dialogue, entry.npc.ToString());
                tutChapter = key;
                return;
            }
        }

        tutChapter = "";
        Debug.Log("Unable to find tutorial entry of key " + key);
        return;
    }
}
