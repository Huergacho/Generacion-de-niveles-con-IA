using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private bool _isGamePaused;

    [Header("Scene Names")]
    [SerializeField] private Dictionary<Scene, float> levels = new Dictionary<Scene, float>(); //TODO: move this to a level manager that does the randomnizer inside the level?
    [SerializeField] private string levelScene = "Level";
    [SerializeField] private string menuScene = "MainMenu";
    [SerializeField] private string victoryScene = "Victory";
    [SerializeField] private string gameOverScene = "GameOver";
    
    private bool isPlayerAlive;

    //Properties
    public bool IsGamePaused { get => _isGamePaused; private set => _isGamePaused = value; } //TODO: Make Everytone check if game is paused to update or something. 
    public PlayerModel Player { get; private set; }
    public string MenuScene => menuScene;
    public string LevelScene => levelScene;
    public bool IsPlayerAlive => isPlayerAlive;
    public InputManager InputManager { get; private set; }

    //Events
    public Action OnPause;
    public Action<PlayerModel> OnPlayerInit;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InputManager = GetComponent<InputManager>();
    }

    public void PlayerIsDead()
    {
        isPlayerAlive = false;
        GameOver();
    }

    public void LevelChange()
    {
        var levelSelected = MyEngine.MyRandom.GetRandomWeight(levels);

        SceneManager.LoadScene(levelSelected.buildIndex);

        levels.Remove(levelSelected);
    }

    public void Pause(bool value)
    {
        IsGamePaused = value;
        //SetCursorActive(value); //TODO: maybe change cursor look on not paused?
        if (value)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        OnPause?.Invoke();
    }

    public void SetCursorActive(bool value)
    {
        if (value)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
        OnPlayerInit?.Invoke(Player);
    }

    private void Victory()
    {
        SceneManager.LoadScene(victoryScene);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(gameOverScene);
    }
}
