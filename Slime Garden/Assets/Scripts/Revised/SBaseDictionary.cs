using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SlimeBaseDictionary")]

public class SBaseDictionary : ScriptableObject
{
    // Array of Pattern Colors and Pattern types. Dictionary is auto-generated from these
    [SerializeField]
    private string[] sColors;

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

    public SlimeBaseSO basicSlime;

    // Serializable array that I can manually add slimes too
    [System.Serializable]
    public class SlimeBaseEntry
    {
        public string slimeBaseName;
        public Color slimeBaseColor;
        public SlimeBaseSO slimeBaseSO;
        public GameObject slimeCrystal;
        public bool isSpecial; //RN this will alwys be false. Find way to re-implement special slimes later
    }
    [SerializeField]
    private SlimeBaseEntry[] slimeBases;

    // Dictionary of data from array. A dictionary is used because it is more easily searchable
    // However, I couldn't just use a dictionary as it isn't Serializable
    private static Dictionary<string, SlimeBaseEntry> slimeBaseDict = new Dictionary<string, SlimeBaseEntry>();

    // Dictionary is filled from the array
    void OnEnable()
    {
        //Debug.Log("slimes.Length: " + slimes.Length);
        Debug.Log("Filling Base Dictionary");

        // Make array the length of each possible slime pattern (+ the Null, Null pattern)
        int arrayLength = sColors.Length;
        slimeBases = new SlimeBaseEntry[arrayLength];
        for (int i = 0; i < slimeBases.Length; i++)
        {
            slimeBases[i] = new SlimeBaseEntry();
        }

        // Add all slime patterns
        int count = 0;
        string sName = "";
        for (int c = 0; c < sColors.Length; c++)
        {
            // Add color to name
            sName += sColors[c];
            // Add to array / dictionary
            slimeBases[count].slimeBaseName = sName;
            slimeBases[count].slimeBaseSO = basicSlime;
            switch (sColors[c])
            {
                case "Amethyst":
                    slimeBases[count].slimeBaseColor = Amethyst;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/AmethystCrystal");
                    break;
                case "Aquamarine":
                    slimeBases[count].slimeBaseColor = Aquamarine;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/AquamarineCrystal");
                    break;
                case "Bixbite":
                    slimeBases[count].slimeBaseColor = Bixbite;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/BixbiteCrystal");
                    break;
                case "Citrine":
                    slimeBases[count].slimeBaseColor = Citrine;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/CitrineCrystal");
                    break;
                case "Emerald":
                    slimeBases[count].slimeBaseColor = Emerald;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/EmeraldCrystal");
                    break;
                case "Jade":
                    slimeBases[count].slimeBaseColor = Jade;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/JadeCrystal");
                    break;
                case "Obsidian":
                    slimeBases[count].slimeBaseColor = Obsidian;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/ObsidianCrystal");
                    break;
                case "Peridot":
                    slimeBases[count].slimeBaseColor = Peridot;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/PeridotCrystal");
                    break;
                case "Quartz":
                    slimeBases[count].slimeBaseColor = Quartz;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/QuartzCrystal");
                    break;
                case "Ruby":
                    slimeBases[count].slimeBaseColor = Ruby;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/RubyCrystal");
                    break;
                case "Sapphire":
                    slimeBases[count].slimeBaseColor = Sapphire;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/SapphireCrystal");
                    break;
                case "Topaz":
                    slimeBases[count].slimeBaseColor = Topaz;
                    slimeBases[count].slimeCrystal = Resources.Load<GameObject>("Crystals/TopazCrystal");
                    break;
            }
            // Reset and Increment
            sName = "";
            count++;
        }

        for (int i = 0; i < slimeBases.Length; i++)
        {
            // If Slime isn't already in the dictionary
            if(!slimeBaseDict.ContainsKey(slimeBases[i].slimeBaseName))
                slimeBaseDict.Add(slimeBases[i].slimeBaseName, slimeBases[i]);
        }

        //Debug.Log(slimeDict);
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
