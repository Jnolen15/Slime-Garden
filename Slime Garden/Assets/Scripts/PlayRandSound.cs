using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandSound : MonoBehaviour
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private AudioClip[] sounds;

    void Start()
    {
        if (playOnStart)
            RandomSound();
    }

    public void RandomSound()
    {
        this.GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
