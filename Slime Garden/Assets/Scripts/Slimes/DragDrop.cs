using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragDrop : MonoBehaviour
{
    public SlimeController sc;
    public bool isHeld = false;
    private AudioSource audioSrc;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject gloveSprite;
    [SerializeField] private GameObject dustFX;

    private void Start()
    {
        audioSrc = this.GetComponent<AudioSource>();
    }

    public void PickedUp()
    {
        isHeld = true;
        sc.ChangeState(SlimeController.State.held);

        gloveSprite.SetActive(true);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
        var animateTo = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        this.transform.DOMove(animateTo, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void LetGo(Vector3 pos)
    {
        isHeld = false;
        sc.ChangeState(SlimeController.State.idle);

        gloveSprite.SetActive(false);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
        this.transform.DOKill();
        this.transform.position = pos;
        Instantiate(dustFX, transform.position, transform.rotation);
    }

    public void LetGo()
    {
        isHeld = false;
        sc.ChangeState(SlimeController.State.idle);

        gloveSprite.SetActive(false);
        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
        this.transform.DOKill();
    }
}
