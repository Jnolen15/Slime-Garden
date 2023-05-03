using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string playScene;
    [SerializeField] private DataPersistenceManager gameDataManager;

    private void Start()
    {
        gameDataManager = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(playScene);
    }

    public void ResetSave()
    {
        gameDataManager.DeleteSave();
    }
}
