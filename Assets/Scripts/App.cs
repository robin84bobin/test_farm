using UnityEngine;
using Data.User;
using System;
using System.IO;
using Commands;
using Commands.Startup;
using Data;
using Data.Catalog;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    const int TiCK_DELTA_TIME = 1;


    public static App Instance; 
    public event Action<int> OnTick = delegate { };

    public CatalogRepository catalog { get; private set; }
    public UserRepository userRepository { get; private set; }
    
    public  string CatalogPath{ get; private set; }
    public  string UserRepositoryPath{ get; private set; }

    public Model.Farm FarmModel { get; private set; }

    void Awake()
    {
        CatalogPath = Application.streamingAssetsPath + "/catalog.json";
        UserRepositoryPath = Application.persistentDataPath + "/user.json";
        
        if (Instance==null)
        {
            Instance = this;
            Init();
        }
    }

    private bool _newGame = false;
    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        if (!File.Exists(UserRepositoryPath))
            _newGame = true;
        
        catalog = new CatalogRepository(new JsonDbProxy(CatalogPath, "catalog"));
        userRepository = new UserRepository(new JsonDbProxy(UserRepositoryPath, "catalog"));

        CommandSequence sequence = new CommandSequence(
            new InitDataCommand(catalog),
            new InitDataCommand(userRepository)
            );
        sequence.OnComplete += OnInitComplete;
        sequence.Execute();
    }

    private void OnInitComplete()
    {
        if (_newGame)
            userRepository.InitStartValuesFrom(catalog);
        
        FarmModel = new Model.Farm();
        FarmModel.Init();

        SceneManager.LoadSceneAsync("Level");
    }

    private float _nextTickTime;
    void FixedUpdate()
    {
        if(Time.time >= _nextTickTime)
        {
            _nextTickTime = Time.time + TiCK_DELTA_TIME;
            OnTick.Invoke(TiCK_DELTA_TIME);
            Debug.Log("tick: " + Time.time);
        }
    }
}
