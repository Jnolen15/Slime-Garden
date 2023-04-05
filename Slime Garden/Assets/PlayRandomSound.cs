using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] buildSounds;

    void Start()
    {
        this.GetComponent<AudioSource>().PlayOneShot(buildSounds[Random.Range(0, buildSounds.Length)]);
    }
}
