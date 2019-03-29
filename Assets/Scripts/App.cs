using UnityEngine;
using System.Collections;
using Data.User;
using System;
using Data;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }
    public event Action OnTick = delegate { };

    const float TiCK_DELTA_TIME = 1f;
    public Repository Repository { get; private set; }
    
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
        Repository = new Repository();

        Repository.OnInitComplete += OnDataInitComplete;
        Repository.Init();
    }

    private void OnDataInitComplete()
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
