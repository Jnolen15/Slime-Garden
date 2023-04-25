using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropObj : MonoBehaviour, IInteractable
{
    public PlayerController pc;
    public InventoryManager inv;
    public CropSO cropData;
    public GameObject particles;

    public void Setup(CropSO data)
    {
        cropData = data;

        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inv = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();
    }

    public void DestroySelf()
    {
        Instantiate(particles,transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void Interact()
    {
        if (inv == null)
        {
            Debug.LogError("Crop not setup!");
            return;
        }

        inv.AddCrop(cropData, 1);
        Destroy(gameObject);
    }
}
