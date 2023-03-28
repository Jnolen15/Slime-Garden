using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpot : MonoBehaviour
{
    private Transform crop;
    private Transform model;
    private GardenManager gm;
    private InventoryManager invManager;

    [SerializeField] private Material dryMat;
    [SerializeField] private Material wetMat;
    [SerializeField] private GameObject waterFX;
    [SerializeField] private GameObject LeafBurstFX;
    [SerializeField] private GameObject DirtBurstFX;

    public CropSO curCropSO;
    public bool hasCrop;
    public bool fullyGrown;
    public int curTick;
    public int wateredTicks;
    public int growthStage;

    private void Awake()
    {
        invManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
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

    public void SetPlant(CropSO crop, bool isGrown, int newCurTick, int newWateredTicks, int newGrowthStage)
    {
        if (!crop)
        {
            Debug.Log("SetPlant was given a null crop SO");
            return;
        }
        
        curCropSO = crop;
        fullyGrown = isGrown;
        hasCrop = true;
        curTick = newCurTick;
        wateredTicks = newWateredTicks;
        growthStage = newGrowthStage;

        this.crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[growthStage];
        this.crop.GetComponent<Renderer>().material = curCropSO.cropMat;

        if(wateredTicks > 0)
            model.GetComponent<Renderer>().material = wetMat;
    }

    public void Plant(CropSO cropData)
    {
        curCropSO = cropData;
        this.crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[0];
        this.crop.GetComponent<Renderer>().material = curCropSO.cropMat;
        hasCrop = true;

        // Effect
        Vector3 pos = new Vector3(crop.position.x, crop.position.y + 0.1f, crop.position.z);
        Instantiate(DirtBurstFX, pos, Quaternion.identity);
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
                Water();
            // -> If watered then nothing
            else Debug.Log("crop already watered. Water left: " + wateredTicks);
        }
        // If fully grown harvest
        else
            Harvest();
    }

    private void Water()
    {
        wateredTicks += 2;
        StartCoroutine(AnimateWater());
    }

    IEnumerator AnimateWater()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        Instantiate(waterFX, pos, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        model.GetComponent<Renderer>().material = wetMat;
        Debug.Log("Crop watered!");
    }

    private void Harvest()
    {
        Debug.Log("Crop harvested!");
        // Yeild
        invManager.AddCrop(curCropSO, 1);

        // Effect
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Instantiate(LeafBurstFX, pos, Quaternion.identity);

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
