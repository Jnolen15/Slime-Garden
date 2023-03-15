using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatControl : MonoBehaviour, IDataPersistence
{
    // SLIME DICTIONARIES ===============
    public SBaseDictionary sBaseDictionary;         //The SO of the dictionary that holds all slime Base SOs.
    public SPatternDictionary sPatternDictionary;   //The SO of the dictionary that holds all slime Pattern SOs.
    public GameObject slimePrefab;                  // Slime prefab, used in loading
    public List<GameObject> activeSlimes;           //List of all active slimes
    public List<SlimeDataEntry> slimeDataList = new List<SlimeDataEntry>(); // List of slime data that is saved and loaded

    void Start()
    {
        // Creates slime duplication
        /*foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Slime"))
        {
            activeSlimes.Add(ob);
        }*/
    }

    public GameObject GetRandomSlime()
    {
        var rand = Random.Range(0, activeSlimes.Count);
        return activeSlimes[rand];
    }


    // ===================== SAVE LOAD =====================
    public void LoadData(GameData data)
    {
        // Create saved slimes from list
        foreach (SlimeDataEntry slime in data.slimeList)
        {
            ConstructSlime(slime);
        }
    }

    private void ConstructSlime(SlimeDataEntry slime)
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
        newSD.Setup();
    }

    public void SaveData(GameData data)
    {
        // Clear previously saved slime list
        data.slimeList.Clear();

        Debug.Log("Slimelist size post clear: " + data.slimeList.Count);

        // Save slime list
        foreach (GameObject slime in activeSlimes)
        {
            var sData = slime.GetComponent<SlimeData>();

            slimeDataList.Add( new SlimeDataEntry(slime.transform.position, 
                sData.displayName, sData.sPattern, sData.sBaseColor, 
                sData.slimeSpeciesBaseColor, sData.sPatternColor, 
                sData.slimeSpeciesPatternColor));

            data.slimeList = slimeDataList;
        }
    }

    // Slime Class
    [System.Serializable]
    public class SlimeDataEntry
    {
        public Vector3 pos;
        public string displayName;
        public string sPattern;
        public string sBaseColor;
        public Color slimeSpeciesBaseColor;
        public string sPatternColor;
        public Color slimeSpeciesPatternColor;

        public SlimeDataEntry(Vector3 curPos, string dispName, string pat, string bColorStr, Color bColor, string pColorStr, Color pColor)
        {
            pos = curPos;
            displayName = dispName;
            sPattern = pat;
            sBaseColor = bColorStr;
            slimeSpeciesBaseColor = bColor;
            sPatternColor = pColorStr;
            slimeSpeciesPatternColor = pColor;
        }
    }
}
