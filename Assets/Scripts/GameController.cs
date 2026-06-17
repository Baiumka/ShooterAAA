using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public EnemyHandler onEnemySpawned;
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
            gameManager.onEnemySpawned += SpawnEnemy;
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

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.onPlayerSpawned -= SpawnPlayer;
            gameManager.onEnemySpawned -= SpawnEnemy;
            gameManager = null;
        }
        if (loader != null)
        {
            loader.OnStartedLoadScene -= LoadingSceneStarted;
            loader.OnEndedLoadScene -= LoadingSceneEnded;
            loader.OnProgressUpdated -= UpdateSceneProgress;
        }
    }

    private void SpawnEnemy(Enemy enemy)
    {
        onEnemySpawned?.Invoke(enemy);
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

        List<SpawnPoint> points =
            FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None)
            .OrderBy(e => e.Id)
            .ToList();

        List<Vec3> enemyPoints = new List<Vec3>();
        Vec3? playerPoint = null;

        foreach(SpawnPoint point in points)
        {
            if(point.Type == SpawnType.ENEMY_SPAWN)
            {
                enemyPoints.Add(point.Vec);
            }
            else if (point.Type == SpawnType.PLAYER_SPAWN)
            {
                playerPoint = point.Vec;
            }
        }

        gameManager.GenerateEnemySpawnPoints(enemyPoints);
        gameManager.GeneratePlayerSpawnPoint(playerPoint);

        gameManager.SpawnPlayer();
        gameManager.SpawnEnemies(2);

    }

    public void StartLoadGame()
    {
        loader.LoadScene(Scene.SIMPLE_SCENE);
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, Screen.height/2, 150, 15), "Spawn Enemy"))
        {
            gameManager.SpawnEnemies(1);
        }
    }


}
