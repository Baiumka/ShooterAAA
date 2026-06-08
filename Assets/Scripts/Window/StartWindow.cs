using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : Window
{
    [SerializeField] private Button newGameButton;
    
    private new void Awake()
    {
        base.Awake();        
    }    
    private new void Start()
    {
        base.Start();        
        newGameButton.onClick.AddListener(StartNewGameButtonClick);        
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        newGameButton.onClick.RemoveAllListeners();
    }

    private void StartNewGameButtonClick()
    {
        GameController.instance.StartLoadGame();
    }
}
