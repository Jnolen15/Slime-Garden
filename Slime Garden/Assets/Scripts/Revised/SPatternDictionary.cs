using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SlimePatternDictionary")]

public class SPatternDictionary : ScriptableObject
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

    // Serializable array that I can manually add slimes too
    [System.Serializable]
    public class SlimePatternEntry
    {
        public string slimePatternName = "Null";
        public string slimePatternColorName = "Null";
        public Color slimePatternColor;
        public SlimePatternSO slimePatternSO;
    }
    [SerializeField]
    private SlimePatternEntry[] slimePatterns;

    // Dictionary of data from array. A dictionary is used because it is more easily searchable
    // But the array is there so I can visualize it in the inspector
    private static Dictionary<string, SlimePatternEntry> slimePatternDict = new Dictionary<string, SlimePatternEntry>();

    // Dictionary is filled from the array
    void OnEnable()
    {
        //Debug.Log("slimes.Length: " + slimes.Length);
        Debug.Log("Generating Pattern Dictionary");

        // ---- RN it first makes an array then fills the dictionary
        // from that array. However, it can just skip the array
        // and load directly into the dictionary. But I can't see
        // the dictionary so I have it load into the array so I can
        // Check that its correct in the inspector
        
        // Make array the length of each possible slime pattern (+ the Null, Null pattern)
        int arrayLength = (sColors.Length * sPatterns.Length) + 1;
        slimePatterns = new SlimePatternEntry[arrayLength];
        for (int i = 0; i < slimePatterns.Length; i++)
        {
            slimePatterns[i] = new SlimePatternEntry();
        }

        // Add Null, Null
        slimePatterns[0].slimePatternName = "Null Null";
        slimePatterns[0].slimePatternSO = Resources.Load<SlimePatternSO>("SlimeSOs/Patterns/Null Null");
        slimePatterns[0].slimePatternColor = Quartz;

        // Add all slime patterns
        int count = 1;
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
                // Add to array / dictionary
                slimePatterns[count].slimePatternName = sName;
                slimePatterns[count].slimePatternSO = Resources.Load<SlimePatternSO>(fileLocationName);
                slimePatterns[count].slimePatternColorName = sColors[c];
                switch (sColors[c])
                {
                    case "Amethyst":
                        slimePatterns[count].slimePatternColor = Amethyst;
                        break;
                    case "Aquamarine":
                        slimePatterns[count].slimePatternColor = Aquamarine;
                        break;
                    case "Bixbite":
                        slimePatterns[count].slimePatternColor = Bixbite;
                        break;
                    case "Citrine":
                        slimePatterns[count].slimePatternColor = Citrine;
                        break;
                    case "Emerald":
                        slimePatterns[count].slimePatternColor = Emerald;
                        break;
                    case "Jade":
                        slimePatterns[count].slimePatternColor = Jade;
                        break;
                    case "Obsidian":
                        slimePatterns[count].slimePatternColor = Obsidian;
                        break;
                    case "Peridot":
                        slimePatterns[count].slimePatternColor = Peridot;
                        break;
                    case "Quartz":
                        slimePatterns[count].slimePatternColor = Quartz;
                        break;
                    case "Ruby":
                        slimePatterns[count].slimePatternColor = Ruby;
                        break;
                    case "Sapphire":
                        slimePatterns[count].slimePatternColor = Sapphire;
                        break;
                    case "Topaz":
                        slimePatterns[count].slimePatternColor = Topaz;
                        break;
                }
                // Reset and Increment
                sName = "";
                fileLocationName = "SlimeSOs/Patterns/";
                count++;
            }
        }

        // Load it into the dictionary
        for (int i = 0; i < slimePatterns.Length; i++)
        {
            // If Slime isn't already in the dictionary
            if (!slimePatternDict.ContainsKey(slimePatterns[i].slimePatternName))
                slimePatternDict.Add(slimePatterns[i].slimePatternName, slimePatterns[i]);
        }

        //Debug.Log(slimeDict);
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
