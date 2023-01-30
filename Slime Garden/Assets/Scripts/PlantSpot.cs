using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    public CropSO curCrop;

    // Update is called once per frame
    void Update()
    {
        
    }

    public CropSO GetCrop()
    {
        return curCrop;
    }

    public void Plant(CropSO crop)
    {
        curCrop = crop;
    }
}
