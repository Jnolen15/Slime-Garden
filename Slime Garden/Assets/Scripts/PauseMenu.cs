using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private DataPersistenceManager gameData;
    [SerializeField] private GameObject pMenu;

    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    public void PauseGame (bool toggle)
    {
        gameData.SaveGame();

        pMenu.SetActive(toggle);

        if (toggle)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void QuitGame()
    {
        gameData.SaveGame();

        Application.Quit();
    }
}
