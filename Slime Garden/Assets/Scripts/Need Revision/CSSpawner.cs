using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the script to spawn Congealed slime
public class CSSpawner : MonoBehaviour
{
    // COMPONENTS ===============
    private SBrain brain;               //The Slime Brain Script
    private Vector3 position;
    private bool gotData = false;
    [SerializeField] private float csCooldownVal = 10f;
    [SerializeField] private int numSlimes;
    [SerializeField] private float totalRarity;
    [SerializeField] private float csSpawnMod;  // The spawn rate modifier

    public GameObject CongealedSlimeDrop;
    public float csCooldown = 10f;
    public float habitatX = 9;                  //X postiton of habitat bounds
    public float habitatY = 9;                  //Y postiton of habitat bounds

    private Color Amethyst = new Color32(0x80, 0x00, 0xFF, 0xFF);
    private Color Aquamarine = new Color32(0x00, 0x00, 0xFF, 0xFF);
    private Color Bixbite = new Color32(0xFF, 0x33, 0xFF, 0xFF);
    private Color Citrine = new Color32(0xFF, 0xFF, 0x00, 0xFF);
    private Color Emerald = new Color32(0x00, 0x9D, 0x00, 0xFF);
    private Color Jade = new Color32(0x00, 0xFF, 0x80, 0xFF);
    private Color Obsidian = new Color32(0x44, 0x44, 0x44, 0xFF);
    private Color Peridot = new Color32(0x99, 0xFF, 0x33, 0xFF);
    private Color Quartz = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    private Color Ruby = new Color32(0xFF, 0x00, 0x00, 0xFF);
    private Color Sapphire = new Color32(0x00, 0x2F, 0xFF, 0xFF);
    private Color Topaz = new Color32(0xFF, 0x80, 0x00F, 0xFF);

    void Start()
    {
        // Get access to brain script
        brain = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();
    }

    void Update()
    {
        // Calculates the CS spawn rate modifier. updated each time a CS is spawned
        if (csCooldownVal <= 0) CalcMod();

        // Spawn CS at random intervals, effected by CS modifier
        SpawnCS();
    }

    // Calculates the CS spawn rate modifier
    private void CalcMod()
    {
        csSpawnMod = 0;
        totalRarity = 0;
        numSlimes = brain.activeSlimes.Count;

        // Get total rarity of slimes
        foreach (GameObject slime in brain.activeSlimes)
        {
            totalRarity += slime.gameObject.GetComponent<SlimeController>().sRarity;
        }

        if(numSlimes > 0)
        {
            int numMod = numSlimes / 10;
            int rareMod = (int)totalRarity / 50;
            float ratioMod = totalRarity / numSlimes;
            csSpawnMod = ratioMod + numMod + rareMod;
        } else
        {
            csSpawnMod = 0;
        }
        //Debug.Log("Num bonus: " + numMod + " Rare bonus: " + rareMod + " Ratio bonus: " + ratioMod + " Total: " + csSpawnMod);
    }

    // Spawn CS at random intervals, effected by CS modifier
    private void SpawnCS()
    {
        if (csCooldownVal <= 0)
        {
            // Spawn CS
            for (int i = 0; i < csSpawnMod; i++)
            {
                position = new Vector3(Random.Range(-habitatX, habitatX), Random.Range(-habitatY, habitatY), 0);
                GameObject drop = Instantiate(CongealedSlimeDrop, position, Quaternion.identity);
                PickColor(drop.GetComponent<CongealedSlime>());
            }

            // Reset cooldown
            csCooldownVal = (csCooldown + Random.Range(0, csCooldown)) - csSpawnMod;
        }
        else
        {
            csCooldownVal -= Time.deltaTime;
        }
    }

    private void PickColor(CongealedSlime drop)
    {
        int randNum = Random.Range(1, 13);
        switch (randNum)
        {
            case 1:
                drop.color = Amethyst;
                break;
            case 2:
                drop.color = Aquamarine;
                break;
            case 3:
                drop.color = Bixbite;
                break;
            case 4:
                drop.color = Citrine;
                break;
            case 5:
                drop.color = Emerald;
                break;
            case 6:
                drop.color = Jade;
                break;
            case 7:
                drop.color = Obsidian;
                break;
            case 8:
                drop.color = Peridot;
                break;
            case 9:
                drop.color = Quartz;
                break;
            case 10:
                drop.color = Ruby;
                break;
            case 11:
                drop.color = Sapphire;
                break;
            case 12:
                drop.color = Topaz;
                break;
        }
    }
}
