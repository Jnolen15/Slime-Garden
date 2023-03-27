using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelManager : MonoBehaviour, IInteractable
{
    [SerializeField] string sceneName;

    private DataPersistenceManager gameData;

    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    public void Interact()
    {
        gameData.SaveGame();
        SceneManager.LoadScene(sceneName);
    }
}
