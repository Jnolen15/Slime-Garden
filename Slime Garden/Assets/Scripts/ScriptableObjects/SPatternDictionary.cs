using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SlimeSOs/SlimePatternDictionary")]
public class SPatternDictionary : SerializedScriptableObject // Make just a normla serialized object if the Dictionary is taken out
{
    // Array of Pattern Colors and Pattern types. Dictionary is auto-generated from these
    [SerializeField]
    private string[] sColors;
    [SerializeField]
    private string[] sPatterns;

    public Color Amethyst;
    public Color Aquamarine;
    public Color Bixbite;
    public Color Citrine;
    public Color Emerald;
    public Color Jade;
    public Color Obsidian;
    public Color Peridot;
    public Color Quartz;
    public Color Ruby;
    public Color Sapphire;
    public Color Topaz;

    // Slime Pattern Entry class
    //[System.Serializable]
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

    // Dictionary of data from array. A dictionary is used because it is more easily searchable
    private static Dictionary<string, SlimePatternEntry> slimePatternDict = new Dictionary<string, SlimePatternEntry>();
    //public Dictionary<string, SlimePatternEntry> slimePatternDictTWO = new Dictionary<string, SlimePatternEntry>();

    // Dictionary is filled
    void OnEnable()
    {
        Debug.Log("Generating Pattern Dictionary");

        // Add Null, Null
        slimePatternDict.Add("Null Null", 
            new SlimePatternEntry("Null Null", "Null", Quartz, Resources.Load<SlimePatternSO>("SlimeSOs/Patterns/Null Null")));

        // Add all slime patterns
        string sName = "";
        string fileLocationName = "SlimeSOs/Patterns/";
        for (int p = 0; p < sPatterns.Length; p++)
        {
            for (int c = 0; c < sColors.Length; c++)
            {
                // Add color to name
                sName += sColors[c];
                sName += " ";
                // Add pattern to name
                sName += sPatterns[p];
                fileLocationName += sPatterns[p];
                // Add to dictionary
                slimePatternDict.Add(sName,
                    new SlimePatternEntry(sName, sColors[c], Quartz, Resources.Load<SlimePatternSO>(fileLocationName)));
                
                switch (sColors[c])
                {
                    case "Amethyst":
                        slimePatternDict[sName].slimePatternColor = Amethyst;
                        break;
                    case "Aquamarine":
                        slimePatternDict[sName].slimePatternColor = Aquamarine;
                        break;
                    case "Bixbite":
                        slimePatternDict[sName].slimePatternColor = Bixbite;
                        break;
                    case "Citrine":
                        slimePatternDict[sName].slimePatternColor = Citrine;
                        break;
                    case "Emerald":
                        slimePatternDict[sName].slimePatternColor = Emerald;
                        break;
                    case "Jade":
                        slimePatternDict[sName].slimePatternColor = Jade;
                        break;
                    case "Obsidian":
                        slimePatternDict[sName].slimePatternColor = Obsidian;
                        break;
                    case "Peridot":
                        slimePatternDict[sName].slimePatternColor = Peridot;
                        break;
                    case "Quartz":
                        slimePatternDict[sName].slimePatternColor = Quartz;
                        break;
                    case "Ruby":
                        slimePatternDict[sName].slimePatternColor = Ruby;
                        break;
                    case "Sapphire":
                        slimePatternDict[sName].slimePatternColor = Sapphire;
                        break;
                    case "Topaz":
                        slimePatternDict[sName].slimePatternColor = Topaz;
                        break;
                }
                // Reset and Increment
                sName = "";
                fileLocationName = "SlimeSOs/Patterns/";
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
            Debug.Log("Found slime: " + foundSlime.slimePatternName);
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
