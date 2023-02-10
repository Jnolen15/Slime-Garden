using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SlimeSOs/SlimeBaseDictionary")]
public class SBaseDictionary : SerializedScriptableObject // Make just a normla serialized object if the Dictionary is taken out
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
    //[System.Serializable]
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

    // Dictionary of data from array. A dictionary is used because it is more easily searchable
    private static Dictionary<string, SlimeBaseEntry> slimeBaseDict = new Dictionary<string, SlimeBaseEntry>();
    //public Dictionary<string, SlimeBaseEntry> slimeBaseDictTWO = new Dictionary<string, SlimeBaseEntry>();

    // Dictionary is filled
    void OnEnable()
    {
        Debug.Log("Filling Base Dictionary");

        // Add all slime patterns
        string sName = "";
        Color color = Quartz;
        string crystal = "QuartzCrystal";
        for (int c = 0; c < sColors.Length; c++)
        {
            // Add color to name
            sName += sColors[c];
            switch (sColors[c])
            {
                case "Amethyst":
                    color = Amethyst;
                    crystal = "Crystals/AmethystCrystal";
                    break;
                case "Aquamarine":
                    color = Aquamarine;
                    crystal = "Crystals/AquamarineCrystal";
                    break;
                case "Bixbite":
                    color = Bixbite;
                    crystal = "Crystals/BixbiteCrystal";
                    break;
                case "Citrine":
                    color = Citrine;
                    crystal = "Crystals/CitrineCrystal";
                    break;
                case "Emerald":
                    color = Emerald;
                    crystal = "Crystals/EmeraldCrystal";
                    break;
                case "Jade":
                    color = Jade;
                    crystal = "Crystals/JadeCrystal";
                    break;
                case "Obsidian":
                    color = Obsidian;
                    crystal = "Crystals/ObsidianCrystal";
                    break;
                case "Peridot":
                    color = Peridot;
                    crystal = "Crystals/PeridotCrystal";
                    break;
                case "Quartz":
                    color = Quartz;
                    crystal = "Crystals/QuartzCrystal";
                    break;
                case "Ruby":
                    color = Ruby;
                    crystal = "Crystals/RubyCrystal";
                    break;
                case "Sapphire":
                    color = Sapphire;
                    crystal = "Crystals/SapphireCrystal";
                    break;
                case "Topaz":
                    color = Topaz;
                    crystal = "Crystals/TopazCrystal";
                    break;
            }

            slimeBaseDict.Add(sName,
                        new SlimeBaseEntry(sName, color, basicSlime, Resources.Load<GameObject>(crystal)));

            // Reset and Increment
            sName = "";
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
