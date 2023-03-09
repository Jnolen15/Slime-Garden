using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    private Transform crop;
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

        crop = transform.GetChild(0);
        model = transform.GetChild(1);

        // Random rotation to give crops some variety
        var randRot = Random.Range(0, 360);
        crop.transform.rotation = Quaternion.Euler(0, randRot, 0);
    }

    public CropSO GetCropSO()
    {
        return curCropSO;
    }

    public void Plant(CropSO crop)
    {
        curCropSO = crop;
        this.crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[0];
        this.crop.GetComponent<Renderer>().material = curCropSO.cropMat;
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
                crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[growthStage];
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
        crop.GetComponent<MeshFilter>().mesh = null;
    }

    public void DestroySelf()
    {
        Debug.Log("Removing self from list");
        gm.RemoveFromList(this);
    }
}
