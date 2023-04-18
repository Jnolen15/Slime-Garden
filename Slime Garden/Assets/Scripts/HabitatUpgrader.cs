using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HabitatUpgrader : MonoBehaviour, IInteractable
{
    private GridDataPersistence gridData;
    private DataPersistenceManager gameData;

    [SerializeField] private bool gridUpgraded;

    void Start()
    {
        gridData = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridDataPersistence>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    public void Interact()
    {
        if (gridUpgraded)
            return;

        Debug.Log("Upgrading grid");

        gridData.UpgradeHabitat();

        gameData.SaveGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
