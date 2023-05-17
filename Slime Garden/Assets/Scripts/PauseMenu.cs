using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private MenuManager menus;
    private DataPersistenceManager gameData;
    [SerializeField] private GameObject pMenu;
    [SerializeField] private Toggle fpsToggle;

    void Start()
    {
        menus = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    public void PauseGame (bool toggle)
    {
        gameData.SaveGame();

        pMenu.SetActive(toggle);

        // Paused
        if (toggle)
        {
            Time.timeScale = 0;

            if (PlayerPrefs.GetInt("FPSEnabled") == 1)
                fpsToggle.isOn = true;
            else if (PlayerPrefs.GetInt("FPSEnabled") == 0)
                fpsToggle.isOn = false;
        }
        // UnPaused
        else
            Time.timeScale = 1;
    }

    public void QuitGame()
    {
        gameData.SaveGame();

        Application.Quit();
    }

    public void ToggleFPS(bool toggle)
    {
        if (toggle)
            PlayerPrefs.SetInt("FPSEnabled", 1);
        else
            PlayerPrefs.SetInt("FPSEnabled", 0);

        menus.ToggleFPS(toggle);
    }
}
