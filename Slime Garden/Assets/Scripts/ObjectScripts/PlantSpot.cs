using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlantSpot : MonoBehaviour
{
    private Transform crop;
    private Transform model;
    private GardenManager gm;
    private InventoryManager invManager;
    private AudioSource audioSrc;

    [SerializeField] private Material dryMat;
    [SerializeField] private Material wetMat;
    [SerializeField] private GameObject waterFX;
    [SerializeField] private GameObject LeafBurstFX;
    [SerializeField] private GameObject DirtBurstFX;

    [SerializeField] private AudioClip[] waterSounds;
    [SerializeField] private AudioClip[] interactSounds;

    public CropSO curCropSO;
    public bool hasCrop;
    public bool fullyGrown;
    public int curTick;
    public int wateredTicks;
    public int growthStage;

    private void Awake()
    {
        invManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GardenManager>();
        gm.AddToList(this);

        audioSrc = this.GetComponent<AudioSource>();

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
        audioSrc.PlayOneShot(interactSounds[Random.Range(0, interactSounds.Length)]);
        Vector3 pos = new Vector3(crop.position.x, crop.position.y + 0.1f, crop.position.z);
        Instantiate(DirtBurstFX, pos, Quaternion.identity);
    }

    public void GrowTick()
    {
        if (!hasCrop || fullyGrown)
            return;

        if (wateredTicks > 0)
        {
            curTick++;
            wateredTicks--;

            // Crop progresses growth stage
            if (curTick == curCropSO.growTicks[growthStage])
            {
                growthStage++;
                StartCoroutine(AnimateGrowth());
                //crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[growthStage];
            }

            // Crop is fully grown
            if (growthStage > (curCropSO.growTicks.Length - 1))
            {
                fullyGrown = true;
            }
        } else
        {
            //Debug.Log("Crop was not watered, could not grow!");
        }

        if(wateredTicks == 0)
        {
            model.GetComponent<Renderer>().material = dryMat;
            crop.GetComponent<Renderer>().material = curCropSO.cropWiltedMat;
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
        audioSrc.PlayOneShot(waterSounds[Random.Range(0, waterSounds.Length)]);
        wateredTicks += 10;
        StartCoroutine(AnimateWater());
    }

    private void Harvest()
    {
        // Yeild. If multiYeild, then give random between that and 1
        if (curCropSO.multiYeild > 0)
        {
            int num = Random.Range(1, curCropSO.multiYeild+1); //+1 because its maxExlusive
            invManager.AddCrop(curCropSO, num);
            Debug.Log("Multi-harvest yerid " + num);
        }
        else
            invManager.AddCrop(curCropSO, 1);

        // Award EXP
        GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>().GainExperience(curCropSO.expYeild);

        // Effect
        audioSrc.PlayOneShot(interactSounds[Random.Range(0, interactSounds.Length)]);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Instantiate(LeafBurstFX, pos, Quaternion.identity);

        // Can be harvested repeatedly
        if (curCropSO.multiHarvest)
        {
            fullyGrown = false;
            growthStage--;
            curTick -= curCropSO.regrowTicks;
            crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[growthStage];
        }
        // Reset
        else
        {
            hasCrop = false;
            fullyGrown = false;
            curTick = 0;
            growthStage = 0;
            curCropSO = null;
            crop.GetComponent<MeshFilter>().mesh = null;
        }
    }

    public void DestroySelf()
    {
        gm.RemoveFromList(this);
        crop.DOKill();
    }

    IEnumerator AnimateWater()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        Instantiate(waterFX, pos, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        crop.DOPunchScale(new Vector3(0, -0.2f, 0), 0.3f);
        model.GetComponent<Renderer>().material = wetMat;
        crop.GetComponent<Renderer>().material = curCropSO.cropMat;
    }

    IEnumerator AnimateGrowth()
    {
        float rand = Random.Range(0, 2f);
        yield return new WaitForSeconds(rand);
        crop.GetComponent<MeshFilter>().mesh = curCropSO.cropStages[growthStage];
        crop.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
    }
}
