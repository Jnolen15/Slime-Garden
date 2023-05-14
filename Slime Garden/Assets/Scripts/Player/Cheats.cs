using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Cheats : MonoBehaviour
{
    private RandomSlime rs;
    private PlayerData pData;
    private InventoryManager inventory;
    private DataPersistenceManager gameData;
    [SerializeField] private LayerMask groundLayerMask;
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

        // Give exp
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

        // Spawn CS
        if (Input.GetKeyDown(KeyCode.X))
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
            {
                Instantiate(cs, new Vector3(mousePos.point.x, 2, mousePos.point.z), Quaternion.identity);
            }
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