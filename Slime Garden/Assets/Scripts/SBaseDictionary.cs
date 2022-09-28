using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SlimeBaseDictionary")]

public class SBaseDictionary : ScriptableObject
{
    // Serializable array that I can manually add slimes too
    [System.Serializable]
    private class SlimeBaseEntry
    {
        public string slimeBaseName;
        public SlimeBaseSO slimeBaseSO;
    }
    [SerializeField]
    private SlimeBaseEntry[] slimeBases;

    // Dictionary of data from array. A dictionary is used because it is more easily searchable
    // However, I couldn't just use a dictionary as it isn't Serializable
    private static Dictionary<string, SlimeBaseSO> slimeBaseDict = new Dictionary<string, SlimeBaseSO>();

    // Dictionary is filled from the array
    void OnEnable()
    {
        //Debug.Log("slimes.Length: " + slimes.Length);
        Debug.Log("Filling Base Dictionary");

        for (int i = 0; i < slimeBases.Length; i++)
        {
            // If Slime isn't already in the dictionary
            if(!slimeBaseDict.ContainsKey(slimeBases[i].slimeBaseName))
                slimeBaseDict.Add(slimeBases[i].slimeBaseName, slimeBases[i].slimeBaseSO);
        }

        //Debug.Log(slimeDict);
        Debug.Log("Done");
    }

    // Method that searches the array for a matching slime name and returns that slime's SO
    // Returns null if slime is not found.
    public static SlimeBaseSO GetSlime(string sName)
    {
        SlimeBaseSO foundSlime = null;
        if (slimeBaseDict.TryGetValue(sName, out foundSlime))
        {
            // Success
            //Debug.Log("Found slime: " + foundSlime);
            return foundSlime;
        }
        else
        {
            // Failure
            Debug.Log("Failed to find slime: " + sName);
            return null;
        }
    }
}
