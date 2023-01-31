using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    [SerializeField] private GameObject defaultCrop;
    private Transform cropSpot;
    public GameObject curCrop;
    public CropSO curCropSO;
    private GardenManager gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GardenManager>();
        gm.AddToList(this);
    }

    void Update()
    {
        cropSpot = transform.GetChild(0);
    }

    public CropSO GetCropSO()
    {
        return curCropSO;
    }

    public void Plant(CropSO crop)
    {
        curCropSO = crop;
        curCrop = Instantiate(defaultCrop, cropSpot.position, cropSpot.rotation, transform);
        curCrop.GetComponent<SpriteRenderer>().sprite = curCropSO.spriteStages[0];
    }

    public void GrowTick()
    {
        Debug.Log("Growth Tick");
    }

    public void DestroySelf()
    {
        Debug.Log("Removing self from list");
        gm.RemoveFromList(this);
    }
}
