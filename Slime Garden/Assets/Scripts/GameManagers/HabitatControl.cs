using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatControl : MonoBehaviour, IDataPersistence
{
    // SLIME DICTIONARIES ===============
    public SBaseDictionary sBaseDictionary;         //The SO of the dictionary that holds all slime Base SOs.
    public SPatternDictionary sPatternDictionary;   //The SO of the dictionary that holds all slime Pattern SOs.
    public GameObject slimePrefab;                  // Slime prefab, used in loading
    public List<string> unlockedSlimePatterns = new List<string>(); // List of unlocked Slime patterns
    public List<GameObject> activeSlimes;           //List of all active slimes
    public List<SlimeDataEntry> slimeDataList = new List<SlimeDataEntry>(); // List of slime data that is saved and loaded

    void Start()
    {
        this.GetComponent<RandomSlime>().SetUnlockedSlimeList(unlockedSlimePatterns);
    }

    public GameObject GetRandomSlime()
    {
        var rand = Random.Range(0, activeSlimes.Count);
        return activeSlimes[rand];
    }

    // ===================== LEVEL UP =====================
    public void UnlockSlime(string patName, bool fromLevelUp)
    {
        if (!unlockedSlimePatterns.Contains(patName))
            unlockedSlimePatterns.Add(patName);

        //Debug.Log($"Habitat added {patName}");
    }

    // ===================== SAVE LOAD =====================
    public void LoadData(GameData data)
    {
        // Load list of unlocked slimes
        unlockedSlimePatterns = data.unlockedSlimes;

        // Create saved slimes from list
        foreach (SlimeDataEntry slime in data.slimeList)
        {
            ConstructSlime(slime);
        }

        // Create new slimes from tamed list
        foreach (SlimeDataEntry slime in data.tamedSlimeList)
        {
            ConstructSlime(slime);
        }
    }

    public void ConstructSlime(SlimeDataEntry slime)
    {
        GameObject newSlime = Instantiate(slimePrefab, slime.pos, Quaternion.identity);
        SlimeData newSD = newSlime.GetComponent<SlimeData>();
        var newBase = SBaseDictionary.GetSlime(slime.sBaseColor);
        string newSlimePattern = (slime.sPatternColor + " " + slime.sPattern);
        var newpattern = SPatternDictionary.GetSlime(newSlimePattern);

        newSD.slimeSpeciesBase = newBase.slimeBaseSO;
        newSD.sBaseColor = newBase.slimeBaseName;
        newSD.slimeSpeciesBaseColor = newBase.slimeBaseColor;
        newSD.slimeSpeciesPattern = newpattern.slimePatternSO;
        newSD.sPatternColor = newpattern.slimePatternColorName;
        newSD.slimeSpeciesPatternColor = newpattern.slimePatternColor;
        newSD.sCrystal = newBase.slimeCrystal;
        newSD.hungerLevel = slime.hungerLevel;

        if (slime.infancyTimer > 0)
        {
            newSD.infancyTimer = slime.infancyTimer;
            newSD.Setup(true, false);
        }
        else
            newSD.Setup(false, false);

        if (slime.displayName != "" && slime.displayName != null)
            newSD.SetName(slime.displayName);
    }

    public void SaveData(GameData data)
    {
        // Save list of unlocked slimes
        data.unlockedSlimes = unlockedSlimePatterns;

        // Clear previously saved slime list
        data.slimeList.Clear();

        // Clear tamed slime list
        if(data.tamedSlimeList.Count > 0)
            data.tamedSlimeList.Clear();

        // Save slime list
        foreach (GameObject slime in activeSlimes)
        {
            var sData = slime.GetComponent<SlimeData>();

            slimeDataList.Add( new SlimeDataEntry(slime.transform.position, sData.sPattern,
                sData.sBaseColor, sData.sPatternColor, sData.infancyTimer, sData.hungerLevel, sData.displayName));

            data.slimeList = slimeDataList;
        }
    }

    // Slime Class
    [System.Serializable]
    public class SlimeDataEntry
    {
        public Vector3 pos;
        public string sPattern;
        public string sBaseColor;
        public string sPatternColor;
        public float infancyTimer;
        public int hungerLevel;
        public string displayName;

        public SlimeDataEntry(Vector3 curPos, string pat, string bColorStr, string pColorStr, float babyTimer, int hunger, string dispName = null)
        {
            pos = curPos;
            sPattern = pat;
            sBaseColor = bColorStr;
            sPatternColor = pColorStr;
            infancyTimer = babyTimer;
            hungerLevel = hunger;
            displayName = dispName;
        }
    }
}
