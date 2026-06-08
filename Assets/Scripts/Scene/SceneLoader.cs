using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void StartSceneLoaderHandler(Scene newScene);
public delegate void EndSceneLoaderHandler(Scene oldScene, Scene newScene);
public delegate void ProgressHandler(float progress);

public class SceneLoader : MonoBehaviour
{
    public const float SCENE_SWITCH_DURATION = 0.5f;

    private static SceneLoader instance;
    public StartSceneLoaderHandler OnStartedLoadScene;
    public EndSceneLoaderHandler OnEndedLoadScene;
    public ProgressHandler OnProgressUpdated;

    private Scene currentScene;

    public Scene CurrentScene { get => currentScene; }

    private void Awake()
    {
        currentScene = Enum.Parse<Scene>(SceneManager.GetActiveScene().name);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }        
    }

    private void Start()
    {
        
    }

    public void LoadScene(Scene scene)
    {
        OnStartedLoadScene.Invoke(scene);
        StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return new WaitForSeconds(SCENE_SWITCH_DURATION);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = operation.progress;            
            OnProgressUpdated?.Invoke(progress);

            if(progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                yield return new WaitForSeconds(SCENE_SWITCH_DURATION);
                if (operation.isDone)
                {
                    OnEndedLoadScene?.Invoke(currentScene, scene);
                    currentScene = scene;
                }
            }
            else
            {
                yield return null;
            }            
        } 
    }
}
