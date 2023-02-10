using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    private Transform cropSprite;
    private Transform model;
    private GardenManager gm;
    private PlayerController pc;

    [SerializeField] private Material dryMat;
    [SerializeField] private Material wetMat;

    public CropSO curCropSO;
    public bool hasCrop;
    public bool fullyGrown;
    public int curTick;
    public int wateredTicks;
    public int growthStage;

    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GardenManager>();
        gm.AddToList(this);

        cropSprite = transform.GetChild(0);
        model = transform.GetChild(1);
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
        if (!hasCrop || fullyGrown)
            return;

        Debug.Log("Growth Tick");

        if (wateredTicks > 0)
        {
            curTick++;
            wateredTicks--;

            // Crop progresses growth stage
            if (curTick == curCropSO.growTicks[growthStage])
            {
                Debug.Log(curCropSO.cropName + " has grown!");
                growthStage++;
                cropSprite.GetComponent<SpriteRenderer>().sprite = curCropSO.spriteStages[growthStage];
            }

            // Crop is fully grown
            if (growthStage > (curCropSO.growTicks.Length - 1))
            {
                Debug.Log(curCropSO.cropName + " is fully grown!");
                fullyGrown = true;
            }
        } else
        {
            Debug.Log("Crop was not watered, could not grow!");
        }

        if(wateredTicks == 0)
        {
            model.GetComponent<Renderer>().material = dryMat;
        }
    }

    // Called by clicked on in default player state
    public void Interact()
    {
        // IF no plant nothing
        if (!hasCrop)
        {
            Debug.Log("No crop planted");
            return;
        }

        // If not fully grown plant
        if (!fullyGrown)
        {
            // -> If not watered water
            if (wateredTicks <= 0)
            {
                wateredTicks += 2;
                model.GetComponent<Renderer>().material = wetMat;
                Debug.Log("Crop watered!");
            }
            // -> If watered then nothing
            else Debug.Log("crop already watered. Water left: " + wateredTicks);
        }
        // If fully grown harvest
        else
            Harvest();
    }

    private void Harvest()
    {
        Debug.Log("Crop harvested!");
        // Yeild
        pc.Money += curCropSO.sellValue;

        // Reset
        hasCrop = false;
        fullyGrown = false;
        curTick = 0;
        growthStage = 0;
        curCropSO = null;
        cropSprite.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void DestroySelf()
    {
        Debug.Log("Removing self from list");
        gm.RemoveFromList(this);
    }
}
