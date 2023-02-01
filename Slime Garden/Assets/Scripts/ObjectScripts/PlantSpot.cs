using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    private Transform cropSpot;
    private Transform cropSprite;
    private GardenManager gm;

    public CropSO curCropSO;
    public bool hasCrop;
    public bool fullyGrown;
    public int curTick;
    public int growthStage;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GardenManager>();
        gm.AddToList(this);

        cropSpot = transform.GetChild(0);
        cropSprite = transform.GetChild(1);
    }

    public CropSO GetCropSO()
    {
        return curCropSO;
    }

    public void Plant(CropSO crop)
    {
        curCropSO = crop;
        cropSprite.GetComponent<SpriteRenderer>().sprite = curCropSO.spriteStages[0];
        hasCrop = true;
    }

    public void GrowTick()
    {
        Debug.Log("Growth Tick");

        if (hasCrop && !fullyGrown)
        {
            curTick++;

            // Crop progresses growth stage
            if(curTick == curCropSO.growTicks[growthStage])
            {
                Debug.Log(curCropSO.cropName + " has grown!");
                growthStage++;
                cropSprite.GetComponent<SpriteRenderer>().sprite = curCropSO.spriteStages[growthStage];
            }

            // Crop is fully grown
            if(growthStage > (curCropSO.growTicks.Length - 1))
            {
                Debug.Log(curCropSO.cropName + " is fully grown!");
                fullyGrown = true;
            }
        }
    }

    public void DestroySelf()
    {
        Debug.Log("Removing self from list");
        gm.RemoveFromList(this);
    }
}
