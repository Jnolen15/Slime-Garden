using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SlimeSOs/SlimeBaseDictionary")]
public class SBaseDictionary : SerializedScriptableObject
{
    [SerializeField] private SlimeBaseSO basicSlime;

    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "@new colorListEntry()")]
    [SerializeField] private List<colorListEntry> sColorList = new List<colorListEntry>();

    public class colorListEntry
    {
        public string cName;
        public Color color;
    }

    // Dictionary enty class. Holds all necessary info
    public class SlimeBaseEntry
    {
        public string slimeBaseName;
        public Color slimeBaseColor;
        public SlimeBaseSO slimeBaseSO;
        public GameObject slimeCrystal;
        public bool isSpecial; //RN this will alwys be false. Find way to re-implement special slimes later

        // Constructor
        public SlimeBaseEntry(string name, Color pColor, SlimeBaseSO data, GameObject crystal)
        {
            slimeBaseName = name;
            slimeBaseColor = pColor;
            slimeBaseSO = data;
            slimeCrystal = crystal;
            isSpecial = false;
        }
    }

    // Dictionary of SlimeBaseEntry. Used to be easily searchable by other scripts
    private static Dictionary<string, SlimeBaseEntry> slimeBaseDict = new Dictionary<string, SlimeBaseEntry>();
    //public Dictionary<string, SlimeBaseEntry> slimeBaseDictTWO = new Dictionary<string, SlimeBaseEntry>();

    // Dictionary is filled
    void OnEnable()
    {
        Debug.Log("Filling Base Dictionary");

        // Add all slime patterns
        for (int c = 0; c < sColorList.Count; c++)
        {
            string crystal = "Crystals/" + sColorList[c].cName + "Crystal";

            slimeBaseDict.Add(sColorList[c].cName,
                    new SlimeBaseEntry(sColorList[c].cName, 
                    sColorList[c].color, basicSlime, 
                    Resources.Load<GameObject>(crystal)));
        }

        //slimeBaseDictTWO = slimeBaseDict;

        Debug.Log("Done");
    }

    // Method that searches the array for a matching slime name and returns that slime's SO
    // Returns null if slime is not found.
    public static SlimeBaseEntry GetSlime(string sName)
    {
        SlimeBaseEntry foundSlime = null;
        if (slimeBaseDict.TryGetValue(sName, out foundSlime))
        {
            // Success
            Debug.Log("Found slime: " + foundSlime.slimeBaseName);
            return foundSlime;
        }
        else
        {
            // Failure
            Debug.LogError("Failed to find slime: " + sName);
            return null;
        }
    }
}