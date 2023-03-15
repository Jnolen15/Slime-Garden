using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    private RandomSlime rs;
    private PlayerData pData;
    [SerializeField] private DataPersistenceManager gameData;

    // Start is called before the first frame update
    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        rs = GetComponentInParent<RandomSlime>();
    }

    // Update is called once per frame
    void Update()
    {
        // Give money
        if (Input.GetKeyDown(KeyCode.M))
        {
            pData.GainMoney(100);
        }
        
        // Spawn a random slime on button press
        if (Input.GetKeyDown(KeyCode.G))
        {
            rs.CreateSlime(Vector3.zero);
        }

        // Save game
        if (Input.GetKeyDown(KeyCode.K))
        {
            gameData.SaveGame();
        }

        // re-load scene game
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}