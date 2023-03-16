using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the script to spawn Congealed slime
public class CSSpawner : MonoBehaviour
{
    // COMPONENTS ===============
    private HabitatControl hControl;               //The Slime Brain Script
    [SerializeField] private float csCooldownVal = 10f;
    [SerializeField] private int numSlimes;
    [SerializeField] private float totalRarity;
    [SerializeField] private float csSpawnMod;  // The spawn rate modifier

    public float csCooldown = 10f;

    void Start()
    {
        // Get access to habitat script
        hControl = this.GetComponent<HabitatControl>();
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
        numSlimes = hControl.activeSlimes.Count;

        // Get total rarity of slimes
        foreach (GameObject slime in hControl.activeSlimes)
        {
            totalRarity += slime.gameObject.GetComponent<SlimeData>().sRarity;
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
                var chosenSlime = hControl.GetRandomSlime();
                chosenSlime.GetComponent<SBrain>().Crystalize();
            }

            // Reset cooldown
            csCooldownVal = (csCooldown + Random.Range(0, csCooldown)) - csSpawnMod;
        }
        else
        {
            csCooldownVal -= Time.deltaTime;
        }
    }
}
