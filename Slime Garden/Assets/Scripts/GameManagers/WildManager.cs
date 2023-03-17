using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildManager : MonoBehaviour, IDataPersistence
{
    // Variables
    [SerializeField] private GameObject wildSlimePref;
    [SerializeField] private Vector2 zoneBorders;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnCoolDown;

    public List<HabitatControl.SlimeDataEntry> tamedSlimes = new List<HabitatControl.SlimeDataEntry>();

    // Refrences
    private RandomSlime randSlime;

    void Start()
    {
        randSlime = this.GetComponent<RandomSlime>();
    }

    void Update()
    {
        if (spawnCoolDown > 0) spawnCoolDown -= Time.deltaTime;
        else
        {
            spawnCoolDown = spawnInterval;
            SpawnSlime();
        }
    }

    private void SpawnSlime()
    {
        Vector3 randPos = new Vector3(Random.Range(-zoneBorders.x, zoneBorders.x), 0, Random.Range(-zoneBorders.y, zoneBorders.y));
        GameObject newSLime = randSlime.CreateSlime(randPos, true);
        newSLime.GetComponent<SlimeController>().ChangeState(SlimeController.State.jump);

        Debug.Log("Spawned slime " + newSLime.GetComponent<SlimeData>().speciesName);
    }

    public void AddTamedSlime(string pat, string bColorStr, string pColorStr)
    {
        tamedSlimes.Add(new HabitatControl.SlimeDataEntry(Vector3.zero, pat, bColorStr, pColorStr));
    }

    // =================== SAVE ===================
    public void LoadData(GameData data)
    {
        // Load tamed slime list if it has not been cleared
        // (IF saving and loading into wild zome without returning to habitat)
        tamedSlimes = data.tamedSlimeList;
    }

    public void SaveData(GameData data)
    {
        // Save list of tamed slimes
        data.tamedSlimeList = tamedSlimes;
    }
}
