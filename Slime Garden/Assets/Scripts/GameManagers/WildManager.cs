using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildManager : MonoBehaviour, IDataPersistence
{
    // SLIME DICTIONARIES ===============
    // Dictionary refrneces needed in scene.
    // Once they are loaded once they should be fine in other scenes but just in case
    public SBaseDictionary sBaseDictionary;
    public SPatternDictionary sPatternDictionary;

    // Variables
    [SerializeField] private GameObject slimeSpawnFX;
    [SerializeField] private Vector2Int spawnInterval;
    [SerializeField] private Vector2Int numSpawns;
    private float spawnCoolDown;

    public List<Transform> spawns = new List<Transform>();
    public List<HabitatControl.SlimeDataEntry> tamedSlimes = new List<HabitatControl.SlimeDataEntry>();
    
    [SerializeField] private List<string> unlockedSlimePatterns = new List<string>(); // List of unlocked Slime patterns

    // Refrences
    private RandomSlime randSlime;

    void Start()
    {
        randSlime = this.GetComponent<RandomSlime>();

        randSlime.SetUnlockedSlimeList(unlockedSlimePatterns);
    }

    void Update()
    {
        if (spawnCoolDown > 0) spawnCoolDown -= Time.deltaTime;
        else
        {
            spawnCoolDown = Random.Range(spawnInterval.x, spawnInterval.y);
            var numtospawn = Random.Range(numSpawns.x, numSpawns.y);
            for(int i = 0; i < numtospawn; i++)
                SpawnSlime();
        }
    }

    private void SpawnSlime()
    {
        Transform randPos = spawns[Random.Range(0, spawns.Count)];
        GameObject newSLime = randSlime.CreateSlime(randPos.position, true);
        newSLime.GetComponent<SlimeController>().ChangeState(SlimeController.State.jump);

        Instantiate(slimeSpawnFX, randPos.position, randPos.rotation);

        Debug.Log("Spawned slime " + newSLime.GetComponent<SlimeData>().speciesName);
    }

    public void AddTamedSlime(string pat, string bColorStr, string pColorStr)
    {
        tamedSlimes.Add(new HabitatControl.SlimeDataEntry(Vector3.zero, pat, bColorStr, pColorStr));
    }

    // ===================== LEVEL UP =====================
    public void UnlockSlime(string patName, bool fromLevelUp)
    {
        if (!unlockedSlimePatterns.Contains(patName))
            unlockedSlimePatterns.Add(patName);

        //Debug.Log($"Wild zone added {patName}");
    }

    // =================== SAVE ===================
    public void LoadData(GameData data)
    {
        // Load tamed slime list if it has not been cleared
        // (IF saving and loading into wild zome without returning to habitat)
        tamedSlimes = data.tamedSlimeList;

        unlockedSlimePatterns = data.unlockedSlimes;
    }

    public void SaveData(GameData data)
    {
        // Save list of tamed slimes
        data.tamedSlimeList = tamedSlimes;
    }
}
