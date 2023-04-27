using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private float typingSpeed;

    private List<string> curText;
    [SerializeField] private int curSentence;
    [SerializeField] private bool textisOpen;
    [SerializeField] private bool isTypying;
    [SerializeField] private bool isFinished;

    [SerializeField] private AudioClip[] talkSounds;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Called by button to skip, then close text
    public void ProgressText()
    {
        // skip text
        if (isTypying)
        {
            StopAllCoroutines();
            bodyText.text = curText[curSentence];
            isFinished = true;
            isTypying = false;

            Vocalize();
        }
        else if (isFinished)
        {
            // If no more sentences, close
            if (curSentence >= curText.Count -1)
            {
                ToggleDialogue(false);
                bodyText.text = "";
            }
            // Display next sentence
            else
            {
                isFinished = false;
                curSentence ++;
                StartCoroutine(TypeLineCharacters(curText[curSentence]));
            }
        }
    }

    // Toggles the dialogue open and closed
    public void ToggleDialogue(bool toggle)
    {
        textisOpen = toggle;
        DialogueBox.SetActive(toggle);

        if (!toggle)
        {
            curText.Clear();
            bodyText.text = "";
            isTypying = false;
            isFinished = false;
        }
    }

    // Refrenced by other scripts to type text
    public void TypeDialogue(List<string> sentences, string name)
    {
        if (!textisOpen)
            ToggleDialogue(true);

        if (!isTypying)
        {
            curSentence = 0;
            curText = sentences;
            nameText.text = name;
            StartCoroutine(TypeLineCharacters(curText[curSentence]));
        }
    }

    // Animate text
    IEnumerator TypeLineCharacters(string line)
    {
        isTypying = true;

        bodyText.text = "";
        int charCount = 0;

        char[] letterArray = line.ToCharArray();

        for (int i = 0; i < line.Length; i++)
        {
            bodyText.text += letterArray[i];

            if (0 == charCount % 6)
                Vocalize();

            charCount++;

            yield return new WaitForSecondsRealtime(typingSpeed);

            //i = line.Length;
        }

        isTypying = false;
        isFinished = true;
    }

    // Plays a dialogue noise
    private void Vocalize()
    {
        if (talkSounds.Length == 0)
            return;

        var randSound = Random.Range(0, talkSounds.Length);
        audioSource.PlayOneShot(talkSounds[randSound]);
    }
}
