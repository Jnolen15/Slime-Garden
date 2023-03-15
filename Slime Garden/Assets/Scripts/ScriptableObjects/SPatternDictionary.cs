using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SlimeSOs/SlimePatternDictionary")]
public class SPatternDictionary : SerializedScriptableObject
{
    // List of Pattern Colors and Pattern types. Dictionary is auto-generated from these
    public class colorListEntry
    {
        public string cName;
        public Color color;
    }

    public class patternListEntry
    {
        public string pName;
        public SlimePatternSO pattern;
    }

    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "@new colorListEntry()")]
    [SerializeField] private List<colorListEntry> sColorList = new List<colorListEntry>();

    [HideReferenceObjectPicker]
    [ListDrawerSettings(CustomAddFunction = "@new patternListEntry()")]
    [SerializeField] private List<patternListEntry> sPatternList = new List<patternListEntry>();


    // Slime Pattern Entry class
    public class SlimePatternEntry
    {
        public string slimePatternName = "Null";
        public string slimePatternColorName = "Null";
        public Color slimePatternColor;
        public SlimePatternSO slimePatternSO;

        // Constructor
        public SlimePatternEntry(string name, string colorName, Color pColor, SlimePatternSO data)
        {
            slimePatternName = name;
            slimePatternColorName = colorName;
            slimePatternColor = pColor;
            slimePatternSO = data;
        }
    }

    // Dictionary of SlimePatternEntry. Used to be easily searchable by other scripts
    private static Dictionary<string, SlimePatternEntry> slimePatternDict = new Dictionary<string, SlimePatternEntry>();
    // A second dictionary to see in inspector. Since static dictionaries can't be serialized
    //public Dictionary<string, SlimePatternEntry> slimePatternDictTWO = new Dictionary<string, SlimePatternEntry>();

    // Dictionary is filled
    void OnEnable()
    {
        Debug.Log("Generating Pattern Dictionary");

        // Add Null, Null
        slimePatternDict.Add("Null Null", 
            new SlimePatternEntry("Null Null", "Null", Color.white, Resources.Load<SlimePatternSO>("SlimeSOs/Patterns/Null Null")));

        // Add all slime patterns
        string sName = "";
        for (int p = 0; p < sPatternList.Count; p++)
        {
            for (int c = 0; c < sColorList.Count; c++)
            {
                // Add color to name
                sName += sColorList[c].cName;
                sName += " ";
                // Add pattern to name
                sName += sPatternList[p].pName;
                // Add to dictionary
                slimePatternDict.Add(sName,
                    new SlimePatternEntry(sName, sColorList[c].cName, 
                    sColorList[c].color, sPatternList[p].pattern));
                
                // Reset
                sName = "";
            }
        }

        //slimePatternDictTWO = slimePatternDict;
        Debug.Log("Done");
    }

    // Method that searches the array for a matching slime name and returns that slime's SO
    // Returns null if slime is not found.
    public static SlimePatternEntry GetSlime(string sName)
    {
        SlimePatternEntry foundSlime = null;
        if (slimePatternDict.TryGetValue(sName, out foundSlime))
        {
            // Success
            //Debug.Log("Found slime: " + foundSlime.slimePatternName);
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
