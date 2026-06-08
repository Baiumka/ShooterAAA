using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class GameController : MonoBehaviour
{
    public static GameController instance;
    private GameManager gameManager;
    private Player player;

    public VoidHandler onSceneStartLoading;
    public VoidHandler onSceneLoaded;
    public PlayerHandler onPlayerSpawned;
    public ProgressHandler onSceneProgressUpdated;

    [SerializeField] private SceneLoader loader;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            gameManager = new GameManager();
            gameManager.GenerateWeapons();
            gameManager.onPlayerSpawned += SpawnPlayer;
            if (loader != null)
            {
                loader.OnStartedLoadScene += LoadingSceneStarted;
                loader.OnEndedLoadScene += LoadingSceneEnded;
                loader.OnProgressUpdated += UpdateSceneProgress;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(FirstLoad());
    }

    private IEnumerator FirstLoad()
    {
        yield return new WaitForSeconds(1);
        EndLoadScene(loader.CurrentScene);
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.onPlayerSpawned -= SpawnPlayer;
            gameManager = null;
        }
        if (loader != null)
        {
            loader.OnStartedLoadScene -= LoadingSceneStarted;
            loader.OnEndedLoadScene -= LoadingSceneEnded;
            loader.OnProgressUpdated -= UpdateSceneProgress;
        }
    }

    private void LoadingSceneStarted(Scene newScene)
    {        
        onSceneStartLoading?.Invoke();
    }
    private void LoadingSceneEnded(Scene oldScene, Scene newScene)
    {
        EndLoadScene(newScene);
    }

    protected void EndLoadScene(Scene newScene)
    {       
        if(newScene == Scene.SIMPLE_SCENE)
        {
            StartGame();
        }
        onSceneLoaded?.Invoke();
    }

    private void UpdateSceneProgress(float progress)
    {
        onSceneProgressUpdated?.Invoke(progress);
    }


    private void SpawnPlayer(Player player)
    {        
        this.player = player;
        onPlayerSpawned?.Invoke(player);
    }  

    private void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameManager.SpawnPlayer();
    }

    public void StartLoadGame()
    {
        loader.LoadScene(Scene.SIMPLE_SCENE);
    }


}
