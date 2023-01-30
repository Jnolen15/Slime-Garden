using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    [SerializeField] private GameObject defaultCrop;
    private Transform cropSpot;
    public GameObject curCrop;
    public CropSO curCropSO;

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
}
