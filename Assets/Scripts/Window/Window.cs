using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private Image loadingScreen;

    protected void Awake()
    {
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.color = Color.black;

    }

    protected void Start()  
    {
        GameController.instance.onSceneStartLoading += HideWindow;
        GameController.instance.onSceneLoaded += ShowWindow;
        GameController.instance.onSceneProgressUpdated += UpdateProgress;
    }

    protected void OnDestroy()
    {
        GameController.instance.onSceneStartLoading -= HideWindow;
        GameController.instance.onSceneLoaded -= ShowWindow;
        GameController.instance.onSceneProgressUpdated -= UpdateProgress;
    }
    #region visability window
    private void HideWindow()
    {
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(Fade(1f));
    }

    private void UpdateProgress(float progress)
    {
        
    }

    protected void ShowWindow()
    {        
        StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float endAlpha)
    {
        float startAlpha = loadingScreen.color.a;
        Color color = loadingScreen.color;
        float elapsedTime = 0f;
        
        while (elapsedTime < SceneLoader.SCENE_SWITCH_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / SceneLoader.SCENE_SWITCH_DURATION);
            color.a = alpha;
            loadingScreen.color = color;
            yield return null;
        }

        // Финальное значение
        color.a = endAlpha;
        loadingScreen.color = color;     
        if(endAlpha <= 0f)
        {
            loadingScreen.gameObject.SetActive(false);
        }
    }
    #endregion
}
