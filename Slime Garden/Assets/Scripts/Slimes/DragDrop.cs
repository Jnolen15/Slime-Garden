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

    public void LetGo()
    {
        isHeld = false;
        sc.ChangeState(SlimeController.State.idle);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
    }

    public void SlimeHeld(Vector3 mousePos)
    {
        this.transform.position = new Vector3(mousePos.x, 0.2f, mousePos.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // border detection
        if (collision.gameObject.tag == "Border")
        {
            isHeld = false;
        }
    }
}
