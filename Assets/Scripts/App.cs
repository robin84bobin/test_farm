using UnityEngine;
using Data.User;
using Commands;
using Commands.Startup;
using Data.Catalog;
using UnityEngine.SceneManagement;
using Zenject;

public class App : MonoBehaviour
{
    public static App Instance; 

    [Inject]
    public CatalogRepository CatalogRepository { get; }
    [Inject]
    public UserRepository UserRepository { get; }
    [Inject]
    public Model.Farm FarmModel { get; }
    
    void Awake()
    {
        if (Instance != null) 
            return;
        Instance = this;
        Init();
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        //try make it via DI
        UserRepository.Setup(CatalogRepository);
        
        CommandSequence sequence = new CommandSequence(
            new InitDataCommand(CatalogRepository),
            new InitDataCommand(UserRepository)
            );
        sequence.OnComplete += OnInitComplete;
        sequence.Execute();
    }

    private void OnInitComplete()
    {
        FarmModel.Init();
        
        SceneManager.LoadSceneAsync("Level");
    }
}
