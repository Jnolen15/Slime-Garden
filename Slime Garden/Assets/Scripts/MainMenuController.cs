using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string playScene;
    

    public void PlayGame()
    {
        SceneManager.LoadScene(playScene);
    }
}
