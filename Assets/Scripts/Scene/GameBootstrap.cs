using UnityEngine;

public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (GameController.instance != null)
            return;

        GameObject prefab = Resources.Load<GameObject>("GameController");
        GameObject.Instantiate(prefab);

        //prefab = Resources.Load<GameObject>("SceneLoader");
        //GameObject.Instantiate(prefab);
    }
}