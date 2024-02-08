using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveLoadManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableSaveLoadSystem = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    [Space(5)]
    [SerializeField] private bool assertSuccessDebug = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    //private Coroutine autoSaveCoroutine; // autoSave disable by default

    public static SaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        #region Singleton and DontDestroyOnLoad
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        #endregion

        if (disableSaveLoadSystem)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }
        // Get File Path
        string dataDirPath = "";
#if UNITY_EDITOR
        dataDirPath = Application.dataPath;
#else
        dataDirPath= Application.persistentDataPath;
#endif
        // Create FileDataHandler
        this.dataHandler = new FileDataHandler(dataDirPath, fileName, useEncryption, assertSuccessDebug);
        // select ProfileId
        //InitializeSelectedProfileId();
    }



    #region Profile Id
    public void ChangeSelectedProfileId(string newProfileId)
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        // delete the data for this profile id
        dataHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        //LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }
    #endregion

    #region New-Load-Save
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (disableSaveLoadSystem)
        {
            return;
        }

        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (disableSaveLoadSystem)
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // timestamp the data so we know when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileId);
#if UNITY_EDITOR
        Debug.Log("Data Kaydedildi. Obje Sayýsý: " + dataPersistenceObjects.Count);
#endif
    }
    #endregion

    // Find all MonoBehaviour scripts that implement IDataPersistence interface in scene
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game");
        }
    }
}
