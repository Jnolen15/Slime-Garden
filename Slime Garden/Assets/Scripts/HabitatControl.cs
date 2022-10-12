using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatControl : MonoBehaviour
{
    // SLIME DICTIONARIES ===============
    public SBaseDictionary sBaseDictionary;         //The SO of the dictionary that holds all slime Base SOs.
    public SPatternDictionary sPatternDictionary;   //The SO of the dictionary that holds all slime Pattern SOs.

    public List<GameObject> activeSlimes;           //List of all active slimes

    void Start()
    {
        foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Slime"))
        {
            activeSlimes.Add(ob);
        }
    }

    void Update()
    {
        
    }
}
