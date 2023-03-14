using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    // This (along with some other save system scripts) was made with help from
    // this tutorial series https://www.youtube.com/watch?v=aUi9aijvpgs

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one data persistence manager in scene");
        }
        instance = this;
    }

    // SAVE AND LOAD GAME ON STARTUP AND QUIT (Move later?)
    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // ================ DATA MANAGEMENT ================
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load save data from file
        gameData = dataHandler.Load();

        // If no data is loaded, make a new data file
        if (this.gameData == null)
        {
            Debug.Log("No data found, starting from default values");
            NewGame();
        }

        // Push loaded data to scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // Pass data to scripts so they can save
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // Save data to a file
        dataHandler.Save(gameData);
    }

    // Finds all scripts that implement IDataPersistence
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
