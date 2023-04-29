using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIToken : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;

    public void Setup(Vector3 destination, Color color)
    {
        this.GetComponent<Image>().color = color;

        this.GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);

        this.transform.DOMove(destination, 1f).SetEase(Ease.InCirc).OnComplete(Kill);
    }

    public void Kill()
    {
        this.transform.DOKill();
        Destroy(gameObject);
    }
}
