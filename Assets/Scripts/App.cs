using UnityEngine;
using System.Collections;
using Data.User;
using System;
using Commands;
using Commands.Startup;
using Data;
using Data.Catalog;
using Data.Repository;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }
    public event Action OnTick = delegate { };

    const float TiCK_DELTA_TIME = 1f;
    public Catalog catalog { get; private set; }
    public UserRepository userRepository { get; private set; }
    
    void Start()
    {
        if (Instance!=null)
        {
            Instance = this;
            Init();
        }
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);
        catalog = new Catalog(new JsonDbProxy("catalog.json", "catalog"));
        userRepository = new UserRepository(new JsonDbProxy("user.json", "catalog"));

        CommandSequence sequence = new CommandSequence(
            new InitDataCommand(catalog),
            new InitDataCommand(userRepository)
            );
        sequence.OnComplete += OnInitComplete;
        sequence.Execute();
    }

    private void OnInitComplete()
    {
        SceneManager.LoadSceneAsync("Level");
    }

    private float _nextTickTime;
    void Update()
    {
        if(Time.time >= _nextTickTime)
        {
            _nextTickTime = Time.time + TiCK_DELTA_TIME;
            OnTick.Invoke();
        }
    }
}
