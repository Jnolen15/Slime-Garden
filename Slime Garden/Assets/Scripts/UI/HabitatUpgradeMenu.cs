using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HabitatUpgradeMenu : MonoBehaviour
{
    private GridDataPersistence gridData;
    private DataPersistenceManager gameData;

    void Start()
    {
        gridData = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridDataPersistence>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    public void Upgrade()
    {
        Debug.Log("Upgrading grid");

        gridData.UpgradeHabitat();

        gameData.SaveGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
