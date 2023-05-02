using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    private RandomSlime rs;
    private PlayerData pData;
    private InventoryManager inventory;
    private DataPersistenceManager gameData;
    [SerializeField] private bool cheatsEnabled;
    [SerializeField] private bool inWild;
    [SerializeField] private CropSO[] cheatCrops;
    [SerializeField] private GameObject cs;

    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        inventory = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
        rs = GetComponentInParent<RandomSlime>();
    }

    void Update()
    {
        // Toggle cheats
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            cheatsEnabled = !cheatsEnabled;
        }

        if (!cheatsEnabled)
            return;

        // Give money
        if (Input.GetKeyDown(KeyCode.M))
        {
            pData.GainMoney(100);
        }

        // Give money
        if (Input.GetKeyDown(KeyCode.J))
        {
            pData.GainExperience(5);
            //Debug.Log($"Gained EXP, now: {pData.Experience}");
        }

        // Give Crops
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach(CropSO crop in cheatCrops)
            {
                inventory.AddCrop(crop, 10);
            }
        }

        // Spawn a random slime on button press
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(inWild)
                rs.CreateSlime(Vector3.zero, true);
            else
                rs.CreateSlime(Vector3.zero, false);
        }

        // Save game
        if (Input.GetKeyDown(KeyCode.K))
        {
            gameData.SaveGame();
        }

        // re-load scene game
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Load Wild
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            gameData.SaveGame();
            SceneManager.LoadScene(1);
        }
        // Load habitat
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            gameData.SaveGame();
            SceneManager.LoadScene(0);
        }

        // Spawn CS
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Instantiate(cs, new Vector3(0, 2, 0), Quaternion.identity);
        }
    }
}