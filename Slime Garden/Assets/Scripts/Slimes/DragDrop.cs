using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public SlimeController sc;
    public bool isHeld = false;

    private AudioSource audioSrc;
    [SerializeField] private AudioClip[] jumpSounds;

    private void Start()
    {
        audioSrc = this.GetComponent<AudioSource>();
    }

    public void PickedUp()
    {
        isHeld = true;
        sc.ChangeState(SlimeController.State.held);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
    }

    public void LetGo(Vector3 pos)
    {
        isHeld = false;
        sc.ChangeState(SlimeController.State.idle);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);

        this.transform.position = pos;
    }

    public void LetGo()
    {
        isHeld = false;
        sc.ChangeState(SlimeController.State.idle);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
    }
}
