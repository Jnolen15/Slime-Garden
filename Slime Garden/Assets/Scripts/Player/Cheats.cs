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
    [SerializeField] private bool inWild;
    [SerializeField] private CropSO cheatCrop;

    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
        rs = GetComponentInParent<RandomSlime>();
    }

    void Update()
    {
        // Give money
        if (Input.GetKeyDown(KeyCode.M))
        {
            pData.GainMoney(100);
        }

        // Give Crops
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventory.AddCrop(cheatCrop, 10);
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
    }
}