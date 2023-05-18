using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCrop : MonoBehaviour, IInteractable
{
    [SerializeField] private CropSO cropSO;
    [SerializeField] private Mesh notGrown;
    [SerializeField] private Mesh harvestable;
    [SerializeField] private int harvestOdds;
    [SerializeField] private Transform crop;
    [SerializeField] private GameObject LeafBurstFX;
    [SerializeField] private AudioClip[] interactSounds;
    private bool fullyGrown;
    private InventoryManager invManager;
    private StatTracker stats;
    private AudioSource audioSrc;

    private void Awake()
    {
        invManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();
        stats = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();

        audioSrc = this.GetComponent<AudioSource>();

        // Random rotation to give crops some variety
        var randRot = Random.Range(0, 360);
        crop.transform.rotation = Quaternion.Euler(0, randRot, 0);
    }

    void Start()
    {
        // Random chance the crop will be harvestable
        var rand = Random.Range(0, 100);
        if(rand < harvestOdds)
        {
            fullyGrown = true;
            crop.GetComponent<MeshFilter>().mesh = harvestable;
        }
    }

    public void Interact()
    {
        if (!fullyGrown)
            return;
        
        // Yeild. If multiYeild, then give random between that and 1
        if (cropSO.multiYeild > 0)
        {
            int num = Random.Range(1, cropSO.multiYeild + 1); //+1 because its maxExlusive
            invManager.AddCrop(cropSO, num);
            Debug.Log("Multi-harvest yerid " + num);
        }
        else
            invManager.AddCrop(cropSO, 1);

        // Award EXP
        GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>().GainExperience(cropSO.expYeild);
        stats.IncrementStat("wildCropsHarvested", 1);

        // Effect
        audioSrc.PlayOneShot(interactSounds[Random.Range(0, interactSounds.Length)]);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Instantiate(LeafBurstFX, pos, Quaternion.identity);

        // multiharvest crop go back to unharvested model
        if (cropSO.multiHarvest)
        {
            fullyGrown = false;
            crop.GetComponent<MeshFilter>().mesh = notGrown;
        }
        // normal crop remove model
        else
        {
            fullyGrown = false;
            crop.GetComponent<MeshFilter>().mesh = null;
        }
    }
}
