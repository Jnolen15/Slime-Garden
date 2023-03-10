using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropObj : MonoBehaviour
{
    public PlayerController pc;
    public InventoryManager inv;
    public CropSO cropData;
    public GameObject particles;

    public void Setup(CropSO data)
    {
        cropData = data;

        var player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        inv = player.GetComponent<InventoryManager>();
    }

    private void OnMouseDown()
    {
        if(inv == null)
        {
            Debug.LogError("Crop not setup!");
            return;
        }

        if (pc.state != PlayerController.State.Default)
        {
            Debug.Log("Not in default State");
            return;
        }

        inv.AddCrop(cropData, 1);
        Destroy(gameObject);
    }

    public void DestroySelf()
    {
        Instantiate(particles,transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
